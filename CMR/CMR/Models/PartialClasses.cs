using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMR.Models
{

        [MetadataType(typeof(CourseMD))]
        public partial class Course
        {
        }

        [MetadataType(typeof(AnnualCourseMD))]
        public partial class AnnualCourse
        {
        }

        [MetadataType(typeof(ProfileMD))]
        public partial class Profile
        {
        }

        [MetadataType(typeof(RoleMD))]
        public partial class Role
        {
        }

        [MetadataType(typeof(ApproveStatuMD))]
        public partial class ApproveStatu
        {
        }

        [MetadataType(typeof(AccountMD))]
        public partial class Account
        {
        }
}