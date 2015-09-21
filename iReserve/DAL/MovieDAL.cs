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

            try
            {
                cmd = new SqlCommand("SELECT Test.BookingID AS BookingId, Test.EmployeeID AS EmployeeNumber, M.Title AS MovieName, Test.ShowDate AS ShowDate, Test.Timing AS Show, Test.BookingDate AS BookingDate, Test.NoOfSeats AS NumberOfSeats, Test.CardType AS CardType, Test.CardNo AS CardNo, Test.Amount AS Amount, Test.Confirmation AS Confirmation FROM (SELECT A.BookingID AS BookingID, A.EmployeeID AS EmployeeID, A.NoOfSeats AS NoOfSeats, A.Confirmation AS Confirmation, A.CardType AS CardType, A.CardNo AS CardNo, A.BookingDate AS BookingDate, A.Amount AS Amount, B.ShowDate AS ShowDate, B.Timing AS Timing, B.MovieID FROM MovieBookingDB AS A JOIN ShowDB AS B ON A.ShowID = B.ShowID WHERE A.EmployeeID=@UserID) AS Test JOIN MovieDB AS M ON Test.MovieID = M.MovieID;", conn);

                cmd.Parameters.AddWithValue("UserID", EmployeeId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ViewMovieBookings booking = new ViewMovieBookings();

                    booking.BookingId = Convert.ToInt32(reader.GetString(0));
                    booking.EmployeeNumber = reader.GetInt32(1);
                    booking.MovieName = reader.GetString(2);
                    booking.ShowDate = reader.GetDateTime(3).ToString();
                    booking.Show = reader.GetString(4);
                    booking.BookingDate = reader.GetDateTime(5).ToString();
                    booking.NumberOfSeats = reader.GetInt32(6);
                    booking.CardType = reader.GetString(7);
                    booking.CardNumber = reader.GetString(8);
                    booking.Amount = reader.GetDouble(9);
                    booking.Status = reader.GetBoolean(10);

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

        public List<ViewCurrentMovies> CurrentMovies() 
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

            List<ViewCurrentMovies> movies = new List<ViewCurrentMovies>();

            try
            {
                //cmd = new SqlCommand("SELECT Test.BookingID AS BookingId, Test.EmployeeID AS EmployeeNumber, M.Title AS MovieName, Test.ShowDate AS ShowDate, Test.Timing AS Show, Test.BookingDate AS BookingDate, Test.NoOfSeats AS NumberOfSeats, Test.CardType AS CardType, Test.CardNo AS CardNo, Test.Amount AS Amount, Test.Confirmation AS Confirmation FROM (SELECT A.BookingID AS BookingID, A.EmployeeID AS EmployeeID, A.NoOfSeats AS NoOfSeats, A.Confirmation AS Confirmation, A.CardType AS CardType, A.CardNo AS CardNo, A.BookingDate AS BookingDate, A.Amount AS Amount, B.ShowDate AS ShowDate, B.Timing AS Timing, B.MovieID FROM MovieBookingDB AS A JOIN ShowDB AS B ON A.ShowID = B.ShowID WHERE A.EmployeeID=@UserID) AS Test JOIN MovieDB AS M ON Test.MovieID = M.MovieID;", conn);
                cmd = new SqlCommand("SELECT Movie.MovieID as MovieID, Movie.Title AS Title, Movie.Language AS Language, Show.ShowDate AS ShowDate FROM MovieDB AS Movie JOIN ShowDB AS Show ON Movie.MovieID = Show.MovieID WHERE (Show.ShowDate<dateadd(ms, -3, dateadd(wk, datediff(wk, 0, getdate()) + 1, 0)) and Show.ShowDate>dateadd(ms, -3, dateadd(wk, datediff(wk, 0, getdate()) + 1, 0))-2);", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                Dictionary<Int32, ViewCurrentMovies> movieListings = new Dictionary<Int32, ViewCurrentMovies>();

                while (reader.Read())
                {
                    int movieId = Convert.ToInt32(reader.GetString(0));
                    if (movieListings.ContainsKey(movieId))
                    {
                        DateTime date = reader.GetDateTime(3);
                        movieListings[movieId].ShowDate += ',' + date.ToString();
                    }
                    else
                    {
                        ViewCurrentMovies movie = new ViewCurrentMovies();

                        movie.MovieID = Convert.ToInt32(reader.GetString(0));
                        movie.MovieName = reader.GetString(1);
                        movie.Language = reader.GetString(2);
                        DateTime date = reader.GetDateTime(3);
                        movie.ShowDate = date.ToString();

                        movieListings[movieId] = movie;
                    }
                }

                reader.Close();

                foreach (var movie in movieListings)
                {
                    movies.Add(movie.Value);
                }
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

            return movies;
        }
    }
}