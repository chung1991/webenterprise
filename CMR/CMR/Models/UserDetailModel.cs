using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMR.Models
{
    public class UserDetailModel
    {
        public UserDetailModel(int accountId,String userName, String fullName, String roleName, String address, String telePhone, DateTime? birthday)
        {
            this.accountId = accountId;
            this.userName = userName;
            this.fullName = fullName;
            this.address = address;
            this.roleName = roleName;
            this.telelphone = telePhone;
            this.birthday = (DateTime) birthday;
        }
        public int accountId { get; set; }
        [Display(Name="User name")]
        public String userName{get;set;}
        [Display(Name="FullName")]
        public String fullName { get; set; }
        [Display(Name="Address")]
        public String address { get; set; }
        [Display(Name="Role")]
        public String roleName { get; set; }
        [Display(Name="Telephone")]
        public String telelphone { get; set; }
        [Display(Name="Birthday")]
        public DateTime birthday { get; set; }
    }
}