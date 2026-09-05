namespace RealEstatePostingApi.Model
{
    public class Property_Type
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
    }
}
