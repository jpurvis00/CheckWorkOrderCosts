
using DataAccessLibrary.Models;

namespace CheckWOCostsLibrary
{
    public static class CheckIfValuesMatch
    {
        /* Math.Abs() gives us the value as a positive # even if the actual value is negative. The database stores the 
        * completion numbers as negative for some reason and all other numbers as positive.
        */

        public static void CheckLaborCostsMatch(decimal actualIssuedLaborCost, decimal timeCardLaborCost, WorkOrderCompletionModel? workOrderCompletionModel)
        {
            var laborCost = actualIssuedLaborCost + timeCardLaborCost;
            
            var costDifference = Math.Round(Math.Abs(workOrderCompletionModel.Total_Labor_Cost), 4) - laborCost;

            /* Giving a .50 cost difference threshold for rounding errors. */
            if(Math.Abs(costDifference) > .5M)
            {
                DisplayErrorMessage.DisplayMessage("Labor costs do not match.  Please fix and rerun.\n" +
                    $"laborCost: {laborCost}    workOrderCompletionModel.Total_Labor_Cost: {workOrderCompletionModel.Total_Labor_Cost}");
            }
        }

        public static void CheckMaterialCostsMatch(decimal actualMaterialCost, decimal actualIssuedMaterialCost, WorkOrderCompletionModel? workOrderCompletionModel)
        {
            if(Math.Round(Math.Abs(workOrderCompletionModel.Total_Mat_Cost), 4) != actualMaterialCost || 
               Math.Round(Math.Abs(workOrderCompletionModel.Total_Mat_Cost), 4) != actualIssuedMaterialCost)
            {
                DisplayErrorMessage.DisplayMessage("Material costs do not match.  Please fix and rerun.\n" +
                    $"actualMaterialCost: {actualMaterialCost}  actualIssuedMaterialCost: {actualIssuedMaterialCost}    workOrderComletionMode.Total_Mat_Cost: {workOrderCompletionModel.Total_Mat_Cost}");
            }
        }

        public static void CheckOHCostsMatch(decimal actualIssuedOHCost, decimal timeCardOHCost, WorkOrderCompletionModel? workOrderCompletionModel)
        {
            var ohCost = actualIssuedOHCost + timeCardOHCost;

            var costDifference = Math.Round(Math.Abs(workOrderCompletionModel.Total_Foh_Cost), 4) - ohCost;

            if(Math.Abs(costDifference) > .5M)
            {
                DisplayErrorMessage.DisplayMessage("OH costs do not match.  Please fix and rerun.\n" +
                    $"ohCost: {ohCost}  workOrderCompletionModel.Total_Foh_Cost: {workOrderCompletionModel.Total_Foh_Cost}");
            }
        }

        public static void CheckSubContractCostsMatch(decimal actualIssuedSubContractCost, decimal subContractingCost, WorkOrderCompletionModel? workOrderCompletionModel)
        {
            var subContractCost = actualIssuedSubContractCost + subContractingCost;

            if (subContractCost != Math.Round(Math.Abs(workOrderCompletionModel.Total_Sc_Cost), 4))
            {
                DisplayErrorMessage.DisplayMessage("Subcontracting costs do not match.  Please fix and rerun.\n" +
                    $"subContractCost: {subContractCost}    workOrderCompletionModel.Total_Sc_cost: {workOrderCompletionModel.Total_Sc_Cost}");
            }
        }
    }
}
