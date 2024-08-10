using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;

namespace IngameScript {
    // todo rename AssemblerManager
    public class AssemblerGroupManager {
        public static void EnsureHierarchy(IMyAssembler master, IList<IMyAssembler> otherAssemblers) {
            master.CooperativeMode = false;
            foreach(var assembler in otherAssemblers) {
                assembler.CooperativeMode = true;
            }
        }

        public static Dictionary<Item, MyFixedPoint> GetPendingTasks(
            IList<IMyAssembler> assemblers,
            Action<string> logger = null
        ) {
            var queue = new List<MyProductionItem>();
            foreach(var assembler in assemblers) {
                var localQueue = new List<MyProductionItem>();
                assembler.GetQueue(localQueue);
                queue.AddRange(localQueue);
            }

            return queue
                .GroupBy(element => element.BlueprintId)
                .ToDictionary(
                    e => e.Key,
                    e => e.Select(item => item.Amount).ToList()
                )
                .ToDictionary(
                    e => DefinitionConstants.Components.Values.Where(c => c.RecipeDefId == e.Key).Single().Item,
                    e => e.Value.Aggregate((a, b) => a + b)
                );
        }

        public static void EnqueueRecipeFor(IMyAssembler masterAssembler, Item item, MyFixedPoint amount) {
            var recipeDef = DefinitionConstants.Components[item].RecipeDefId;

            if(recipeDef.HasValue && amount > 0 && masterAssembler.Mode == MyAssemblerMode.Assembly) {
                masterAssembler.AddQueueItem(recipeDef.Value, amount);
            }
        }
    }
}