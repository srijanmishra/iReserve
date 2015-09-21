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
                if (Status.Equals(null))
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
                    
                    booking.BookingId = Convert.ToInt32(reader.GetString(0));
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
    }
}