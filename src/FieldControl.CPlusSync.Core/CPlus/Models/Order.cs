using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldControl.CPlusSync.Core.CPlus.Models
{
    public class Order
    {
        public string Identifier { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string ScheduledTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public string EmployeeName { get; set; }
        public string StatusName { get; set; }
        public string ServiceName { get; set; }
        public Customer Customer { get; set; }
    }
}
