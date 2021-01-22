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

        static async Task Main(string[] args)
        {
            if(args.Length > 0)
            {
                string value = String.Join(" ", args);
                await SendArticleAsync(value); // Send the message and wait since async 
                Console.WriteLine($"Sent: {value}"); 
            }
            else
            {
                string message = await ReceiveArticleAsync();
                Console.WriteLine($"Message in the queue: {message}"); 
            }
            
        }

        static CloudQueue GetQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString); 
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient(); 
            CloudQueue queue  = queueClient.GetQueueReference("newsqueue");
            return(queue); 
        }
        static async Task<string> ReceiveArticleAsync()
        {
            CloudQueue queue = GetQueue(); 

            if(await queue.ExistsAsync())
            {
                CloudQueueMessage message = await queue.GetMessageAsync(); 

                if(message!=null)
                {
                    string retMessage = message.AsString; 
                    await queue.DeleteMessageAsync(message); 
                    return(retMessage);
                }
         


            }

            return("Queue does not exist or is empty!"); 
        }

        static async Task SendArticleAsync(string newsMessage)
        {
            CloudQueue queue = GetQueue(); 
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