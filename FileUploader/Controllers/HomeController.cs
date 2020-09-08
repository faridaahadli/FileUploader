using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileUploader.Models;
using FileUploader.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.IO.Compression;


namespace FileUploader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context=context;
            _hostingEnvironment = hostingEnvironment;
        }
       

        public IActionResult Index()
        {
            AddFileViewModel model = new AddFileViewModel();
            model.Categories = _context.Categories.ToList();
            model.Files = _context.CustomFiles.ToList();
            return View(model);
        }

        public IActionResult Category()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult CreateCategory(string category)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "file", category);
            if (category == null)
                return View("Category");
            if (Directory.Exists(path))
            {
                return View("Category");
            }
            Directory.CreateDirectory(path);
            Category categoryObj = new Category();
            categoryObj.Name = category;
            _context.Categories.Add(categoryObj);
            _context.SaveChanges();
            return RedirectToAction("Category");
        }
        [HttpPost]
        public async Task<IActionResult> AddFileAsync(IFormFile source,int category)
        {
            if (source == null || !FileManager.CheckFileType(source))
                return View("Category");
            CustomFile addedFile = new CustomFile();
            string cat = _context.Categories.FirstOrDefault(x => x.Id == category).Name;
            var res=await FileManager.SaveFile(Path.Combine(_hostingEnvironment.WebRootPath, "file",cat), source);
            addedFile.Source = res;
            addedFile.CategoryId = category;
           
            addedFile.Extension = Path.GetExtension(addedFile.Source);
            _context.CustomFiles.Add(addedFile);
            _context.SaveChanges();
            return View("Category");
        }
        [HttpPost]
        public async Task<IActionResult> DownloadFile(string[] selectedFile)
        {
            
           using (var stream = new MemoryStream())
            {
               
                    using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create))
                    {
                    for(int i = 0; i < selectedFile.Length; i++)
                    {
                        var catId = _context.CustomFiles.FirstOrDefault(p => p.Source == selectedFile[i]).CategoryId;
                        var filename = _context.Categories.First(p => p.Id == catId).Name;
                        var path = Path.Combine(_hostingEnvironment.WebRootPath, "file", filename, selectedFile[i]);
                        archive.CreateEntryFromFile(path, selectedFile[i]);
                    }
                        
                    }
               
                
                return File(stream.ToArray(), "application/zip", "Archive.zip");
            }

        }

    }
}
