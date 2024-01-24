
using DataAccessLibrary.Models;

namespace CheckWOCostsLibrary
{
    public static class TimeCardDetails
    {
        public static (decimal, decimal) GetTimeCardDetails(List<TimeCardDetailsWithSetupTeardownModel> timeCardDetailsWithSetupTeardown)
        {
            decimal timeCardLaborCost = 0;
            decimal timeCardOHCost = 0;

            foreach (var timeCard in timeCardDetailsWithSetupTeardown)
            {   
                if(String.Equals(timeCard.Tc_Reporting_Type, "SETUP") || string.Equals(timeCard.Tc_Reporting_Type, "TEAR_DOWN"))
                {
                    CheckForSetupOrTearDownOperations(timeCard);
                }

                if (String.Equals(timeCard.Tc_Reporting_Type, "RUN"))
                {
                    CheckWorkCenterForNoLaborCost(timeCard);
                }

                timeCardLaborCost += timeCard.Total_Labor_Cost;
                timeCardOHCost += timeCard.Total_Foh_Cost;
            }

            return (Math.Round(timeCardLaborCost, 4), Math.Round(timeCardOHCost, 4));
        }

        private static void CheckWorkCenterForNoLaborCost(TimeCardDetailsWithSetupTeardownModel timeCard)
        {
            if (String.Equals(timeCard.Work_Center_No, "OTTO TSUGAMI S206") || String.Equals(timeCard.Work_Center_No, "PREPACK"))
            {
                if(timeCard.Total_Labor_Cost != 0)
                {
                    DisplayErrorMessage.DisplayMessage("There is a work center with a labor cost > 0 that should be 0.  Please fix and rerun.");
                }
            }
        }

        private static void CheckForSetupOrTearDownOperations(TimeCardDetailsWithSetupTeardownModel timeCard)
        {
            if (timeCard.Total_Labor_Cost != 0 || timeCard.Total_Foh_Cost != 0)
            {
                DisplayErrorMessage.DisplayMessage("\nThere is a SETUP or TEAR_DOWN operation with a cost.  Please fix and rerun.\n");
            }

            if (timeCard.Good_Qty != 0 || timeCard.Scrap_Qty != 0)
            {
                DisplayErrorMessage.DisplayMessage("\nThere are quantities recorded for a SETUP or TEAR_DOWN operation.  This is not allowed.  Please fix and rerun.\n");
            }
        }
    }
}
