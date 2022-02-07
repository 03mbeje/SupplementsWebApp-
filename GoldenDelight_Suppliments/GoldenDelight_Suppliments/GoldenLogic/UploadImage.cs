using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.IO;
using System.Web;
using GoldenDelight_Suppliments.Models;

namespace GoldenDelight_Suppliments.GoldenLogic
{
    public class UploadImage
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public int UploadImageInDataBase(HttpPostedFileBase file, Suppliment suppliment)
        {
            suppliment.SupImage = ConvertToBytes(file);

            var supItem = new Suppliment
            {
                SupName = suppliment.SupName,
                SupDesc = suppliment.SupDesc,
                Quantity = suppliment.Quantity,
                Price = suppliment.Price,
                CategoryID = suppliment.CategoryID,
                SupImage = suppliment.SupImage
            };
            db.Suppliments.Add(supItem);
            int i = db.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
            public byte[] ConvertToBytes(HttpPostedFileBase image)
            {
                byte[] imageBytes = null;
                BinaryReader reader = new BinaryReader(image.InputStream);
                imageBytes = reader.ReadBytes((int)image.ContentLength);
                return imageBytes;
            }
    }
}