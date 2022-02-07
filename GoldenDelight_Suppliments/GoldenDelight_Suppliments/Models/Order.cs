using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldenDelight_Suppliments.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("ClientProfile")]
        public string Email { get; set; }

        [DisplayName("DateCreated")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        
        [DisplayName("Process Status")]
        public string ProcessStatus { get; set; }

        [DisplayName("Payment Status")]
        public string PaymentStatus { get; set; }

        //Payments
        public ICollection<OrderItem> OrderItems { get; set; }
        public ClientProfile ClientProfile { get; set; }

    }
}