namespace RealEstatePostingApi.Model
{
    public class District
    {
        public int Id { get; set; }
        public int City_Id { get; set; } // This is the city id from the City table (e.g., Ho Chi Minh City, etc.)
        public string Name { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public virtual City? City { get; set; }
        public virtual ICollection<Ward>? Wards { get; set; }
        public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();

    }
}
