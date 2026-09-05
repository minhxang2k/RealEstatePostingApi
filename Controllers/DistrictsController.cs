
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;

[ApiController]
[Route("[controller]")]
public class DistrictsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DistrictsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: DISTRICTS
    [HttpGet]
    public async Task<ActionResult<District>> Index()    
    {
        return Ok(await _context.District.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<District>> SelectDistrictByCityId(int id)
    {
        try
        {
            var district = await _context.District.FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }
            return Ok(district);
        }catch(Exception e)
        {
            throw new Exception(e.Message);
        }
       
    }

    // GET: DISTRICTS/Create
    [HttpPost]
    public async Task<ActionResult<District>> Create([FromBody] CreateDistrictDto dto)
    {
        var district = new District
        {
            City_Id = dto.City_Id,
            Name = dto.Name,
            Created_Date = DateTime.Now
        };

        _context.District.Add(district);
        await _context.SaveChangesAsync();

        return Ok(district);
    }

    // POST: DISTRICTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

    // GET: DISTRICTS/Edit/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> Edit(int? id, [FromBody] UpdateDistrictDto dto)
    {
        try
        {
            var district = await _context.District.FindAsync(id);

            if (district == null)
            {
                return NotFound("District not found.");
            }

            //update the district properties based on the provided DTO

            //update only the properties that are not null in the DTO
            if (dto.Name != null)
            {
                district.Name = dto.Name;
            }

            //update the City_Id only if it is provided in the DTO
            if (dto.City_Id.HasValue)
            {
                var cityExists = await _context.City
                    .AnyAsync(c => c.Id == dto.City_Id.Value);

                if (!cityExists)
                {
                    return BadRequest("City does not exist.");
                }

                district.City_Id = dto.City_Id.Value;
            }
            district.Updated_Date = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(district);
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while updating district: {e.Message}");
        }
    }

    // POST: DISTRICTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    
     [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDistrict(int id)
    {
        try
        {
            var district = await _context.District.FindAsync(id);

            if (district == null)
            {
                return NotFound();
            }

            _context.District.Remove(district);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    }
    // POST: DISTRICTS/Delete/5
   

    private bool DistrictExists(int? id)
    {
        return _context.District.Any(e => e.Id == id);
    }
}
