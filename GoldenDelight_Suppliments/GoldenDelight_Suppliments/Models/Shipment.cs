using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GoldenDelight_Suppliments.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentID { get; set; }
        public int OrderID { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }

        public int EstimateDuration { get; set; }

        public DateTime ArrivalDate { get; set; }

        public Order Order { get; set; }
        public ClientProfile ClientProfile { get; set; }

    }
}