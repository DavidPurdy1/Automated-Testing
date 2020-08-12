using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using System;

namespace ConsoleTests.src
{
    public class DataRetrieval
    {
        private ILog debugLog;
        public DataRetrieval(ILog debugLog)
        {
            this.debugLog = debugLog;
        }
        public void ValidateDocumentAdd(string guid, string date, string number)
        {
            string num = "";
            string method = MethodBase.GetCurrentMethod().Name;
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("DBDocumentData"));
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandTimeout = 60;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "spGetDocumentDataFromMostRecent";
            Print(method, "has started");
            using (SqlDataReader reader = command.ExecuteReader())
            {
                DataTable table = new DataTable();
                table.Load(reader);

                var index = table.Rows[0]["value"].ToString().IndexOf(".");
                num = table.Rows[0]["value"].ToString().Substring(0, index);
                if (!guid.Equals(table.Rows[0]["DOCUMENT_NAME"].ToString()))
                {
                    Print(method, "Document Name Data Does Not Match");
                    throw new AssertFailedException("Document Name Data Does Not Match");
                }
                else if (!guid.Equals(table.Rows[2]["value"].ToString()))
                {
                    Print(method, "predicted " + guid);
                    Print(method, "actual " + table.Rows[2]["value"].ToString());

                    Print(method, "Document guid string Data Does Not Match");
                    throw new AssertFailedException("Document guid string Data Does Not Match");
                }
                else if (!date.Equals(DateTime.Parse(table.Rows[1]["value"].ToString())))
                {
                    Print(method, "predicted " + date.ToString());
                    Print(method, "actual " + table.Rows[1]["value"].ToString());

                    Print(method, "Document date Data Does Not Match");
                    throw new AssertFailedException("Document date Data Does Not Match");
                }
                else if (!number.Equals(num))
                {
                    Print(method, "predicted " + number);
                    Print(method, "actual " + table.Rows[0]["value"].ToString());

                    Print(method, "Document number Data Does Not Match");
                    throw new AssertFailedException("Document number Data Does Not Match");
                }
                reader.Close();
                connection.Close();
            }
            Print(method, "Document Data Matches in Table");
        }
        public void Print(string method, string toPrint)
        {
            debugLog.Info(method + " " + toPrint);
        }
    }
}
