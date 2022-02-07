using GoldenDelight_Suppliments.GoldenLogic;
using GoldenDelight_Suppliments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Configuration;
using System.Text;
using SendGrid;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;

namespace GoldenDelight_Suppliments.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ClientLogic obj = new ClientLogic();
        public string shoppingCartID { get; set; }

        // GET: Shopping
        //public ActionResult Index() //Displays products
        //{
        //    int cartItems = obj.GetCartItems().Count();
        //    if (cartItems != 0)
        //    {
        //        ViewBag.TotalItems = cartItems;
        //    }
        //    shoppingCartID = obj.GetCartID();
        //    return View(db.Suppliments.ToList());
        //}


        public ActionResult Index(string searchProduct)
        {
            int cartItems = obj.GetCartItems().Count();
            if (cartItems != 0)
            {
                ViewBag.TotalItems = cartItems;
            }
            shoppingCartID = obj.GetCartID();

            var suppliments = db.Suppliments.ToList();

            var ProductList = db.Suppliments.Where(x => x.SupName.Contains(searchProduct)).ToList();

            if (searchProduct != null)
            {
                var Productsearch = db.Suppliments.Where(x => x.SupName.Contains(searchProduct)).ToList().Count();

                if (Productsearch == 0)
                {
                    ViewBag.NullEmail = " Sorry... There are no products related to this search (" + searchProduct + ")";
                    return View(ProductList);
                }
                else
                {
                    return View(ProductList);
                }
            }
            else
            {
                return View(suppliments);
            }
        }



        public ActionResult ViewCart()
        {
            shoppingCartID = obj.GetCartID();
            ViewBag.Total = obj.GetCartTotal(id: shoppingCartID);
            ViewBag.TotalQTY = obj.GetCartItems().FindAll(x => x.CartID == shoppingCartID).Sum(q => q.Quantity);
            ViewBag.TotalItems = obj.GetCartItems().FindAll(x => x.CartID == shoppingCartID).Sum(q => q.Quantity);

            var CartItems = db.CartItems.Include(c => c.Suppliment).ToList().FindAll(x => x.CartID == shoppingCartID);

            return View(CartItems);
        }

        [HttpPost]
        public ActionResult ViewCart(List<CartItem> items)
        {
            shoppingCartID = obj.GetCartID();

            foreach (var i in items)
            {
                obj.UpdateCart(i.ItemID, i.Quantity);
            }
            ViewBag.Total = obj.GetCartTotal(shoppingCartID);
            ViewBag.TotalQTY = obj.GetCartItems().FindAll(x => x.CartID == shoppingCartID).Sum(q => q.Quantity);
            ViewBag.TotalItems = obj.GetCartItems().FindAll(x => x.CartID == shoppingCartID).Sum(q => q.Quantity);
            return View(db.CartItems.Include(e => e.Suppliment).ToList().FindAll(x => x.CartID == shoppingCartID));
        }

        public ActionResult AddToCart(int id)
        {
            var item = db.Suppliments.Find(id);
            if (item != null)
            {
                obj.AddToCart(id);
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }

        public ActionResult RemoveFromCart(string id)
        {
            var item = db.CartItems.Find(id);
            if (item != null)
            {
                obj.RemoveFromCart(id: id);
                return RedirectToAction("ViewCart");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }

        public ActionResult PlaceOrder(int id)
        {
            var customer = db.ClientProfiles.ToList().Find(x => x.Email == HttpContext.User.Identity.Name);
            db.Orders.Add(new Order()
            {
                Email = customer.Email,
                DateCreated = DateTime.Now,
                PaymentStatus = "Awaiting",
                ProcessStatus = "Pending",
            });
            db.SaveChanges();
            var order = db.Orders.ToList()
                .FindAll(x => x.Email == customer.Email)
                .LastOrDefault();

            var items = obj.GetCartItems();

            foreach (var item in items)
            {
                var x = new OrderItem()
                {

                    OrderID = order.OrderID,
                    SupID = item.SupID,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                db.OrderItems.Add(x);
                db.SaveChanges();
            }
            obj.EmptyCart();

            return RedirectToAction("OrderSummary", new { id = order.OrderID });
        }

        public ActionResult OrderSummary(int id)
        {
            var order = db.Orders.Find(id);
            OrderViewModel summary = new OrderViewModel();
            summary.OrderId = id;
            summary.orderItems = summary.GetItems(id);
            summary.clientAddresses = db.ClientAddresses.Where(x => x.Email == User.Identity.Name).OrderByDescending(x=>x.AddressID).FirstOrDefault();
            ViewBag.AllTotal = (obj.GetOrdertTotal(id) + 1500).ToString("C");

            return View(summary);
        }
        public ActionResult OrderSuccess(int? id)
        {
            if (id != null)
            {
                var order = db.Orders.Find(id);

                obj.UpdateStock((int)id);

                try
                {
                    order.PaymentStatus = "Paid";
                    db.SaveChanges();
                }
                catch (Exception ex) { }
            }
            return View();
        }

        public ActionResult Secure_Payment(int id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = db.ClientProfiles.Find(order.Email);
            ViewBag.Items = db.OrderItems.ToList().FindAll(x => x.OrderID == order.OrderID);
            ViewBag.Total = obj.GetOrdertTotal(order.OrderID);

            string tot = (obj.GetOrdertTotal(id) + 1500).ToString();

            return Redirect(PaymentLink(tot, "Order Payment | Order No: " + order.OrderID, order.OrderID));
        }

        public string PaymentLink(string totalCost, string paymentSubjetc, int order_id)
        {
            string paymentMode = ConfigurationManager.AppSettings["PaymentMode"], site, merchantId, merchantKey, returnUrl;

            site = "https://sandbox.payfast.co.za/eng/process?";
            merchantId = "10022900";
            merchantKey = "qq34viiias2on";
            returnUrl = "https://goldendelightsuppliments21934306.azurewebsites.net/Home/OrderSuccess/";
            
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("merchant_id=" + HttpUtility.HtmlEncode(merchantId));
            stringBuilder.Append("&merchant_key=" + HttpUtility.HtmlEncode(merchantKey));
            stringBuilder.Append("&return_url= " + HttpUtility.HtmlEncode("https://goldendelightsuppliments21934306.azurewebsites.net/Home/OrderSuccess/" + order_id));
   
            string amt = totalCost;
            amt = amt.Replace(",", ".");

            stringBuilder.Append("&amount=" + HttpUtility.HtmlEncode(amt));
            stringBuilder.Append("&item_name=" + HttpUtility.HtmlEncode(paymentSubjetc));
            stringBuilder.Append("&email_confirmation=" + HttpUtility.HtmlEncode("1"));
            stringBuilder.Append("&confirmation_address=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_ConfirmationAddress"]));

            return (site + stringBuilder);
        }

        //Fetches All Processed Customer Orders 
        public ActionResult MyOrders()
        {
            var CusOrders = new List<Order>();
            var GetCustomer = db.Orders.Include(x => x.ClientProfile).Where(x => x.ClientProfile.Email == User.Identity.Name && x.ProcessStatus == "Processed").OrderByDescending(x=>x.OrderID).ToList();

            foreach (var item in GetCustomer)
            {
                CusOrders = db.Orders.Where(x => x.Email == item.Email).OrderByDescending(x=>x.OrderID).ToList();
            }
            return View(CusOrders);

        }
        //Fetches Active Orders Only
        public ActionResult TrackOrder()
        {
            var ActiveOrders = new List<Order>();
            var GetCustomer = db.Orders.Where(x => x.ProcessStatus != "Complete").Include(x => x.ClientProfile).Where(x => x.ClientProfile.Email == User.Identity.Name).OrderByDescending(x=>x.OrderID).ToList();

            foreach (var item in GetCustomer)
            {
                ActiveOrders = db.Orders.Where(x => x.Email == item.Email).OrderByDescending(x=>x.OrderID).ToList();
            }
            return View(ActiveOrders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = db.Orders.Find(id);
            try
            {
                ViewBag.Items = db.OrderItems.ToList().FindAll(x => x.OrderID == order.OrderID);


                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<OrderItem>)ViewBag.Items)
                {
                    string items = "<tr> " +
                                   "<td>" + item.Suppliment.SupName + " </td>" +
                                   "<td>" + item.Quantity + " </td>" +
                                   "<td>R " + item.Price + " </td>" +
                                   "<tr/>";
                    table += items;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + obj.GetOrdertTotal(order.OrderID).ToString("C") + "</th>" +
                         "</tr>";
                table += "</table>";
            }
            catch (Exception ex) { }

            ViewBag.Order = order;
            ViewBag.Account = db.ClientProfiles.Find(order.Email);
            ViewBag.Total = obj.GetOrdertTotal(order.OrderID).ToString("C");
            ViewBag.Address = db.ClientAddresses.Where(x => x.Email == order.Email).OrderByDescending(x => x.AddressID).FirstOrDefault();

            return View(order);
        }

        [HttpGet]
        public ActionResult ScheduleDelivery(int id)
        {
            Shipment shipment = new Shipment();

            var order = db.Orders.Find(id);
            shipment.OrderID = order.OrderID;
            shipment.Email = order.Email;
            return PartialView(shipment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleDelivery(Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                var status = db.Orders.Find(shipment.OrderID);
                status.ProcessStatus = "Processed";

                shipment.ArrivalDate = shipment.DeliveryDate.AddDays(shipment.EstimateDuration);
                db.Shipments.Add(shipment);
                db.SaveChanges();
                return RedirectToAction("Index", "Orders");
            }
            return View(shipment);
        }

        public ActionResult ShipmentDetails(int id)
        {
            var shipment = db.Shipments.Where(x => x.OrderID == id)
                .Include(x=>x.ClientProfile)
                .Include(x=>x.Order).FirstOrDefault();
            return PartialView(shipment);
        }
    }
}