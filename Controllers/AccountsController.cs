
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;

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
   
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Account.ToListAsync());
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
