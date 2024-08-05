using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;

namespace IngameScript
{
    public class AssemblerGroupManager
    {
        private IMyAssembler masterAssembler;
        private List<IMyAssembler> others;

        public AssemblerGroupManager(IMyAssembler masterAssembler, List<IMyAssembler> others)
        {
            this.masterAssembler = masterAssembler;
            this.others = others ?? new List<IMyAssembler>();
        }
        public void EnsureHierarchy()
        {
            masterAssembler.CooperativeMode = false;
            others.ForEach(
                assembler => assembler.CooperativeMode = true
            );
        }

        public Dictionary<Item, MyFixedPoint> PendingTasks(Action<string> logger)
        {
            var allAssemblers = new List<IMyAssembler>
            {
                masterAssembler
            };
            allAssemblers.AddRange(others);

            var queue = new List<MyProductionItem>();

            allAssemblers.ForEach(assembler =>
            {
                var localQueue = new List<MyProductionItem>();
                assembler.GetQueue(localQueue);
                queue.AddRange(localQueue);
            });

            //logger(queue.Count.ToString());

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

            //logger(grouped.First().Key.ToString());

            //var summed = grouped.ToDictionary(
            //    e => e.Key,
            //    e => e.Value.Aggregate((a, b) => a + b)
            //);
            //return summed.ToDictionary(e =>
            //    DefinitionConstants.Components.Values.Where(c => c.RecipeDefId == e.Key).Single().Item,
            //    e => MyFixedPoint.Zero//e.Value
            //);
        }

        public void EnqueueRecipeFor(Item item, MyFixedPoint amount)
        {
            var recipeDef = DefinitionConstants.Components[item].RecipeDefId;
            if (recipeDef.HasValue)
            {
                masterAssembler.AddQueueItem(recipeDef.Value, amount);
            }
        }
    }
}