
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;

public class StreetsController : Controller
{
    private readonly AppDbContext _context;

    public StreetsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: STREETS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Street.ToListAsync());
    }

    // GET: STREETS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var street = await _context.Street
            .FirstOrDefaultAsync(m => m.Id == id);
        if (street == null)
        {
            return NotFound();
        }

        return View(street);
    }

    // GET: STREETS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: STREETS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Ward_Id,Name,Created_Date,Updated_Date,Ward,Posts")] Street street)
    {
        if (ModelState.IsValid)
        {
            _context.Add(street);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(street);
    }

    // GET: STREETS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var street = await _context.Street.FindAsync(id);
        if (street == null)
        {
            return NotFound();
        }
        return View(street);
    }

    // POST: STREETS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Ward_Id,Name,Created_Date,Updated_Date,Ward,Posts")] Street street)
    {
        if (id != street.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(street);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StreetExists(street.Id))
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
        return View(street);
    }

    // GET: STREETS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var street = await _context.Street
            .FirstOrDefaultAsync(m => m.Id == id);
        if (street == null)
        {
            return NotFound();
        }

        return View(street);
    }

    // POST: STREETS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var street = await _context.Street.FindAsync(id);
        if (street != null)
        {
            _context.Street.Remove(street);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StreetExists(int? id)
    {
        return _context.Street.Any(e => e.Id == id);
    }
}
