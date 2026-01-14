using Ecommerce_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(myContext context, IWebHostEnvironment env)
        {
            this._context = context;
            this._env = env;
        }
        public IActionResult Index()
        {
            string admin_session = HttpContext.Session.GetString("admin_session");
            if (admin_session != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string adminEmail, string adminPassword)
        {
            var row = _context.tbl_admin.FirstOrDefault(x => x.admin_email == adminEmail && x.admin_password == adminPassword);
            if (row != null && row.admin_password == adminPassword)
            {
                HttpContext.Session.SetString("admin_session", row.admin_id.ToString());
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.message = "Invalid Username and Password";
                return View();
            }

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin_session");
            return RedirectToAction("Login");
        }
        public IActionResult Profile()
        {
            var adminId = HttpContext.Session.GetString("admin_session");
            var row = _context.tbl_admin.Where(x => x.admin_id == int.Parse(adminId)).ToList();

            return View(row);
        }
        [HttpPost]
        public IActionResult Profile(Admin admin)
        {
            _context.tbl_admin.Update(admin);
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }
        public IActionResult ChangeProfileImage(IFormFile admin_image, Admin admin)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "Admin_image", admin_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            admin_image.CopyTo(fs);
            admin.admin_image = admin_image.FileName;
            _context.tbl_admin.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }
        public IActionResult fetchCustomer()
        {
            return View(_context.tbl_customer.ToList());
        }
        public IActionResult CustomerDetails(int id)
        {
            return View(_context.tbl_customer.FirstOrDefault(c => c.customer_id == id));
        }
        public IActionResult UpdateCustomer(int id)
        {
            var customer = _context.tbl_customer.Find(id);
            return View(customer);
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customer customer, IFormFile customer_image)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "Customer_image", customer_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            customer_image.CopyTo(fs);
            customer.customer_image = customer_image.FileName;
            _context.tbl_customer.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("fetchCustomer");
        }
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.tbl_customer.Find(id);
            _context.tbl_customer.Remove(customer);
            _context.SaveChanges();
            return RedirectToAction("fetchCustomer");
        }
        public IActionResult DeletePermission(int id)
        {
            return View(_context.tbl_customer.FirstOrDefault(c => c.customer_id == id));
        }
        public IActionResult fetchCategory()
        {
            return View(_context.tbl_category.ToList());
        }
        public IActionResult addCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult addCategory(Category cat)
        {
            _context.tbl_category.Add(cat);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }
        public IActionResult updateCategory(int id)
        {
            var category = _context.tbl_category.Find(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult updateCategory(Category cat)
        {
            _context.tbl_category.Update(cat);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }

        public IActionResult deleteCategory(int id)
        {
            var category = _context.tbl_category.Find(id);
            _context.tbl_category.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");

        }
        public IActionResult deletePermissionCategory(int id)
        {
            return View(_context.tbl_category.FirstOrDefault(c => c.category_id == id));
        }

        public IActionResult fetchProduct()
        {
            return View(_context.tbl_product.ToList());
        }
        public IActionResult addProduct()
        {
            List<Category> categories = _context.tbl_category.ToList();
            ViewData["category"] = categories;
            return View();
        }

        [HttpPost]
        public IActionResult addProduct(Product prod, IFormFile product_image)
        {
            string imageName = Path.Combine(_env.WebRootPath, "Product_images", product_image.FileName);
            FileStream fs = new FileStream(imageName, FileMode.Create);
            product_image.CopyTo(fs);
            prod.product_image = product_image.FileName;
            _context.tbl_product.Add(prod);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");

        }
        public IActionResult ProductDetails(int id)
        {
            return View(_context.tbl_product.Include(p => p.Category).FirstOrDefault(p => p.product_id == id));
        }
        public IActionResult updateProduct(int id)
        {
            List<Category> categories = _context.tbl_category.ToList();
            ViewData["category"] = categories;
            var product = _context.tbl_product.Find(id);
            ViewBag.categoryselectedId = product.cat_id;
            return View(product);
        }
        [HttpPost]
        public IActionResult updateProduct(Product prod)
        {
            _context.tbl_product.Update(prod);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");
        }

      
        [HttpPost]
        public IActionResult ChangeProductImage(IFormFile product_image, Product product)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "Product_images", product_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            product_image.CopyTo(fs);
            product.product_image = product_image.FileName;
            _context.tbl_product.Update(product);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");
        }
        public IActionResult deleteProduct(int id)
        {
            var product = _context.tbl_product.Find(id);
            _context.tbl_product.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");

        }
        public IActionResult deletePermissionProduct(int id)
        {
            return View(_context.tbl_product.Include(p => p.Category).FirstOrDefault(p => p.product_id == id));
        }
        public IActionResult fetchfeedback()
        {
            return View(_context.tbl_feedback.ToList());
        }
        public IActionResult deletefeedback(int id)
        {
            var feedback = _context.tbl_feedback.Find(id);
            _context.tbl_feedback.Remove(feedback);
            _context.SaveChanges();
            return RedirectToAction("fetchfeedback");

        }
        public IActionResult deletePermissionfeedback(int id)
        {
            return View(_context.tbl_feedback.FirstOrDefault(f => f.feedback_id == id));
        }
    }

}
