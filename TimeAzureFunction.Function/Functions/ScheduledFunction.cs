using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using TimeAzureFunction.Common.Models;
using TimeAzureFunction.Function.Entities;

namespace TimeAzureFunction.Function.Functions
{
    public static class ScheduledFunction
    {
        [FunctionName("ScheduledFunction")]
        public static async Task Run(
            [TimerTrigger("0 */2 * * * *")] TimerInfo myTimer,
              [Table("todo", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
              CloudTable consolidateTable,
            ILogger log)
        {
            log.LogInformation($"Deleting completed function executed at: {DateTime.Now}");

            string filter = TableQuery.GenerateFilterConditionForBool("Issconsolidated", QueryComparisons.Equal, false);
            TableQuery<TimesEntity> query = new TableQuery<TimesEntity>().Where(filter);
            TableQuerySegment<TimesEntity> consolidados = await timeTable.ExecuteQuerySegmentedAsync(query, null);
            int deleted = 0;

            foreach (TimesEntity notConsolidated in consolidados)
            {
                //  [1,1,1,5,5,6,6,6,7]
                //[1,5,6,7]
               
              

                ConsolidatedEntity consolidatedEntity = new ConsolidatedEntity
                {
                    EmployeId = notConsolidated.EmployeId,
                    Time = notConsolidated.Time,
                   // minutesWorked =  sum(completedTodo.Time(completedTodo.EmployeId))
                };
                
                await consolidateTable.ExecuteAsync(TableOperation.Delete(notConsolidated));
                await timeTable.ExecuteAsync(TableOperation.Delete(notConsolidated));
                deleted++;
            }

            log.LogInformation($"Deleting: {deleted} items at: {DateTime.Now}");
        }
    }
}
