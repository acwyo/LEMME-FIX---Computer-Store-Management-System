using ASG_D.Code;
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
    internal class User
    {
        private string username;
        private string password;
        private string email;
        private string phoneno;
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());

        public string Username { get => username; set => username = value; }

        public string Password { get => password; set => password = value; }

        public string Email { get => email; set => email = value; }

        public string Phoneno { get => phoneno; set => phoneno = value; }

        public User(string u, string p) //u = admin; p = admin;
        {
            username = u;
            password = p;
        }
        public User(string u, string e, string num, string p)
        {
            username = u;
            email = e;
            phoneno = num;
            password = p;
        }
        public static void PUSER_UPDATE_PASSWORD(int userID, string b_pass)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());
            con.Open();
            using (var cmd = new SqlCommand("puser_update_password", con))
            {
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.StoredProcedure;
                var param1 = cmd.Parameters.AddWithValue("@userID", userID);
                var param2 = cmd.Parameters.AddWithValue("@password", b_pass);
                cmd.ExecuteNonQuery();
            }
            con.Close();

        }

        public string login(string un) //un = admin
        {
            //string status = null;
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());
            //con.Open();

            ////SqlCommand objectName = new Constructor (SqlQuery, connectionString)
            //SqlCommand cmd = new SqlCommand("select count(*) from users where username='" + username + "'and password ='" + password + "'", con);

            //int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            //if (count > 0)
            //{
            //    SqlCommand cmd2 = new SqlCommand("select role from users where username='" + username + "' and password = '" + password + "'", con);
            //    string userRole = cmd2.ExecuteScalar().ToString();

            //    if (userRole == "admin")
            //    {
            //        frmRegister a = new frmRegister(un);
            //        a.ShowDialog();
            //    }
            //    else
            //    {
            //        status = "Please login at " + userRole + "login page.";
            //    }
            //}
            //else
            //    status = "Incorrect username or password";
            //con.Close();
            //
            //return status;

            string status = null;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());
            con.Open();


            SqlCommand cmd = new SqlCommand("select count(*) from [user] where username ='" + username + "' and password ='" + password + "'", con);

            int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            if (count > 0)
            {
                //find the user role
                SqlCommand cmd2 = new SqlCommand("select role from [user] where username ='" + username + "' and password ='" + password + "'", con);

                string userRole = cmd2.ExecuteScalar().ToString();
                if (userRole == "receptionist")
                {
                    frmPersonal_Information_EK a = new frmPersonal_Information_EK(un);
                    a.ShowDialog();
                }
                else if (userRole == "customer")
                {
                    using (var cmd_user = new SqlCommand("PUSER_GET_CUSTOMER", con))
                    {
                        cmd_user.CommandTimeout = 300;
                        cmd_user.CommandType = CommandType.StoredProcedure;
                        var param1 = cmd_user.Parameters.AddWithValue("@username", username);
                        var da = new SqlDataAdapter(cmd_user);
                        DataSet ds_customer = new DataSet();
                        da.Fill(ds_customer);

                        DataTable b_dt_user_customer = new DataTable();
                        b_dt_user_customer = ds_customer.Tables[0];

                        se_user b_user = new se_user();

                        b_user.username = un;
                        b_user.name = b_dt_user_customer.Rows[0]["name"].ToString();
                        b_user.password = b_dt_user_customer.Rows[0]["password"].ToString();
                        b_user.role = userRole;
                        b_user.email = b_dt_user_customer.Rows[0]["email"].ToString();
                        b_user.phoneno = b_dt_user_customer.Rows[0]["phoneno"].ToString();
                        b_user.userID = Convert.ToInt32(b_dt_user_customer.Rows[0]["userID"].ToString());
                        b_user.customerID = Convert.ToInt32(b_dt_user_customer.Rows[0]["customerID"].ToString());
                        b_user.profile_pic = b_dt_user_customer.Rows[0]["profile_pic"].ToString();


                        se._user = b_user;

                    }
                    FormChange f2 = new FormChange();
                    f2.ShowDialog();

                }
                else if (userRole == "technician")
                {
                    PENDING_U a = new PENDING_U(un);
                    a.ShowDialog();
                }
                else if (userRole == "admin")
                {
                    frmRegister a = new frmRegister(un);
                    a.ShowDialog();
                }

            }
            else
            {
                status = "Incorrect username/password";
            }
            con.Close();

            return status;
        }

        public string ResetPassword()
        {
            string status;
            con.Open();


            SqlCommand cmd = new SqlCommand("select count(*) from [user] where username = '" + username + "'", con);
            int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            if (count != 0)
            {
                SqlCommand cmd2 = new SqlCommand("select role from [user] where username = '" + username + "'", con);
                string role = cmd2.ExecuteScalar().ToString();

                int count2 = 0;

                if (role == "customer")
                {
                    SqlCommand cmd3 = new SqlCommand("select count(*) from customer where email = '" + email + "' and phoneno = '" + phoneno + "' and userID = (select ID from [user] where username = '" + username + "')", con);
                    count2 = Convert.ToInt32(cmd3.ExecuteScalar().ToString());
                }
                else if (role == "technician")
                {
                    SqlCommand cmd4 = new SqlCommand("select count(*) from technician where email = '" + email + "' and phoneno = '" + phoneno + "' and userID = (select ID from [user] where username = '" + username + "')", con);
                    count2 = Convert.ToInt32(cmd4.ExecuteScalar().ToString());
                }
                else if (role == "receptionist")
                {
                    SqlCommand cmd5 = new SqlCommand("select count(*) from receptionist where email = '" + email + "' and phoneno = '" + phoneno + "' and userID = (select ID from [user] where username = '" + username + "')", con);
                    count2 = Convert.ToInt32(cmd5.ExecuteScalar().ToString());
                }
                else
                {
                    SqlCommand cmd6 = new SqlCommand("select count(*) from admin where email = '" + email + "' and phoneno = '" + phoneno + "' and userID = (select ID from [user] where username = '" + username + "')", con);
                    count2 = Convert.ToInt32(cmd6.ExecuteScalar().ToString());
                }

                if (count2 != 0)
                {
                    SqlCommand cmd7 = new SqlCommand("update [user] set password = '" + password + "' where username = '" + username + "'", con);
                    int i = cmd7.ExecuteNonQuery();

                    if (i != 0)
                    {
                        status = "Password reset successfull.";
                    }
                    else
                    {
                        status = "Unable to update password.";
                    }
                }
                else
                {
                    status = "Wrong email or phone number.";
                }


            }
            else
            {
                status = "User not found.";
            }

            con.Close();

            return status;
        }


    }
}
