
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;
using RealEstatePostingApi.Model;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AccountsController : Controller
{
    private readonly AppDbContext _context;

    public AccountsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: ACCOUNTS
    [HttpGet]
    public async Task<ActionResult<Account>> Index()    
    {
        return View(await _context.Account.ToListAsync());
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<Account>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var existingAccount = await _context.Account
                .FirstOrDefaultAsync(a => a.Email == loginDto.Username && a.Password_Hash == HashPassword(loginDto.Password));
            if (existingAccount == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(existingAccount);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while logging in: {ex.Message}");
        }
    }
    public class RegisterDto
    {
       public CreateAccountDto account { get; set; } 
        public CreateUserDto user { get; set; }
    }

    [HttpPost("Register")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Register([FromBody] RegisterDto dto)
    {
        if (dto == null)
            return BadRequest("Account data is null.");

        var existingAccount = await _context.Account
            .FirstOrDefaultAsync(a => a.Email == dto.account.Email);

        if (existingAccount != null)
            return Conflict("An account with this email already exists.");

        var account = new Account
        {
            Email = dto.account.Email,
            Password_Hash = HashPassword(dto.account.Password),
            Role_Id = 1, //admin role
            Auth_Provider = dto.account.Auth_Provider,
            Is_Active = true,
            Created_Date = DateTime.Now
        };

        var user = new User
        {
            Account = account,
            Username = dto.user.Username,
            Phone_Number = string.Empty,
            Avatar_Url = string.Empty,
            Sex = string.Empty,
            Created_Date = DateTime.Now
        };

        _context.Account.Add(account);
        _context.User.Add(user);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Index),
            new { id = account.Id },
            new
            {
                id = account.Id,
                email = account.Email,
                roleId = account.Role_Id,
                username = user.Username
            }
        );
    }

    private string HashPassword(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }


    //    // GET: ACCOUNTS/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var account = await _context.Account
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (account == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(account);
    //    }

    //    // GET: ACCOUNTS/Create
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: ACCOUNTS/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,Email,Password_Hash,Auth_Provider,Provider_User_Id,Role_Id,Is_Active,Created_Date,Updated_Date")] Account account)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(account);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(account);
    //    }

    //    // GET: ACCOUNTS/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var account = await _context.Account.FindAsync(id);
    //        if (account == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(account);
    //    }

    //    // POST: ACCOUNTS/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int? id, [Bind("Id,Email,Password_Hash,Auth_Provider,Provider_User_Id,Role_Id,Is_Active,Created_Date,Updated_Date")] Account account)
    //    {
    //        if (id != account.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(account);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!AccountExists(account.Id))
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
    //        return View(account);
    //    }

    //    // GET: ACCOUNTS/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var account = await _context.Account
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (account == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(account);
    //    }

    //    // POST: ACCOUNTS/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int? id)
    //    {
    //        var account = await _context.Account.FindAsync(id);
    //        if (account != null)
    //        {
    //            _context.Account.Remove(account);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool AccountExists(int? id)
    //    {
    //        return _context.Account.Any(e => e.Id == id);
    //    }
}
