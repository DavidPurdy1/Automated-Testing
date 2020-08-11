using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace ConsoleTests.src
{
    public class DataRetrieval
    {
        public bool ValidateDocumentAdd(string guid, string date, string number)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("DBDocumentData"));
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandTimeout = 60;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "spGetDocumentDataFromMostRecent";
            using (SqlDataReader reader = command.ExecuteReader())
            {
                DataTable table = new DataTable();
                table.Load(reader);

                if (!guid.Equals(table.Rows[0]["DOCUMENT_NAME"].ToString()))
                {
                    //Print(method, "Document Name Data Does Not Match");
                    return false;
                }
                else if (!guid.Equals(table.Rows[2]["value"].ToString()))
                {
                    //Print(method, "Document guid string Data Does Not Match");
                    return false;
                }
                else if (!date.Equals(table.Rows[1]["value"].ToString()))
                {
                    //Print(method, "Document date Data Does Not Match");
                    return false;
                }
                else if (!number.Equals(table.Rows[0]["value"].ToString()))
                {
                    //Print(method, "Document number Data Does Not Match");
                    return false;
                }
                reader.Close();
                connection.Close();
                return true;
            }
        }
    }
}
