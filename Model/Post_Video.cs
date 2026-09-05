namespace RealEstatePostingApi.Model
{
    public class Post_Video
    {
        public int Id { get; set; }
        public int Post_Id { get; set; } // This is the post id from the Post table
        public string Video_Url { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public Post? Post { get; set; }
    }
}
