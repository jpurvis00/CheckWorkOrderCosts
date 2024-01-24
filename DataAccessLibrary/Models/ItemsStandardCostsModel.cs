using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class ItemsStandardCostsModel
    {
        public string Item_No { get; set; }
        public decimal Standard_Cost { get; set; }
        public DateOnly Effective_From { get; set; }
        public DateOnly Effective_Till { get; set; }
    }
}
