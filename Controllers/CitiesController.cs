
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;
using RealEstatePostingApi.Model;

[ApiController]
[Route("[controller]")]
public class CitiesController : Controller
{
    private readonly RealEstatePostingApi.Context.AppDbContext _context;


    public CitiesController(RealEstatePostingApi.Context.AppDbContext context)
    {
        _context = context;
    }

    // GET: CITYS
    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        var cities = await _context.City.ToListAsync();
        return Ok(cities);
    }

    [HttpGet("{cityId}/districts")]
    public async Task<ActionResult<IEnumerable<District>>> GetDistrictsByCity(int cityId)
    {
        try
        {
            var cityExists = await _context.City
            .AnyAsync(c => c.Id == cityId);

            if (!cityExists)
            {
                return NotFound("City not found.");
            }

            var districts = await _context.District
                .Where(d => d.City_Id == cityId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(districts);
        }catch(Exception e)
        {
            throw new Exception($"An error occurred while finding district by city id: {e.Message}");
        }
        
    }
    // POST: CITYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<City>> Create([FromBody] CreateCityDTO dto)
    {
        try
        {
            var city = new City
            {
                Name = dto.Name,
                Created_Date = DateTime.Now,
                Updated_Date = null
            };
            _context.City.Add(city);
            await _context.SaveChangesAsync();

            return Ok(city);
            //return CreatedAtAction(
            //      nameof(GetCities),
            //      new { id = city.Id },
            //      city
            //  );
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating the city: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<City>> Update(int? id, UpdateCityDTO updateCityDTO)
    {
        try
        {
            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            city.Name = updateCityDTO.Name;
            city.Updated_Date = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(city);
            }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while updating the city: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        try
        {
            var city = await _context.City.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            _context.City.Remove(city);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"An error occurred while deleting the city: {ex.Message}");
        }
       
    }

}
