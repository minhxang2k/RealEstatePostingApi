using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Model;
namespace RealEstatePostingApi.Context
{
    public class AppDbContext : DbContext
    {
        //public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Office_Location> Office_Location { get; set; }
        public virtual DbSet<Password_Reset> Password_Reset { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Post_Detail> Post_Detail { get; set; }
        public virtual DbSet<Post_Picture> Post_Picture { get; set; }
        public virtual DbSet<Post_Video> Post_Video { get; set; }
        public virtual DbSet<Property_Type> Property_Type { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Save_Listing> Save_Listing { get; set; }
        public virtual DbSet<Setting> Setting  { get; set; }
        public virtual DbSet<Status> Status  { get; set; }
        public virtual DbSet<Street> Street { get; set; }
        public virtual DbSet<Transaction_Type> Transaction_Type { get; set; }
        public virtual DbSet<Transaction_Type_Assignment> Transaction_Type_Assignments  { get; set; }
        public virtual DbSet<User> User  { get; set; }
        public virtual DbSet<Ward> Ward  { get; set; }
        public virtual DbSet<Website_Picture> Website_Picture { get; set; }
        public virtual DbSet<Website_Traffic> Website_Traffic { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(p => p.Latitude).HasPrecision(8, 6);
                entity.Property(p => p.Longitude).HasPrecision(9, 6);
            });

            modelBuilder.Entity<Office_Location>(entity =>
            {
                entity.Property(p => p.Latitude).HasPrecision(8, 6);
                entity.Property(p => p.Longitude).HasPrecision(9, 6);
            });

            modelBuilder.Entity<Post_Detail>(entity =>
            {
                entity.Property(p => p.Facade).HasPrecision(10, 2);
                entity.Property(p => p.Square).HasPrecision(10, 4);
            });
        }
    }

}
