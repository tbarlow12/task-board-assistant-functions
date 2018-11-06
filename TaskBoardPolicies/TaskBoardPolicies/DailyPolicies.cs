using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using TaskBoardAssistant;
using Microsoft.Extensions.Configuration;

namespace TaskBoardPolicies
{
    public static class DailyPolicies
    {
        [FunctionName("DailyPolicies")]
        public static void Run([TimerTrigger("0 0 1 * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var config = GetConfig(context);
            string storageConnectionString = config["STORAGE_CONNECTION_STRING"];
            string containerName = config["POLICY_CONTAINER_NAME"];
            string blobName = config["DAILY_POLICY_FILENAME"];
            Assistant.ExecuteFromBlob(storageConnectionString, containerName, blobName);
        }

        private static IConfigurationRoot GetConfig(ExecutionContext context)
        {
            return new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
