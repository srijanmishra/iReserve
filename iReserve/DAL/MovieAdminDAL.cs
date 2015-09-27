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
		
                conn = new SqlConnection(ConnectionStr);
				conn.Open();
			}
			catch (Exception e)
			{
				Debug.WriteLine("SQL Server connection failed " + e.Message);
				return false;
			}

			bool addMovieStatus = false;
            int movieId;

			try
			{
                cmd = new SqlCommand("SELECT COUNT(*) FROM MovieDB WHERE Title = @title AND Language = @language", conn);
                cmd.Parameters.AddWithValue("title", movie.MovieName);
                cmd.Parameters.AddWithValue("language", movie.Language);

                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    cmd = new SqlCommand("INSERT INTO MovieDB([Title], [Language]) OUTPUT inserted.MovieID VALUES (@title, @language)", conn);
                    cmd.Parameters.AddWithValue("title", movie.MovieName);
                    cmd.Parameters.AddWithValue("language", movie.Language);

                    movieId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                else
                {
                    cmd = new SqlCommand("SELECT MovieID FROM MovieDB WHERE Title = @title AND Language = @language", conn);
                    cmd.Parameters.AddWithValue("title", movie.MovieName);
                    cmd.Parameters.AddWithValue("language", movie.Language);

                    movieId = Convert.ToInt32(cmd.ExecuteScalar());
                }

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

            conn.Close();

			return addMovieStatus;
		}

		public bool RemoveShow(int movieId, string show)
		{
			bool updateStatus = false;
			try
			{
				ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			
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
                cmd = new SqlCommand("DELETE FROM MovieBookingDB WHERE ShowID IN (SELECT ShowID FROM ShowDB WHERE MovieID = @movieid AND Timing = '@show')", conn);
                cmd.Parameters.AddWithValue("movieid", movieId);
                cmd.Parameters.AddWithValue("show", show);

                int count1 = Convert.ToInt32(cmd.ExecuteNonQuery());

                if (count1 >= 0)
                {
                    cmd = new SqlCommand("DELETE from ShowDB WHERE MovieID=@movieid AND Timing = @show", conn);
                    cmd.Parameters.AddWithValue("movieid", movieId);
                    cmd.Parameters.AddWithValue("show", show);

                    int count2 = cmd.ExecuteNonQuery();

                    if (count2 >= 1)
                    {
                        updateStatus = true;
                    }

                    else
                    {
                        updateStatus = false;
                    }
                }

                else
                {
                    updateStatus = false;
                }
			}

			catch (Exception err)
			{
				Debug.WriteLine(err.Message);
				updateStatus = false;
            }

            conn.Close();

			return updateStatus;
		}
        
        public MovieViewModel MovieShows()
        {
            var movieList = new MovieViewModel();
            
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
           
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
                movieList.MovieIdList = new List<int>();
                movieList.MovieItemList = new List<Models.AddMovie>();

                cmd = new SqlCommand("SELECT A.MovieID, A.ShowDate, A.Timing, A.BookedTickets, B.Title, B.Language FROM ShowDB AS A, MovieDB AS B WHERE ShowDate > CURRENT_TIMESTAMP AND A.MovieID = B.MovieID", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    int movieId = reader.GetInt32(0);
                    
                    if (!movieList.MovieIdList.Contains(movieId))
	                {
                        movieList.MovieIdList.Add(movieId);
                        AddMovie movie = new AddMovie();
                        movie.MovieName = reader.GetString(4);
                        movie.Language = reader.GetString(5);
                        movie.ShowDate = reader.GetDateTime(1);
                        movie.Show = reader.GetString(2);
                        movie.BookedTickets = reader.GetInt32(3);
                        movieList.MovieItemList.Add(movie);
	                }
                    else
                    {
                        int tempId = movieList.MovieIdList.FindIndex(x=>x.Equals(movieId));
                        movieList.MovieItemList[tempId].Show += reader.GetString(2);
                        movieList.MovieItemList[tempId].BookedTickets += reader.GetInt32(3);
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