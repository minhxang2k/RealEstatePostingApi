
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;


[ApiController]
[Route("[controller]")]
public class WebsitePictureController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public WebsitePictureController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet("List")]
    // GET: WEBSITE_PICTURES
    public async Task<IActionResult> Index()
    {
        var pictures = await _context.Website_Picture.ToListAsync();
        return Ok(pictures);
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        [FromForm] string position,
        [FromForm] int websiteOrder)
    {
        var error = ValidateImage(file);

        if (error != null)
        {
            return BadRequest(error);
        }

        var imageUrl = await SaveImageAsync(file);

        var picture = new Website_Picture
        {
            Image_Url = imageUrl,
            Position = position,
            Display_Order = websiteOrder,
            Is_Active = true,
            Created_Date = DateTime.Now
        };

        _context.Website_Picture.Add(picture);

        await _context.SaveChangesAsync();

        return Ok(picture);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    int id,
    [FromForm] IFormFile file,
    [FromForm] string position,
    [FromForm] int websiteOrder,
    [FromForm] bool isActive)
    {
        var picture = await _context.Website_Picture
            .FindAsync(id);

        if (picture == null)
        {
            return NotFound("Record không tồn tại.");
        }

        var error = ValidateImage(file);

        if (error != null)
        {
            return BadRequest(error);
        }

        // Lưu ảnh mới trước
        var newImageUrl = await SaveImageAsync(file);

        // Lưu URL ảnh cũ
        var oldImageUrl = picture.Image_Url;

        // Update record
        picture.Image_Url = newImageUrl;
        picture.Position = position;
        picture.Display_Order = websiteOrder;
        picture.Is_Active = isActive;
        picture.Updated_Date = DateTime.Now;

        await _context.SaveChangesAsync();

        // Chỉ xóa ảnh cũ sau khi DB update thành công
        DeleteImage(oldImageUrl);

        return Ok(picture);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromForm] IFormFile file, [FromForm] UpdateWebsite_PictureDto updateDto)
    {
        var picture = await _context.Website_Picture
            .FindAsync(id);

        if (picture == null)
        {
            return NotFound("Record không tồn tại.");
        }


        if (file != null && file.Length > 0)
        {
            var error = ValidateImage(file);

            if (error != null)
            {
                return BadRequest(error);
            }
            // Lưu ảnh mới trước
            updateDto.Image_Url = await SaveImageAsync(file);

            // Lưu URL ảnh cũ
            var oldImageUrl = picture.Image_Url;

            picture.Image_Url = updateDto.Image_Url;

            // Chỉ xóa ảnh cũ sau khi DB update thành công
            DeleteImage(oldImageUrl);
        }

        if (!string.IsNullOrEmpty(updateDto.Position))

        {
            picture.Position = updateDto.Position;
           
        }
        
        if(updateDto.Display_Order.HasValue)
        {
            picture.Display_Order = updateDto.Display_Order.Value;
        }

        if (updateDto.Is_Active.HasValue &&
        updateDto.Is_Active.Value != picture.Is_Active)
        {
            picture.Is_Active = updateDto.Is_Active.Value;
        }

        picture.Updated_Date = DateTime.Now;

        // Update record
        await _context.SaveChangesAsync();
       
        return Ok(picture);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var picture = await _context.Website_Picture
            .FindAsync(id);

        if (picture == null)
        {
            return NotFound("Record không tồn tại.");
        }

        DeleteImage(picture.Image_Url);

        _context.Website_Picture.Remove(picture);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Đã xóa ảnh và record.",
            id = id
        });
    }

    private string GetFilePath(string imageUrl)
    {
        var fileName = Path.GetFileName(imageUrl);

        return Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "website",
            fileName
        );
    }

    //private string GetFilePath(string imageUrl)
    //{
    //    var fileName = Path.GetFileName(
    //        new Uri(imageUrl).LocalPath
    //    );

    //    return Path.Combine(
    //        _environment.WebRootPath,
    //        "uploads",
    //        "website",
    //        fileName
    //    );
    //}

    private bool ImageExists(string imageUrl)
    {
        var filePath = GetFilePath(imageUrl);

        return System.IO.File.Exists(filePath);
    }

    private void DeleteImage(string imageUrl)
    {
        var filePath = GetFilePath(imageUrl);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }

    private async Task<string> SaveImageAsync(IFormFile file)
    {
        var uploadFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "website"
        );

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var extension = Path
            .GetExtension(file.FileName)
            .ToLowerInvariant();

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(
            uploadFolder,
            fileName
        );

        using (var stream = new FileStream(
            filePath,
            FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/website/{fileName}";
    }

    private string? ValidateImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
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

        var extension = Path
            .GetExtension(file.FileName)
            .ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            return "Chỉ cho phép JPG, JPEG, PNG hoặc WEBP.";
        }

        if (file.Length > 1024 * 1024 * 1024)
        {
            return "Ảnh không được vượt quá 1GB.";
        }

        return null;
    }

}