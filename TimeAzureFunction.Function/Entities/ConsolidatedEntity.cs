using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace TimeAzureFunction.Function.Entities
{
    public class ConsolidatedEntity : TableEntity
    {
        public int EmployeId { get; set; }

        public DateTime Time { get; set; }

        public int minutesWorked { get; set; }
    }
}
