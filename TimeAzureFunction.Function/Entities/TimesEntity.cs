using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace TimeAzureFunction.Function.Entities
{
    public class TimesEntity : TableEntity
    {
        public int EmployeId { get; set; }

        public DateTime Time { get; set; }

        public int Type { get; set; }

        public bool Issconsolidated { get; set; }

    }
}
