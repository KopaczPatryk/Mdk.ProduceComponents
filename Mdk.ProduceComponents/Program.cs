using Sandbox.Game.GameSystems;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript {
    //ref - may 
    //in - cannot 
    //out - must 

    partial class Program : MyGridProgram {
        private readonly CustomTicker Ticker;
        private readonly IMyTextSurface MainScreen;

        private readonly Dictionary<Item, int> RelevantComponents = new Dictionary<Item, int>{
                {Item.BulletproofGlass, 200},
                {Item.Computer, 200 },
                {Item.ConstructionComp, 200},
                {Item.Display, 200},
                {Item.Girder, 200},
                {Item.InteriorPlate, 200},
                {Item.LargeSteelTube, 200},
                {Item.MetalGrid, 200},
                {Item.Motor, 200},
                {Item.PowerCell, 200},
                {Item.SmallSteelTube, 200},
                {Item.SteelPlate, 500},
            };

        private IList<IMyInventory> Inventories;
        private IList<IMyTerminalBlock> BlocksOnThisGrid;

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            MainScreen = GridTerminalSystem.GetBlockWithName("pScreen") as IMyTextSurface;


            var blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(blocks);

            BlocksOnThisGrid = blocks
                .Where(block => block.CubeGrid == Me.CubeGrid)
                .ToList();

            Ticker = new CustomTicker();

            Action every5Seconds = () => {
                MainScreen.Clear();

                List<IMyAssembler> assemblers = new List<IMyAssembler>();
                GridTerminalSystem.GetBlocksOfType(assemblers);
                assemblers.RemoveAll(block => block.CubeGrid != Me.CubeGrid);

                string masterAssemblerName = "masterAssembler";

                IMyAssembler masterAssembler = assemblers
                    .Where(assembler => assembler.CustomName == masterAssemblerName)
                    .First();

                IList<IMyAssembler> nonMasterAssemblers = assemblers
                    .Where(assembler => assembler.CustomName != masterAssemblerName)
                    .ToList();

                AssemblerManager.EnsureHierarchy(masterAssembler, nonMasterAssemblers);
                BlockManager.GetAllInventories(BlocksOnThisGrid, out Inventories);

                foreach(var relevantComponent in RelevantComponents) {
                    var quantity = InventoryManager.GetAvailableCount(ref Inventories, relevantComponent.Key);
                    var tasks = AssemblerManager.GetPendingTasks(assemblers, Echo);

                    MyFixedPoint scheduledCount = 0;
                    if(tasks.ContainsKey(relevantComponent.Key)) {
                        scheduledCount = tasks[relevantComponent.Key];
                    }

                    MyFixedPoint needed = relevantComponent.Value - (quantity + scheduledCount);

                    if(needed > 0) {
                        AssemblerManager.EnqueueRecipeFor(masterAssembler, relevantComponent.Key, needed);
                    }

                    MainScreen.WriteLine($"{DefinitionConstants.Components[relevantComponent.Key].DisplayName}: {quantity} ({scheduledCount})");
                }
            };
            Ticker.Every5Seconds += every5Seconds;
        }

        public void Main(string argument, UpdateType updateSource) {
            Ticker.Tick();
        }
    }
}
