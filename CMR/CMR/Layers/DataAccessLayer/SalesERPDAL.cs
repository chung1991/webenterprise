using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMR.Models;

namespace CMR.Layers.DataAccessLayer
{
    public class SalesERPDAL : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("TblEmployee");
            base.OnModelCreating(modelBuilder);
        }
    }
}