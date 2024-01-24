using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class TimeCardDetailsWithSetupTeardownModel
    {
        public string Work_Order_No { get; set; }
        public string Tc_Reporting_Type { get; set; }
        public int Good_Qty { get; set; }
        public int Scrap_Qty { get; set; }
        public string Work_Center_No { get; set; }
        public decimal Total_Labor_Cost { get; set; }
        public decimal Total_Foh_Cost { get; set; }
    }
}
