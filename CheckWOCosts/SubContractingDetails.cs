
using DataAccessLibrary.Models;

namespace CheckWOCostsLibrary
{
    public static class SubContractingDetails
    {
        public static decimal GetSubContractingDetails(List<SubContractingModel> subContractingDetails)
        {
            decimal subContractingCost = 0;
            
            foreach (var subContractingOp in subContractingDetails)
            {
                subContractingCost += subContractingOp.Total_Sc_Cost;
            }

            return Math.Round(subContractingCost, 4);
        }
    }
}
