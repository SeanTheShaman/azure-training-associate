using System;
using System.IO; 
using Microsoft.Extensions.Configuration; 

namespace photo_sharing_app
{
    class Program
    {
        static void Main(string[] args)
        {

            // Build the configuration from the appsettings.json: 
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json"); 
            var configuration = builder.Build(); 
        }

        
    }
}
