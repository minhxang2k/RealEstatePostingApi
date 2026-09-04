namespace RealEstatePostingApi.Model
{
    public class User
    {
        public int Id { get; set; }
        public int Account_Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Phone_Number { get; set; } = string.Empty;
        public string Avatar_Url { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
