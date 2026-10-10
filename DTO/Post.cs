namespace RealEstatePostingApi.DTO
{
    public class UploadPostDto
    {
        public int Account_Id { get; set; }
        public string Title { get; set; }
        public string Content  { get; set; }
        public string Address { get; set; }
        public  decimal Latitude { get; set; }
        public decimal  Longitude { get; set; }
        public int Price { get; set; }
        public int City_Id { get; set; }
        public int District_Id { get; set; }
        public int ward_Id { get; set; }
        public int street_Id { get; set; }
        public int Property_Type_Id { get; set; }
        public int transaction_Type_Assignment_Id { get; set; }
        public bool Is_Recommended { get; set; }
        public int Status_Id { get; set; }
    }
}
