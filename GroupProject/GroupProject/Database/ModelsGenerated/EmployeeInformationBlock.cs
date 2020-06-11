namespace GroupProject.Database.ModelsGenerated
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeInformationBlock")]
    public partial class EmployeeInformationBlock
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int InformationBlockId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual InformationBlock InformationBlock { get; set; }
    }
}
