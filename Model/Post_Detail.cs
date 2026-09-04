namespace RealEstatePostingApi.Model
{
    public class Post_Detail
    {
        public int Id { get; set; }
        public int Post_Id { get; set; }
        public decimal Square {  get; set; }
        public int bathRoom { get; set; }
        public int BedRoom { get; set; }
        public string House_Direction { get; set; } = string.Empty;
        public decimal Facade { get; set; }
        public int Total_Floor { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
