using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.Entities.Blocks;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
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
    enum Components
    {
        
    }
    class ItemConstant
    {
        
    }
    class DefinitionContants
    {
        const string ComponentTypeId = "MyObjectBuilder_Component/";

        public static Dictionary<string, string> components = new Dictionary<string, string> {
            { "Bulletproof Glass", "BulletproofGlass" },
            { "Canvas", "Canvas" },
            { "Computer", "Computer" },
            { "Construction Comp.", "Construction" },
            { "Detector Comp.", "Detector" },
            { "Display", "Display" },
            { "Engineer Plushie", "EngineerPlushie" },
            { "Explosives", "Explosives" },
            { "Girder", "Girder" },
            { "Gravity Comp.", "GravityGenerator" },
            { "Interior Plate", "InteriorPlate" },
            { "Large Steel Tube", "LargeTube" },
            { "Medical Comp.", "Medical" },
            { "Metal Grid", "MetalGrid" },
            { "Motor", "Motor" },
            { "Power Cell", "PowerCell" },
            { "Radio - comm Comp.", "RadioCommunication" },
            { "Reactor Comp.", "Reactor" },
            { "Saberoid Plushie", "SabiroidPlushie" },
            { "Small Steel Tube", "SmallTube" },
            { "Solar Cell", "SolarCell" },
            { "Steel Plate", "SteelPlate" },
            { "Superconductor", "Superconductor" },
            { "Thruster Comp.", "Thrust" },
            { "Zone Chip", "ZoneChip" },
        };
        
        string getComponentDef()
    }

    partial class Program : MyGridProgram
    {
        public IMyTextSurface MainScreen { get; private set; }
        public List<MyDefinitionId> IngotDefs { get; private set; }

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            MainScreen = GridTerminalSystem.GetBlockWithName("pScreen") as IMyTextSurface;
            List<string> componentNames = new List<string>();


            List<string> IngotNames = new List<string>() {
                "Iron",
                "Cobalt",
                "Gold",
                "Ice",
                "Iron",
                "Magnesium",
                "Nickel",
                "Organic",
                "Platinum",
                "Scrap",
                "Silicon",
                "Silver",
                "Stone",
                "Uranium",
            };

            var ingotDefNames = IngotNames.Select(name => $"MyObjectBuilder_Ore/{name}").ToList();

            IngotDefs = ingotDefNames.Select(defName => MyDefinitionId.Parse(defName)).ToList();
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource)
        {
            /*
            IMyProductionBlock, IMyInventory
                    IMyAssembler assembler = null;
            IMyRefinery refinery = null;

             */
            //List<IMyRefinery> refineries = new List<IMyRefinery>();
            //List<IMyInventory> cargos = new List<IMyInventory>();
            //List<IMyEntity> inventories = new List<IMyEntity>();
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();

            GridTerminalSystem.GetBlocks(blocks);

            var allInventories = blocks
                .Where(block => block.CubeGrid == Me.CubeGrid)
                .Where(block => block.HasInventory)
                .Select(block => block.GetInventory());

            Dictionary<string, MyFixedPoint> quantities = new Dictionary<string, MyFixedPoint>();

            foreach (var def in IngotDefs)
            {
                var count = allInventories
                    .Select(inv => inv.GetItemAmount(def))
                    .Aggregate((a, b) => a + b);

                var name = def.SubtypeId.ToString();
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
