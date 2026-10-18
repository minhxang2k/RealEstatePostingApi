namespace RealEstatePostingApi.Model
{
    public class Post
    {
        public int Id { get; set; }
        public int Account_Id { get; set; }//foreign key to Account
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long Price { get; set; }
        public int City_Id { get; set; } //foreign key to City
        public int District_Id { get; set; } //foreign key to District
        public int Ward_Id { get; set; } //foreign key to Ward
        public int Street_Id { get; set; } //foreign key to Street
        public int Property_Type_Id { get; set; } //foreign key to Property_Type
        public int Transaction_Type_Assignment_Id { get; set; } //foreign key to Transaction_Type_Assignment
        public bool Is_Recommended { get; set; }
        public int Status_Id { get; set; }//foreign key to Status
        public int View_Count { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
        public DateTime? Delete_Scheduled_At { get; set; }

        //navigation properties
        public virtual Account? Account { get; set; }
        public virtual City? City { get; set; }
        public virtual District? District { get; set; }
        public virtual Ward? Ward { get; set; }
        public virtual Street? Street { get; set; }
        public virtual Property_Type? Property_Type { get; set; }
        public virtual Transaction_Type_Assignment? Transaction_Type_Assignment { get; set; }
        public virtual Status? Status { get; set; } = null;
        public virtual Post_Detail? Post_Details { get; set; }
        public virtual ICollection<Post_Picture>? Post_Pictures { get; set; }
        public virtual Post_Video? Post_Video { get; set; } 
        public virtual ICollection<Save_Listing>? Save_Listings { get; set; } = new List<Save_Listing>();
    }
}
