namespace RealEstatePostingApi.Model
{
    public class District
    {
        public int Id { get; set; }
        public int City_Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
