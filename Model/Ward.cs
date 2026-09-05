namespace RealEstatePostingApi.Model
{
    public class Ward
    {
        public int Id { get; set; }
        public int District_Id { get; set; } // This is the district id from the District table
        public string Name { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public virtual District? District { get; set; }
        public virtual ICollection<Street>? Streets { get; set; } = new List<Street>();
        public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
    }
}
