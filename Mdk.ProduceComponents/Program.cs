﻿using Sandbox.Game.GameSystems;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            Ticker = new CustomTicker();

            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            string masterAssemblerName = "masterAssembler";

            Action requestCrafting = () => {
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
                        {Item.BulletproofGlass, 300},
                        {Item.Computer, 300 },
                        {Item.ConstructionComp, 300},
                        {Item.Display, 300},
                        {Item.Girder, 300},
                        {Item.InteriorPlate, 300},
                        {Item.LargeSteelTube, 300},
                        {Item.MetalGrid, 300},
                        {Item.Motor, 300},
                        {Item.PowerCell, 200},
                        {Item.SmallSteelTube, 300},
                        {Item.SteelPlate, 1500},
                    }
                );
            };

            Action unclogAssemblersAndRefineries = () => {
                var targetCargo = BlocksOnThisGrid
                    .Where(block => block.CustomName == "Cargo Comp")
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
                        var inputs = assembler.InputInventory;

                        List<MyInventoryItem> items = new List<MyInventoryItem>();
                        inputs.GetItems(items);

                        foreach(var item in items) {
                            if(inputs.CanTransferItemTo(targetCargo, item.Type))
                                inputs.TransferItemTo(targetCargo, item);
                        }
                    }

                    foreach(var assembler in assemblers) {
                        var outputs = assembler.OutputInventory;

                        List<MyInventoryItem> items = new List<MyInventoryItem>();
                        outputs.GetItems(items);

                        foreach(var item in items) {
                            if(outputs.CanTransferItemTo(targetCargo, item.Type)) {
                                outputs.TransferItemTo(targetCargo, item);
                            }
                        }
                    }
                    
                    foreach(var refinery in refineries) {
                        var outputs = refinery.OutputInventory;

                        List<MyInventoryItem> items = new List<MyInventoryItem>();
                        outputs.GetItems(items);

                        foreach(var item in items) {
                            if(outputs.CanTransferItemTo(targetCargo, item.Type)) {
                                outputs.TransferItemTo(targetCargo, item);
                            }
                        }
                    }
                }
            };

            Ticker.Every5Seconds += requestCrafting;
            Ticker.Every30Seconds += unclogAssemblersAndRefineries;
        }

        public void Main(string argument, UpdateType updateSource) {
            Ticker?.Tick();
        }
    }
}
