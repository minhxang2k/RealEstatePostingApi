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
        public virtual DbSet<Roles> Role { get; set; }
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
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(a => a.Email).HasMaxLength(255);
                entity.Property(a => a.Password_Hash).HasMaxLength(255);
                entity.Property(a => a.Auth_Provider).HasMaxLength(50);
                entity.HasMany(p => p.Posts)
                    .WithOne(a => a.Account)
                    .HasForeignKey(p => p.Account_Id)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(a => a.Password_Resets)
                    .WithOne(pr => pr.Account)
                    .HasForeignKey(pr => pr.Account_Id)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(a => a.Role)
                    .WithMany(r => r.Accounts)
                    .HasForeignKey(a => a.Role_Id)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(a => a.User)
                    .WithOne(u => u.Account)
                    .HasForeignKey<User>(u => u.Account_Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(a => a.Setting)
                    .WithOne(s => s.Account)
                    .HasForeignKey<Setting>(s => s.Account_Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Save_Listing>(entity =>
            {
                entity.HasOne(sl => sl.User)
                    .WithMany(u => u.Save_Listings)
                    .HasForeignKey(sl => sl.User_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(sl => sl.Post)
                    .WithMany(p => p.Save_Listings)
                    .HasForeignKey(sl => sl.Post_Id)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(p => p.Latitude).HasPrecision(8, 6);
                entity.Property(p => p.Longitude).HasPrecision(9, 6);

                entity.HasOne(p => p.Account)
                    .WithMany(a => a.Posts)
                    .HasForeignKey(p => p.Account_Id)
                    .OnDelete(DeleteBehavior.NoAction);

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

                entity.HasOne(p => p.Post_Details)
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
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(c => c.Name).HasMaxLength(255);

                entity.HasMany(p => p.Posts)
                    .WithOne(c => c.City)
                    .HasForeignKey(p => p.City_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(c => c.Districts)
                    .WithOne(d => d.City)
                    .HasForeignKey(d => d.City_Id)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasOne(p => p.City)
                    .WithMany(c => c.Districts)
                    .HasForeignKey(p => p.City_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(p => p.Posts)
                    .WithOne(d => d.District)
                    .HasForeignKey(p => p.District_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(d => d.Wards)
                    .WithOne(w => w.District)
                    .HasForeignKey(w => w.District_Id)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.HasMany(p => p.Posts)
                    .WithOne(w => w.Ward)
                    .HasForeignKey(p => p.Ward_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(w => w.District)
                    .WithMany(d => d.Wards)
                    .HasForeignKey(w => w.District_Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(w => w.Streets)
                    .WithOne(s => s.Ward)
                    .HasForeignKey(s => s.Ward_Id)
                    .OnDelete(DeleteBehavior.NoAction);
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
