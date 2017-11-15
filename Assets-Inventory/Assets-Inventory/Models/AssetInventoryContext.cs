namespace Assets_Inventory.Models
{
    using System.Data.Entity;

    public partial class AssetInventoryContext : DbContext
    {
        public AssetInventoryContext() : base("name=AssetInventoryContext")
        {
        }

        public virtual DbSet<ActionLog> ActionLogs { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<AssetType> AssetTypes { get; set; }
        public virtual DbSet<ConnectionLog> ConnectionLogs { get; set; }
        public virtual DbSet<LocationLog> LocationLogs { get; set; }
    }
}
