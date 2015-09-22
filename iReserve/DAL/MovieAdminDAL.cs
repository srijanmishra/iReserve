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
    public class MovieAdminDAL
    {
        string ConnectionStr = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public bool AddMovie(AddMovie movie)
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
                return false;
            }

            bool addMovieStatus = false;

            try
            {
                cmd = new SqlCommand("INSERT INTO MovieDB([Title], [Language]) OUTPUT inserted.MovieID VALUES (@title, @language)", conn);
                cmd.Parameters.AddWithValue("title", movie.MovieName);
                cmd.Parameters.AddWithValue("language", movie.Language);
                
                int movieId = Convert.ToInt32(cmd.ExecuteScalar());
                SqlCommand cmdShow = new SqlCommand("INSERT into ShowDB([MovieID], [ShowDate], [Timing], [BookedTickets], [Price]) VALUES (@movieid, @showdate, @show, @bookedTickets, @price)", conn);
                cmdShow.Parameters.AddWithValue("movieid", movieId);
                cmdShow.Parameters.AddWithValue("showDate", movie.ShowDate);
                cmdShow.Parameters.AddWithValue("show", movie.Show);
                cmdShow.Parameters.AddWithValue("bookedTickets", 0);
                cmdShow.Parameters.AddWithValue("price", movie.Cost);
                
                if (cmdShow.ExecuteNonQuery().Equals(1))
                {
                    addMovieStatus = true;
                }

                conn.Close();
            }

            catch (SqlException err)
            {
                Debug.WriteLine("SQL Server connection failed " + err.Message);
            }

            catch (InvalidOperationException err)
            {
                Debug.WriteLine("SQL Server connection failed " + err.Message);
            }

            catch (Exception err)
            {
                Debug.WriteLine("ERROR: " + err.Message);
            }

            return addMovieStatus;
        }
    }
}