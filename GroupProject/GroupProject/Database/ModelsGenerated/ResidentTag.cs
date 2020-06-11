namespace GroupProject.Database.ModelsGenerated
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ResidentTag")]
    public partial class ResidentTag
    {
        public int Id { get; set; }

        public int ResidentId { get; set; }

        public int TagId { get; set; }

        public virtual Resident Resident { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
