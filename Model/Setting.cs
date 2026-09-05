namespace RealEstatePostingApi.Model
{
    public class Setting
    {
        public int Id { get; set; }
        public int Account_Id { get; set; } // This is the account id from the Account table
        public string Background_Theme { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public virtual Account? Account { get; set; }
    }
}
