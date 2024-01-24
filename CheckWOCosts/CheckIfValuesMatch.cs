
using Dapper;
using DataAccessLibrary.Models;

namespace CheckWOCostsLibrary
{
    public static class CheckIfValuesMatch
    {
        /* Math.Abs() gives us the value as a positive # even if the actual value is negative. The database stores the 
         * completion numbers as negative for some reason as well as other values. I can't be sure what is being returned
         * from the db.
         */

        public static void CheckIssuedMaterialCostsVsItemStandardCosts
            (List<IssuedMaterialCostsVsItemStandardCostsModel> issuedMaterialCostsVsItemStandardCosts)
        {
            decimal costSwingPercentage = .05M;

            foreach (var item in issuedMaterialCostsVsItemStandardCosts)
            {
                /* A 5% cost swing was decided by Kimberly, created the method below so we only have to change the percentage
                 * in one place in the future and other methods can use it if necessary. 
                 */
                (decimal itemStandardCostLowerRange, decimal itemStandardCostUpperRange) = 
                    FindCostLowerUpperRangeByPercent(item.Item_Standard_Mat_Cost, costSwingPercentage);

                if (item.WO_Issues_Mat_Cost < itemStandardCostLowerRange || item.WO_Issues_Mat_Cost > itemStandardCostUpperRange)
                {
                    Console.WriteLine($"\nThere is a cost difference greater than {Math.Round((costSwingPercentage * 100), 0)}% on an issued material. " +
                        $"\n  Item: {item.Item_No} {item.Description}" +
                        $"\n  Item Standard Material Cost: {item.Item_Standard_Mat_Cost} " +
                        $"\n  Issued Material Cost: {item.WO_Issues_Mat_Cost}:      Lot No: {item.Lot_No}");
                }
            }
        }

        private static (decimal, decimal) FindCostLowerUpperRangeByPercent(decimal itemStandardMatCost, decimal rangePercentage)
        {
                decimal fivePercentCostSwing = Math.Round(itemStandardMatCost * .05M, 4);
                decimal itemStandardCostLowerRange = Math.Round(itemStandardMatCost - fivePercentCostSwing, 4);
                decimal itemStandardCostUpperRange = Math.Round(itemStandardMatCost + fivePercentCostSwing, 4);

            return (itemStandardCostLowerRange, itemStandardCostUpperRange);
        }


        public static void CheckLaborCostsMatch(decimal actualIssuedLaborCost, 
            decimal timeCardLaborCost, List<WorkOrderCompletionModel> workOrderCompletionModel)
        {
            decimal allCompletionLaborCosts = 0M;
            var laborCost = actualIssuedLaborCost + timeCardLaborCost;
           
            foreach(var wo in  workOrderCompletionModel)
            {
                allCompletionLaborCosts += Math.Round(Math.Abs(wo.Total_Labor_Cost), 4);
            }
            
            var costDifference = allCompletionLaborCosts - laborCost;

            /* Giving a .50 cost difference threshold for rounding errors. */
            if(Math.Abs(costDifference) > .5M)
            {
                DisplayErrorMessage.DisplayMessage("Labor costs do not match.  Please fix and rerun.\n" +
                    $"laborCost: {laborCost}    workOrderCompletionModel.Total_Labor_Cost: {allCompletionLaborCosts}");
            }
        }

        public static void CheckMaterialCostsMatch(decimal actualMaterialCost, 
            decimal actualIssuedMaterialCost, List<WorkOrderCompletionModel> workOrderCompletionModel)
        {
            decimal allCompletionMaterialCosts = 0M;

            foreach(var wo in workOrderCompletionModel)
            {
                allCompletionMaterialCosts += Math.Round(Math.Abs(wo.Total_Mat_Cost), 4);
            }
            
            var actMaterialCostDifference =  allCompletionMaterialCosts - actualMaterialCost;
            var actIssuedMaterialCostDifference = allCompletionMaterialCosts - actualIssuedMaterialCost;

            if (Math.Abs(actMaterialCostDifference) > .5M ||
                Math.Abs(actIssuedMaterialCostDifference) > .5M)
            {
                DisplayErrorMessage.DisplayMessage("Material costs do not match.  Please fix and rerun.\n" +
                    $"actualMaterialCost: {actualMaterialCost}  actualIssuedMaterialCost: {actualIssuedMaterialCost}    " +
                    $"workOrderCompletionMode.Total_Mat_Cost: {allCompletionMaterialCosts}");
            }
        }

        public static void CheckOHCostsMatch(decimal actualIssuedOHCost, 
            decimal timeCardOHCost, List<WorkOrderCompletionModel> workOrderCompletionModel)
        {
            decimal allCompletionOHCosts = 0M;

            foreach(var wo in workOrderCompletionModel)
            {
                allCompletionOHCosts += Math.Round(Math.Abs(wo.Total_Foh_Cost), 4);
            }

            var ohCost = actualIssuedOHCost + timeCardOHCost;

            var costDifference = allCompletionOHCosts - ohCost;
            
            if(Math.Abs(costDifference) > .5M)
            {
                DisplayErrorMessage.DisplayMessage("OH costs do not match.  Please fix and rerun.\n" +
                    $"ohCost: {ohCost}  workOrderCompletionModel.Total_Foh_Cost: {allCompletionOHCosts}");
            }
        }

        public static void CheckSubContractCostsMatch(decimal actualIssuedSubContractCost, 
            decimal subContractingCost, List<WorkOrderCompletionModel> workOrderCompletionModel)
        {
            decimal allCompletionSubContractCosts = 0M;

            foreach (var wo in workOrderCompletionModel)
            {
                allCompletionSubContractCosts += Math.Round(Math.Abs(wo.Total_Sc_Cost), 4);
            }
            var subContractCost = actualIssuedSubContractCost + subContractingCost;
            var costDifference = allCompletionSubContractCosts - subContractCost;

            if (Math.Abs(costDifference) > .5M)
            {
                DisplayErrorMessage.DisplayMessage("Subcontracting costs do not match.  Please fix and rerun.\n" +
                    $"subContractCost: {subContractCost}    workOrderCompletionModel.Total_Sc_cost: {allCompletionSubContractCosts}");
            }
        }
    }
}
