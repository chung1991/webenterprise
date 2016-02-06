using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMR.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Enter First Name")]
        public string FirstName { get; set; }

        [StringLength(5, ErrorMessage = "Last Name length should not be greater than 5")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter Salary")]
        public int Salary { get; set; }
    }
}