namespace RealEstatePostingApi.Model
{
    public class Save_Listing
    {
        public int Id { get; set; }
        public int User_Id { get; set; } // This is the user id from the User table
        public int Post_Id { get; set; } // This is the post id from the Post table
        public DateTime Created_Date { get; set; }

        // Navigation
        public virtual User? User { get; set; }
        public virtual Post? Post { get; set; }

    }
}
