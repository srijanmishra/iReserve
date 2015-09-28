using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using iReserve.Models;
using iReserve.ViewModels;
using System.Web.Mvc;

namespace iReserve.DAL
{
    public class FoodCourtAdminDAL
    {
        string ConnectionStr = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public List<string> FoodCourts()
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<string> obj = new List<string>();

            try
            {
                cmd = new SqlCommand("SELECT FoodCourtName FROM FoodCourtDB;", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj.Add(reader.GetString(0));
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return obj;
        }

        public List<string> Caterers()
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<string> obj = new List<string>();

            try
            {
                cmd = new SqlCommand("SELECT CatererName FROM CatererDB;", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj.Add(reader.GetString(0));
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return obj;
        }

        public List<string> Dishes()
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
               
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<string> obj = new List<string>();

            try
            {
                cmd = new SqlCommand("SELECT DishName FROM DishDB;", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj.Add(reader.GetString(0));
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return obj;
        }

        public bool AddMenuDetails(string FoodCourtName, string CatererName, DateTime ServingDate, int NoOfPlates, string DishName)
        {
            bool InsertSuccess = false;
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
              
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                InsertSuccess = false;
            }

            try
            {
                cmd = new SqlCommand("SELECT FoodCourtID FROM FoodCourtDB WHERE FoodCourtName = @foodcourtname;", conn);
                cmd.Parameters.AddWithValue("foodcourtname", FoodCourtName);

                int FoodCourtID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("SELECT CatererID FROM CatererDB WHERE CatererName = @caterername;", conn);
                cmd.Parameters.AddWithValue("caterername", CatererName);

                int CatererID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("SELECT DishID FROM DishDB WHERE DishName = @dishname;", conn);
                cmd.Parameters.AddWithValue("dishname", DishName);

                int DishID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("SELECT COUNT(*) FROM MenuDB WHERE FoodCourtID = @foodcourtid AND CatererID = @catererid AND ServingDate = @servingdate AND DishID = @dishid", conn);
                cmd.Parameters.AddWithValue("foodcourtid", FoodCourtID);
                cmd.Parameters.AddWithValue("catererid", CatererID);
                cmd.Parameters.AddWithValue("servingdate", ServingDate);
                cmd.Parameters.AddWithValue("dishid", DishID);

                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    cmd = new SqlCommand("INSERT INTO MenuDB VALUES (@foodcourtid, @catererid, @servingdate, @noofplates, @dishid);", conn);
                    cmd.Parameters.AddWithValue("foodcourtid", FoodCourtID);
                    cmd.Parameters.AddWithValue("catererid", CatererID);
                    cmd.Parameters.AddWithValue("servingdate", ServingDate);
                    cmd.Parameters.AddWithValue("noofplates", NoOfPlates);
                    cmd.Parameters.AddWithValue("dishid", DishID);

                    if (cmd.ExecuteNonQuery().Equals(1))
                    {
                        InsertSuccess = true;
                    }
                    else
                    {
                        InsertSuccess = false;
                    }
                }

                else
                {
                    InsertSuccess = false;
                }
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                InsertSuccess = false;
            }

            conn.Close();

            return InsertSuccess;
        }

        public MenuViewModel GetMenus()
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
             
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            MenuViewModel menuList = new MenuViewModel();

            try
            {
                menuList.isSelected = new List<bool>();
                menuList.MenuIdList = new List<int>();
                menuList.MenuItemList = new List<MenuDetails>();

                cmd = new SqlCommand("SELECT T1.MenuID AS MenuID, T2.FoodCourtName AS FoodCourtName, T3.CatererName AS CatererName, T4.DishName AS DishName, T1.ServingDate AS ServingDate, T1.NoOfPlates AS NoOfPlates FROM MenuDB AS T1, (SELECT A.MenuID AS MenuID, B.FoodCourtName AS FoodCourtName FROM MenuDB AS A JOIN FoodCourtDB AS B ON A.FoodCourtID = B.FoodCourtID) AS T2, (SELECT A.MenuID AS MenuID, B.CatererName AS CatererName FROM MenuDB AS A JOIN CatererDB AS B ON A.CatererID = B.CatererID) AS T3, (SELECT A.MenuID AS MenuID, B.DishName AS DishName FROM MenuDB AS A JOIN DishDB AS B ON A.DishID = B.DishID) AS T4 WHERE T1.MenuID = T2.MenuID AND T1.MenuID = T3.MenuID AND T1.MenuID = T4.MenuID", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MenuDetails ListItem = new MenuDetails();
                    int id = reader.GetInt32(0);

                    menuList.MenuIdList.Add(id);

                    ListItem.FoodCourtName = reader.GetString(1);
                    ListItem.CatererName = reader.GetString(2);
                    ListItem.DishName = reader.GetString(3);
                    ListItem.ServingDate = reader.GetDateTime(4);
                    ListItem.NumberOfPlates = reader.GetInt32(5);

                    menuList.MenuItemList.Add(ListItem);

                    menuList.isSelected.Add(false);
                } 

                reader.Close();
            }

            catch (SqlException err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return menuList;
        }

        public bool RemoveMenuItemFromTable(int menuID)
        {
            bool IsRemoved = false;
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                IsRemoved = false;
            }

            try
            {
                cmd = new SqlCommand("DELETE FROM FoodBookingDB WHERE MenuID = @menuId", conn);
                cmd.Parameters.AddWithValue("menuId", menuID);

                int count1 = cmd.ExecuteNonQuery();

                if (count1 >= 0)
                {
                    cmd = new SqlCommand("DELETE FROM MenuDB WHERE MenuID = @menuId", conn); 
                    cmd.Parameters.AddWithValue("menuId", menuID);

                    int count2 = cmd.ExecuteNonQuery();

                    if (count2.Equals(1))
                    {
                        IsRemoved = true;
                    }

                    else
                    {
                        IsRemoved = false;
                    }
                }

                else
                {
                    IsRemoved = false;
                }
            }

            catch (Exception err)
            {
                Debug.WriteLine("SQL operation failed:" + err.Message);
                IsRemoved = false;
            }

            conn.Close();

            return IsRemoved;
        }

