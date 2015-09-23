using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using iReserve.Models;
using System.Diagnostics;
using iReserve.ViewModels;

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

		public bool RemoveShow(int movieId)
		{
			bool updateStatus = false;
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
				updateStatus = false;
			}

			try
			{
				cmd = new SqlCommand("DELETE from ShowDB WHERE MovieID=@movieid", conn);
				cmd.Parameters.AddWithValue("movieid", movieId);
				
				int count = (Int32)cmd.ExecuteNonQuery();

				if (count.Equals(0))
				{
					updateStatus = false;
				}

				else
				{
					updateStatus = true;
				}

				conn.Close();
			}

			catch (Exception err)
			{
				Debug.WriteLine(err.Message);
				updateStatus = false;
			}

			return updateStatus;
		}
        
        public Dictionary<int, AddMovie> MovieShows()
        {
            var movieList = new Dictionary<int,AddMovie>();
            
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

            try
            {
                cmd = new SqlCommand("SELECT MovieID, ShowDate, Timing, BookedTickets FROM ShowDB WHERE ShowDate>CURRENT_TIMESTAMP", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    int movieId = reader.GetInt32(0);
                    SqlCommand cmdMovie = new SqlCommand("SELECT Title, Language FROM MovieDB WHERE MovieID=@movieid", conn);
                    cmdMovie.Parameters.AddWithValue("movieid", movieId);
                    SqlDataReader movieReader = cmdMovie.ExecuteReader();
                    if (!movieList.ContainsKey(movieId))
	                {
                        AddMovie movie = new AddMovie();
                        movie.MovieName = movieReader.GetString(0);
                        movie.Language = movieReader.GetString(1);
                        movie.ShowDate = Convert.ToDateTime(reader.GetString(1));
                        movie.Show = reader.GetString(2);
                        movie.BookedTickets = reader.GetInt32(3);
                        movieList[movieId] = movie;
	                }
                    else
                    {
                        movieList[movieId].Show += reader.GetString(2);
                        movieList[movieId].BookedTickets += reader.GetInt32(3);
                    }
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return movieList;
        }
    
    }

	
}