
using CheckWOCostsUI;
using DataAccessLibrary;
using CheckWOCostsLibrary;

internal class Program
{
    private static void Main(string[] args)
    {
        OracleCrud oracleConnection = new OracleCrud(Configuration.GetConnectionString());

        var dayToCheck = GetDayToCheckClosedWorkOrders.GetDayToCheck();
        var closedWorkOrders = oracleConnection.GetClosedWorkOrders(dayToCheck);

        var currentDateTime = DateTime.Now;
        var date = DateOnly.FromDateTime(currentDateTime);

        Console.WriteLine(date.AddDays(-dayToCheck));

        foreach (var wo in closedWorkOrders)
        {
            Console.WriteLine($"***** WO: {wo.Work_Order_No} *****");

            //string testingOrderNumb = "906156";

            //var costSummaryDetails = oracleConnection.GetCostSummaryActualMaterialCost(testingOrderNumb);
            var costSummaryDetails = oracleConnection.GetCostSummaryActualMaterialCost(wo.Work_Order_No);
            var actualMaterialCost = CostSummaryActualMaterialCost.GetCostSummaryActualMaterialCost(costSummaryDetails);

            //var woIssuesWCostDetails = oracleConnection.GetWOIssuesWithCost(testingOrderNumb);
            var woIssuesWCostDetails = oracleConnection.GetWOIssuesWithCost(wo.Work_Order_No);
            var (actualIssuedMaterialCost, actualIssuedLaborCost, actualIssuedOHCost, actualIssuedSubContractCost) = WorkOrderIssuesWithCost.GetWorkOrderIssuesWithCostDetails(woIssuesWCostDetails);
            Console.WriteLine($"mat cost: {actualIssuedMaterialCost}    labor cost: {actualIssuedLaborCost}     oh cost: {actualIssuedOHCost}   sub cost: {actualIssuedSubContractCost}");

            //var timeCardDetailsWithSetupTeardown = oracleConnection.GetTimeCardDetailsWithSetupTeardown(testingOrderNumb);
            var timeCardDetailsWithSetupTeardown = oracleConnection.GetTimeCardDetailsWithSetupTeardown(wo.Work_Order_No);
            var (timeCardLaborCost, timeCardOHCost) = TimeCardDetails.GetTimeCardDetails(timeCardDetailsWithSetupTeardown);
            Console.WriteLine($"labor cost: {timeCardLaborCost}     oh cost: {timeCardOHCost}");

            //var subContractingDetails = oracleConnection.GetSubContractingDetails(testingOrderNumb);
            var subContractingDetails = oracleConnection.GetSubContractingDetails(wo.Work_Order_No);
            var subContractingCost = SubContractingDetails.GetSubContractingDetails(subContractingDetails);
            Console.WriteLine($"sub cost for parent part: {subContractingCost}");

            //var workOrderCompletionDetails = oracleConnection.GetWorkOrderCompletionDetails(testingOrderNumb);
            var workOrderCompletionDetails = oracleConnection.GetWorkOrderCompletionDetails(wo.Work_Order_No);

            var issuedMaterialCostsVsItemStandardCosts = oracleConnection.GetIssuedMaterialCostsVsItemStandardCosts(wo.Work_Order_No);

            CheckIfValuesMatch.CheckMaterialCostsMatch(actualMaterialCost, actualIssuedMaterialCost, workOrderCompletionDetails);

            CheckIfValuesMatch.CheckLaborCostsMatch(actualIssuedLaborCost, timeCardLaborCost, workOrderCompletionDetails);

            CheckIfValuesMatch.CheckOHCostsMatch(actualIssuedOHCost, timeCardOHCost, workOrderCompletionDetails);

            CheckIfValuesMatch.CheckSubContractCostsMatch(actualIssuedSubContractCost, subContractingCost, workOrderCompletionDetails);

            CheckIfValuesMatch.CheckIssuedMaterialCostsVsItemStandardCosts(issuedMaterialCostsVsItemStandardCosts);

            Console.WriteLine("**********************\n");
        }

        Console.WriteLine("Finished processing work orders.");
        Console.ReadLine();
    }
}