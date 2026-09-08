namespace RealEstatePostingApi.DTO
{
    public class CreateUserDto
    {
        public int Account_Id { get; set; }
        public string Username { get; set; } = "admin";
    }
}
