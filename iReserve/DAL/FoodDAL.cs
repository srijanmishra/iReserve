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
                Debug.WriteLine("Connection String = " + ConnectionStr);
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
                cmd = new SqlCommand("SELECT T3.BookingID AS BookingID, T4.FoodCourtName AS FoodCourtName, T4.CatererName AS CatererName, T3.DishName AS DishName, T3.Price AS Price, T3.NoOfPlates AS NoOfPlates, T3.Cost AS Cost, T3.BookingDate AS BookingDate FROM (SELECT T1.BookingID AS BookingID, T1.MenuID AS MenuID, T1.NoOfPlates AS NoOfPlates, T1.BookingDate AS BookingDate, T1.Cost AS Cost, T1.FoodCourtID AS FoodCourtID, T1.CatererID AS CatererID, T1.DishID AS DishID, T2.DishName AS DishName, T2.Price AS Price FROM (SELECT C.BookingID AS BookingID, C.MenuID AS MenuID, C.NoOfPlates AS NoOfPlates, C.BookingDate AS BookingDate, C.Cost AS Cost, D.FoodCourtID AS FoodCourtID, D.CatererID AS CatererID, D.DishID AS DishID FROM FoodBookingDB AS C JOIN MenuDB AS D ON C.MenuID = D.MenuID WHERE C.EmployeeID = @EmployeeID) AS T1 JOIN (SELECT E.DishID AS DishID, E.DishName AS DishName, E.Price AS Price FROM DishDB AS E ) AS T2 ON T1.DishID = T2.DishID) AS T3 JOIN (SELECT A.CatererID AS CatererID, A.CatererName AS CatererName, B.FoodCourtID AS FoodCourtID, B.FoodCourtName AS FoodCourtName FROM CatererDB AS A, FoodCourtDB AS B WHERE (',' + RTRIM(A.FoodCourtID) + ',') LIKE ('%,' + (B.FoodCourtID) + ',%')) AS T4 ON T3.CatererID = T4.CatererID", conn);
            
            cmd.Parameters.AddWithValue("EmployeeId", EmployeeId);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewMealBookings booking = new ViewMealBookings();

                booking.BookingId = Convert.ToInt32(reader.GetString(0));
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

            return bookings;
        }
    }
}