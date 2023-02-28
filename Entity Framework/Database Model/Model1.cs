using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Entity_Framework.Database_Model
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
        }

        //public virtual DbSet<Table1> Table1 { get; set; }
        public List<Table1> Table1 =new List<Table1>();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table1>()
                .Property(e => e.PID)
                .IsFixedLength();

            modelBuilder.Entity<Table1>()
                .Property(e => e.Groups)
                .IsFixedLength();

            modelBuilder.Entity<Table1>()
                .Property(e => e.Name)
                .IsFixedLength();
        }
    }
}
