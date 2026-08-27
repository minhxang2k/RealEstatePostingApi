namespace RealEstatePostingApi.Model
{
    public class Post_Picture
    {
        public int Id { get; set; }
        public int Post_Id { get; set; }
        public string Image_Url { get; set; } = string.Empty;
        public string Picture_Type { get; set; }
        public bool Is_Thumbnail { get; set; }
        public DateTime Created_Date { get; set; } 
        public DateTime Updated_Date { get; set; }
    }
}
