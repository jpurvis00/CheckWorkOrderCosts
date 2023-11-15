
namespace DataAccessLibrary
{
    public static class GetDayToCheckClosedWorkOrders
    {
        public static int GetDayToCheck()
        {
            bool isValidInt = false;

            while(isValidInt == false)
            {
                Console.WriteLine("What day would you like to check the closed work orders?\n" +
                                "ex. 1 = 1 day before current date\n" +
                                "    2 = 2 days before current date\n" +
                                "    etc.");

                var dayToCheck = Console.ReadLine();

                isValidInt = int.TryParse(dayToCheck, out var day);

                if (isValidInt == true && day > 0)
                {
                    return day;
                }
                else
                {
                    Console.WriteLine("Please input a valid integer.\n");
                    isValidInt = false;
                }
            }

            return 0;
        }
    }
}
