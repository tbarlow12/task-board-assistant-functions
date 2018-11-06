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
        /// <summary>
        /// Normal Schedule: 0 0 1 * * *
        /// 
        ///  System.Private.CoreLib: Exception while executing function: DailyPolicies. System.Private.CoreLib: One or more errors occurred. ((Line: 1, Col: 1, Idx: 0) - (Line: 1, Col: 1, Idx: 0): Exception during deserialization). YamlDotNet: (Line: 1, Col: 1, Idx: 0) - (Line: 1, Col: 1, Idx: 0): Exception during deserialization. YamlDotNet: Property '?provider' not found on type 'TaskBoardAssistant.Models.PolicyCollection'.
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        /// <param name="context"></param>
        [FunctionName("DailyPolicies")]
        public static void Run([TimerTrigger("0 0 1 * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var config = GetConfig(context);
            string connectionString = config["AzureWebJobsStorage"];
            string containerName = config["POLICY_CONTAINER_NAME"];
            string blobName = config["DAILY_POLICY_FILENAME"];
            var result = Assistant.ExecuteFromBlob(connectionString, containerName, blobName);
            log.LogInformation($"C# Timer trigger function finished at: {DateTime.Now}");
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
