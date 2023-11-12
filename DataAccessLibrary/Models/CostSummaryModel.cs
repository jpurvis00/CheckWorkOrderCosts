
namespace DataAccessLibrary.Models
{
    public class CostSummaryModel
    {
        public string Work_Order_No { get; set; }
        public int WO_Qty { get; set; }
        public string Transaction_Doc_Type { get; set; }
        public decimal Total_Mat_Cost { get; set; }
    }
}
