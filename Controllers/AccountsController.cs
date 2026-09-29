
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;
using RealEstatePostingApi.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


[ApiController]
[Route("[controller]")]
public class AccountsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AccountsController(
        AppDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    private string GenerateJwtToken(Account account)
    {
        var jwtSettings = _configuration
            .GetSection("JWT");

        var key = jwtSettings["Key"];

        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expireMinutes = jwtSettings["ExpireMinutes"];

        if (string.IsNullOrEmpty(key))
            throw new Exception("JWT Key is missing.");

        if (string.IsNullOrEmpty(issuer))
            throw new Exception("JWT Issuer is missing.");

        if (string.IsNullOrEmpty(audience))
            throw new Exception("JWT Audience is missing.");

        if (account.Role == null)
            throw new Exception("Account Role is null.");


        var claims = new List<Claim>
    {
        new Claim(
            ClaimTypes.NameIdentifier,
            account.Id.ToString()
        ),

        new Claim(
            ClaimTypes.Email,
            account.Email
        ),

        new Claim(
            ClaimTypes.Role,
            account.Role.Role
        )
    };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key!)
        );

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(
                double.Parse(jwtSettings["ExpiresDays"]!)
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // GET: ACCOUNTS
    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<Account>> GetById(int id)    
    {
        var account = await _context.Account.Include(a => a.Role).Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
        if (account == null)
        {
            return NotFound("Account not found");
        }
        //var token = GenerateJwtToken(account);

        return Ok( new AccountResponseDto
        {
            //token = token,
            Id = account.Id,
            Email = account.Email,
            RoleId = account.Role_Id,
            Role = account.Role?.Role,
            IsActive = account.Is_Active,
            CreatedDate = account.Created_Date,
            Username = account.User?.Username,
            Sex = account.User?.Sex

        }); 
        
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        var tokens = new TokenRespond();
            var existingAccount = await _context.Account.Include(a => a.Role).Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Email == loginDto.Email);

            Console.WriteLine(existingAccount);
            if (existingAccount == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var hashedPassword = HashPassword(loginDto.Password);

            if (existingAccount.Password_Hash != hashedPassword)
            {
                return Unauthorized("Password is incorrect");
            }

            if(existingAccount.Is_Active == false)
            {
                return Unauthorized("Account is inactive.");
            }

        tokens.token = GenerateJwtToken(existingAccount);
        return Ok(new AccountLoginResponseDto
        {
           
            Id = existingAccount.Id,
            Email = existingAccount.Email,
            RoleId = existingAccount.Role_Id,
            Role = existingAccount.Role?.Role,
            IsActive = existingAccount.Is_Active,
            //CreatedDate = existingAccount.Created_Date,
            Username = existingAccount.User?.Username,
            Sex = existingAccount.User?.Sex,
            Token = tokens.token,
        });

    }

    private class TokenRespond
    {
        public string? token;
    }

    public class RegisterDto
    {
       public CreateAccountDto account { get; set; } 
        public CreateUserDto user { get; set; }
    }

    [HttpPost("Register")]
    [Authorize(Roles = "admin")]
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
            Role_Id = dto.account.Role_Id, //admin role
            Auth_Provider = dto.account.Auth_Provider,
            Is_Active = true,
            Created_Date = DateTime.Now
        };

        var user = new User
        {
            Account = account,
            Username = dto.user.Username,
            Phone_Number = dto.user.Phone_Number,
            Avatar_Url = string.Empty,
            Sex = dto.user.Sex,
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

//    {
//    "Account":{
//        "Email":"admin4",
//        "Password":"2222",
//        "Role_Id":1,
//        "Auth_Provider":"local"
//    },

//    "User":{
//        "Username":"admin3",
//        "Phone_Number":"0987654321",
//        "Sex":"male"
//    }
//}


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
