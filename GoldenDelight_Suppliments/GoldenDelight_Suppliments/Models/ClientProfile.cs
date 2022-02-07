﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoldenDelight_Suppliments.Models
{
    public class ClientProfile
    {
        [Key]
        public string Email { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("SA ID")]
        public string SAID { get; set; }

        [Display(Name = "Date Of Birth:")]
        public string BirthDate
        {
            get
            {
                string prefix = "19";

                if (SAID.Substring(0, 1) == "0" ||
                     SAID.Substring(0, 1) == "1" ||
                     SAID.Substring(0, 1) == "2" ||
                     SAID.Substring(0, 1) == "3")
                {
                    prefix = "20";
                }
                else
                {
                    prefix = "19";
                }
                return SAID != null ? prefix + SAID.Substring(0, 2) + "/" + SAID.Substring(2, 2) + "/" + SAID.Substring(4, 2) : "";
            }
            set
            {

            }
        }

        [Display(Name = "Age :")]
        public string Age
        {
            get
            {
                DateTime dt = DateTime.ParseExact(BirthDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime today = DateTime.Today;
                int age = today.Year - dt.Year;
                if (dt > today.AddYears(-age)) age--;
                return SAID != null ? age.ToString() : "";
            }
            set
            {

            }
        }
        [Display(Name = "Gender:")]
        public string Gender
        {
            get
            {
                var gendeCode = (SAID.Substring(6, 4));
                var gender = int.Parse(gendeCode) < 5000 ? "Female" : "Male";
                return SAID != null ? gender : "";
            }
            set
            {

            }
        }

        public ICollection<ClientAddress> ClientAddresses { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}