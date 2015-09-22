using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using iReserve.Models;

namespace iReserve.DAL
{
    public class FoodCourtAdminDAL
    {
        string ConnectionStr = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public List<string> FoodCourts()
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Debug.WriteLine("Connection String = " + ConnectionStr);
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<string> obj = new List<string>();

            try
            {
                cmd = new SqlCommand("SELECT FoodCourtName FROM FoodCourtDB;", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj.Add(reader.GetString(0));
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return obj;
        }

        public List<string> Caterers()
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Debug.WriteLine("Connection String = " + ConnectionStr);
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<string> obj = new List<string>();

            try
            {
                cmd = new SqlCommand("SELECT CatererName FROM CatererDB;", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj.Add(reader.GetString(0));
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return obj;
        }

        public List<string> Dishes()
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Debug.WriteLine("Connection String = " + ConnectionStr);
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<string> obj = new List<string>();

            try
            {
                cmd = new SqlCommand("SELECT DishName FROM DishDB;", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj.Add(reader.GetString(0));
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return obj;
        }

        public bool AddMenuDetails(string FCName, string CName, DateTime SDate, int NPlates, string DName)
        {
            bool InsertSuccess = false;
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Debug.WriteLine("Connection String = " + ConnectionStr);
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                InsertSuccess = false;
            }

            try
            {
                cmd = new SqlCommand("SELECT FoodCourtID FROM FoodCourtDB WHERE FoodCourtName = @FCN;", conn);
                cmd.Parameters.AddWithValue("FCN", FCName);

                int FCID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("SELECT CatererID FROM CatererDB WHERE CatererName = @CN;", conn);
                cmd.Parameters.AddWithValue("CN", CName);

                int CID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("SELECT DishID FROM DishDB WHERE DishName = @DN;", conn);
                cmd.Parameters.AddWithValue("DN", DName);

                int DID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("INSERT INTO MenuDB VALUES (@FCID, @CID, @SD, @NP, @DID);", conn);
                cmd.Parameters.AddWithValue("FCID", FCID);
                cmd.Parameters.AddWithValue("CID", CID);
                cmd.Parameters.AddWithValue("SD", SDate);
                cmd.Parameters.AddWithValue("NP", NPlates);
                cmd.Parameters.AddWithValue("DID", DID);

                if (cmd.ExecuteNonQuery().Equals(1))
                {
                    InsertSuccess = true;
                }
                else
                {
                    InsertSuccess = false;
                }
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                InsertSuccess = false;
            }

            conn.Close();

            return InsertSuccess;
        }
    }
}