namespace RealEstatePostingApi.Model
{
    public class Street
    {
        public int Id { get; set; }
        public int Ward_Id { get; set; } // This is the ward id from the Ward table
        public string Name { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public virtual Ward? Ward { get; set; }
        public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
    }
}
