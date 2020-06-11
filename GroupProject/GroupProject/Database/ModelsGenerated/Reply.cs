namespace GroupProject.Database.ModelsGenerated
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reply")]
    public partial class Reply
    {
        public int Id { get; set; }

        public int? RequestId { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Body { get; set; }

        [StringLength(100)]
        public string File { get; set; }

        public DateTime Date { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Request Request { get; set; }
    }
}
