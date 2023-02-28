namespace Entity_Framework.Database_Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Table1
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(30)]
        public string PID { get; set; }

        [StringLength(30)]
        public string Groups { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }
    }
}
