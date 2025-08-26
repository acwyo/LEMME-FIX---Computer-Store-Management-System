using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASG_D
{
    internal class Receptionist
    {
        private string recName;
        private string email;
        private string phoneNum;
        private string username;
        private string password;
        private string ic;
        private string address;
        private int userid;
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());

        public string Email { get => email; set => email = value; }

        public string PhoneNum { get => phoneNum; set => phoneNum = value; }

        public string Username { get => username; set => username = value; }

        public string Password { get => password; set => password = value; }

        public string Ic { get => ic; set => ic = value; }

        public string Address { get => address; set => address = value; }

        public int Userid { get => userid; set => userid = value; }

        public Receptionist(string u, string p)
        {
            username = u;
            password = p;
        }

        public Receptionist(string n, string i, string e, string num, string a, string u, int id)
        {
            recName = n;
            ic = i;
            email = e;
            phoneNum = num;
            address = a;
            username = u;
            userid = id;
        }

        public string addReceptionist()
        {
            string status = null;

            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from receptionist where ic = '" + ic + "'", con);
            int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            con.Close();

            if (count == 0)
            {
                con.Open();
                //SqlCommand cmd2 = new SqlCommand("insert into receptionist(name, ic, email, phoneno, address, userID) values(@name, @ic, @em, @num, @add ,(select ID from [user] where username = '@username'))", con);
                SqlCommand cmd2 = new SqlCommand("insert into receptionist(name, ic, email, phoneno, address, userID) values(@name, @ic, @em, @num, @add ,@id)", con);

                cmd2.Parameters.AddWithValue("@name", recName);
                cmd2.Parameters.AddWithValue("@ic", ic);
                cmd2.Parameters.AddWithValue("@em", email);
                cmd2.Parameters.AddWithValue("@num", phoneNum);
                cmd2.Parameters.AddWithValue("@add", address);
                cmd2.Parameters.AddWithValue("@id", userid);

                int i = cmd2.ExecuteNonQuery();
                con.Close();

                if (i != 0)
                    status = "Registration Succesful.";
                else
                    status = "Unable to register.";
            }
            else
            {
                con.Open();

                SqlCommand cmd3 = new SqlCommand("delete from [user] where username = '" + username + "'", con);
                int j = cmd3.ExecuteNonQuery();

                con.Close();

                if (j != 0)
                {
                    status = "Record exist, registration canceled, user deleted.";
                }
                else
                {
                    status = "Record exist.";
                }

            }


            return status;
        }

        public string addReceptionistUser()
        {
            string status = null;

            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from [user] where username = '" + username + "' and role = 'receptionist'", con);
            int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            con.Close();

            if (count == 0)
            {
                con.Open();

                SqlCommand cmd3 = new SqlCommand("select MAX([id]) from [user]", con);
                int userid = Convert.ToInt32(cmd3.ExecuteScalar().ToString()) + 1;

                SqlCommand cmd2 = new SqlCommand("insert into [user] ([id], username, password, role) values(@id, @username, @password, 'receptionist')", con);

                cmd2.Parameters.AddWithValue("@username", username);
                cmd2.Parameters.AddWithValue("@password", password);
                cmd2.Parameters.AddWithValue("@id", userid);

                int i = cmd2.ExecuteNonQuery();
                con.Close();

                if (i != 0)
                {
                    status = "User added.";
                    frmPersonalInformation a = new frmPersonalInformation(userid, username, "receptionist");
                    a.ShowDialog();
                }
                else
                {
                    status = "Unable to register.";
                }
            }
            else
            {
                status = "Username used, please try another one";
            }

            return status;
        }

        public static ArrayList viewReceptionist()
        {
            ArrayList nm = new ArrayList();
            con.Open();
            SqlCommand cmd = new SqlCommand("select name from receptionist", con);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                nm.Add(rd.GetString(0));
                //add element into arraylist
            }
            con.Close();
            return nm;
        }
    }
}
