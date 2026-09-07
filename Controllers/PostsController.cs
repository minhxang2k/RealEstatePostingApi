
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
using RealEstatePostingApi.Context;

[ApiController]
[Route("[controller]")]
public class PostsController : Controller
{
    private readonly RealEstatePostingApi.Context.AppDbContext _context;

    public PostsController(RealEstatePostingApi.Context.AppDbContext context)
    {
        _context = context;
    }

    // GET: POSTS
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Post.ToListAsync());
    }

}
//    // GET: POSTS/Details/5
//    public async Task<IActionResult> Details(int? id)
//    {
//        if (id == null)
//        {
//            return NotFound();
//        }

//        var post = await _context.Post
//            .FirstOrDefaultAsync(m => m.Id == id);
//        if (post == null)
//        {
//            return NotFound();
//        }

//        return View(post);
//    }

//    // GET: POSTS/Create
//    public IActionResult Create()
//    {
//        return View();
//    }

//    // POST: POSTS/Create
//    // To protect from overposting attacks, enable the specific properties you want to bind to.
//    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Create([Bind("Id,Account_Id,Title,Content,Address,Latitude,Longitude,Price,City_Id,District_Id,Ward_Id,Street_Id,Property_Type_Id,Transaction_Type_Assignment_Id,Is_Recommended,Status_Id,View_Count,Created_Date,Updated_Date,Delete_Scheduled_At")] Post post)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.Add(post);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }
//        return View(post);
//    }

//    // GET: POSTS/Edit/5
//    public async Task<IActionResult> Edit(int? id)
//    {
//        if (id == null)
//        {
//            return NotFound();
//        }

//        var post = await _context.Post.FindAsync(id);
//        if (post == null)
//        {
//            return NotFound();
//        }
//        return View(post);
//    }

//    // POST: POSTS/Edit/5
//    // To protect from overposting attacks, enable the specific properties you want to bind to.
//    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(int? id, [Bind("Id,Account_Id,Title,Content,Address,Latitude,Longitude,Price,City_Id,District_Id,Ward_Id,Street_Id,Property_Type_Id,Transaction_Type_Assignment_Id,Is_Recommended,Status_Id,View_Count,Created_Date,Updated_Date,Delete_Scheduled_At")] Post post)
//    {
//        if (id != post.Id)
//        {
//            return NotFound();
//        }

//        if (ModelState.IsValid)
//        {
//            try
//            {
//                _context.Update(post);
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!PostExists(post.Id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return RedirectToAction(nameof(Index));
//        }
//        return View(post);
//    }

//    // GET: POSTS/Delete/5
//    public async Task<IActionResult> Delete(int? id)
//    {
//        if (id == null)
//        {
//            return NotFound();
//        }

//        var post = await _context.Post
//            .FirstOrDefaultAsync(m => m.Id == id);
//        if (post == null)
//        {
//            return NotFound();
//        }

//        return View(post);
//    }

//    // POST: POSTS/Delete/5
//    [HttpPost, ActionName("Delete")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> DeleteConfirmed(int? id)
//    {
//        var post = await _context.Post.FindAsync(id);
//        if (post != null)
//        {
//            _context.Post.Remove(post);
//        }

//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(Index));
//    }

//    private bool PostExists(int? id)
//    {
//        return _context.Post.Any(e => e.Id == id);
//    }
//}
