namespace RealEstatePostingApi.Model
{
    public class Account
    {
        public int Id { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string Password_Hash { get; set; } = string.Empty;
        public string Auth_Provider { get; set; } = string.Empty;
        public int Provider_User_Id { get; set; } 
        public int Role_Id { get; set; }
        public bool Is_Active { get; set; }
        public DateTime Created_Date { get; set; } 
        public DateTime Updated_Date { get;set; }
    }
}
