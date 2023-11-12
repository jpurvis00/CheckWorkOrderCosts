
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Dapper;

namespace DataAccessLibrary
{
    internal class OracleDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
        {
            /* using statement will make sure that our db connection always gets closed.  Even if we
             * run in to an unhandled exception, it will be closed.
             * IDbConnection statement and new SqlConnection will create a connection to our db that is 
             * passed in using the connectionString var.
             */
            using (IDbConnection connection = new OracleConnection(connectionString))
            {
                /* The Query statement is a dapper statement.  It queries our db connection and uses
                 * the sqlStatment var to execute some sql code(ie. select Id, FirstName, LastName from 
                 * dbo.Contacts.  parameters hold any limiter to our call(ie. the Id # we are looking for).
                 * Can also be no limiters for the parameter var.  This Query statement can return multiple
                 * rows which is why we store it in a List.  The T generic for the Query<T> is a model of 
                 * 1 row that we are trying to get out(ie. whatever fields are in that row - FirstName, LastName, 
                 * PhoneNumber).  
                 */
                connection.Open();

                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();

                connection.Close();

                return rows;
            }
        }
    }
}
