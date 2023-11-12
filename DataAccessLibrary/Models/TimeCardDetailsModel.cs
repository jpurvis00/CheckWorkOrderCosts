
namespace DataAccessLibrary.Models
{
    public class TimeCardDetailsModel
    {
        public string Work_Order_No { get; set; }
        public int WO_Qty { get; set; }
        public string Transaction_Doc_Type { get; set; }
        public int Transaction_Doc_No { get; set; }
        public int Transaction_Doc_Line_No { get; set; }
        public string Employee_Number { get; set; }
        public string Operation_Code { get; set; }
        public string Work_Center_No { get; set; }
        public int Good_Qty{ get; set; }
        public int Scrap_Qty{ get; set; }
        public int Qa_Hold_Qty{ get; set; }
        public decimal Total_Labor_Cost { get; set; }
        public decimal Total_Foh_Cost { get; set; }
    }
}
