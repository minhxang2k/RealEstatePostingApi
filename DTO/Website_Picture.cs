namespace RealEstatePostingApi.DTO
{
    public class CreateWebsite_PictureDto
    {
        public string Image_Url { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int Display_Order { get; set; }
        public bool Is_Active { get; set; } = true;
    }

}
