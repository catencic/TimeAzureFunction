using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TimeAzureFunction.Common.Models;
using TimeAzureFunction.Function.Entities;

namespace TimeAzureFunction.Text.Helpers
{
    public class TestFactory
    {
        public static TimesEntity GettimeEntity()
        {
            return new TimesEntity
            {

                ETag = "*",
                PartitionKey = "TIME",
                RowKey = Guid.NewGuid().ToString(),
                EmployeId = 1,
                Time = DateTime.UtcNow,
                Type = 0,
                Issconsolidated = false
            };
        }


        public static DefaultHttpRequest CreateHttpRequest(Guid employeId, Times timeRequest)
        {
            string request = JsonConvert.SerializeObject(timeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{employeId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid employeId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{employeId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Times timeRequest)
        {
            string request = JsonConvert.SerializeObject(timeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        public static Times GetTodoRequest()
        {
            return new Times
            {
               EmployeId = 1,
               Time = DateTime.UtcNow,
               Type = 0,
               Issconsolidated = false
            };
        }

        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
