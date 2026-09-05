namespace RealEstatePostingApi.Model
{
    public class Transaction_Type_Assignment
    {
        public int Id { get; set; }
        public int Transaction_Type_Id { get; set; } // This is the transaction type id from the Transaction_Type table
        public int Contact_Id { get; set; }// This is the contact id from the Contact table
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        //navigation
        public virtual Transaction_Type? Transaction_Type { get; set; }
        public virtual Contact? Contact { get; set; }
        public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
    }
}
