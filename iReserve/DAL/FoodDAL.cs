using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using iReserve.Models;

namespace iReserve.DAL
{
    public class FoodDAL
    {
        string ConnectionStr = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public List<ViewMealBookings> MealBookings(int EmployeeId)
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
           
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<ViewMealBookings> bookings = new List<ViewMealBookings>();

            try
            {
                cmd = new SqlCommand("SELECT T3.BookingID AS BookingID, T4.FoodCourtName AS FoodCourtName, T4.CatererName AS CatererName, T3.DishName AS DishName, T3.Price AS Price, T3.NoOfPlates AS NoOfPlates, T3.Cost AS Cost, T3.BookingDate AS BookingDate FROM (SELECT T1.BookingID AS BookingID, T1.MenuID AS MenuID, T1.NoOfPlates AS NoOfPlates, T1.BookingDate AS BookingDate, T1.Cost AS Cost, T1.FoodCourtID AS FoodCourtID, T1.CatererID AS CatererID, T1.DishID AS DishID, T2.DishName AS DishName, T2.Price AS Price FROM (SELECT C.BookingID AS BookingID, C.MenuID AS MenuID, C.NoOfPlates AS NoOfPlates, C.BookingDate AS BookingDate, C.Cost AS Cost, D.FoodCourtID AS FoodCourtID, D.CatererID AS CatererID, D.DishID AS DishID FROM FoodBookingDB AS C JOIN MenuDB AS D ON C.MenuID = D.MenuID WHERE C.EmployeeID = @EmployeeID) AS T1 JOIN (SELECT E.DishID AS DishID, E.DishName AS DishName, E.Price AS Price FROM DishDB AS E ) AS T2 ON T1.DishID = T2.DishID) AS T3 JOIN (SELECT A.CatererID AS CatererID, A.CatererName AS CatererName, B.FoodCourtID AS FoodCourtID, B.FoodCourtName AS FoodCourtName FROM CatererDB AS A, FoodCourtDB AS B WHERE (',' + CAST(RTRIM(A.FoodCourtID) AS NVARCHAR(7)) + ',') LIKE ('%,' + CAST((B.FoodCourtID) AS NVARCHAR(7)) + ',%')) AS T4 ON T3.CatererID = T4.CatererID", conn);

                cmd.Parameters.AddWithValue("EmployeeId", EmployeeId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ViewMealBookings booking = new ViewMealBookings();

                    booking.BookingId = reader.GetInt32(0);
                    booking.FoodCourtName = reader.GetString(1);
                    booking.CatererName = reader.GetString(2);
                    booking.DishName = reader.GetString(3);
                    booking.PricePerPlate = Convert.ToDouble(reader.GetDecimal(4));
                    booking.NumberOfPlates = reader.GetInt32(5);
                    booking.TotalAmount = Convert.ToDouble(reader.GetDecimal(6));
                    booking.DateOfBooking = reader.GetDateTime(7).ToString();

                    bookings.Add(booking);
                }

                reader.Close();
            }

            catch (SqlException err)
            {
                Debug.WriteLine("SQL Server connection failed " + err.Message);
                return null;
            }

            catch (InvalidOperationException err)
            {
                Debug.WriteLine("SQL Server connection failed " + err.Message);
                return null;
            }

            catch (Exception err)
            {
                Debug.WriteLine("ERROR: " + err.Message);
                return null;
            }

            conn.Close();

            return bookings;
        }

        public List<MealSearchModel> SearchByFoodCourt(string FoodCourtName)
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<MealSearchModel> searchResults = new List<MealSearchModel>();

            try
            {
                cmd = new SqlCommand("SELECT EXT1.MenuID AS MenuID, EXT1.DishName AS DishName, EXT1.DishType AS DishType, EXT2.CatererName AS CatererName, EXT2.Specialty, EXT1.Price AS Price, EXT1.NoOfPlates AS NoOfPlates FROM (SELECT T1. MenuID AS MenuID, T1.CatererID AS CatererID, T1.NoOfPlates AS NoOfPlates, T2.DishName AS DishName, T2.Price AS Price, T2.IsVegetarian AS DishType FROM (SELECT A.MenuID AS MenuID, B.FoodCourtID AS FoodCourtID, A.CatererID AS CatererID, A.DishID AS DishID, A.NoOfPlates AS NoOfPlates FROM MenuDB AS A, FoodCourtDB AS B WHERE A.FoodCourtID = B.FoodCourtID AND B.FoodCourtName = @FCName) AS T1, DishDB AS T2 WHERE T1.DishID = T2.DishID) AS EXT1, (SELECT C.CatererName AS CatererName, C.CatererID AS CatererID, C.Specialty AS Specialty FROM CatererDB AS C, FoodCourtDB AS D WHERE (',' + CAST(RTRIM(C.FoodCourtID) AS NVARCHAR(7)) + ',') LIKE ('%,' + CAST((D.FoodCourtID) AS NVARCHAR(7)) + ',%')) AS EXT2 WHERE EXT1.CatererID = EXT2.CatererID", conn);

                cmd.Parameters.AddWithValue("FCName", FoodCourtName);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MealSearchModel menuItem = new MealSearchModel();

                    menuItem.MenuId = reader.GetInt32(0);
                    menuItem.DishName = reader.GetString(1);
                    if (reader.GetBoolean(2))
                    {
                        menuItem.DishType = 'V';
                    }
                    else
                    {
                        menuItem.DishType = 'N';
                    }
                    menuItem.CatererName = reader.GetString(3);
                    menuItem.CatererSpecialty = reader.GetString(4);
                    menuItem.PricePerPlate = Convert.ToDouble(reader.GetDecimal(5));
                    menuItem.NoOfPlatesAvailable = reader.GetInt32(6);

                    searchResults.Add(menuItem);
                }

