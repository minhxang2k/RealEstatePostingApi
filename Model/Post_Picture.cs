namespace RealEstatePostingApi.Model
{
    public class Post_Picture
    {
        public int Id { get; set; }
        public int Post_Id { get; set; } // This is the post id from the Post table
        public string Image_Url { get; set; } = string.Empty;
        public string Picture_Type { get; set; } = string.Empty;
        public bool Is_Thumbnail { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public Post? Post { get; set; }
    }
}
