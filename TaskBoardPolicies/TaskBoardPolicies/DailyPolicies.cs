using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using TaskBoardAssistant;

namespace TaskBoardPolicies
{
    public static class DailyPolicies
    {
        [FunctionName("DailyPolicies")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string storageConnectionString = Environment.GetEnvironmentVariable("STORAGE_CONNECTION_STRING");
            string containerName = Environment.GetEnvironmentVariable("POLICY_CONTAINER_NAME");
            string blobName = Environment.GetEnvironmentVariable("DAILY_POLICY_FILENAME");

            Assistant.ExecuteFromBlob(storageConnectionString, containerName, blobName);
        }
    }
}
