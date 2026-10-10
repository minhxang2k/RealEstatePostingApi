namespace RealEstatePostingApi.DTO
{
    public class UploadPost_DetailDto
    {
        public decimal Square { get; set; }
        public int Bedroom { get; set; }
        public int Bathroom { get; set; }
        public string House_Direction { get; set; }
        public decimal Facade{ get; set; }
        public int total_Floor { get; set; }
    }
}
