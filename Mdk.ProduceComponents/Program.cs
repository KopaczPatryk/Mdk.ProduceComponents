using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.Entities.Blocks;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Noise.Combiners;
using VRage.Scripting;
using VRageMath;

namespace IngameScript
{


    partial class Program : MyGridProgram
    {
        public IMyTextSurface MainScreen { get; private set; }
        public List<Component> RelevantComponents { get; private set; }

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            MainScreen = GridTerminalSystem.GetBlockWithName("pScreen") as IMyTextSurface;
            
            MainScreen.WriteText("");

             RelevantComponents = new List<Component>
            {
                Component.Display,
                Component.BulletproofGlass,
                Component.MetalGrid,
                Component.Computer,
                Component.ConstructionComp,
                Component.Girder,
            };

        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {

            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(blocks);

            var blocksWithInventories = blocks
                .Where(block => block.CubeGrid == Me.CubeGrid)
                .Where(block => block.HasInventory)
                .ToList();

            var outputInventories = blocksWithInventories
                .Where(block => block is IMyProductionBlock)
                .Select(block => block.GetInventory())
                .ToList();

            var allInventories = blocksWithInventories
                .Select(block => block.GetInventory())
                .Concat(outputInventories).ToList();

            Dictionary<string, MyFixedPoint> quantities = new Dictionary<string, MyFixedPoint>();

            var relevant = DefinitionConstants.components.Where(comp => RelevantComponents.Contains(comp.Key)).Select(e=>e.Value).ToList();
            
            foreach (var def in relevant)
            {
                var count = allInventories
                 .Select(inv => inv.GetItemAmount(def.MyDefinitionId))
                 .Aggregate((a, b) => a + b);

                var name = def.DisplayName;
                quantities.Add(name, count);
            }

            StringBuilder sb = new StringBuilder();

            foreach (var quantity in quantities)
            {
                sb.AppendLine($"{quantity.Key}: {quantity.Value}");
            }

            MainScreen.WriteText(sb);
        }
    }
}
