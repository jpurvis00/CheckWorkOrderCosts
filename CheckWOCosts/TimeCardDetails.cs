
using DataAccessLibrary.Models;

namespace CheckWOCostsLibrary
{
    public static class TimeCardDetails
    {
        public static (decimal, decimal) GetTimeCardDetails(List<TimeCardDetailsModel> timeCardDetails)
        {
            decimal timeCardLaborCost = 0;
            decimal timeCardOHCost = 0;

            foreach (var timeCard in timeCardDetails)
            {
                CheckForSetupOrTearDownOperations(timeCard);

                CheckWorkCenterForNoLaborCost(timeCard);

                timeCardLaborCost += timeCard.Total_Labor_Cost;
                timeCardOHCost += timeCard.Total_Foh_Cost;
            }

            return (Math.Round(timeCardLaborCost, 4), Math.Round(timeCardOHCost, 4));
        }

        private static void CheckWorkCenterForNoLaborCost(TimeCardDetailsModel timeCard)
        {
            if (String.Equals(timeCard.Work_Center_No, "OTTO TSUGAMI S206") || String.Equals(timeCard.Work_Center_No, "PREPACK"))
            {
                if(timeCard.Total_Labor_Cost != 0)
                {
                    DisplayErrorMessage.DisplayMessage("There is a work center with a labor cost > 0 that should be 0.  Please fix and rerun.");
                }
            }
        }

        private static void CheckForSetupOrTearDownOperations(TimeCardDetailsModel timeCard)
        {
            if (String.Equals(timeCard.Operation_Code, "SETUP") || string.Equals(timeCard.Operation_Code, "TEAR_DOWN"))
            {
                if (timeCard.Total_Labor_Cost != 0 && timeCard.Total_Foh_Cost != 0)
                {
                    DisplayErrorMessage.DisplayMessage("There is a SETUP or TEAR_DOWN operation with a cost.  Please fix and rerun.");
                }
            }
        }
    }
}
