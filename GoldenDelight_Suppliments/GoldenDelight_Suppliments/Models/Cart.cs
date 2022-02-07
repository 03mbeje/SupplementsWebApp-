using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoldenDelight_Suppliments.Models
{
    public class Cart
    {
        public string CartId { get; set; }

        public DateTime DateCreated { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}