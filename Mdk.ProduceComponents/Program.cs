using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    class MyClass
    {

    }
    partial class Program : MyGridProgram
    {
        private readonly IMyTextSurface MainScreen;
        private readonly List<Item> RelevantComponents;
        private readonly BlockProvider BlockProvider;

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
            BlockProvider = new BlockProvider(GridTerminalSystem, Me);
        }

        public void Main(string argument, UpdateType updateSource)
        {
            MainScreen.Clear();
            List<IMyInventory> inventories;
            BlockProvider.GetAllInventories(out inventories);

            var countProvider = new InventoryManager(inventories);

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

                MainScreen.WriteLine($"{DefinitionConstants.Components[relevantComponent].DisplayName}: {quantity} ({scheduledCount})");
            }
        }
    }
}
