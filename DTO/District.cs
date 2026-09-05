namespace RealEstatePostingApi.DTO
{
    public class CreateDistrictDto
    {
        public int City_Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class  UpdateDistrictDto
    {
        public int? City_Id { get; set; }
        public string? Name { get; set; }
    }
}
