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
    public class PartyDAL
    {
        string ConnectionStr = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public List<ViewPartyBookings> Bookings(int EmployeeId, string Status)
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

            List<ViewPartyBookings> bookings = new List<ViewPartyBookings>();
            try
            {
                if (Status.Length < 1)
                {
                    cmd = new SqlCommand("SELECT A.BookingID AS BookingID, B.VenueName AS VenueName, A.BookingDate AS BookingDate, A.EventDate AS EventDate, A.Cost AS Amount, A.ApprovalStatus AS ApprovalStatus FROM PartyBookingDB AS A JOIN VenueDB AS B ON A.VenueID = B.VenueID WHERE A.EmployeeID = @EmployeeId", conn);
                    cmd.Parameters.AddWithValue("EmployeeId", EmployeeId);
                }

                else
                {
                    cmd = new SqlCommand("SELECT A.BookingID AS BookingID, B.VenueName AS VenueName, A.BookingDate AS BookingDate, A.EventDate AS EventDate, A.Cost AS Amount, A.ApprovalStatus AS ApprovalStatus FROM PartyBookingDB AS A JOIN VenueDB AS B ON A.VenueID = B.VenueID WHERE A.EmployeeID = @EmployeeId AND A.ApprovalStatus = @Status", conn);
                    cmd.Parameters.AddWithValue("EmployeeId", EmployeeId);
                    cmd.Parameters.AddWithValue("Status", Status);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ViewPartyBookings booking = new ViewPartyBookings();

                    booking.BookingId = reader.GetInt32(0);
                    booking.VenueName = reader.GetString(1);
                    booking.BookingDate = reader.GetDateTime(2).ToString();
                    booking.EventDate = reader.GetDateTime(3).ToString();
                    booking.Amount = Convert.ToDouble(reader.GetDecimal(4));
                    booking.ApprovalStatus = reader.GetString(5);

                    bookings.Add(booking);
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return bookings;
        }

        public bool MakeBooking(PartyBooking booking)
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
            bool bookingStatus = false;

            try
            {
                cmd = new SqlCommand("SELECT COUNT(*) FROM PartyBookingDB WHERE VenueID = @venueid AND BookingDate = @bookingdate");
                cmd.Parameters.AddWithValue("venueid", booking.VenueID);
                cmd.Parameters.AddWithValue("bookingdate", DateTime.Now);

                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    cmd = new SqlCommand("INSERT INTO PartyBookingDB([EmployeeID], [VenueID], [BookingDate], [EventDate], [Cost]) VALUES (@employeeid, @venueid, @bookingdate, @eventdate, @cost)", conn);
                    cmd.Parameters.AddWithValue("employeeid", booking.EmployeeID);
                    cmd.Parameters.AddWithValue("venueid", booking.VenueID);
                    cmd.Parameters.AddWithValue("bookingdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("eventdate", booking.EventDate);
                    cmd.Parameters.AddWithValue("cost", booking.Cost);

                    if (cmd.ExecuteNonQuery().Equals(1))
                    {
                        bookingStatus = true;
                    }
                }

                else
                {
                    bookingStatus = false;
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

            return bookingStatus;
        }

        public List<VenueDetails> ViewVenues()
        {
            PartyDAL agent = new PartyDAL();
            var venues = new List<VenueDetails>();

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
                cmd = new SqlCommand("SELECT VenueID, VenueName, VenueAddress, VenueCapacity FROM VenueDB", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    var venue = new VenueDetails();
                    venue.VenueID = reader.GetInt32(0);
                    venue.VenueItem.VenueName = reader.GetString(1);
                    venue.VenueItem.VenueAddress = reader.GetString(2);
                    venue.VenueItem.VenueCapacity = reader.GetInt32(3);

                    venues.Add(venue);
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();           

            return venues;
        }
    }
}