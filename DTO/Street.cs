namespace RealEstatePostingApi.DTO
{
    public class CreateStreetDto
    {
        public string Name { get; set; } = string.Empty;
        public int Ward_Id { get; set; }
    }

    public class  UpdateStreetDto
    {
        public string? Name { get; set; }
        public int? Ward_Id { get; set; }
    }
}
