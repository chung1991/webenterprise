using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMR.Models
{
    public class AccountMD
    {
        [Display(Name="User name")]
        public string userName { get; set; }
    }

    public class ProfileMD
    {
        [Display(Name="Fullname")]
        public string name { get; set; }
        [Display(Name="Address")]
        public string address { get; set; }
        [Display(Name="Telephone")]
        public string telephone { get; set; }
        [Display(Name="Birthday")]
        public Nullable<System.DateTime> dateOfBirth { get; set; }
    }

    public class RoleMD{
        [Display(Name = "Role name")]
        public string roleName { get; set; }
    }

    public class ApproveStatuMD
    {
        [Display(Name = "Status")]
        public string name { get; set; }
    }
    public class CourseMD
    {
        [Display(Name = "Course name")]
        public string name { get; set; }
    }

    public class AnnualCourseMD
    {
        [Display(Name = "Academic year")]
        public Nullable<int> academicYear { get; set; }
    }

    public class AnnualCourseRecordMD
    {
        [Display(Name="Fail count")]
        [Required]
        public Nullable<int> failCount { get; set; }
        [Display(Name="Pass count")]
        [Required]
        public Nullable<int> passCount { get; set; }

        [Display(Name = "Credit count")]
        [Required]
        public Nullable<int> creditCount { get; set; }
        [Display(Name = "Distinct count")]
        [Required]
        public Nullable<int> distinctionCount { get; set; }

        [Display(Name = "Evaluation")]
        [Required]
        public string evaluation { get; set; }
    }
}