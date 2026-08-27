using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
namespace RealEstatePostingApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Post> Post { get; set; }    

    }
}
