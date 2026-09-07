namespace RealEstatePostingApi.DTO
{
    public class CreateWardDto
    {
        public string Name { get; set; } = string.Empty;
        public int District_Id { get; set; }
    }

    public class UpdateWardDto
    {
        public string? Name { get; set; } 
        public int? District_Id { get; set; }
    }
}
