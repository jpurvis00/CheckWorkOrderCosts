
using DataAccessLibrary.Models;

namespace CheckWOCostsLibrary
{
    public static class WorkOrderIssuesWithCost
    {
        public static (decimal, decimal, decimal, decimal) GetWorkOrderIssuesWithCostDetails(List<WorkOrderIssuesWCostModel> workOrderIssues)
        {
            var materialCost = CalculateMaterialCost(workOrderIssues);

            var laborCost = CalculateLaborCost(workOrderIssues);

            var fixedOHCost = CalculateFixedOHCost(workOrderIssues);
            
            var subContractCost = CalculateSubContractCost(workOrderIssues);

            return (materialCost, laborCost, fixedOHCost, subContractCost);
        }

        private static decimal CalculateSubContractCost(List<WorkOrderIssuesWCostModel> workOrderIssues)
        {
            decimal subContractCost = 0;

            foreach(var workOrder in workOrderIssues)
            {
                subContractCost += workOrder.Total_Sc_Cost;
            }

            return Math.Round(subContractCost, 4);
        }

        private static decimal CalculateMaterialCost(List<WorkOrderIssuesWCostModel> workOrderIssues)
        {
            decimal materialCost = 0;
            
            foreach (var workOrder in workOrderIssues)
            {
                materialCost += workOrder.Total_Mat_Cost;

                CheckForMissingCost(workOrder.Total_Mat_Cost);
            }

            return Math.Round(materialCost, 4);
        }

        private static decimal CalculateLaborCost(List<WorkOrderIssuesWCostModel> workOrderIssues)
        {
            decimal laborCost = 0;

            foreach (var workOrder in workOrderIssues)
            {
                laborCost += workOrder.Total_Labor_Cost;
            }

            return Math.Round(laborCost, 4);
        }

        private static decimal CalculateFixedOHCost(List<WorkOrderIssuesWCostModel> workOrderIssues)
        {
            decimal fixedOHCost = 0;

            foreach (var workOrder in workOrderIssues)
            {
                fixedOHCost += workOrder.Total_Foh_Cost;
            }

            return Math.Round(fixedOHCost, 4);
        }

        private static void CheckForMissingCost(decimal materialCost)
        {
            if (materialCost == 0)
            {
                DisplayErrorMessage.DisplayMessage("There is a material issue missing cost.  Please correct and rerun.");
            }
        }
    }
}
