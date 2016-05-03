using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMR.Models
{
    public class StatisticModel
    {
        public int academicYear {get;set;}
        public int pendingCount { get; set; }
        public int waitingCount { get; set; }
        public int approvedCount { get; set; }
        public int rejectedCount { get; set; }


    }
}