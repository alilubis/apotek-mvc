using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Tesseract;

namespace OCR.MVC.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public const string folderName = "images/";
        public const string fileName = "download.jpg";
        public const string trainedDataFolderName = "tessdata";
        public IActionResult Index()
        {
            ViewBag.ShowResult = false;
            return View();
        }

        [HttpPost]
        public JsonResult SaveCapture(string data)
        {
            byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);
            MemoryStream stream = new(imageBytes);
            IFormFile file = new FormFile(stream, 0, imageBytes.Length, "download", "download" + ".jpg");
            var filePath = Path.Combine(folderName, fileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
            }

            var result = ScanImage();
            return Json(new { result });
        }

        public String ScanImage()
        {
            string tessPath = Path.Combine(trainedDataFolderName, "");
            var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default);
            var img = Pix.LoadFromFile(folderName + fileName);

            var page = engine.Process(img);
            string result = page.GetText();
            return result;
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
