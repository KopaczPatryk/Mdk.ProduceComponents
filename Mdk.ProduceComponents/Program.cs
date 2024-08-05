using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public IMyTextSurface MainScreen { get; private set; }
        public List<Item> RelevantComponents { get; private set; }

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            MainScreen = GridTerminalSystem.GetBlockWithName("pScreen") as IMyTextSurface;

            RelevantComponents = new List<Item>
            {
                Item.BulletproofGlass,
                Item.Computer,
                Item.ConstructionComp,
                Item.Display,
                Item.Girder,
                Item.InteriorPlate,
                Item.LargeSteelTube,
                Item.MetalGrid,
                Item.Motor,
                Item.PowerCell,
                Item.SmallSteelTube,
                Item.SteelPlate,
            };
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            MainScreen.WriteText("");

            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(blocks);

            var blocksWithInventories = blocks
                .Where(block => block.CubeGrid == Me.CubeGrid)
                .Where(block => block.HasInventory)
                .ToList();

            var outputInventories = blocksWithInventories
                .Where(block => block is IMyProductionBlock)
                .Cast<IMyProductionBlock>()
                .Select(block => block.OutputInventory)
                .ToList();

            var allInventories = blocksWithInventories
                .Select(block => block.GetInventory())
                .Concat(outputInventories).ToList();

            var countProvider = new InventoryManager(allInventories);

            var assemblers = new List<IMyAssembler>();
            GridTerminalSystem.GetBlocksOfType(assemblers);
            assemblers.RemoveAll(block => block.CubeGrid != Me.CubeGrid);

            string masterAssemblerName = "masterAssembler";

            var masterAssembler = assemblers
                .Where(assembler => assembler.CustomName == masterAssemblerName)
                .First();

            var nonMasterAssemblers = assemblers
                .Where(assembler => assembler.CustomName != masterAssemblerName)
                .ToList();

            var assemblerManager = new AssemblerGroupManager(masterAssembler, nonMasterAssemblers);
            assemblerManager.EnsureHierarchy();

            foreach (var relevantComponent in RelevantComponents)
            {
                var quantity = countProvider.GetAvailableCount(relevantComponent);
                var tasks = assemblerManager.PendingTasks(Echo);
                MyFixedPoint scheduledCount;

                if (tasks.ContainsKey(relevantComponent))
                {
                    scheduledCount = tasks[relevantComponent];
                }
                else
                {
                    scheduledCount = 0;
                }

                MyFixedPoint needed = 200 - (quantity + scheduledCount);

                if (needed > 0)
                {
                    assemblerManager.EnqueueRecipeFor(relevantComponent, needed);
                }

                MainScreen.WriteText($"{DefinitionConstants.Components[relevantComponent].DisplayName}: {quantity} ({scheduledCount})\n", true);
            }
        }
    }
}
