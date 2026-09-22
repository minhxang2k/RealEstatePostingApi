
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
    [HttpPatch("{id}")]
    public async Task<ActionResult<Website_Traffic>> Edit(int? id, [FromBody] UpdateWebsiteTrafficDto updateDto)
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
        if(updateDto == null)
        {
            return BadRequest("Update data is required.");
        }

        if(updateDto.Visit_Count.HasValue && updateDto.Visit_Count.Value != 0)
        {
            website_traffic.Visit_Count = updateDto.Visit_Count.Value;
        }
        
        return Ok(website_traffic);
    }

 
   
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
