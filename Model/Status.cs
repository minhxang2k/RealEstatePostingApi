namespace RealEstatePostingApi.Model
{
    public class Status
    {
        public int Id { get; set; }
        public string status { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
