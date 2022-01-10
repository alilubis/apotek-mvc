using Microsoft.AspNetCore.Mvc;

namespace OCR.MVC.WebApp.Controllers
{
    public class TransactionsController : Controller
    {
        public IActionResult Order()
        {
            return View();
        }
        public IActionResult Selling()
        {
            return View();
        }
        public IActionResult Cashier()
        {
            return View();
        }
    }
}
