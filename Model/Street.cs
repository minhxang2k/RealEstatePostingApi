namespace RealEstatePostingApi.Model
{
    public class Street
    {
        public int Id { get; set; }
        public int Ward_Id { get; set; }
        public string Name { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
