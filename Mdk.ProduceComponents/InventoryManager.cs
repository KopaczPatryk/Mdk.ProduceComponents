using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript {
    public static class InventoryManager {
        public static MyFixedPoint GetAvailableCount(ref IList<IMyInventory> inventories, Item item) {
            var def = DefinitionConstants.Components[item].DefinitionId;

            var count = inventories
             .Select(inv => inv.GetItemAmount(def))
             .Aggregate((a, b) => a + b);

            return count;
        }
    }
}