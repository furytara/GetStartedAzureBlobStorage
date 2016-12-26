using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRUDBlogStorage.Models
{
    public class ImageUploadViewModel
    {
        public string Guid { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]        
        public HttpPostedFileBase FileAttachment { get; set; }
    }
}