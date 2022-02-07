using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldenDelight_Suppliments.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OItemID { get; set; }
        public int SupID { get; set; }
        public int OrderID { get; set; }

        [DisplayName("Quantity")]
        public int Quantity { get; set; }

        [DisplayName("Price")]
        public double Price { get; set; }

        public virtual Suppliment Suppliment { get; set; }
        public virtual Order Order { get; set; }
    }
}