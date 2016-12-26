using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDBlogStorage.Models
{
    public class BlobItemViewModel
    {
        public long Length { get; set; }
        public string Uri { get; set; }

        public BlobType Type { get; set; }
    }

    public enum BlobType {
        BlobBlock = 0, BlobPage = 1, BlobDirectory = 2
    }
}