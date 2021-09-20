using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Order
    {
        public Order() {
            OrderDetails = new List<OrderDetails>();
        }
        public int Id { get; set; }
        [Display(Name="Order No")]
        public String OrderNo { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Phone { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        public String Email { get; set; }
        [Required]
        public String Address { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual List<OrderDetails> OrderDetails { get; set; }
    }
}
