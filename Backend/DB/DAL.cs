using Unomart.Controllers;
using Unomart.Managers;
using Unomart.Models;
using System.Data.OleDb;

namespace Unomart.DB
{
    public static class DAL
    {
        private static OleDbConnection conn = new(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DB\\AppDb.accdb");

        public static Cache ItemMaster = new();

        //Get
        static public User getUser(string UID)
        {
            if (OperatingSystem.IsWindows())
            {

                OleDbDataReader reader = SQL("SELECT * FROM T_Users WHERE UID='" + UID + "';");
                User u = new();
                while (reader.Read())
                {
                    u.UID = reader["UID"].ToString();
                    u.Email = reader["Email"].ToString();
                    u.Username = reader["Username"].ToString();
                    u.Hash = reader["Hash"].ToString();
                    u.CurrencyCode = reader["CurrencyCode"].ToString();
                }
                ;
                return u;
            }
            return null;
        }
        static public User getUserFromEmail(string email)
        {
            if (OperatingSystem.IsWindows())
            {

                OleDbDataReader reader = SQL("SELECT * FROM T_Users WHERE Email='" + email + "';");
                User u = new();
                while (reader.Read())
                {
                    u.UID = reader["UID"].ToString();
                    u.Email = reader["Email"].ToString();
                    u.Username = reader["Username"].ToString();
                    u.Hash = reader["Hash"].ToString();
                }
                ;
                return u;
            }
            return null;
        }
        static public Address[]? getAddress(string UID)
        {
            if (OperatingSystem.IsWindows())
            {
                OleDbDataReader reader = SQL("SELECT * FROM T_Address WHERE UID='" + UID + "';");
                List<Address> ads = new();
                while (reader.Read())
                {
                    Address ad = new();
                    ad.UID = reader["UID"].ToString();
                    ad.AID = reader["AID"].ToString();
                    ad.UserAddress = reader["UserAddress"].ToString();
                    ads.Add(ad);
                };
                return ads.ToArray();
            }
            return null;
        }
        static public OrderItem[]? getOrderItems(string OID, string code)
        {
            if (OperatingSystem.IsWindows())
            {
                OleDbDataReader reader = SQL("SELECT * FROM T_OrderLines WHERE OID='" + OID + "';");
                List<OrderItem> orders = new List<OrderItem>();
                while (reader.Read())
                {
                    OrderItem ui = new();

                    ui.OID = reader["OID"].ToString();
                    ui.IID = reader["IID"].ToString();
                    ui.ItemName = reader["ItemName"].ToString();
                    ui.ItemAmount = int.Parse(reader["ItemAmount"].ToString());
                    ui.ItemCost = CurrencyManager.Convert(float.Parse(reader["ItemCost"].ToString()), code);
                    ui.ItemImage = ItemMaster.GetImage(ui.ItemName);

                    orders.Add(ui);
                }
                ;
                return orders.ToArray();
            }
            return null;
        }
        static public Order[]? getOrders(string UID)
        {
            if (OperatingSystem.IsWindows())
            {
                string code = getUser(UID).CurrencyCode;
                OleDbDataReader reader = SQL("SELECT * FROM T_Orders WHERE UID='" + UID + "' ORDER BY OrderDate DESC;");
                List<Order> orders = new List<Order>();
                while (reader.Read())
                {
                    Order ui = new();

                    ui.OID = reader["OID"].ToString();
                    ui.UID = reader["UID"].ToString();
                    ui.DeliveryAddress = reader["DeliveryAddress"].ToString();
                    ui.DeliveryCharge = CurrencyManager.Convert(float.Parse(reader["DeliveryCharge"].ToString()), code);
                    ui.Discount = (float)(Math.Round((1.0f - float.Parse(reader["Discount"].ToString())) * 100) / 100.0f);
                    ui.Total = CurrencyManager.Convert(float.Parse(reader["OrderTotal"].ToString()), code);
                    ui.Items = int.Parse(reader["OrderItems"].ToString());
                    ui.DateTime = DateTime.Parse(reader["OrderDate"].ToString());
                    orders.Add(ui);
                }
                ;
                return orders.ToArray();
            }
            return null;
        }
        static public dynamic getTotal(string UID, string code)
        {
            float total = 0.0f;
            int items = 0;
            OleDbDataReader reader = SQL("SELECT * FROM T_Cart WHERE UID='" + UID + "'");
            while (reader.Read())
            {
                int x = int.Parse(reader["Amount"].ToString());
                total += ItemMaster.getItemPrice(reader["ItemName"].ToString()) * x;
                items += x;
            }

            return new { total = CurrencyManager.Convert(total, code), items = items };
        }
        static public CartItem[] getCart(string UID, string code)
        {
            List<CartItem> cs = new List<CartItem>();
            OleDbDataReader reader = SQL("SELECT * FROM T_Cart WHERE UID='" + UID + "'");
            while (reader.Read())
            {
                CartItem c = new();
                c.Item = ItemMaster.getItem(code, reader["ItemName"].ToString());
                c.ItemAmount = int.Parse(reader["Amount"].ToString());
                cs.Add(c);
            }

            return cs.ToArray();
        }
        static public int getAmount(string UID, string item)
        {
            OleDbDataReader reader = SQL("SELECT Amount FROM T_Cart WHERE UID='" + UID + "' AND ItemName='" + item + "'");
            while (reader.Read())
            {
                return int.Parse(reader["Amount"].ToString());
            }

            return 0;
        }


        //Set
        static public void setUser(User user)
        {
            if (OperatingSystem.IsWindows())
            {
                SQL("INSERT INTO T_Users(UID, Email, Username, Hash,CurrencyCode) VALUES ('" + user.UID + "','" + user.Email + "','" + user.Username + "','" + UsersController.Hash(user.Hash, user.Email) + "', CurrencyCode='USD');");
                ;
            }
        }
        static public void setAddress(Address address)
        {
            if (OperatingSystem.IsWindows())
            {
                OleDbDataReader reader = SQL("SELECT COUNT(AID) AS x FROM T_Address WHERE UID='" + address.UID + "'");
                while (reader.Read())
                    if (int.Parse(reader["x"].ToString()) < 6)
                    {
                        SQL("INSERT INTO T_Address(AID,UID, UserAddress) VALUES ('" + address.AID + "','" + address.UID + "','" + address.UserAddress + "');");
                    }
            }
            ;

        }
        static public void setOrderItem(OrderItem orderItem)
        {
            if (OperatingSystem.IsWindows())
            {
                SQL("INSERT INTO T_OrderLines(IID, OID, ItemName, ItemAmount,itemCost) VALUES ('" + Guid.NewGuid() + "','" + orderItem.OID + "','" + orderItem.ItemName + "'," + orderItem.ItemAmount + "," + orderItem.ItemCost + ")");
                ;
            }
        }
        static public void setOrder(Order order)
        {
            order.Discount = (float)(Math.Round(order.Discount * 100) / 100.0f);
            if (OperatingSystem.IsWindows())
            {
                order.Discount = (float)(Math.Round((1 - order.Discount) * 100.0f) / 100.0f);

                SQL("INSERT INTO T_Orders(UID, OID,DeliveryAddress,DeliveryCharge, Discount, OrderTotal, OrderDate, OrderItems) VALUES ('" +
                    order.UID + "','" + order.OID + "','" + order.DeliveryAddress + "'," + order.DeliveryCharge + "," + (1 - order.Discount) + "," + order.Total + ",'" + DateTime.Now.ToString() + "'," + order.Items + ");");
                ;
            }
        }


        //Put
        static public void updateUser(User user)
        {
            if (OperatingSystem.IsWindows())
            {
                SQL("UPDATE T_Users SET UserName = '" + user.Username + "', Hash='" + user.Hash + "' WHERE Email='" + user.Email + "';");
                ;
            }
        }
        static public void updateUser(User user, bool un)
        {
            if (OperatingSystem.IsWindows())
            {
                SQL("UPDATE T_Users SET UserName = '" + user.Username + "' WHERE Email='" + user.Email + "';");
                ;
            }
        }
        static public void updateAddress(string AID, string Address)
        {
            if (OperatingSystem.IsWindows())
            {
                SQL("UPDATE T_Address SET UserAddress = '" + Address + "' WHERE AID='" + AID + "';");
                ;
            }
        }
        static public void updateCurrency(string UID, string code)
        {
            if (OperatingSystem.IsWindows())
            {
                SQL("UPDATE T_Users SET CurrencyCode = '" + code + "' WHERE UID='" + UID + "';");
            }
        }
        static public void updateCartItem(string UID, string item, int value)
        {
            if (OperatingSystem.IsWindows())
            {
                OleDbDataReader reader = SQL("SELECT * FROM T_Cart WHERE UID='" + UID + "' AND ItemName='" + item + "';");
                bool b = true;
                while (reader.Read())
                {
                    b = false;
                    SQL("UPDATE T_Cart SET Amount = " + value + " WHERE UID='" + UID + "' AND ItemName='" + item + "';");
                }

                if (b)
                    SQL("INSERT INTO T_Cart(CID,UID, ItemName,Amount) VALUES ('" + Guid.NewGuid() + "','" + UID + "','" + item + "'," + value + ");");
                SQL("DELETE FROM T_Cart WHERE Amount=0;");
            }
        }


        //Delete
        static public void removeAddress(string AID, string UID)
        {
            if (OperatingSystem.IsWindows())
            {
                OleDbDataReader reader = SQL("SELECT COUNT(AID) AS x FROM T_Address WHERE UID='" + UID + "'");
                while (reader.Read())
                {
                    if (int.Parse(reader["x"].ToString()) > 1)
                        SQL("DELETE FROM T_Address WHERE AID='" + AID + "';");
                }
                ;
            }
        }
        static public void removeCart(string UID)
        {
            if (OperatingSystem.IsWindows())
            {
                SQL("DELETE FROM T_Cart WHERE UID='" + UID + "';");
            }
        }


        static public void PrepareCache()
        {
            if (OperatingSystem.IsWindows())
            {
                OleDbDataReader reader = SQL("SELECT * FROM T_ItemMaster;");
                List<Item> items = new List<Item>();
                while (reader.Read())
                {
                    Item it = new();
                    it.ID = int.Parse(reader["ID"].ToString());
                    it.ItemPrice = float.Parse(reader["ItemPrice"].ToString());
                    it.ItemCategory = reader["ItemCategory"].ToString();
                    it.ItemName = reader["ItemName"].ToString();
                    it.ItemDescription = reader["ItemDescription"].ToString();
                    it.ItemQuantity = reader["ItemQuantity"].ToString();
                    it.ItemImage = reader["ItemImage"].ToString();
                    items.Add(it);
                }
                ItemMaster.Set(items.ToArray());
            }
        }
        static OleDbDataReader SQL(string query)
        {
            return new OleDbCommand(query, conn).ExecuteReader();
        }
        static public void Open()
        {
            if (OperatingSystem.IsWindows())
                conn.Open();
        }
        static public void Close()
        {
            if (OperatingSystem.IsWindows())
                conn.Close();
        }

    }
}

