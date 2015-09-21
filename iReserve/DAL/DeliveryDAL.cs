﻿using System;
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
        int i = 1;

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
                cmd = new SqlCommand("SELECT EmployeeID FROM PartyBookingDB WHERE ApprovalStatus = 'P'", conn);
                
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bookings.EmployeeIDCollection.Add(reader.GetInt32(0));
                    i = i + 1;
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            try
            {
                for (int j = 0; j < i; j++)
                {
                    List<ViewPartyBookings> temp = agent.Bookings(bookings.EmployeeIDCollection[i]);
                    foreach (var item in temp)
                    {
                        bookings.bookingCollection.Add(item);
                    }
                }
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            return bookings;
        }
    }
}