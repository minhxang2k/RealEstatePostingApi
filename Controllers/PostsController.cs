using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;
using RealEstatePostingApi.Model;
using System.Security.Cryptography;

[ApiController]
[Route("[controller]")]
public class PostsController : Controller
{
    private readonly RealEstatePostingApi.Context.AppDbContext _context;

    public PostsController(RealEstatePostingApi.Context.AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }
    private readonly IWebHostEnvironment _environment;

    // GET: POSTS

    [HttpGet("List")]
    [RequestSizeLimit(2_147_483_648L)]          // 2 GB — matches ValidateVideo's per-file limit
    [RequestFormLimits(MultipartBodyLengthLimit = 2_147_483_648L)]
    public async Task<IActionResult> List()
    {
        return View(await _context.Post.ToListAsync());
    }

    [HttpGet("List/{id}")]
    [RequestSizeLimit(2_147_483_648L)]          // 2 GB — matches ValidateVideo's per-file limit
    [RequestFormLimits(MultipartBodyLengthLimit = 2_147_483_648L)]
    public async Task<IActionResult> List(int id)
    {
        var posts = await _context.Post
            .AsNoTracking()
            .Include(p => p.Post_Details)
            .Include(p => p.Post_Pictures)
            .Include(p => p.Post_Video)
            .OrderByDescending(p => p.Created_Date)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Content,
                p.Address,
                p.Latitude,
                p.Longitude,
                p.Price,
                p.City_Id,
                p.District_Id,
                p.Ward_Id,
                p.Street_Id,
                p.Property_Type_Id,
                p.Transaction_Type_Assignment_Id,
                p.Is_Recommended,
                p.Status_Id,
                p.View_Count,
                p.Created_Date,
                p.Updated_Date,

                Post_Detail = p.Post_Details == null
                    ? null
                    : new
                    {
                        p.Post_Details.Square,
                        p.Post_Details.BedRoom,
                        p.Post_Details.bathRoom,
                        p.Post_Details.House_Direction,
                        p.Post_Details.Facade,
                        p.Post_Details.Total_Floors
                    },

                Post_Pictures = p.Post_Pictures
                    .Select(pic => new
                    {
                        pic.Id,
                        pic.Image_Url,
                        pic.Picture_Type,
                        pic.Is_Thumbnail
                    })
                    .ToList(),

                Post_Video = p.Post_Video == null
                ? null
                : new
                {
                    p.Post_Video.Id,
                    p.Post_Video.Video_Url
                }
            }).FirstOrDefaultAsync(p => p.Id == id);

        return Ok(posts);
    }

    public class UploadPost
    {
        public UploadPostDto Post { get; set; }
        public UploadPost_DetailDto Post_Detail { get; set; }
        public List<UploadPost_PictureDto> Post_Pictures { get; set; }
        public List<IFormFile> picturefile { get; set; } = new();
        public IFormFile videofile { get; set; } 
        //public UploadPost_VideoDto Post_Video { get; set; }
    }

    [HttpPost("Upload")]
    [RequestSizeLimit(2_147_483_648L)]          // 2 GB — matches ValidateVideo's per-file limit
    [RequestFormLimits(MultipartBodyLengthLimit = 2_147_483_648L)]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> Upload([FromForm] UploadPost uploadPost)

    {
        var post = new Post
        {
            Account_Id = uploadPost.Post.Account_Id,
            Title = uploadPost.Post.Title,
            Content = uploadPost.Post.Content,
            Address = uploadPost.Post.Address,
            Latitude = uploadPost.Post.Latitude,
            Longitude = uploadPost.Post.Longitude,
            Price = uploadPost.Post.Price,
            City_Id = uploadPost.Post.City_Id,
            District_Id = uploadPost.Post.District_Id,
            Ward_Id = uploadPost.Post.ward_Id,
            Street_Id = uploadPost.Post.street_Id,
            Property_Type_Id = uploadPost.Post.Property_Type_Id,
            Transaction_Type_Assignment_Id = uploadPost.Post.transaction_Type_Assignment_Id,
            Is_Recommended = uploadPost.Post.Is_Recommended,
            Status_Id = uploadPost.Post.Status_Id,
            View_Count = 0,
            Created_Date = DateTime.Now
        };
        _context.Post.Add(post);
        await _context.SaveChangesAsync();

        var postDetail = new Post_Detail
        {
            Post_Id = post.Id,
            Square = uploadPost.Post_Detail.Square,
            BedRoom = uploadPost.Post_Detail.Bedroom,
            bathRoom = uploadPost.Post_Detail.Bathroom,
            House_Direction = uploadPost.Post_Detail.House_Direction,
            Facade = uploadPost.Post_Detail.Facade,
            Total_Floors = uploadPost.Post_Detail.total_Floor,
            Created_Date = DateTime.Now
        };

        var pictureError = ValidateImage(uploadPost.picturefile);
        var videoError = ValidateVideo(uploadPost.videofile);

        if(pictureError != null)
        {
            return BadRequest("error valitade image:" + pictureError);
        }

        if(videoError != null)
        {
            return BadRequest("error validate video:" + videoError);
        }

        var VideoUrl = await SaveVideoAsync(uploadPost.videofile, post.Id);

        var postPictures = new List<Post_Picture>();

        if (uploadPost.Post_Pictures != null && uploadPost.picturefile != null)
        {
        // Map DTOs to files by index; use the smaller count to avoid out-of-range
        var count = Math.Min(uploadPost.Post_Pictures.Count, uploadPost.picturefile.Count);

            if (uploadPost.Post_Pictures == null ||
                uploadPost.picturefile == null ||
                uploadPost.Post_Pictures.Count != uploadPost.picturefile.Count)
            {
                    return BadRequest(
                        "Số lượng metadata ảnh phải bằng số lượng file ảnh."
                    );
            }

            for (int i = 0; i < count; i++)
            {
                var dto = uploadPost.Post_Pictures[i];
                var file = uploadPost.picturefile[i];
                var isThumbnail = dto?.Is_Thumbnail ?? false;

                var pictureUrl = await SaveImageAsync(file, isThumbnail, post.Id);

                var picture = new Post_Picture
                {
                    Post_Id = post.Id,
                    Image_Url = pictureUrl,
                    Picture_Type = dto?.Picture_Type,
                    Is_Thumbnail = isThumbnail,
                    Created_Date = DateTime.Now
                };

                postPictures.Add(picture);
            }
        }

        var postVideo = new Post_Video
        {
            Post_Id = post.Id,
            Video_Url = VideoUrl,
            Created_Date = DateTime.Now
        };

       
        _context.Post_Detail.Add(postDetail);
        _context.Post_Picture.AddRange(postPictures);
        _context.Post_Video.Add(postVideo);

        await _context.SaveChangesAsync();

        return Ok("Post đã tạo thành công");
    }


    private string getPosition(bool isThumbnail)
    {
        switch (isThumbnail)
        {
            case true:
                return "thumbnail";
            default: 
                return "picture";
      
        }
    }

    private string GetPicturesPath(string imageUrl, bool isThumbnail, int postId)
    {
        var fileName = Path.GetFileName(imageUrl);
        var path = getPosition(isThumbnail);
        return Path.Combine(
            _environment.WebRootPath,
            "Post",
            postId.ToString(),
            path,
            fileName
        );
    }

    private string GetVideosPath(string Url, int postId)
    {
        var fileName = Path.GetFileName(Url);
        
        return Path.Combine(
            _environment.WebRootPath,
            "Post",
            postId.ToString(),
            "video",
            fileName
        );
    }

    private void DeleteImage(string imageUrl, bool isThumbnail, int postId)
    {
        var filePath = GetPicturesPath(imageUrl, isThumbnail, postId);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }

    private void DeleteVideo(string videoUrl, int postId)
    {
        var filePath = GetVideosPath(videoUrl, postId);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }

    private async Task<string> SaveImageAsync(IFormFile file, bool isThumbnail, int postId)
    {
        string path = getPosition(isThumbnail);

        var uploadFolder = Path.Combine(
            _environment.WebRootPath,
            "Post",
            postId.ToString(),
            path
        );

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var extension = Path
            .GetExtension(file.FileName)
            .ToLowerInvariant();

        // Hash the file name to avoid conflicts
        var hashedFileName = await CalculateFileHashAsync(file);
        var fileName = $"{hashedFileName}{extension}";

        var filePath = Path.Combine(
            uploadFolder,
            fileName
        );

        if (!System.IO.File.Exists(filePath))
        {
            await using var stream = new FileStream(
                filePath,
                FileMode.CreateNew
            );

            await file.CopyToAsync(stream);
        }

        return $"/Post/{postId}/{path}/{fileName}";
    }
    private async Task<string> SaveVideoAsync(IFormFile file, int postId)
    {
        

        var uploadFolder = Path.Combine(
            _environment.WebRootPath,
            "Post",
            postId.ToString(),
            "video"
        );

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var extension = Path
            .GetExtension(file.FileName)
            .ToLowerInvariant();

        // Hash the file name to avoid conflicts
        var hashedFileName = await CalculateFileHashAsync(file);
        var fileName = $"{hashedFileName}{extension}";

        var filePath = Path.Combine(
            uploadFolder,
            fileName
        );

        if (!System.IO.File.Exists(filePath))
        {
            await using var stream = new FileStream(
                filePath,
                FileMode.CreateNew
            );

            await file.CopyToAsync(stream);
        }

        return $"/Post/{postId}/video/{fileName}";
    }

    private string? ValidateImage(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            return "Vui lòng chọn ảnh.";
        }

        var allowedExtensions = new[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        // validate each file
        foreach (var f in files)
        {
            if (f == null || f.Length == 0)
            {
                return "File ảnh không được để trống.";
            }
            var extension = Path.GetExtension(f.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return "Chỉ cho phép JPG, JPEG, PNG hoặc WEBP.";
            }

            // per-file size limit (1 GB here)
            if (f.Length > 1024L * 1024 * 1024)
            {
                return "Ảnh không được vượt quá 1GB.";
            }
        }

        return null;
    }

    private string? ValidateVideo(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return "Vui lòng chọn video.";
        }

        var allowedExtensions = new[]
        {
        ".mp4",
        ".webm",
        ".mov"
    };

        var extension = Path
            .GetExtension(file.FileName)
            .ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            return "Chỉ cho phép MP4, WEBM hoặc MOV.";
        }

        // Giới hạn 2 GB
        if (file.Length > 2000L * 1024 * 1024)
        {
            return "Video không được vượt quá 2 GB.";
        }

        return null;
    }
    private async Task<string> CalculateFileHashAsync(IFormFile file)
    {
        using var sha256 = SHA256.Create();

        using var stream = file.OpenReadStream();

        var hash = await sha256.ComputeHashAsync(stream);

        return Convert.ToHexString(hash).ToLowerInvariant();
    }

}
//    // GET: POSTS/Details/5
//    public async Task<IActionResult> Details(int? id)
//    {
//        if (id == null)
//        {
//            return NotFound();
//        }

