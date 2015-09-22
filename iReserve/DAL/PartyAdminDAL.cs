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
    public class PartyAdminDAL
    {
        string ConnectionStr = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public bool AddVenue(AddVenue venue)
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

            bool addVenueStatus = false;

            try
            {
                SqlCommand cmd = new SqlCommand("INSERT into VenueDB([VenueName], [VenueAddress], [VenueCapacity]) VALUES (@venue, @address, @capacity)", conn);
                cmd.Parameters.AddWithValue("venue", venue.VenueName);
                cmd.Parameters.AddWithValue("address", venue.VenueAddress);
                cmd.Parameters.AddWithValue("capacity", venue.VenueCapacity);
                
                if (cmd.ExecuteNonQuery().Equals(1))
                {
                    addVenueStatus = true;
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

            return addVenueStatus;
        }
    }
}