using System.Data.Entity;

namespace Assets_Inventory.Models
{
    public class AssetTypeDbInitializer : DropCreateDatabaseAlways<AssetInventoryContext>
    {
        protected override void Seed(AssetInventoryContext context)
        {
            context.AssetTypes.Add(new AssetType { Name = "Компьютер", Active = true });
            context.AssetTypes.Add(new AssetType { Name = "Монитор", Active = true });
            context.AssetTypes.Add(new AssetType { Name = "Принтер", Active = true });
            context.AssetTypes.Add(new AssetType { Name = "Сканер", Active = true });
            context.AssetTypes.Add(new AssetType { Name = "МФУ", Active = true });

            base.Seed(context);
        }
    }
}