namespace RealEstatePostingApi.Model
{
    public class Contact
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Phone_Number { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
