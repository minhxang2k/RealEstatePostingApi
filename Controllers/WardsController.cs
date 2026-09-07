
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;
using Microsoft.Identity.Client;

[ApiController]
[Route("[controller]")]
public class WardsController : ControllerBase
{
    private readonly AppDbContext _context;

    public WardsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: WARDS
    [HttpGet]
    public async Task<ActionResult<Ward>> Index()    
    {
        try
        {
            var wards = await _context.Ward.ToListAsync();
            if (wards == null || wards.Count == 0)
            {
                return NotFound("No wards found.");
            }
            return Ok(wards);
        }
        catch(Exception e)
        {
            throw new Exception($"there an error occurred while listing ward list: {e.Message}");
        }
       
    }

    // GET: WARDS/Details/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Ward>> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ward = await _context.Ward
            .FirstOrDefaultAsync(m => m.Id == id);
        if (ward == null)
        {
            return NotFound("can not find because its does not exists");
        }

        return Ok(ward);
    }

    // GET: WARDS/Create
    [HttpPost]
    public async Task<ActionResult<Ward>> Create([FromBody] CreateWardDto dto)
    {
        try
        {
            var ward = new Ward
            {
                District_Id = dto.District_Id,
                Name = dto.Name,
                Created_Date = DateTime.Now,
            };
            _context.Ward.Add(ward);
            await _context.SaveChangesAsync();
            return Ok(ward);
        }
        catch (Exception ex)
        {   
            throw new Exception("an error occurred while creating the ward: " + ex.Message);
        }
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWardDto dto)
    {
        try
        {
            var ward = await _context.Ward.FindAsync(id);
            if(ward == null)
            {
                return NotFound("can not find because its does not exists");
            }

            if (dto.Name != null)
            {
                ward.Name = dto.Name;
            }

            if (dto.District_Id.HasValue)
            {
                ward.District_Id = dto.District_Id.Value;
            }

            ward.Updated_Date = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(ward);
        }
        catch (Exception e)
        {
            throw new Exception($"an error occurred while updating ward: {e.Message}");
        }


    }
    // POST: WARDS/Delete/5
    [HttpDelete]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var ward = await _context.Ward.FindAsync(id);
        if (ward != null)
        {
            _context.Ward.Remove(ward);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool WardExists(int? id)
    {
        return _context.Ward.Any(e => e.Id == id);
    }
}
