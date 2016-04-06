using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMR.Models
{
        public class LoginModel{
            [Display(Name="User name")]
            [Required]
            public String userName{get;set;}
            [Display(Name="Password")]
            [Required]
            [DataType(DataType.Password)]
            public String passWord{get;set;}
            public Boolean rememberMe{get;set;}
        }

        public class RegisterModel{
            public RegisterModel()
            {

            }
            public int accountId { get; set; }

            [Display(Name="User name")]
            [Required]
            public String userName { get; set; }

            [Display(Name="Password")]
            [Required]
            [DataType(DataType.Password)]
            public String passWord { get; set; }
            [Display(Name="Confirm pass")]
            [DataType(DataType.Password)]
            [Required]
            [Compare("passWord",ErrorMessage="Password and Password Confirmation don't match")]
            public String confirmPassWord { get; set; }
            [Display(Name="Role")]
            [Required]
            public int roleId { get; set; }
            [Display(Name="Fullname")]
            [Required]
            public String fullName { get; set; }
            
            [Display(Name="Telephone")]
            [Required]
            [DataType(DataType.PhoneNumber)]
            public String telephone { get; set; }
            [Display(Name="Birthday")]
            [DataType(DataType.Date)]
            [Required]
            public DateTime dateOfBirth { get; set; }

            [Display(Name = "Address")]
            public String address { get; set; }

            [Display(Name = "Email")]
            public String email { get; set; }
    }
}