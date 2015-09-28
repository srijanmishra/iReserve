using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using iReserve.Models;
using iReserve.ViewModels;

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
                cmd = new SqlCommand("SELECT COUNT(*) FROM VenueDB WHERE VenueName = @venue AND VenueAddress = @address AND VenueCapacity = @capacity", conn);
                cmd.Parameters.AddWithValue("venue", venue.VenueName);
                cmd.Parameters.AddWithValue("address", venue.VenueAddress);
                cmd.Parameters.AddWithValue("capacity", venue.VenueCapacity);

                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    cmd = new SqlCommand("INSERT into VenueDB([VenueName], [VenueAddress], [VenueCapacity]) VALUES (@venue, @address, @capacity)", conn);
                    cmd.Parameters.AddWithValue("venue", venue.VenueName);
                    cmd.Parameters.AddWithValue("address", venue.VenueAddress);
                    cmd.Parameters.AddWithValue("capacity", venue.VenueCapacity);

                    if (cmd.ExecuteNonQuery().Equals(1))
                    {
                        addVenueStatus = true;
                    }
                }

                else
                {
                    addVenueStatus = false;
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

            return addVenueStatus;
        }

        public bool RemoveVenue(int venueId)
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
                cmd = new SqlCommand("DELETE FROM PartyBookingDB WHERE VenueID = @venueid", conn);
                cmd.Parameters.AddWithValue("venueid", venueId);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("DELETE FROM VenueDB WHERE VenueID=@venueid", conn);
                cmd.Parameters.AddWithValue("venueid", venueId);

                int count = (Int32)cmd.ExecuteNonQuery();

                if (!count.Equals(1))
                {
                    updateStatus = false;
                }

                else
                {
                    updateStatus = true;
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

        public VenueViewModel VenueList()
        {
           var venueList = new VenueViewModel();
            
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
                cmd = new SqlCommand("SELECT VenueID, VenueName, VenueAddress, VenueCapacity FROM VenueDB", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                venueList.VenueIdList = new List<int>();
                venueList.VenueList = new List<AddVenue>();
                venueList.isSelected = new List<bool>();

                while (reader.Read())
                {
                    AddVenue venue = new AddVenue();
                    int venueid = reader.GetInt32(0);
                    venueList.VenueIdList.Add(venueid);
                    venue.VenueName = reader.GetString(1);
                    venue.VenueAddress = reader.GetString(2);
                    venue.VenueCapacity = reader.GetInt32(3);
                    venueList.VenueList.Add(venue);
                    venueList.isSelected.Add(false);
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return venueList;
        }
    
    }
}