namespace GroupProject.Database.ModelsGenerated
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Setting")]
    public partial class Setting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setting()
        {
            Residents = new HashSet<Resident>();
        }

        public int Id { get; set; }

        public int? HouseId { get; set; }

        public int? SettingTypeId { get; set; }

        public int SettingNumber { get; set; }

        public double? Size { get; set; }

        public int? RoomsNumber { get; set; }

        public int EntranceNumber { get; set; }

        public virtual House House { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resident> Residents { get; set; }

        public virtual SettingType SettingType { get; set; }
    }
}
