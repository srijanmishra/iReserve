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

        public List<ViewPartyBookings> Bookings(int EmployeeId)
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
            cmd = new SqlCommand("SELECT BookingID, BookingDate, EventDate, Cost, ApprovalStatus, VenueID FROM PartyBookingDB WHERE EmployeeID=@EmployeeId;", conn);
            cmd.Parameters.AddWithValue("EmployeeId", EmployeeId);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewPartyBookings booking = new ViewPartyBookings();
                booking.BookingId = (int)reader[0];
                booking.BookingDate = (string)reader[1];
                booking.EventDate = (string)reader[2];
                booking.Amount = (int)reader[3];
                booking.ApprovalStatus = (string)reader[4];
                int venueId = (int)reader[5];

                cmd = new SqlCommand("SELECT VenueName FROM VenueDB WHERE VenueID=@venueId", conn);
                cmd.Parameters.AddWithValue("venueId", venueId);

                SqlDataReader venueReader = cmd.ExecuteReader();
                booking.VenueName = (string)venueReader[0];
                
                bookings.Add(booking);
            }

            return bookings;
        }
    }
}