using Sandbox.Game.GameSystems;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript {
    internal class CraftingRequester {
        internal static void Work(IMyAssembler masterAssembler,
                                  IList<IMyAssembler> allAssemblers,
                                  IList<IMyInventory> inventories,
                                  IMyTextSurface outputScreen,
                                  Dictionary<Item, int> requestedComponents) {
            outputScreen?.Clear();

            foreach(var relevantComponent in requestedComponents) {
                var quantity = InventoryManager.GetAvailableCount(ref inventories, relevantComponent.Key);
                var tasks = AssemblerManager.GetPendingTasks(allAssemblers);

                MyFixedPoint scheduledCount = 0;
                if(tasks.ContainsKey(relevantComponent.Key)) {
                    scheduledCount = tasks[relevantComponent.Key];
                }

                MyFixedPoint needed = relevantComponent.Value - (quantity + scheduledCount);

                if(needed > 0) {
                    AssemblerManager.EnqueueRecipeFor(masterAssembler, relevantComponent.Key, needed);
                }

                outputScreen?.WriteLine($"{DefinitionConstants.Components[relevantComponent.Key].DisplayName}: {quantity} +{scheduledCount} ({relevantComponent.Value})");
            }
        }
    }
}
