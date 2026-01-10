using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Project.Models
{
    public class Faqs
    {
        [Key]
        public int faqd_id { get; set; }
        public string faqd_question { get; set; }
        public string faqd_answer { get; set; }
    }
}