        public bool UpdateMenuItem(UpdateMenuDetails obj)
        {
            bool UpdateSuccess = false;
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
          
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                UpdateSuccess = false;
            }

            try
            {

                cmd = new SqlCommand("SELECT FoodCourtID FROM FoodCourtDB WHERE FoodCourtName = @foodcourtname;", conn);
                cmd.Parameters.AddWithValue("foodcourtname", obj.MenuItem.FoodCourtName);

                int FoodCourtID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("SELECT CatererID FROM CatererDB WHERE CatererName = @caterername;", conn);
                cmd.Parameters.AddWithValue("caterername", obj.MenuItem.CatererName);

                int CatererID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("SELECT DishID FROM DishDB WHERE DishName = @dishname;", conn);
                cmd.Parameters.AddWithValue("dishname", obj.MenuItem.DishName);

                int DishID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("UPDATE MenuDB SET FoodCourtID = @foodcourtid, CatererID = @catererid, ServingDate = @servingdate, NoOfPlates = @noofplates, DishID = @dishid WHERE MenuID = @menuid", conn);
                cmd.Parameters.AddWithValue("foodcourtid", FoodCourtID);
                cmd.Parameters.AddWithValue("catererid", CatererID);
                cmd.Parameters.AddWithValue("servingdate", obj.MenuItem.ServingDate);
                cmd.Parameters.AddWithValue("noofplates", obj.MenuItem.NumberOfPlates);
                cmd.Parameters.AddWithValue("dishid", DishID);
                cmd.Parameters.AddWithValue("menuid", obj.MenuID);

                int count = cmd.ExecuteNonQuery();

                if (count.Equals(1))
                {
                    UpdateSuccess = true;
                }

                else
                {
                    UpdateSuccess = false;
                }
            }

            catch (Exception err)
            {
                Debug.WriteLine("SQL operation failed:" + err.Message);
                UpdateSuccess = false;
            }

            conn.Close();

            return UpdateSuccess;
        }

        public List<string> GetSpecialties()
        {
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
             
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SQL Server connection failed:" + e.Message);
                return null;
            }

            List<string> obj = new List<string>();

            try
            {
                cmd = new SqlCommand("SELECT DISTINCT Specialty FROM CatererDB;", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj.Add(reader.GetString(0));
                }

                reader.Close();
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return null;
            }

            conn.Close();

            return obj;
        }
    }
}