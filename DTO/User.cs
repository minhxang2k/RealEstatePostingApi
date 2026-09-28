namespace RealEstatePostingApi.DTO
{
    public class CreateUserDto
    {
        public int Account_Id { get; set; }
        public string Username { get; set; } = "admin";

        public string Phone_Number { get; set; } = string.Empty;

        public string Sex { get; set; } = string.Empty;
    }

    public class UserUpdateDTO
    {
        public string? UserName { get; set; }
        public string? ImageUrl { get; set; }

        public string? PhoneNumber { get; set; } 
        public string? Sex { get; set; } 
    }
}
