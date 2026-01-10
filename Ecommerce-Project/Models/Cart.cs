using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Project.Models
{
    public class Cart
    {
        [Key]
        public int cart_id { get; set; }
        public int prod_id { get; set; }
        public int cust_id { get; set; }
        public int product_quantity { get; set; }
        public string cart_status { get; set; }
    }
}
