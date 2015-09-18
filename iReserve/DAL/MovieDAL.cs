using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using iReserve.Models;
using System.Diagnostics;

namespace iReserve.DAL
{
    public class MovieDAL
    {
        string ConnectionStr = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public List<ViewMovieBookings> Bookings(int EmployeeId)
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

            List<ViewMovieBookings> bookings = new List<ViewMovieBookings>();
            cmd = new SqlCommand("SELECT * FROM MovieBookingDB WHERE EmployeeID=@EmployeeId;", conn);
            cmd.Parameters.AddWithValue("EmployeeId", EmployeeId);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewMovieBookings booking = new ViewMovieBookings();
                booking.BookingId = (int)reader[0];
                booking.EmployeeNumber = (int)reader[1];
                booking.NumberOfSeats = (int)reader[3];
                booking.Status = (bool)reader[4];
                booking.CardType = (string)reader[5];
                booking.CardNumber = (string)reader[6];
                booking.Amount = (int)reader[7];

                int showId = (int) reader[2];

                cmd = new SqlCommand("SELECT MovieID, ShowDate, Timing FROM ShowDB WHERE ShowID=@showId", conn);
                cmd.Parameters.AddWithValue("showId", showId);

                SqlDataReader showReader = cmd.ExecuteReader();

                int movieId = (int)showReader[0];
                booking.ShowDate = (string)showReader[1];
                booking.Show = (string)showReader[2];

                cmd = new SqlCommand("SELECT Title FROM MovieDB WHERE MovieID=@movieId", conn);
                cmd.Parameters.AddWithValue("movieId", movieId);

                SqlDataReader movieReader = cmd.ExecuteReader();

                booking.MovieName = (string)movieReader[0];

                bookings.Add(booking);
            }

            return bookings;
        }
    }
}