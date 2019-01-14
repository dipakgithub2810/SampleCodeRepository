using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;

namespace AzureBlobStorageconsole
{
    class Program
    {
        static void Main(string[] args)
        {

            string connstring = ConfigurationManager.ConnectionStrings["AzureStorageAccount"].ConnectionString;
            string localfolder = ConfigurationManager.AppSettings["sourcefolder"];
            string destContainer = ConfigurationManager.AppSettings["destContainer"];

            CloudStorageAccount sa = CloudStorageAccount.Parse(connstring);
            CloudBlobClient bc = sa.CreateCloudBlobClient();
            CloudBlobContainer container = bc.GetContainerReference(destContainer);

            container.CreateIfNotExists();

            string[] fileEntries = Directory.GetFiles(localfolder);

            foreach(string filepath in fileEntries)
            {
                string key = Path.GetFileName(filepath);
                uploadblob(container, key, filepath, true);
            }
            

        }


        static void uploadblob(CloudBlobContainer container, string key, string filename, bool deleteAfter)
        {
            CloudBlockBlob b = container.GetBlockBlobReference(key);

            using (var fs = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
            {

                b.UploadFromStream(fs);
            }

        }
    }
}
