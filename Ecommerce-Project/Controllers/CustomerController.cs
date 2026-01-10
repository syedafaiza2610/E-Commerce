using Ecommerce_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Project.Controllers
{
    public class CustomerController : Controller
    {
        private readonly myContext _context;

        public CustomerController(myContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            List<Category> category= _context.tbl_category.ToList();
            ViewData["category"] = category;
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

    }
}
