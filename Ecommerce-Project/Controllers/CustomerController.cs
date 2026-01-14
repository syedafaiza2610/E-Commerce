using Ecommerce_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Project.Controllers
{
    public class CustomerController : Controller
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _env;

        public CustomerController(myContext context, IWebHostEnvironment env)
        {
            this._context = context;
            this._env = env;
        }
        public IActionResult Index()
        {
            List<Category> category= _context.tbl_category.ToList();
            ViewData["category"] = category;
            List<Product> products = _context.tbl_product.ToList();
            ViewData["product"] = products;
            ViewBag.checkSession = HttpContext.Session.GetString("customerSession");
            return View();
        }
        public IActionResult customerLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerLogin(string customerEmail, string customerPassword)
        {
            var customer = _context.tbl_customer
                .FirstOrDefault(c => c.customer_email == customerEmail);

            if (customer != null && customer.customer_password == customerPassword)
            {
                HttpContext.Session.SetString("customerSession",customer.customer_id.ToString());

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = "Incorrect Username or Password";
                return View();
            }
        }

        public IActionResult customerRegister()
        {
            return View();
        }
        [HttpPost]
        public IActionResult customerRegister(Customer customer)
        {
            _context.tbl_customer.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("customerLogin");
        }
        public IActionResult customerLogout()
        {
            HttpContext.Session.Remove("customerSession");
            return RedirectToAction("Index");
        }
        public IActionResult customerProfile()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerSession")))
            {
                return RedirectToAction("customerLogin");
            }
            else
            {
                List<Category> category = _context.tbl_category.ToList();
                ViewData["category"] = category;
                var customerId = HttpContext.Session.GetString("customerSession");
                var row = _context.tbl_customer.Where(c => c.customer_id == int.Parse(customerId)).ToList();
                return View(row);
            }
        }
        [HttpPost]
        public IActionResult updateProfile(Customer customer)
        {
            _context.tbl_customer.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("customerProfile");
        }
        public IActionResult ChangeProfileImage(IFormFile customer_image, Customer customer)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "Customer_image", customer_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            customer_image.CopyTo(fs);
            customer.customer_image = customer_image.FileName;
            _context.tbl_customer.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("customerProfile");
        }
        public IActionResult feedback()
        {
            List<Category> category = _context.tbl_category.ToList();
            ViewData["category"] = category;
            return View();
        }
        [HttpPost]
        public IActionResult feedback(Feedback feedback)
        {
            TempData["message"] = "Thank You For Your Feedback";
            _context.tbl_feedback.Add(feedback);
            _context.SaveChanges();
            return RedirectToAction("feedback");
        }
        public IActionResult fetchAllProducts()
        {
            List<Category> category = _context.tbl_category.ToList();
            ViewData["category"] = category;
            List<Product> products = _context.tbl_product.ToList();
            ViewData["product"] = products;
            return View();
        }
        public IActionResult productDetails(int id)
        {
            List<Category> category = _context.tbl_category.ToList();
            ViewData["category"] = category;
            List<Product> products = _context.tbl_product.ToList();
            ViewData["product"] = products;
            var product = _context.tbl_product.Where(p => p.product_id == id).ToList();
            return View(product);
        }
        public IActionResult About()
        {
            List<Category> category = _context.tbl_category.ToList();
            ViewData["category"] = category;
            List<Product> products = _context.tbl_product.ToList();
            ViewData["product"] = products;
            return View();
        }
    }
}
