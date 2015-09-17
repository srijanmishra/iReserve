using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iReserve.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace iReserve.DAL
{
    public class UserAccountDAL
    {
        string ConnectionStr = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        
        public bool LoginCheck(UserLoginModel login)
        {
            bool loginApproved = false;
            HttpContext.Current.Session["userRole"] = "X";
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Debug.WriteLine("Connection String = " + ConnectionStr);
                conn = new SqlConnection(ConnectionStr);
                conn.Open();

                cmd = new SqlCommand("select count(EmployeeID) from EmployeeDB where EmployeeID=@uname and Password=@pswd", conn);
                cmd.Parameters.AddWithValue("uname", login.UserName);
                cmd.Parameters.AddWithValue("pswd", login.Password);

                int count = (Int32)cmd.ExecuteScalar();

                if (!count.Equals(1))
                {
                    loginApproved = false;
                }

                else
                {
                    cmd = new SqlCommand("select UserRole from EmployeeDB where EmployeeID=@uname and Password=@pswd", conn);
                    cmd.Parameters.AddWithValue("uname", login.UserName);
                    cmd.Parameters.AddWithValue("pswd", login.Password);

                    SqlDataReader resultSet = cmd.ExecuteReader();

                    if (resultSet == null || !resultSet.Read())
                    {
                        loginApproved = false;
                    }

                    else
                    {
                        string DBRole = resultSet[0].ToString();
                        if (DBRole.Contains(login.Role))
                        {
                            HttpContext.Current.Session["userRole"] = login.Role;
                            loginApproved = true;
                        }

                        else
                        {
                            loginApproved = false;
                        }

                        resultSet.Close();
                    }
                }

                conn.Close();
            }

            catch (Exception)
            {
                loginApproved = false;
            }
            return loginApproved;
        }

        public bool NewUserRegister(UserRegisterModel regUser)
        {
            bool registerApproved = false;

            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                conn = new SqlConnection(ConnectionStr);
                conn.Open();

                cmd = new SqlCommand("insert into EmployeeDB (EmployeeID, EmployeeName, DoJ, Email, PhoneNo, Password, UserRole, Deductions) values (@userID, @uname, @DoJ, @Email, @Phno, @pswd, 'U', 0.0)", conn);
                cmd.Parameters.AddWithValue("userID", regUser.EmployeeID);
                cmd.Parameters.AddWithValue("uname", regUser.Name);
                cmd.Parameters.AddWithValue("DoJ", regUser.DateOfJoining);
                cmd.Parameters.AddWithValue("Email", regUser.EmailId);
                cmd.Parameters.AddWithValue("Phno", regUser.PhoneNumber);
                cmd.Parameters.AddWithValue("pswd", regUser.Password);

                if (cmd.ExecuteNonQuery().Equals(1))
                {
                    registerApproved = true;
                }

                else
                {
                    registerApproved = false;
                }

                conn.Close();
            }

            catch (Exception)
            {
                registerApproved = false;
            }

            return registerApproved;
        }

        public bool NameCheck(string uName)
        {
            bool nameApproved = false;
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Debug.WriteLine("Connection String = " + ConnectionStr);
                conn = new SqlConnection(ConnectionStr);
                conn.Open();

                cmd = new SqlCommand("select count(EmployeeID) from EmployeeDB where EmployeeName=@uname", conn);
                cmd.Parameters.AddWithValue("uname", uName);
                
                int count = (Int32)cmd.ExecuteScalar();

                if (!count.Equals(1))
                {
                    nameApproved = true;
                }

                else
                {
                    nameApproved = false;
                }

                conn.Close();
            }

            catch (Exception)
            {
                nameApproved = false;
            }
            return nameApproved;
        }

    }
}