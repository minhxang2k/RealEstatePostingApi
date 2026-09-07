namespace RealEstatePostingApi.Model
{
    public class Roles
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public virtual ICollection<Account>? Accounts { get; set; } = new List<Account>();
    }
}
