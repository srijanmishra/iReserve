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
                
        public bool NewUserRegister(UserRegisterModel regUser)
        {
            bool registerApproved = false;

            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                conn = new SqlConnection(ConnectionStr);
                conn.Open();

                string insertDate = regUser.DateOfJoining.ToString("MM/dd/yyyy");

                cmd = new SqlCommand("SELECT UserRole FROM EmployeeDB WHERE EmployeeID = @userID");
                cmd.Parameters.AddWithValue("userID", regUser.EmployeeID);
                
                string userRole = Convert.ToString(cmd.ExecuteScalar());

                if (userRole != null && !userRole.Contains("U"))
                {
                    userRole = userRole + "U";
                    cmd = new SqlCommand("INSERT INTO EmployeeDB (EmployeeID, EmployeeName, DoJ, Email, PhoneNo, Password, UserRole, Deductions) VALUES (@userID, @uname, @DoJ, @Email, @Phno, @pswd, @userRole, 0.0)", conn);
                    cmd.Parameters.AddWithValue("userID", regUser.EmployeeID);
                    cmd.Parameters.AddWithValue("uname", regUser.Name);
                    cmd.Parameters.AddWithValue("DoJ", insertDate);
                    cmd.Parameters.AddWithValue("Email", regUser.EmailId);
                    cmd.Parameters.AddWithValue("Phno", regUser.PhoneNumber);
                    cmd.Parameters.AddWithValue("pswd", regUser.Password);
                    cmd.Parameters.AddWithValue("userRole", userRole);

                    Debug.WriteLine("COMMAND: " + cmd.Parameters.ToString());

                    if (cmd.ExecuteNonQuery().Equals(1))
                    {
                        registerApproved = true;
                    }

                    else
                    {
                        registerApproved = false;
                    }
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

        public bool NameCheck(string uName, string pswd)
        {
            bool nameApproved = false;
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
       
                conn = new SqlConnection(ConnectionStr);
                conn.Open();

                if (pswd.Length > 0)
                {
                    cmd = new SqlCommand("select count(EmployeeID) from EmployeeDB where EmployeeName=@uname and Password=@pswd", conn);
                    cmd.Parameters.AddWithValue("uname", uName);
                    cmd.Parameters.AddWithValue("pswd", pswd);
                }

                else
                {
                    cmd = new SqlCommand("select count(EmployeeID) from EmployeeDB where EmployeeName=@uname", conn);
                    cmd.Parameters.AddWithValue("uname", uName);
                }
                
                int count = (Int32)cmd.ExecuteScalar();

                if (!count.Equals(1))
                {
                    nameApproved = false;
                }

                else
                {
                    nameApproved = true;
                }

                conn.Close();
            }

            catch (Exception)
            {
                nameApproved = false;
            }
            return nameApproved;
        }

        public bool IDCheck(string uName, string pswd)
        {
            bool nameApproved = false;
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    
                conn = new SqlConnection(ConnectionStr);
                conn.Open();

                if (pswd.Length > 0)
                {
                    cmd = new SqlCommand("select count(EmployeeID) from EmployeeDB where EmployeeID=@uname and Password=@pswd", conn);
                    cmd.Parameters.AddWithValue("uname", uName);
                    cmd.Parameters.AddWithValue("pswd", pswd);
                }

                else
                {
                    cmd = new SqlCommand("select count(EmployeeID) from EmployeeDB where EmployeeID=@uname", conn);
                    cmd.Parameters.AddWithValue("uname", uName);
                }

                int count = (Int32)cmd.ExecuteScalar();

                if (!count.Equals(1))
                {
                    nameApproved = false;
                }

                else
                {
                    nameApproved = true;
                }

                conn.Close();
            }

            catch (Exception)
            {
                nameApproved = false;
            }
            return nameApproved;
        }

        public bool RoleCheck(string uName, string pswd, string role)
        {
            bool roleApproved = false;
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    
                conn = new SqlConnection(ConnectionStr);
                conn.Open();


                cmd = new SqlCommand("select UserRole from EmployeeDB where EmployeeID=@uname and Password=@pswd", conn);
                cmd.Parameters.AddWithValue("uname", uName);
                cmd.Parameters.AddWithValue("pswd", pswd);

                SqlDataReader resultSet = cmd.ExecuteReader();

                if (resultSet == null || !resultSet.Read())
                {
                    roleApproved = false;
                }

                else
                {
                    string DBRole = resultSet[0].ToString();
                    if (DBRole.Contains(role))
                    {
                        HttpContext.Current.Session["userRole"] = role;
                        roleApproved = true;
                    }

                    else
                    {
                        roleApproved = false;
                    }

                    resultSet.Close();
                }

                conn.Close();
            }

            catch (SqlException err)
            {
                Debug.Write(err.Message);
            }

            catch (Exception err)
            {
                Debug.Write(err.Message);
                roleApproved = false;
            }
            return roleApproved;
        }

        public bool PasswordChecker(string uName, string pswd)
        {
            bool pswdApproved = false;

            try 
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            
                conn = new SqlConnection(ConnectionStr);
                conn.Open();

                cmd = new SqlCommand("select count(EmployeeID) from EmployeeDB where EmployeeName=@uname and Password=@pswd", conn);
                cmd.Parameters.AddWithValue("uname", uName);
                cmd.Parameters.AddWithValue("pswd", pswd);

                int count = (Int32)cmd.ExecuteScalar();

                if (!count.Equals(1))
                {
                    pswdApproved = false;
                }

                else
                {
                    pswdApproved = true;
                }

                conn.Close();
            } 

            catch(Exception)
            {
                pswdApproved = false;
            }

            return pswdApproved;
        }

        public bool PasswordChanger(string uName, string pswdOld, string pswdNew)
        {
            bool pswdApproved = false;

            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
              
                conn = new SqlConnection(ConnectionStr);
                conn.Open();

                cmd = new SqlCommand("update EmployeeDB set Password=@pswdNew where EmployeeID=@uName and Password=@pswdOld", conn);
                cmd.Parameters.AddWithValue("uname", uName);
                cmd.Parameters.AddWithValue("pswdNew", pswdNew);
                cmd.Parameters.AddWithValue("pswdOld", pswdOld);

                int count = (Int32)cmd.ExecuteScalar();

                if (!count.Equals(1))
                {
                    pswdApproved = false;
                }

                else
                {
                    pswdApproved = true;
                }

                conn.Close();
            }

            catch (Exception)
            {
                pswdApproved = false;
            }

            return pswdApproved;
        }
    }
}