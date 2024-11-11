using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.Core.Utils
{
    public class FileUploadHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly static string _customDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");

        public FileUploadHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            if (!Directory.Exists(_customDirectory))
            {
                Directory.CreateDirectory(_customDirectory);
            }
        }

        public static async Task<string> UploadFile(IFormFile file)
        {
            if (file.Length > 0)
            {
                try
                {
                    string fileExtension = Path.GetExtension(file.FileName); 
                    string fileName = $"{fileExtension}"; 
                    string filePath = Path.Combine(_customDirectory, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return fileName;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return "Upload failed, file is empty.";
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
