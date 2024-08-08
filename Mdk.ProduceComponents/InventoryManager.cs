using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    public class InventoryManager
    {
        private readonly IList<IMyInventory> inventories;

        public InventoryManager(IList<IMyInventory> inventories)
        {
            this.inventories = inventories;
        }

        public MyFixedPoint GetAvailableCount(Item item)
        {
            var def = DefinitionConstants.Components[item].DefinitionId;

            var count = inventories
             .Select(inv => inv.GetItemAmount(def))
             .Aggregate((a, b) => a + b);

            return count;
        }
    }
}