                reader.Close();
            }

            catch (SqlException err)
            {
                Debug.WriteLine("SQL Server connection failed " + err.Message);
                return null;
            }

            catch (InvalidOperationException err)
            {
                Debug.WriteLine("SQL operation failed " + err.Message);
                return null;
            }

            catch (Exception err)
            {
                Debug.WriteLine("ERROR: " + err.Message);
                return null;
            }

            conn.Close();

            return searchResults;
        }

        public List<MealSearchModel> SearchByCaterer(string CatererName)
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<MealSearchModel> searchResults = new List<MealSearchModel>();

            try
            {
                cmd = new SqlCommand("SELECT T1.MenuID AS MenuID, T2.DishName AS DishName, T2.IsVegetarian AS DishType, T3.FoodCourtName AS FoodCourtName, T1.Specialty AS Specialty, T2.Price AS Price, T1.NoOfPlates AS NoOfPlates FROM (SELECT A1.Specialty AS Specialty, A2.MenuID AS MenuID, A2.FoodCourtID AS FoodCourtID, A2.CatererID AS CatererID, A2.NoOfPlates AS NoOfPlates, A2.DishID AS DishID FROM CatererDB AS A1, MenuDB AS A2 WHERE A1.CatererID = A2.CatererID AND A1.CatererName = @CName) AS T1, DishDB AS T2, FoodCourtDB AS T3 WHERE T1.DishID = T2.DishID AND T1.FoodCourtID = T3.FoodCourtID", conn);

                cmd.Parameters.AddWithValue("CName", CatererName);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MealSearchModel menuItem = new MealSearchModel();

                    menuItem.MenuId = reader.GetInt32(0);
                    menuItem.DishName = reader.GetString(1);
                    if (reader.GetBoolean(2))
                    {
                        menuItem.DishType = 'V';
                    }
                    else
                    {
                        menuItem.DishType = 'N';
                    }
                    menuItem.FoodCourtName = reader.GetString(3);
                    menuItem.CatererSpecialty = reader.GetString(4);
                    menuItem.PricePerPlate = Convert.ToDouble(reader.GetDecimal(5));
                    menuItem.NoOfPlatesAvailable = reader.GetInt32(6);

                    searchResults.Add(menuItem);
                }

                reader.Close();
            }

            catch (SqlException err)
            {
                Debug.WriteLine("SQL Server connection failed " + err.Message);
                return null;
            }

            catch (InvalidOperationException err)
            {
                Debug.WriteLine("SQL operation failed " + err.Message);
                return null;
            }

            catch (Exception err)
            {
                Debug.WriteLine("ERROR: " + err.Message);
                return null;
            }

            conn.Close();

            return searchResults;
        }

        public List<MealSearchModel> SearchBySpecialty(string Specialty)
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
      
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<MealSearchModel> searchResults = new List<MealSearchModel>();

            try
            {
                cmd = new SqlCommand("SELECT T1.MenuID AS MenuID, T2.DishName AS DishName, T2.IsVegetarian AS DishType, T3.FoodCourtName AS FoodCourtName, T1.CatererName AS CatererName, T2.Price AS Price, T1.NoOfPlates AS NoOfPlates FROM (SELECT A1.CatererName AS CatererName, A2.MenuID AS MenuID, A2.FoodCourtID AS FoodCourtID, A2.CatererID AS CatererID, A2.NoOfPlates AS NoOfPlates, A2.DishID AS DishID FROM CatererDB AS A1, MenuDB AS A2 WHERE A1.CatererID = A2.CatererID AND A1.Specialty = @CSpecialty) AS T1, DishDB AS T2, FoodCourtDB AS T3 WHERE T1.DishID = T2.DishID AND T1.FoodCourtID = T3.FoodCourtID", conn);

                cmd.Parameters.AddWithValue("CSpecialty", Specialty);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MealSearchModel menuItem = new MealSearchModel();

                    menuItem.MenuId = reader.GetInt32(0);
                    menuItem.DishName = reader.GetString(1);
                    if (reader.GetBoolean(2))
                    {
                        menuItem.DishType = 'V';
                    }
                    else
                    {
                        menuItem.DishType = 'N';
                    }
                    menuItem.FoodCourtName = reader.GetString(3);
                    menuItem.CatererName = reader.GetString(4);
                    menuItem.PricePerPlate = Convert.ToDouble(reader.GetDecimal(5));
                    menuItem.NoOfPlatesAvailable = reader.GetInt32(6);

                    searchResults.Add(menuItem);
                }

                reader.Close();
            }

            catch (SqlException err)
            {
                Debug.WriteLine("SQL Server connection failed " + err.Message);
                return null;
            }

            catch (InvalidOperationException err)
            {
                Debug.WriteLine("SQL operation failed " + err.Message);
                return null;
            }

            catch (Exception err)
            {
                Debug.WriteLine("ERROR: " + err.Message);
                return null;
            }

            conn.Close();

            return searchResults;
        }

        public List<MealSearchModel> SearchByDishType(bool DishType)
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
              
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<MealSearchModel> searchResults = new List<MealSearchModel>();

            try
            {
                cmd = new SqlCommand("SELECT T1.MenuID AS MenuID, T2.DishName AS DishName, T3.FoodCourtName AS FoodCourtName, T1.CatererName AS CatererName, T1.Specialty AS Specialty, T2.Price AS Price, T1.NoOfPlates AS NoOfPlates FROM (SELECT A1.CatererName AS CatererName, A2.MenuID AS MenuID, A2.FoodCourtID AS FoodCourtID, A2.CatererID AS CatererID, A2.NoOfPlates AS NoOfPlates, A2.DishID AS DishID, A1.Specialty AS Specialty FROM CatererDB AS A1, MenuDB AS A2 WHERE A1.CatererID = A2.CatererID) AS T1, DishDB AS T2, FoodCourtDB AS T3 WHERE T1.DishID = T2.DishID AND T1.FoodCourtID = T3.FoodCourtID AND T2.IsVegetarian = @DType", conn);

                cmd.Parameters.AddWithValue("DType", DishType);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MealSearchModel menuItem = new MealSearchModel();

                    menuItem.MenuId = reader.GetInt32(0);
                    menuItem.DishName = reader.GetString(1);
                    menuItem.FoodCourtName = reader.GetString(2);
                    menuItem.CatererName = reader.GetString(3);
                    menuItem.CatererSpecialty = reader.GetString(4);
                    menuItem.PricePerPlate = Convert.ToDouble(reader.GetDecimal(5));
                    menuItem.NoOfPlatesAvailable = reader.GetInt32(6);

                    searchResults.Add(menuItem);
                }

                reader.Close();
            }

            catch (SqlException err)
            {
                Debug.WriteLine("SQL Server connection failed " + err.Message);
                return null;
            }

            catch (InvalidOperationException err)
            {
                Debug.WriteLine("SQL operation failed " + err.Message);
                return null;
            }

            catch (Exception err)
            {
                Debug.WriteLine("ERROR: " + err.Message);
                return null;
            }

            conn.Close();

            return searchResults;
        }

        public bool InsertBookingIntoDatabase(MakeBookingDetails obj)
        {
            bool isInserted = false;
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
             
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                isInserted = false;
            }

            try
            {
                cmd = new SqlCommand("INSERT INTO FoodBookingDB VALUES (@empId, @menuId, @noOfPlates, @bookingDate, @amount)", conn);
                cmd.Parameters.AddWithValue("empId", Convert.ToInt32(obj.EmployeeId));
                cmd.Parameters.AddWithValue("menuId", Convert.ToInt32(obj.MenuId));
                cmd.Parameters.AddWithValue("noOfPlates", Convert.ToInt32(obj.NumberOfPlates));
                cmd.Parameters.AddWithValue("bookingDate", Convert.ToDateTime(obj.DateOfBooking));
                cmd.Parameters.AddWithValue("amount", Convert.ToDecimal(obj.TotalAmount));

                int count1 = cmd.ExecuteNonQuery();

                if (count1 >= 0)
                {
                   isInserted = true;                    
                }

                else
                {
                    isInserted = false;
                }
            }

            catch (Exception err)
            {
                Debug.WriteLine("SQL operation failed:" + err.Message);
                isInserted = false;
            }

            conn.Close();

            return isInserted;
        }
    }
}