//        var post = await _context.Post
//            .FirstOrDefaultAsync(m => m.Id == id);
//        if (post == null)
//        {
//            return NotFound();
//        }

//        return View(post);
//    }

//    // GET: POSTS/Create
//    public IActionResult Create()
//    {
//        return View();
//    }

//    // POST: POSTS/Create
//    // To protect from overposting attacks, enable the specific properties you want to bind to.
//    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Create([Bind("Id,Account_Id,Title,Content,Address,Latitude,Longitude,Price,City_Id,District_Id,Ward_Id,Street_Id,Property_Type_Id,Transaction_Type_Assignment_Id,Is_Recommended,Status_Id,View_Count,Created_Date,Updated_Date,Delete_Scheduled_At")] Post post)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.Add(post);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }
//        return View(post);
//    }

//    // GET: POSTS/Edit/5
//    public async Task<IActionResult> Edit(int? id)
//    {
//        if (id == null)
//        {
//            return NotFound();
//        }

//        var post = await _context.Post.FindAsync(id);
//        if (post == null)
//        {
//            return NotFound();
//        }
//        return View(post);
//    }

//    // POST: POSTS/Edit/5
//    // To protect from overposting attacks, enable the specific properties you want to bind to.
//    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(int? id, [Bind("Id,Account_Id,Title,Content,Address,Latitude,Longitude,Price,City_Id,District_Id,Ward_Id,Street_Id,Property_Type_Id,Transaction_Type_Assignment_Id,Is_Recommended,Status_Id,View_Count,Created_Date,Updated_Date,Delete_Scheduled_At")] Post post)
//    {
//        if (id != post.Id)
//        {
//            return NotFound();
//        }

//        if (ModelState.IsValid)
//        {
//            try
//            {
//                _context.Update(post);
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!PostExists(post.Id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return RedirectToAction(nameof(Index));
//        }
//        return View(post);
//    }

//    // GET: POSTS/Delete/5
//    public async Task<IActionResult> Delete(int? id)
//    {
//        if (id == null)
//        {
//            return NotFound();
//        }

//        var post = await _context.Post
//            .FirstOrDefaultAsync(m => m.Id == id);
//        if (post == null)
//        {
//            return NotFound();
//        }

//        return View(post);
//    }

//    // POST: POSTS/Delete/5
//    [HttpPost, ActionName("Delete")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> DeleteConfirmed(int? id)
//    {
//        var post = await _context.Post.FindAsync(id);
//        if (post != null)
//        {
//            _context.Post.Remove(post);
//        }

//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(Index));
//    }

//    private bool PostExists(int? id)
//    {
//        return _context.Post.Any(e => e.Id == id);
//    }
//}
