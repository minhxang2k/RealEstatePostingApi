namespace RealEstatePostingApi.Model
{
    public class Post_Video
    {
        public int Id { get; set; }
        public int Post_Id { get; set; }
        public string Video_Url { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
