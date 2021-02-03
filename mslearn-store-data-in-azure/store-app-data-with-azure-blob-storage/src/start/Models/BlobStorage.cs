using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using System.Linq; 
using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Blob; 

namespace FileUploader.Models
{
    public class BlobStorage : IStorage
    {
        private readonly AzureStorageConfig storageConfig;

        public BlobStorage(IOptions<AzureStorageConfig> storageConfig)
        {
            this.storageConfig = storageConfig.Value;
        }

        private CloudBlobContainer CreateBlobContainer(string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConfig.ConnectionString); 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient(); 
            CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(containerName); 
            return cloudBlobContainer; 
        }

        public Task Initialize()
        {
            return CreateBlobContainer(storageConfig.FileContainerName).CreateIfNotExistsAsync();             
        }

        public Task Save(Stream fileStream, string name)
        {
            CloudBlobContainer blobContainer = CreateBlobContainer(storageConfig.FileContainerName); 
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(name); 
            return blockBlob.UploadFromStreamAsync(fileStream); 
        }

        public async Task<IEnumerable<string>> GetNames()
        {
            List<string> names = new List<string>(); 

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConfig.ConnectionString); 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient(); 
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(storageConfig.FileContainerName); 

            BlobContinuationToken continuationToken = null; 
            BlobResultSegment resultSegment = null; 

            do { 
                resultSegment = await blobContainer.ListBlobsSegmentedAsync(continuationToken); 

                names.AddRange(resultSegment.Results.OfType<ICloudBlob>().Select(b=>b.Name)); // Add the name of the blob to the list. 
            }
            while(continuationToken!=null); 

            return names; 
        }

        public Task<Stream> Load(string name)
        {
            CloudBlobContainer blobContainer = CreateBlobContainer(storageConfig.FileContainerName); 
            return blobContainer.GetBlobReference(name).OpenReadAsync(); 
        }
    }
}