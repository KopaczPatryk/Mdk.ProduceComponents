using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript {
    public static class InventoryUtils {
        public static void TransferAllTo(this IMyInventory inventory, IMyInventory targetInventory) {
            List<MyInventoryItem> items = new List<MyInventoryItem>();
            inventory.GetItems(items);

            foreach(var item in items) {
                if(inventory.CanTransferItemTo(targetInventory, item.Type)) {
                    inventory.TransferItemTo(targetInventory, item);
                }
            }
        }
    }
}
