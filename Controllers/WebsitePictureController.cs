
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;

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
        return Ok(await _context.Website_Picture.ToListAsync());
    }


    [HttpPost("Upload")]
    public async Task<ActionResult<Website_Picture>> Upload(
          IFormFile file,
          [FromForm] string position,
          [FromForm] int websiteOrder)
    {
        // Kiểm tra file
        if (file == null || file.Length == 0)
        {
            return BadRequest("Vui lòng chọn ảnh.");
        }

        // Kiểm tra Position
        if (string.IsNullOrWhiteSpace(position))
        {
            return BadRequest("Position không được để trống.");
        }

        // Các định dạng ảnh được phép
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
            return BadRequest(
                "Chỉ cho phép JPG, JPEG, PNG hoặc WEBP.");
        }

        // Giới hạn 5 MB
        if (file.Length > 5 * 1024 * 1024)
        {
            return BadRequest("Ảnh không được vượt quá 5MB.");
        }

        // Đường dẫn:
        // wwwroot/uploads/website
        var uploadFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "website");

        // Nếu thư mục chưa tồn tại thì tạo
        if (!Directory.Exists(uploadFolder) || uploadFolder == null)
        {
            Directory.CreateDirectory(uploadFolder);
        }

        // Tạo tên file mới
        // Không dùng tên file gốc của người dùng
        var fileName = $"{Guid.NewGuid()}{extension}";

        // Đường dẫn vật lý của file
        var filePath = Path.Combine(
            uploadFolder,
            fileName);

        // Lưu file
        using (var stream = new FileStream(
            filePath,
            FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // URL để website truy cập ảnh
        var imageUrl =
            $"{Request.Scheme}://{Request.Host}/uploads/website/{fileName}";

        var websitePicture = new Website_Picture
        {
            Image_Url = imageUrl,
            Position = position,
            Display_Order = websiteOrder,
            Is_Active = true,
            Created_Date = DateTime.Now,
            Updated_Date = null
        };

        _context.Website_Picture.Add(websitePicture);

        await _context.SaveChangesAsync();

        return Ok(websitePicture);
    }


}