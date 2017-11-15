namespace Assets_Inventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Asset
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Inv_id { get; set; }

        public int? AssetTypeId { get; set; }

        [StringLength(50)]
        public string Brand { get; set; }

        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(20)]
        public string Netw_name { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [StringLength(100)]
        public string Connection { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Active { get; set; }

        public virtual AssetType AssetType { get; set; }
        public IEnumerable<ActionLog> ActionLog { get; set; }
        public IEnumerable<ConnectionLog> ConnectionLog { get; set; }
        public IEnumerable<LocationLog> LocationLog { get; set; }

        public Asset()
        {
            CreateDate = DateTime.Now;
            Active = true;
        }
    }
}
