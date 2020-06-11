namespace GroupProject.Database.ModelsGenerated
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TagInformationBlock")]
    public partial class TagInformationBlock
    {
        public int Id { get; set; }

        public int TagId { get; set; }

        public int InformationBlockId { get; set; }

        public virtual InformationBlock InformationBlock { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
