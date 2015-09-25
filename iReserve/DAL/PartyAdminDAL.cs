﻿using System;
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
                cmd = new SqlCommand("DELETE from VenueDB WHERE VenueID=@venueid", conn);
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

                conn.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                updateStatus = false;
            }

            return updateStatus;
        }

        public Dictionary<int, AddVenue> VenueList()
        {
           var venueList = new Dictionary<int,AddVenue>();
            
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
                
                while (reader.Read())
                {
                    AddVenue venue = new AddVenue();
                    int venueId = reader.GetInt32(0);
                    venue.VenueName = reader.GetString(1);
                    venue.VenueAddress = reader.GetString(2);
                    venue.VenueCapacity = reader.GetInt32(3);
                    venueList[venueId] = venue;	                
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