﻿
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
            
            var costDifference = Math.Abs(workOrderCompletionModel.Total_Labor_Cost) - laborCost;

            /* Giving a .50 cost difference threshold for rounding errors. */
            if(Math.Abs(costDifference) > .5M)
            {
                DisplayErrorMessage.DisplayMessage("Labor costs do not match.  Please fix and rerun.");
            }
        }

        public static void CheckMaterialCostsMatch(decimal actualMaterialCost, decimal actualIssuedMaterialCost, WorkOrderCompletionModel? workOrderCompletionModel)
        {
            if(Math.Abs(workOrderCompletionModel.Total_Mat_Cost) != actualMaterialCost || 
               Math.Abs(workOrderCompletionModel.Total_Mat_Cost) != actualIssuedMaterialCost)
            {
                DisplayErrorMessage.DisplayMessage("Material costs do not match.  Please fix and rerun.");
            }
        }

        public static void CheckOHCostsMatch(decimal actualIssuedOHCost, decimal timeCardOHCost, WorkOrderCompletionModel? workOrderCompletionModel)
        {
            var ohCost = actualIssuedOHCost + timeCardOHCost;

            var costDifference = Math.Abs(workOrderCompletionModel.Total_Foh_Cost) - ohCost;

            if(Math.Abs(costDifference) > .5M)
            {
                DisplayErrorMessage.DisplayMessage("OH costs do not match.  Please fix and rerun.");
            }
        }

        public static void CheckSubContractCostsMatch(decimal actualIssuedSubContractCost, decimal subContractingCost, WorkOrderCompletionModel? workOrderCompletionModel)
        {
            var subContractCost = actualIssuedSubContractCost + subContractingCost;

            if (subContractCost != Math.Round(Math.Abs(workOrderCompletionModel.Total_Sc_Cost), 4))
            {
                DisplayErrorMessage.DisplayMessage("Subcontracting costs do not match.  Please fix and rerun.");
            }
        }
    }
}