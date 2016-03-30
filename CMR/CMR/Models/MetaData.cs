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
        [Required]
        [StringLength(30, ErrorMessage = "Length of course name must be less than 30")]
        [Display(Name = "Course name")]
        [RegularExpression(@"^([a-zA-Z0-9ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ \&\-]+)$", ErrorMessage = "Special character is not allowed !")]
        public string courseName { get; set; }

        [Required]
        [Display(Name = "Faculty name")]
        public Nullable<int> facultyId { get; set; }
    }

    public class AnnualCourseMD
    {
        [Display(Name = "Academic year")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime academicYear { get; set; }

        [Display(Name = "Course name")]
        [Required]
        public Nullable<int> courseId { get; set; }
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

    public class CourseMonitoringReportMD
    {
        [Display(Name="Fail Count")]
        public Nullable<int> failCount { get; set; }

        [Display(Name="Pass Count")]
        public Nullable<int> passCount { get; set; }

        [Display(Name="Credit Count")]
        public Nullable<int> creditCount { get; set; }

        [Display(Name="Dist Count")]
        public Nullable<int> distinctionCount { get; set; }

        [Display(Name="Evaluation")]
        public string evaluation { get; set; }
    }
}