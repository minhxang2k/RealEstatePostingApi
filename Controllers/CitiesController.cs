
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;

[ApiController]
[Route("[controller]")]
public class CitiesController : Controller
{
    private readonly AppDbContext _context;

    
    public CitiesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: CITYS
    [HttpGet]
    public async Task<IActionResult> Index()    
    {
        return View(await _context.City.ToListAsync());
    }

    // GET: CITYS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var city = await _context.City
            .FirstOrDefaultAsync(m => m.Id == id);
        if (city == null)
        {
            return NotFound();
        }

        return View(city);
    }

    // GET: CITYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CITYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Created_Date,Updated_Date")] City city)
    {
        if (ModelState.IsValid)
        {
            _context.Add(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(city);
    }

    // GET: CITYS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var city = await _context.City.FindAsync(id);
        if (city == null)
        {
            return NotFound();
        }
        return View(city);
    }

    // POST: CITYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Created_Date,Updated_Date")] City city)
    {
        if (id != city.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(city);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(city.Id))
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
        return View(city);
    }

    // GET: CITYS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var city = await _context.City
            .FirstOrDefaultAsync(m => m.Id == id);
        if (city == null)
        {
            return NotFound();
        }

        return View(city);
    }

    // POST: CITYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var city = await _context.City.FindAsync(id);
        if (city != null)
        {
            _context.City.Remove(city);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CityExists(int? id)
    {
        return _context.City.Any(e => e.Id == id);
    }
}
