using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoldenDelight_Suppliments.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public ICollection<Suppliment> Suppliment { get; set; }

    }
}