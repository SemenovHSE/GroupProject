namespace GroupProject.Database.ModelsGenerated
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            Replies = new HashSet<Reply>();
        }

        public int Id { get; set; }

        public int? ResidentId { get; set; }

        public int? StatusId { get; set; }

        [Required]
        [StringLength(100)]
        public string Theme { get; set; }

        [Required]
        [StringLength(1000)]
        public string Body { get; set; }

        [StringLength(100)]
        public string File { get; set; }

        public DateTime Date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reply> Replies { get; set; }

        public virtual Resident Resident { get; set; }

        public virtual Status Status { get; set; }
    }
}
