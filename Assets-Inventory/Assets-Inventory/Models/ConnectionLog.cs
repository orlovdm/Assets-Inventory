namespace Assets_Inventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ConnectionLog")]
    public partial class ConnectionLog
    {
        public int Id { get; set; }

        public int AssetId { get; set; }

        [StringLength(20)]
        public string ConnectTo { get; set; }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        public DateTime Timestamp { get; set; }

        public string Notes { get; set; }

        public virtual Asset Asset { get; set; }

        public ConnectionLog()
        {
            Timestamp = DateTime.Now;
        }
    }
}
