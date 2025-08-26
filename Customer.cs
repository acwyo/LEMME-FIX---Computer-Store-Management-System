using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASG_D
{
    internal class Customer
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());
        
        public static DataTable Fdt_CUSTOMER_GET_LATEST_REQUEST(int customerID)
        {
            DataSet ds = new DataSet();
            con.Open();
            using (var cmd = new SqlCommand("PUSER_CUSTOMER_GET_LATEST_REQUEST", con))
            {
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.StoredProcedure;
                var param1 = cmd.Parameters.AddWithValue("@customerID", customerID);
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            con.Close();
            DataTable b_dt_request = new DataTable();
            if (ds != null)
                b_dt_request = ds.Tables[0];

            return b_dt_request;
        }
        public static DataTable Fdt_CUSTOMER_GET_LIST_REQUEST(int customerID)
        {
            DataSet ds = new DataSet();
            con.Open();
            using (var cmd = new SqlCommand("PUSER_CUSTOMER_GET_LIST_REQUEST", con))
            {
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.StoredProcedure;
                var param1 = cmd.Parameters.AddWithValue("@customerID", customerID);
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            con.Close();
            DataTable b_dt_request = new DataTable();
            if (ds != null)
                b_dt_request = ds.Tables[0];

            return b_dt_request;
        }

        public static void PUSER_CUSTOMER_UPDATE(int customerID, string b_name, string b_email, string b_phone)
        {
            DataSet ds = new DataSet();
            con.Open();
            using (var cmd = new SqlCommand("puser_customer_update", con))
            {
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.StoredProcedure;
                var param1 = cmd.Parameters.AddWithValue("@customerID", customerID);
                var param2 = cmd.Parameters.AddWithValue("@name", b_name);
                var param3 = cmd.Parameters.AddWithValue("@email", b_email);
                var param4 = cmd.Parameters.AddWithValue("@phoneno", b_phone);
                cmd.ExecuteNonQuery();
            }
            con.Close();

        }

        public static void PUSER_CUSTOMER_UPDATE_PHOTO(int customerID, string b_path_photo)
        {
            DataSet ds = new DataSet();
            con.Open();
            using (var cmd = new SqlCommand("puser_customer_update_photo", con))
            {
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.StoredProcedure;
                var param1 = cmd.Parameters.AddWithValue("@customerID", customerID);
                var param2 = cmd.Parameters.AddWithValue("@path_photo", b_path_photo);
                cmd.ExecuteNonQuery();
            }
            con.Close();

        }
    }
}
