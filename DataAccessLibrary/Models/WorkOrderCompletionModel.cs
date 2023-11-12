
namespace DataAccessLibrary.Models
{
    public class WorkOrderCompletionModel
    {
        public string Work_Order_No { get; set; }
        public int WO_Qty { get; set; }
        public string Transaction_Doc_Type { get; set; }
        public int Transaction_Doc_No { get; set; }
        public int Transaction_Doc_Line_No { get; set; }
        public int Transaction_Qty_Buom { get; set; }
        public decimal Total_Mat_Cost { get; set; }
        public decimal Total_Labor_Cost { get; set; }
        public decimal Total_Foh_Cost { get; set; }
        public decimal Total_Sc_Cost { get; set; }
        public string Lot_No { get; set; }
    }
}
