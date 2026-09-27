
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;

[ApiController]
[Route("[controller]")]
public class UsersController : Controller
{
    private readonly AppDbContext _context;
    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByAccountId(int id)
    {
        var user = await _context.User.FirstOrDefaultAsync(u  => u.Account_Id == id);
        if (user == null)
        {
            return NotFound("User not found");
        }
        return Ok(user);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userDto)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Account_Id == id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        // Update user properties
       if(userDto.UserName != null)
        {
            user.Username = userDto.UserName;
        }
        if(userDto.ImageUrl != null)
        {
            user.Avatar_Url = userDto.ImageUrl;
        }
        if(userDto.PhoneNumber != null)
        {
            user.Phone_Number = userDto.PhoneNumber;
        }
        if(userDto.Sex != null)
        {
            user.Sex = userDto.Sex;
        }
        user.Updated_Date = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(user);
    }
}
