using System;
using Microsoft.EntityFrameworkCore;

//===================================================​====
// Date :​ 23 Des 2021
// Author :​ A.M.lubis
// Description :​ Declare DB Context using model builder
//===================================================​====
// Revision History:​
// Name	|Date	|Description |​
//	    |	    |	         |​
//===================================================​

namespace OCR.MVC.WebApp.Models
{
    public class ANFBContext : DbContext
    {
        public ANFBContext(DbContextOptions<ANFBContext> options)
            : base(options)
        {
        }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>().ToTable("Supplier");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
        }
    }
}
