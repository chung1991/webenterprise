//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMR.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AnnualCourse
    {
        public AnnualCourse()
        {
            this.CLAnnualCourses = new HashSet<CLAnnualCourse>();
            this.CourseMonitoringReports = new HashSet<CourseMonitoringReport>();
        }
    
        public int annualCourseId { get; set; }
        public Nullable<int> academicYear { get; set; }
        public Nullable<int> courseId { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual ICollection<CLAnnualCourse> CLAnnualCourses { get; set; }
        public virtual ICollection<CourseMonitoringReport> CourseMonitoringReports { get; set; }
    }
}
