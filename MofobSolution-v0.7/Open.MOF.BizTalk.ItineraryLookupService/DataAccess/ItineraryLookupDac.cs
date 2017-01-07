using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;

namespace Open.MOF.BizTalk.ItineraryLookupService.DataAccess
{
    public class ItineraryLookupDac
    {
        public static string[] FindItineraryConnectionStringFromMessageDescriptor(string messageDescriptor)
        {
            string[] result = new string[2];
            string sql = "SELECT ItineraryName, ItineraryVersion FROM dbo.MessageItineraryMapping WHERE MessageDescriptor=@MessageDescriptor";

            SqlConnection sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ItineraryDBConnection"].ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
            SqlParameter sqlParameter = new SqlParameter("@MessageDescriptor", SqlDbType.NVarChar, 1024);
            sqlParameter.Value = messageDescriptor;
            sqlCommand.Parameters.Add(sqlParameter);

            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                    result[0] = reader.GetString(0);
                if (!reader.IsDBNull(1))
                    result[1] = reader.GetString(1);
            }

            return result;
        }
    }
}
