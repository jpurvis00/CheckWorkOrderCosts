
using DataAccessLibrary.Models;

namespace CheckWOCostsLibrary
{
    public static class CostSummaryActualMaterialCost
    {
        public static decimal GetCostSummaryActualMaterialCost(List<CostSummaryModel> costSummary)
        {
            decimal materialCost = 0;

            foreach(var cost in costSummary)
            {
                materialCost += cost.Total_Mat_Cost;              
            }

            return decimal.Round(materialCost, 4);
        }
    }
}
