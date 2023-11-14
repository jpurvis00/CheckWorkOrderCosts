
using CheckWOCostsUI;
using DataAccessLibrary;
using CheckWOCostsLibrary;

internal class Program
{
    private static void Main(string[] args)
    {
        OracleCrud oracleConnection = new OracleCrud(Configuration.GetConnectionString());

        var closedWorkOrders = oracleConnection.GetClosedWorkOrders();
        
        foreach (var wo in closedWorkOrders)
        {
            /* test with wo 905778 for subcontract, scrap, ops with no labor */

            Console.WriteLine($"***** WO: {wo.Work_Order_No} *****");

            var costSummaryDetails = oracleConnection.GetCostSummaryActualMaterialCost(wo.Work_Order_No);
            var actualMaterialCost = CostSummaryActualMaterialCost.GetCostSummaryActualMaterialCost(costSummaryDetails);

            var woIssuesWCostDetails = oracleConnection.GetWOIssuesWithCost(wo.Work_Order_No);
            var (actualIssuedMaterialCost, actualIssuedLaborCost, actualIssuedOHCost, actualIssuedSubContractCost) = WorkOrderIssuesWithCost.GetWorkOrderIssuesWithCostDetails(woIssuesWCostDetails);
            Console.WriteLine($"mat cost: {actualIssuedMaterialCost}    labor cost: {actualIssuedLaborCost}     oh cost: {actualIssuedOHCost}   sub cost: {actualIssuedSubContractCost}");

            var timeCardDetails = oracleConnection.GetTimeCardDetails(wo.Work_Order_No);
            var (timeCardLaborCost, timeCardOHCost) = TimeCardDetails.GetTimeCardDetails(timeCardDetails);
            Console.WriteLine($"labor cost: {timeCardLaborCost}     oh cost: {timeCardOHCost}");

            var subContractingDetails = oracleConnection.GetSubContractingDetails(wo.Work_Order_No);
            var subContractingCost = SubContractingDetails.GetSubContractingDetails(subContractingDetails);
            Console.WriteLine($"sub cost for parent part: {subContractingCost}");

            var workOrderCompletionDetails = oracleConnection.GetWorkOrderCompletionDetails(wo.Work_Order_No).FirstOrDefault();

            CheckIfValuesMatch.CheckMaterialCostsMatch(actualMaterialCost, actualIssuedMaterialCost, workOrderCompletionDetails);

            CheckIfValuesMatch.CheckLaborCostsMatch(actualIssuedLaborCost, timeCardLaborCost, workOrderCompletionDetails);

            CheckIfValuesMatch.CheckOHCostsMatch(actualIssuedOHCost, timeCardOHCost, workOrderCompletionDetails);

            CheckIfValuesMatch.CheckSubContractCostsMatch(actualIssuedSubContractCost, subContractingCost, workOrderCompletionDetails);

            Console.WriteLine("**********************************\n");
        }

        Console.WriteLine("Finished processing.");
        Console.ReadLine();
    }
}