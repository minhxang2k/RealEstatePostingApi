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

                entity.HasOne(p => p.Account)
                    .WithMany(a => a.Posts)
                    .HasForeignKey(p => p.Account_Id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.City)
                    .WithMany(c => c.Posts)
                    .HasForeignKey(p => p.City_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p => p.District)
                    .WithMany(d => d.Posts)
                    .HasForeignKey(p => p.District_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p => p.Ward)
                    .WithMany(w => w.Posts)
                    .HasForeignKey(p => p.Ward_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p => p.Street)
                    .WithMany(s => s.Posts)
                    .HasForeignKey(p => p.Street_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p => p.Property_Type)
                    .WithMany(pt => pt.Posts)
                    .HasForeignKey(p => p.Property_Type_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p => p.Transaction_Type_Assignment)
                    .WithMany(tt => tt.Posts)
                    .HasForeignKey(p => p.Transaction_Type_Assignment_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p => p.Status)
                    .WithMany(s => s.Posts)
                    .HasForeignKey(p => p.Status_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p=>p.Post_Details)
                    .WithOne(pd => pd.Post)
                    .HasForeignKey<Post_Detail>(pd => pd.Post_Id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Post_Pictures)
                    .WithOne(pp => pp.Post)
                    .HasForeignKey(pp => pp.Post_Id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Post_Video)
                    .WithOne(pp => pp.Post)
                    .HasForeignKey<Post_Video>(pp => pp.Post_Id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Save_Listings)
                    .WithOne(p => p.Post)
                    .HasForeignKey(p => p.Post_Id)
                    .OnDelete(DeleteBehavior.Cascade);
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
