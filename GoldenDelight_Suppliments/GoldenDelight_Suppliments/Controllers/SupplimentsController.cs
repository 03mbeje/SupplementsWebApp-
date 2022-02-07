using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoldenDelight_Suppliments.Models;
using GoldenDelight_Suppliments.GoldenLogic;

namespace GoldenDelight_Suppliments.Controllers
{
    public class SupplimentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private SupplimentLogic ib = new SupplimentLogic();
        CategoryLogic cb = new CategoryLogic();
        public string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";
        public SupplimentsController() { }

        // GET: Suppliments
        public ActionResult Index(string searchProduct)
        {
            var suppliments = db.Suppliments.Include(s => s.Category).OrderByDescending(x=>x.SupID).ToList();

            var ProductList = db.Suppliments.Where(x => x.SupName.Contains(searchProduct) ||
                                         x.Category.CategoryName.Contains(searchProduct))
                                         .Include(x => x.Category)
                                         .OrderByDescending(x=>x.SupID)
                                         .ToList();

            if (searchProduct != null)
            {
                var Productsearch = db.Suppliments.Where(x => x.SupName.Contains(searchProduct) || 
                                                  x.Category.CategoryName.Contains(searchProduct))
                                                  .Include(x => x.Category).ToList().Count();

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

        // GET: Suppliments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliment suppliment = db.Suppliments.Include(x=>x.Category).FirstOrDefault();
            if (suppliment == null)
            {
                return HttpNotFound();
            }
            return View(suppliment);
        }



        /// <summary>
        /// Retrive Image from database 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Suppliments where temp.SupID == Id select temp.SupImage;
            byte[] cover = q.First();
            return cover;
        }

        // GET: Suppliments/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Suppliments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliment suppliment)
        {
            ViewBag.CategoryID = new SelectList(cb.all(), "CategoryID", "CategoryName");
            HttpPostedFileBase file = Request.Files["ImageData"];
            UploadImage service = new UploadImage();
            int i = service.UploadImageInDataBase(file, suppliment);
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(suppliment);
        }

        // GET: Suppliments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliment suppliment = db.Suppliments.Find(id);
            if (suppliment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", suppliment.CategoryID);
            return View(suppliment);
        }

        // POST: Suppliments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliment suppliment, HttpPostedFileBase ImageData)
        {
            if (ModelState.IsValid)
            {
                if (ImageData != null)
                {
                    suppliment.SupImage = new byte[ImageData.ContentLength];
                    ImageData.InputStream.Read(suppliment.SupImage, 0, ImageData.ContentLength);
                }
                db.Entry(suppliment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", suppliment.CategoryID);
            return View(suppliment);
        }

        // GET: Suppliments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliment suppliment = db.Suppliments.Include(x => x.Category).FirstOrDefault();
            if (suppliment == null)
            {
                return HttpNotFound();
            }
            return View(suppliment);
        }

        // POST: Suppliments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliment suppliment = db.Suppliments.Find(id);
            db.Suppliments.Remove(suppliment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Fall_catalog()
        {
            return View(ib.all());
        }

        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp_cart_ID = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[CartSessionKey] = temp_cart_ID.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[CartSessionKey].ToString();
        }



    }
}
