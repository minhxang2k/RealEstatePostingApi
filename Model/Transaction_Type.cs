namespace RealEstatePostingApi.Model
{
    public class Transaction_Type
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public virtual ICollection<Transaction_Type_Assignment>? Transaction_Type_Assignments { get; set; } = new List<Transaction_Type_Assignment>();
    }
}
