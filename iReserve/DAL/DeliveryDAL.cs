using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iReserve.Models;
using iReserve.DAL;
using iReserve.ViewModels;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace iReserve.DAL
{
    public class DeliveryDAL
    {
        string ConnectionStr = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public DeliveryModel Bookings()
        {
            PartyDAL agent = new PartyDAL();
            DeliveryModel bookings = new DeliveryModel();

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
                cmd = new SqlCommand("SELECT A.BookingID AS BookingID, B.VenueName AS VenueName, A.BookingDate AS BookingDate, A.EventDate AS EventDate, A.Cost AS Amount, A.ApprovalStatus AS ApprovalStatus FROM PartyBookingDB AS A JOIN VenueDB AS B ON A.VenueID = B.VenueID WHERE A.EmployeeID = @EmployeeId", conn);
                cmd.Parameters.AddWithValue("EmployeeId", EmployeeId);

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



            bookings.bookingCollection = agent.Bookings();
            
            return bookings;
        }
    }
}