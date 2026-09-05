namespace RealEstatePostingApi.Model
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public virtual ICollection<District>? Districts { get; set; } = new List<District>();
        public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
    }
}
