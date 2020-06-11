namespace GroupProject.Database.ModelsGenerated
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeHouse")]
    public partial class EmployeeHouse
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int HouseId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual House House { get; set; }
    }
}
