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

        private IList<IMyInventory> Inventories;
        private IList<IMyTerminalBlock> BlocksOnThisGrid;

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            Ticker = new CustomTicker();

            string masterAssemblerName = "masterAssembler";
            
            Action every5Seconds = () => {
                IMyTextSurface componentPoolScreen = GridTerminalSystem.GetBlockWithName("pScreen") as IMyTextSurface;

                var blocks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocks(blocks);
                
                BlocksOnThisGrid = blocks
                    .Where(block => block.CubeGrid == Me.CubeGrid)
                    .ToList();

                IList<IMyAssembler> assemblers = new List<IMyAssembler>();
                BlockManager.GetAllAssemblers(lookAmong: BlocksOnThisGrid, assemblers: out assemblers);

                IMyAssembler masterAssembler = assemblers
                    .Where(assembler => assembler.CustomName == masterAssemblerName)
                    .First();

                //AssemblerManager.EnsureHierarchy(masterAssembler, assemblers);
                BlockManager.GetAllInventories(BlocksOnThisGrid, out Inventories);

                CraftingRequester.Work(
                    masterAssembler: masterAssembler,
                    allAssemblers: assemblers,
                    outputScreen: componentPoolScreen,
                    inventories: Inventories,
                    requestedComponents: new Dictionary<Item, int>{
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
                    }
                );
            };
            Ticker.Every5Seconds += every5Seconds;
        }

        public void Main(string argument, UpdateType updateSource) {
            Ticker.Tick();
        }
    }
}
