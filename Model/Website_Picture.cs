namespace RealEstatePostingApi.Model
{
    public class Website_Picture
    {
        public int Id { get; set; }
        public string Image_Url { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int Website_Order { get; set; }
        public bool Is_Active { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
