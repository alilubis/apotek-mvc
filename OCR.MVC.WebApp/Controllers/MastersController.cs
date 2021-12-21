using Microsoft.AspNetCore.Mvc;

namespace OCR.MVC.WebApp.Controllers
{
    public class MastersController : Controller
    {
        public IActionResult Medicines()
        {
            return View();
        }
        public IActionResult Doctors()
        {
            return View();
        }
        public IActionResult Patients()
        {
            return View();
        }
    }
}
