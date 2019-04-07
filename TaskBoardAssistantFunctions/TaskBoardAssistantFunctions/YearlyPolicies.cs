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
    public static class YearlyPolicies
    {
        /// <summary>
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        /// <param name="context"></param>
        [FunctionName("YearlyPolicies")]
        public static void Run([TimerTrigger("0 10 0 1 Jan *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var result = Assistant.ExecuteFromGitHub("tbarlow12", "task-board-assistant-policies", "yearly.json");
            log.LogInformation($"C# Timer trigger function finished at: {DateTime.Now}");
        }
    }
}
