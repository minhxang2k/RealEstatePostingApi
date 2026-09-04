namespace RealEstatePostingApi.Model
{
    public class Post
    {
        public int Id { get; set; }
        public int Account_Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Price { get; set; }
        public int City_Id { get; set; }
        public int District_Id { get; set; }
        public int Ward_Id { get; set; }
        public int Street_Id { get; set; }
        public int Property_Type_Id { get; set; }
        public int Transaction_Type_Assignment_Id { get; set; }
        public bool Is_Recommended { get; set; }
        public int Status_Id { get; set; }
        public int View_Count { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
        public DateTime? Delete_Scheduled_At { get; set; }
    }
}
