namespace RealEstatePostingApi.DTO
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CreateAccountDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Auth_Provider { get; set; } = string.Empty;
        public int Role_Id { get; set; }
        public string Sex { get; set; } = string.Empty;
        public int? Provider_User_Id { get; set; } = 1; // This is the user id from the auth provider (e.g., Google, Facebook, etc.)
        public int Role { get; set; } = 1; // Default role is Admin
    }

    public class AccountResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Username { get; set; }
        public string? Sex { get; set; }
    }

    public class AccountLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Username { get; set; }
        public string? Sex { get; set; }
    }
}
