
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;

[ApiController]
[Route("[controller]")]
public class ContactsController : Controller
{
    private readonly RealEstatePostingApi.Context.AppDbContext _context;

    public ContactsController(RealEstatePostingApi.Context.AppDbContext context)
    {
        _context = context;
    }

    // GET: CONTACTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Contact.ToListAsync());
    }

    // GET: CONTACTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contact = await _context.Contact
            .FirstOrDefaultAsync(m => m.Id == id);
        if (contact == null)
        {
            return NotFound();
        }

        return View(contact);
    }

    // GET: CONTACTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CONTACTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Username,Phone_Number,Created_Date,Updated_Date")] Contact contact)
    {
        if (ModelState.IsValid)
        {
            _context.Add(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(contact);
    }

    // GET: CONTACTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contact = await _context.Contact.FindAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        return View(contact);
    }

    // POST: CONTACTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Username,Phone_Number,Created_Date,Updated_Date")] Contact contact)
    {
        if (id != contact.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(contact);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(contact.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(contact);
    }

    // GET: CONTACTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contact = await _context.Contact
            .FirstOrDefaultAsync(m => m.Id == id);
        if (contact == null)
        {
            return NotFound();
        }

        return View(contact);
    }

    // POST: CONTACTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var contact = await _context.Contact.FindAsync(id);
        if (contact != null)
        {
            _context.Contact.Remove(contact);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ContactExists(int? id)
    {
        return _context.Contact.Any(e => e.Id == id);
    }
}
