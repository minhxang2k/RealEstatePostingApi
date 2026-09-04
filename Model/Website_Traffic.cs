namespace RealEstatePostingApi.Model
{
    public class Website_Traffic
    {
        public int Id { get; set; }
        public DateOnly Visit_Date { get; set; }
        public int Visit_Count { get; set; } = 0;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
