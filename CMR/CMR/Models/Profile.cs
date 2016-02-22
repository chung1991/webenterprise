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
    
    public partial class Profile
    {
        public Profile()
        {
            this.Accounts = new HashSet<Account>();
        }
        public Profile(String name, String address, String phone, DateTime birthday)
        {
            this.name = name;
            this.address = address;
            this.telephone = phone;
            this.dateOfBirth = birthday;
        }
        public Profile(int? profileId, String name, String address, String phone, DateTime birthday)
        {
            this.profileId = (int)profileId;
            this.name = name;
            this.address = address;
            this.telephone = phone;
            this.dateOfBirth = birthday;
        }
        public int profileId { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public Nullable<System.DateTime> dateOfBirth { get; set; }
    
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
