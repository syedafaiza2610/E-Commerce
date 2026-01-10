    using System.ComponentModel.DataAnnotations;
namespace Ecommerce_Project.Models
{


    public class Product
    {
        [Key]
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int product_price { get; set; }
        public string product_description { get; set; }
        public string product_image { get; set; }
        public int cat_id { get; set; }
        public Category Category { get; set; }
    }
}
