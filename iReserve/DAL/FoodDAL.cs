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
                Debug.WriteLine("SQL Server connection failed " + e.Message);
                return null;
            }

            List<ViewMealBookings> bookings = new List<ViewMealBookings>();
            cmd = new SqlCommand("SELECT BookingID, NoOfPlates, BookingDate, Cost, MenuID FROM FoodBookingDB WHERE EmployeeID=@EmployeeId;", conn);
            cmd.Parameters.AddWithValue("EmployeeId", EmployeeId);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewMealBookings booking = new ViewMealBookings();
                booking.BookingId = (int)reader[0];
                booking.NumberOfPlates = (int)reader[1];
                booking.DateOfBooking = (string)reader[2];
                booking.TotalAmount = (double)reader[3];
                int menuId = (int)reader[4];

                cmd = new SqlCommand("SELECT FoodCourtID, CatererID, DishID FROM MenuDB WHERE MenuID=@menuId", conn);
                cmd.Parameters.AddWithValue("menuId", menuId);

                SqlDataReader menuReader = cmd.ExecuteReader();

                int foodCourtId = (int)menuReader[0];
                int catererId = (int)menuReader[1];
                int dishId = (int)menuReader[2];

                cmd = new SqlCommand("SELECT FoodCourtName FROM FoodCourtDB WHERE FoodCourtID=@foodCourtId", conn);
                cmd.Parameters.AddWithValue("foodCourtId", foodCourtId);

                SqlDataReader foodCourtReader = cmd.ExecuteReader();

                booking.FoodCourtName = (string)foodCourtReader[0];

                cmd = new SqlCommand("SELECT CatererName FROM CatererDB WHERE CatererID=@catererId", conn);
                cmd.Parameters.AddWithValue("catererId", catererId);

                SqlDataReader catererReader = cmd.ExecuteReader();

                booking.CatererName = (string)catererReader[0];

                cmd = new SqlCommand("SELECT DishName FROM DishDB WHERE DishID=@dishId", conn);
                cmd.Parameters.AddWithValue("catererId", catererId);

                SqlDataReader dishReader = cmd.ExecuteReader();

                booking.DishName = (string)dishReader[0];

                bookings.Add(booking);
            }

            return bookings;
        }
    }
}