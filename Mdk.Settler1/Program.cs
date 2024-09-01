using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript {
    //ref - may
    //in - cannot
    //out - must

    partial class Program : MyGridProgram {
        private readonly CustomTicker Ticker;

        private IList<IMyTerminalBlock> BlocksOnThisGrid;

        public Program() {
            Ticker = new CustomTicker();
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            string masterAssemblerName = "masterAssembler";


            Action unclogAssemblersAndRefineries = () => {
                var blocks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocks(blocks);

                BlocksOnThisGrid = blocks
                    .Where(block => block.CubeGrid == Me.CubeGrid)
                    .ToList();

                var targetCargo = BlocksOnThisGrid
                    .Where(block => block.CustomName == "Settler cargo1")
                    .First()
                    .GetInventory();

                if(targetCargo != null) {
                    IList<IMyAssembler> assemblers;
                    BlockManager.GetAllAssemblers(
                        lookAmong: BlocksOnThisGrid,
                        assemblers: out assemblers);

                    IList<IMyRefinery> refineries;
                    BlockManager.GetAllRefineries(
                        lookAmong: BlocksOnThisGrid,
                        refineries: out refineries);

                    foreach(var assembler in assemblers) {
                        assembler.InputInventory.TransferAllTo(targetCargo);
                        assembler.OutputInventory.TransferAllTo(targetCargo);
                    }

                    foreach(var refinery in refineries) {
                        refinery.OutputInventory.TransferAllTo(targetCargo);
                    }
                }
            };

            Action requestCrafting = () => {
                IMyTextSurface componentPoolScreen = GridTerminalSystem.GetBlockWithName("p1") as IMyTextSurface;

                var blocks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocks(blocks);

                BlocksOnThisGrid = blocks
                    .Where(block => block.CubeGrid == Me.CubeGrid)
                    .ToList();

                IList<IMyAssembler> assemblers = new List<IMyAssembler>();
                BlockManager.GetAllAssemblers(
                    lookAmong: BlocksOnThisGrid,
                    assemblers: out assemblers);

                IMyAssembler masterAssembler = assemblers
                    .First(assembler => assembler.CustomName == masterAssemblerName);
                
                IList<IMyInventory> Inventories;
                BlockManager.GetAllInventories(BlocksOnThisGrid, out Inventories);

                CraftingRequester.Work(
                    masterAssembler: masterAssembler,
                    allAssemblers: assemblers,
                    outputScreen: componentPoolScreen,
                    inventories: Inventories,
                    requestedComponents: new Dictionary<Item, int>{
                        {Item.BulletproofGlass, 1000},
                        {Item.Computer, 1000 },
                        {Item.ConstructionComp, 1000},
                        {Item.Display, 1000},
                        {Item.Girder, 1000},
                        {Item.InteriorPlate, 1000},
                        {Item.LargeSteelTube, 1000},
                        {Item.MetalGrid, 1000},
                        {Item.Motor, 1000},
                        {Item.PowerCell, 1000},
                        {Item.SmallSteelTube, 1000},
                        {Item.SteelPlate, 1000},
                        {Item.DetectorComp, 500},
                    }
                );
            };

            Ticker.Every30Seconds += unclogAssemblersAndRefineries;
            Ticker.Every5Seconds += requestCrafting;
        }

        public void Main(string argument, UpdateType updateSource) {
            Ticker?.Tick();
        }
    }
}
