namespace Assets_Inventory.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class AssetType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public bool Active { get; set; }

        public IEnumerable<Asset> Assets { get; set; }
    }
}
