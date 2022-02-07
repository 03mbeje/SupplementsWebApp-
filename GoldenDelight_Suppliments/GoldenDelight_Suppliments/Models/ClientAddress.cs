using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldenDelight_Suppliments.Models
{
    public class ClientAddress
    {
        [Key]
        public int AddressID { get; set; }

        [Required]
        [DisplayName("Street")]
        public string Street { get; set; }

        [Required]
        [DisplayName("Suburb")]
        public string Suburb { get; set; }

        [Required]
        [DisplayName("City")]
        public string City { get; set; }

        [Required]
        [DisplayName("Province")]
        public string Province { get; set; }

        [Required]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [ForeignKey("ClientProfile")]
        public string Email { get; set; }

        public ClientProfile ClientProfile { get; set; }


    }
}
