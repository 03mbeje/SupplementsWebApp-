using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GoldenDelight_Suppliments.Models
{
    public class Suppliment
    {
        [Key]
        public int SupID { get; set; }

        [DisplayName("Category")]
        public int CategoryID { get; set; }

        [DisplayName("Name")]
        public string SupName { get; set; }

        [DisplayName("Description")]
        public string SupDesc { get; set; }

        [DisplayName("Image")]
        public byte[] SupImage { get; set; }

        [DisplayName("Price")]
        public double Price { get; set; }

        [DisplayName("Quantity")]
        public int Quantity { get; set; }

        public Category Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }


    }
}