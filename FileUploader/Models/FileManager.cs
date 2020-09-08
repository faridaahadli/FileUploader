using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileUploader.Models
{
    public class FileManager
    {
        public static Dictionary<string, string> MimeTypes = new Dictionary<string, string>
        {
            {"jpg","image/jpg"},
            {"jpeg", "image/jpeg"},
            {"png", "image/png"},
            {"gif", "image/gif"},
            {"doc","application/msword/doc" },
            {"docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            {"pdf","application/pdf" }

        };
        public static bool CheckFileType(IFormFile uploadFile)
        {
            return MimeTypes.ContainsValue(uploadFile.ContentType);
        }
        public static async Task<string> SaveFile(string folder, IFormFile uploadFile)
        {
            string filename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadFile.FileName);

            string path = Path.Combine(folder, filename);

            using (var stream = new FileStream(path, FileMode.Create))

            {

                await uploadFile.CopyToAsync(stream);

            }
            return filename;
        }
    }
}
