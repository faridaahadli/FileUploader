using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.DbModels
{
    public class CustomFile
    {
        public int Id { get; set; }
        [Required]
        public string Source { get; set; }
        [Required]
        public string Extension { get; set; }
        [Required(ErrorMessage ="Please,select the category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
