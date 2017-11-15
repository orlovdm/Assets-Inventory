namespace Assets_Inventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LocationLog")]
    public partial class LocationLog
    {
        public int Id { get; set; }

        public int AssetId { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        public DateTime Timestamp { get; set; }

        public string Notes { get; set; }

        public virtual Asset Asset { get; set; }

        public LocationLog()
        {
            Timestamp = DateTime.Now;
        }
    }
}
