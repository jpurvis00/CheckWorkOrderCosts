using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class IssuedMaterialCostsVsItemStandardCostsModel
    {
        public string Item_No { get; set; }
        public string Description { get; set; }
        public string Lot_No { get; set; }
        public int Transaction_Qty { get; set; }
        public string Transaction_Uom { get; set; }
        public decimal WO_Issues_Unit_Price { get; set; }
        public decimal WO_Issues_Mat_Cost { get; set; }  
        public decimal WO_Issues_Labor_Cost { get; set; }  
        public decimal WO_Issues_OH_Cost { get; set; }  
        public decimal WO_Issues_Sub_Cntg_Cost { get; set; }
        public decimal Item_Standard_Cost { get; set; }
        public decimal Item_Standard_Mat_Cost { get; set; }
        public decimal Item_Standard_Labor_Cost { get; set; }
        public decimal Item_Standard_Fixed_OH_Cost { get; set; }
        public decimal Item_Standard_Sub_Cntg_Cost { get; set; }
        public DateTime Effective_Till { get; set; }
    }
}
