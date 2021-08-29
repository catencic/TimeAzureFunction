using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using TimeAzureFunction.Common.Models;
using TimeAzureFunction.Common.Responces;
using TimeAzureFunction.Function.Entities;

namespace TimeAzureFunction.Function.Functions
{
    public static class TimesApi
    {
        [FunctionName(nameof(CreateTimes))]
        public static async Task<IActionResult> CreateTimes(
             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "times")] HttpRequest req,
             [Table("times", Connection = "AzureWebJobsStorage")] CloudTable timesTable,
             ILogger log)
        {
            log.LogInformation("Recieved a new Time");



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Times time = JsonConvert.DeserializeObject<Times>(requestBody);

            if (string.IsNullOrEmpty(time?.EmployeId.ToString()) ||
                string.IsNullOrEmpty(time?.Time.ToString()) ||
                string.IsNullOrEmpty(time?.Type.ToString()))
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSucces = false,
                    Message = "The request must have all the parameters."
                });
            }

            TimesEntity timeEntity = new TimesEntity
            {
                Time = time.Time,
                ETag = "*",
                Issconsolidated = false,
                PartitionKey = "TIME",
                RowKey = Guid.NewGuid().ToString(),
                EmployeId = time.EmployeId,
                Type = time.Type
            };

            TableOperation addOperation = TableOperation.Insert(timeEntity);
            await timesTable.ExecuteAsync(addOperation);
            //
            string message = "New time stored in table";
            log.LogInformation(message);



            return new OkObjectResult(new Response
            {
                IsSucces = true,
                Message = message,
                Result = timeEntity
            });
        }
    }
}
