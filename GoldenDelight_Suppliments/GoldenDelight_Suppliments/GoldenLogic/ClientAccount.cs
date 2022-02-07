using GoldenDelight_Suppliments.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GoldenDelight_Suppliments.GoldenLogic
{
    public class ClientAccount
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<ClientProfile> all()
        {
            return db.ClientProfiles.ToList();
        }
        public bool add(ClientProfile model)
        {
            try
            {
                db.ClientProfiles.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(ClientProfile model)
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
        public ClientProfile find_by_id(int? id)
        {
            return db.ClientProfiles.Find(id);
        }

        public string getGender(string id_num)
        {
            if (Convert.ToInt16(id_num.Substring(7, 1)) >= 5)
                return "Male";
            else
                return "Female";
        }
    }
}