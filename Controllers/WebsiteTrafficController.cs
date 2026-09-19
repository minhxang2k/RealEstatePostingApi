
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("[controller]")]
public class WebsiteTrafficController : Controller
{
    private readonly AppDbContext _context;

    public WebsiteTrafficController(AppDbContext context)
    {
        _context = context;
    }

    // GET: WEBSITE_TRAFFICS
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Website_Traffic>>> Index()
    {
        return await _context.Website_Traffic.ToListAsync();
    }

    // GET: WEBSITE_TRAFFICS/Details/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Website_Traffic>> Details(DateOnly? date)
    {
        if (date == null || date.ToString() == "")
        {
            return NotFound("Can not find any date");
        }

        var website_traffic = await _context.Website_Traffic
            .FirstOrDefaultAsync(m => m.Visit_Date == date);
        if (website_traffic == null)
        {
            return NotFound();
        }

        return Ok(website_traffic);
    }

    private int visitCount = 0;
    private DateOnly visitDate = DateOnly.FromDateTime(DateTime.Now);


    // POST: WEBSITE_TRAFFICS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<ActionResult<Website_Traffic>> Create()
    {
        
        
        if (ModelState.IsValid)
        {
            var website_traffic = new Website_Traffic
            {
                Visit_Date = visitDate,
                Visit_Count = visitCount,
                Created_Date = DateTime.Now
            };
            
            _context.Add(website_traffic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return Ok();
    }

    // GET: WEBSITE_TRAFFICS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var website_traffic = await _context.Website_Traffic.FindAsync(id);
        if (website_traffic == null)
        {
            return NotFound();
        }
        return View(website_traffic);
    }

    // POST: WEBSITE_TRAFFICS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Visit_Date,Visit_Count,Created_Date,Updated_Date")] Website_Traffic website_traffic)
    {
        if (id != website_traffic.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(website_traffic);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Website_TrafficExists(website_traffic.Id))
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
        return View(website_traffic);
    }

    // GET: WEBSITE_TRAFFICS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var website_traffic = await _context.Website_Traffic
            .FirstOrDefaultAsync(m => m.Id == id);
        if (website_traffic == null)
        {
            return NotFound();
        }

        return View(website_traffic);
    }

    // POST: WEBSITE_TRAFFICS/Delete/5
   
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var website_traffic = await _context.Website_Traffic.FindAsync(id);
        if (website_traffic != null)
        {
            _context.Website_Traffic.Remove(website_traffic);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Website_TrafficExists(int? id)
    {
        return _context.Website_Traffic.Any(e => e.Id == id);
    }
}
