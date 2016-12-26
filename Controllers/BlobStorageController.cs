using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob;
using CRUDBlogStorage.Models;
using CRUDBlogStorage.Services;
using System.IO; // Namespace for Blob storage types

namespace CRUDBlogStorage.Controllers
{
    public class BlobStorageController : Controller
    {
        BlobStorageService _service = new BlobStorageService();

        // GET: BlobStorage
        public ActionResult Index()
        {
            List<BlobItemViewModel> items = new List<BlobItemViewModel>();

            // Retrieve a reference to a container.
            CloudBlobContainer container = _service.GetCloudBlogContainer();

            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    items.Add(new BlobItemViewModel { Length = blob.Properties.Length, Uri = blob.Uri.ToString(), Type = Models.BlobType.BlobBlock });
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;
                    items.Add(new BlobItemViewModel { Length = pageBlob.Properties.Length, Uri = pageBlob.Uri.ToString(), Type = Models.BlobType.BlobPage });
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;
                    items.Add(new BlobItemViewModel { Length = 0, Uri = directory.Uri.ToString(), Type = Models.BlobType.BlobDirectory });
                }
            }

            return View(items);
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]        
        public ActionResult Upload(ImageUploadViewModel model)
        {
            CloudBlobContainer container = _service.GetCloudBlogContainer();

            if (ModelState.IsValid)
            {
                // Retrieve reference to a blob named by user.
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(model.FileName);

                // Create or overwrite the "myblob" blob with contents from a local file.
                blockBlob.UploadFromStream(model.FileAttachment.InputStream);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public FileResult Download(string blobName)
        {
            CloudBlobContainer container = _service.GetCloudBlogContainer();

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
             
            Stream blobStream = blockBlob.OpenRead();
            return File(blobStream, blockBlob.Properties.ContentType, blobName);
        }

        public ActionResult Delete(string blobName)
        {
            CloudBlobContainer container = _service.GetCloudBlogContainer();

            // Retrieve reference to a blob named "myblob.txt".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            // Delete the blob.
            blockBlob.Delete();

            return RedirectToAction("Index");
        }
    }
}