using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using System.Xml;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.res = TempData["res"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Upload()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormFile file, string text, string typee, string mes)
        {
            if (mes==null)
            {
                var fileName = Path.GetFileName(file.FileName);
                string tex = string.Empty;
                var ext = Path.GetExtension(fileName).ToLowerInvariant();
                if (ext == ".docx" || ext == ".txt")
                    if (ext == ".docx")
                    {
                        using (var localFile = System.IO.File.OpenWrite("ff.docx"))
                        using (var uploadedFile = file.OpenReadStream())
                        {
                            uploadedFile.CopyTo(localFile);
                        }
                        using (var wordyDocument = WordprocessingDocument.Open("ff.docx", false))
                        {
                            tex = wordyDocument.MainDocumentPart.Document.Body.InnerText;
                        }
                        System.IO.File.Delete("ff.docx");
                    }
                    else
                    {
                        using (var localFile = System.IO.File.OpenWrite("ff.txt"))
                        using (var uploadedFile = file.OpenReadStream())
                        {
                            uploadedFile.CopyTo(localFile);
                        }
                        tex = System.IO.File.ReadAllText("ff.txt");
                        System.IO.File.Delete("ff.txt");
                    }
                if (typee.Contains("ye"))
                {
                    TempData["res"] = FuncSupply.Decrypt(tex, text);
                }
                else
                {
                    TempData["res"] = FuncSupply.Encrypt(tex, text);
                }
            }
            else
            {
                if (typee.Contains("ye"))
                {
                    TempData["res"] = FuncSupply.Decrypt(mes, text);
                }
                else
                {
                    TempData["res"] = FuncSupply.Encrypt(mes, text);
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult DownloadTxt(string result)
        {
            if (result!=null)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(result);
                MemoryStream stream = new MemoryStream(byteArray);
                return File(stream, FuncSupply.GetContentType("fff.txt"), Path.GetFileName("Result.txt"));
            }
            else return NoContent();
        }
        public IActionResult DownloadDocx(string result)
        {
            if (result!=null)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(result);
                using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(Path.GetTempPath() + "ffff.docx", WordprocessingDocumentType.Document))
                {
                    // Add a main document part. 
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    // Create the document structure and add some text.
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    Paragraph para = body.AppendChild(new Paragraph());
                    Run run = para.AppendChild(new Run());
                    run.AppendChild(new Text(result));
                }
                return PhysicalFile(Path.GetTempPath() + "ffff.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "ffds.docx");
            }
            else return NoContent();
        }

    }
}