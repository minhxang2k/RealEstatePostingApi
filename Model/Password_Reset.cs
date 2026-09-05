namespace RealEstatePostingApi.Model
{
    public class Password_Reset
    {
        public int Id { get; set; }
        public int Account_Id { get; set; } // This is the account id from the Account table
        public string Code_Hash { get; set; } = string.Empty;
        public DateTime Expires_At { get; set; }
        public int Attempt_Count { get; set; }
        public DateTime Last_Sent_At { get; set; }
        public bool Is_Used { get; set; }
        public DateTime Created_Date { get; set; }

        // Navigation
        public virtual Account? Account { get; set; }
    }
}
