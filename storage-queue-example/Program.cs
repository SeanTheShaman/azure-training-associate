using System;

// Added references for use with AzureStorage: 
using System.Threading.Tasks; 
using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Queue; 

namespace _
{
    class Program
    {


        // Connection string - generated az storage account show-connection-string --name <name> --resource-group <resource_group>
        private const string connectionString = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=seantheshamanstorage;AccountKey=RHERnYmxyeCXHdiY+k5ZfPwfrpq10OZJH58svKfbf5zTZtbQfYsCejDFSjU0bSIDAjc6c3Yh1pHJwwbM6Wm+Fg==";

        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                string value = String.Join(" ", args);
                SendArticleAsync(value).Wait(); // Send the message and wait since async 
                Console.WriteLine($"Sent: {value}"); 
            }
        }

        static async Task SendArticleAsync(string newsMessage)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString); 
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient(); 
            CloudQueue queue  = queueClient.GetQueueReference("newsqueue");
            if(await queue.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Queue did not exist. Created queue."); 
            }

            CloudQueueMessage message = new CloudQueueMessage(newsMessage); 
            await queue.AddMessageAsync(message); 
        }
    }
}



// az storage message peek --queue-name newsqueue --connection-string "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=seantheshamanstorage;AccountKey=RHERnYmxyeCXHdiY+k5ZfPwfrpq10OZJH58svKfbf5zTZtbQfYsCejDFSjU0bSIDAjc6c3Yh1pHJwwbM6Wm+Fg=="