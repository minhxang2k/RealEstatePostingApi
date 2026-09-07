
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;
using RealEstatePostingApi.DTO;

[ApiController]
[Route("[controller]")]
public class RolesController : Controller
{
    private readonly RealEstatePostingApi.Context.AppDbContext _context;


    public RolesController(RealEstatePostingApi.Context.AppDbContext context)
    {
        _context = context;
    }

    // GET: ROLES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
    {
        var roles = await _context.Role.ToListAsync();
        return Ok(roles);
    }

    [HttpGet("{id}/accounts")]
    public async Task<ActionResult<IEnumerable<Account>>> GetAccountsByRole(int id)
    {
        try
        {
            var roleExists = await _context.Role
            .AnyAsync(r => r.Id == id);

            if (!roleExists)
            {
                return NotFound("Role not found.");
            }

            var accounts = await _context.Account
                .Where(a => a.Role_Id == id)
                .AsNoTracking()
                .ToListAsync();

            return Ok(accounts);
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while finding accounts by role id: {e.Message}");
        }

    }
    // POST: CITYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<ActionResult<Roles>> Create([FromBody] CreateRoleDTO dto)
    {
        try
        {
            var role = new Roles
            {
                Role = dto.Role,
                Created_Date = DateTime.Now,
                Updated_Date = null
            };
            
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            return Ok(role);
            //return CreatedAtAction(
            //      nameof(GetRoles),
            //      new { id = role.Id },
            //      role
            //  );
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating the Role: {ex.Message}");
        }
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Roles>> Update(int? id, UpdateRoleDTO updateRoleDTO)
    {
        try
        {
            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            if(updateRoleDTO.Role != null)
            {
                role.Role = updateRoleDTO.Role;
            }
            role.Updated_Date = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(role);
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while updating the role: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var role = await _context.Role.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            _context.Role.Remove(role);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"An error occurred while deleting the role: {ex.Message}");
        }

    }

}
