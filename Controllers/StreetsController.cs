
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;
using Microsoft.Identity.Client;

[ApiController]
[Route("[controller]")]
public class StreetsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StreetsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: STREETS
    [HttpGet]
    public async Task<ActionResult<Street>> Index()
    {
        try
        {
            var streets = await _context.Street.ToListAsync();
            if (streets == null || streets.Count == 0)
            {
                return NotFound("No streets found.");
            }
            return Ok(streets);
        }
        catch (Exception e)
        {
            throw new Exception($"there an error occurred while listing street list: {e.Message}");
        }

    }

    // GET: STREETS/Details/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Street>> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var street = await _context.Street  
            .FirstOrDefaultAsync(m => m.Id == id);
        if (street == null)
        {
            return NotFound("can not find because its does not exists");
        }

        return Ok(street);
    }

    // GET: STREETS/Create
    [HttpPost]
    public async Task<ActionResult<Street>> Create([FromBody] CreateStreetDto dto)
    {
        try
        {
            var street = new Street
            {
                Ward_Id = dto.Ward_Id,
                Name = dto.Name,
                Created_Date = DateTime.Now,
            };
            _context.Street.Add(street);
            await _context.SaveChangesAsync();
            return Ok(street);
        }
        catch (Exception ex)
        {
            throw new Exception("an error occurred while creating the street: " + ex.Message);
        }
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStreetDto dto)
    {
        try
        {
            var street = await _context.Street.FindAsync(id);
            if (street == null)
            {
                return NotFound("can not find because its does not exists");
            }

            if (dto.Name != null)
            {
                street.Name = dto.Name;
            }

            if (dto.Ward_Id.HasValue)
            {
                street.Ward_Id = dto.Ward_Id.Value;
            }

            street.Updated_Date = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(street);
        }
        catch (Exception e)
        {
            throw new Exception($"an error occurred while updating street: {e.Message}");
        }
    }
    // POST: STREETS/Delete/5
    [HttpDelete]
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
