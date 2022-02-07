using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoldenDelight_Suppliments.Models
{
    public class CartItem
    {
        [Key]
        public string ItemID { get; set; }

        [DisplayName("Cart ID")]
        public string CartID { get; set; }

        [DisplayName("Suppliment")]
        public int SupID { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }

        [DisplayName("Quantity")]
        public int Quantity { get; set; }

        [DisplayName("Quantity Cost")]
        public double Price { get; set; }

        public Cart Cart { get; set; }
        public Suppliment Suppliment { get; set; }
    }
}