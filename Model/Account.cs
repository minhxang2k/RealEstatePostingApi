namespace RealEstatePostingApi.Model
{
    public class Account
    {
        public int Id { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string Password_Hash { get; set; } = string.Empty;
        public string Auth_Provider { get; set; } = string.Empty;
        public int? Provider_User_Id { get; set; } // This is the user id from the auth provider (e.g., Google, Facebook, etc.)
        public int Role_Id { get; set; } // This is the role id from the Role table (e.g., Admin, User, etc.)
        public bool Is_Active { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        // Navigation
        public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
        public virtual User? User { get; set; }
        public virtual Roles? Role { get; set; } 
        public virtual ICollection<Password_Reset>? Password_Resets { get; set; } = new List<Password_Reset>();
        public virtual Setting? Setting { get; set; }
    }
}
