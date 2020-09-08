using FileUploader.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Models
{
    public class AddFileViewModel
    {
        public List<Category> Categories { get; set; }
        
        public List<CustomFile> Files { get; set; }
    }
}
