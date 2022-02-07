using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoldenDelight_Suppliments.Models;
using System.Data.Entity;

namespace GoldenDelight_Suppliments.GoldenLogic
{
    public class SupplimentLogic
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Suppliment> all()
        {
            return db.Suppliments.Include(i => i.Category).ToList();
        }

        public bool add(Suppliment model)
        {
            try
            {
                db.Suppliments.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }

        public bool edit(Suppliment model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool delete(Suppliment model)
        {
            try
            {
                db.Suppliments.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }

        public Suppliment find_by_id(int? id)
        {
            return db.Suppliments.Find(id);
        }
        //public List<StockCart_Item> get_cart_items(int id)
        //{
        //    //return db.StockCart_Items.
        //}
        public void updateStock_Received(int item_id, int quantity)
        {
            var item = db.Suppliments.Find(item_id);
            item.Quantity += quantity;
            db.SaveChanges();
        }
        public void updateOrder(int id, double price)
        {
            var item = db.OrderItems.Find(id);
            item.Price = price;
            db.SaveChanges();
        }

    }
}