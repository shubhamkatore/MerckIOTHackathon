using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DBHelper
{
    public class SQLOperation
    {
        public static DataTable ExecuteCommand(string ConnectionString, string Command,bool output, Dictionary<string, string> Parameters = null)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (SqlCommand cmd = new SqlCommand(Command, connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (Parameters != null)
                {
                    cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    foreach (var paramval in Parameters)
                    {
                        cmd.Parameters.AddWithValue(paramval.Key, paramval.Value);
                    }
                }
                connection.Open();
                if (output)
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }
            }
            return dt;
        }
    }
}
