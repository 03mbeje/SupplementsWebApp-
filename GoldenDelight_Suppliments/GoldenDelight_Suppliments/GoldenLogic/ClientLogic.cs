using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoldenDelight_Suppliments.Models;

namespace GoldenDelight_Suppliments.GoldenLogic
{
    public class ClientLogic
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";


        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[name: CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(value: System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = temp.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[name: CartSessionKey].ToString();
        }

        public void AddToCart(int id)
        {
            shoppingCartID = GetCartID();

            var item = db.Suppliments.Find(id);
            if (item != null)
            {
                var cartItem =
                    db.CartItems.FirstOrDefault(x => x.CartID == shoppingCartID && x.SupID == item.SupID);
                if (cartItem == null)
                {
                    var cart = db.Carts.Find(shoppingCartID);
                    if (cart == null)
                    {
                        db.Carts.Add(entity: new Cart()
                        {
                            CartId = shoppingCartID,
                            DateCreated = DateTime.Now
                        });
                        db.SaveChanges();
                    }

                    db.CartItems.Add(entity: new CartItem()
                    {
                        ItemID = Guid.NewGuid().ToString(),
                        CartID = shoppingCartID,
                        SupID = item.SupID,
                        DateAdded = DateTime.Now,
                        Quantity = 1,
                        Price = item.Price
                    }
                        );
                }
                else
                {
                    cartItem.Quantity++;
                }
                db.SaveChanges();
            }
        }

        public void RemoveFromCart(string id)
        {
            shoppingCartID = GetCartID();

            var item = db.CartItems.Find(id);
            if (item != null)
            {
                var cartItem =
                    db.CartItems.FirstOrDefault(predicate: x => x.CartID == shoppingCartID && x.ItemID == item.ItemID);
                if (cartItem != null)
                {
                    db.CartItems.Remove(entity: cartItem);
                }
                db.SaveChanges();
            }
        }

        public void EmptyCart()
        {
            shoppingCartID = GetCartID();
            foreach (var item in db.CartItems.ToList().FindAll(match: x => x.CartID == shoppingCartID))
            {
                db.CartItems.Remove(item);
            }
            try
            {
                db.Carts.Remove(db.Carts.Find(shoppingCartID));
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }

        public List<CartItem> GetCartItems()
        {
            shoppingCartID = GetCartID();
            return db.CartItems.ToList().FindAll(match: x => x.CartID == shoppingCartID);
        }


        public void UpdateCart(string id, int qty)
        {
            var item = db.CartItems.Find(id);
            if (qty < 0)
                item.Quantity = qty / -1;
            else if (qty == 0)
                RemoveFromCart(item.ItemID);
            else
                item.Quantity = qty;
            db.SaveChanges();
        }

        public double GetCartTotal(string id)
        {
            double amount = 0;
            foreach (var item in db.CartItems.ToList().FindAll(match: x => x.CartID == id))
            {
                amount += (item.Price * item.Quantity);
            }
            return amount;
        }

        public double GetOrdertTotal(int id)
        {
            double amount = 0;
            foreach (var item in db.OrderItems.ToList().FindAll(match: x => x.OrderID == id))
            {
                amount += (item.Price * item.Quantity);
            }
            return amount;
        }

        public void UpdateStock(int id)
        {
            var order = db.Orders.Find(id);
            List<OrderItem> items = db.OrderItems.ToList().FindAll(x => x.OrderID == id);
            foreach (var item in items)
            {
                var product = db.Suppliments.Find(item.SupID);
                if (product != null)
                {
                    if ((product.Quantity) >= 0)
                    {
                        product.Quantity -= item.Quantity;
                    }
                    else
                    {
                        item.Quantity = product.Quantity;
                        product.Quantity = 0;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex) { }
                }
            }
        }
    }
}