using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoldenDelight_Suppliments.Models
{
    public class OrderViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public int OrderId { get; set; }
        public IEnumerable<OrderItem> orderItems { get; set; }
        public ClientAddress clientAddresses { get; set; }

        public List<OrderItem> GetItems(int id)
        {
            List<OrderItem> items = db.OrderItems.Where(o => o.OrderID == id).ToList();
            return items;
        }

        //public List<ClientAddress> GetAddress(int id)
        //{
        //    List<ClientAddress> addresses = db.ClientAddresses.Where(o => o.AddressID == id).ToList();
        //    return addresses;
        //}
    }
}