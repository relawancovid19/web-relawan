using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Volunteers.Helpers
{
    public class UploadFileHelper
    {
        public static string ValidateFileName(string _fileName)
        {
            return Path.GetFileNameWithoutExtension(_fileName).Replace(" ", "-").ToLower();
        }
        public static string ToCustomDomain(string fileUrl)
        {
            var customDomain = ConfigurationManager.AppSettings["StorageCustomDomain"];
            var actualBlob = ConfigurationManager.AppSettings["StorageUrl"];

            if (customDomain != null && actualBlob != null)
            {
                return fileUrl.Replace(actualBlob, customDomain);
            }
            return fileUrl;
        }
        public static async Task<string> UploadBannerImage(HttpPostedFileBase fileToUpload)
        {
            var id = Guid.NewGuid().ToString();
            var permalink = "banner/" + id;
            if (fileToUpload == null || fileToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;
            string fName = ValidateFileName(fileToUpload.FileName);
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                string fileName = String.Format("{0}{1}", permalink + "/" + fName.Replace(" ", "-").ToLower(),
                    Path.GetExtension(fileToUpload.FileName).ToLower());

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.Properties.ContentType = fileToUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(fileToUpload.InputStream);


                var uriBuilder = new UriBuilder(blockBlob.Uri);
                uriBuilder.Scheme = "https";
                fullPath = uriBuilder.ToString().Replace(":443", "").ToLower();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                return "";
            }
            return ToCustomDomain(fullPath);
        }
        public static async Task<string> UploadOrganizationImageAsync(HttpPostedFileBase fileToUpload)
        {
            var id = Guid.NewGuid().ToString();
            var permalink = "avatar/" + id;
            if (fileToUpload == null || fileToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;
            string fName = ValidateFileName(fileToUpload.FileName);
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                string fileName = String.Format("{0}{1}", permalink + "/" + fName.Replace(" ", "-").ToLower(),
                    Path.GetExtension(fileToUpload.FileName).ToLower());

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.Properties.ContentType = fileToUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(fileToUpload.InputStream);


                var uriBuilder = new UriBuilder(blockBlob.Uri);
                uriBuilder.Scheme = "https";
                fullPath = uriBuilder.ToString().Replace(":443", "").ToLower();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                return "";
            }
            return ToCustomDomain(fullPath);
        }
    }
}