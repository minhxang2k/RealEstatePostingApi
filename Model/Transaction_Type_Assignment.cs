namespace RealEstatePostingApi.Model
{
    public class Transaction_Type_Assignment
    {
        public int Id { get; set; }
        public int Transaction_Type_Id { get; set; }
        public int Contact_Id { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
