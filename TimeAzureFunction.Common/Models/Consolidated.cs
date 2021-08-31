using System;

namespace TimeAzureFunction.Common.Models
{
    public class Consolidated
    {
        public int EmployeId { get; set; }

        public DateTime Time { get; set; }

        public TimeSpan minutesWorked { get; set; }
    }
}
