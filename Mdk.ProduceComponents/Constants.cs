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
    public enum Item
    {
        // Comps
        BulletproofGlass,
        Canvas,
        Computer,
        ConstructionComp,
        DetectorComp,
        Display,
        EngineerPlushie,
        Explosives,
        Girder,
        GravityComp,
        InteriorPlate,
        LargeSteelTube,
        MedicalComp,
        MetalGrid,
        Motor,
        PowerCell,
        RadioCommComp,
        ReactorComp,
        SaberoidPlushie,
        SmallSteelTube,
        SolarCell,
        SteelPlate,
        Superconductor,
        ThrusterComp,
        ZoneChip,
        //Metals
        Cobalt,
        Gold,
        Ice,
        Iron,
        Magnesium,
        Nickel,
        Organic,
        Platinum,
        Scrap,
        Silicon,
        Silver,
        Stone,
        Uranium,
    }

    public class ItemConstant
    {
        public Item Item { get; }
        public string DisplayName { get; }
        public MyDefinitionId DefinitionId { get; }
        public MyDefinitionId? RecipeDefId { get; }

        //public string DefinitionIdString
        //{
        //    get
        //    {
        //        return $"{TypeId}/{SubtypeId}";
        //    }
        //}

        public ItemConstant(Item item,
                            string displayName,
                            string definitionId,
                            string recipeDefId)
        {
            Item = item;
            DisplayName = displayName;
            DefinitionId = MyDefinitionId.Parse(definitionId);
            if (recipeDefId != null)
            {
                RecipeDefId = MyDefinitionId.Parse(recipeDefId);
            }
        }
    }

    public class DefinitionConstants
    {
        private DefinitionConstants() { }
        private const string componentTypeId = "MyObjectBuilder_Component";
        private const string ingotTypeId = "MyObjectBuilder_Ingot";

        public static Dictionary<Item, ItemConstant> Components = new Dictionary<Item, ItemConstant> {
            { Item.BulletproofGlass,    new ItemConstant(Item.BulletproofGlass, "Bulletproof Glass", "MyObjectBuilder_Component/BulletproofGlass", "MyObjectBuilder_BlueprintDefinition/BulletproofGlass" ) },
            { Item.Canvas,              new ItemConstant(Item.Canvas, "Canvas", "MyObjectBuilder_Component/Canvas", "MyObjectBuilder_BlueprintDefinition/Position0030_Canvas" )},
            { Item.Computer,            new ItemConstant(Item.Computer, "Computer", "MyObjectBuilder_Component/Computer", "MyObjectBuilder_BlueprintDefinition/ComputerComponent" ) },
            { Item.ConstructionComp,    new ItemConstant(Item.ConstructionComp, "Construction Comp.", "MyObjectBuilder_Component/Construction", "MyObjectBuilder_BlueprintDefinition/ConstructionComponent" ) },
            { Item.DetectorComp,        new ItemConstant(Item.DetectorComp, "Detector Comp.", "MyObjectBuilder_Component/Detector", "MyObjectBuilder_BlueprintDefinition/DetectorComponent" ) },
            { Item.Display,             new ItemConstant(Item.Display, "Display", "MyObjectBuilder_Component/Display", "MyObjectBuilder_BlueprintDefinition/Display" ) },
            { Item.EngineerPlushie,     new ItemConstant(Item.EngineerPlushie, "Engineer Plushie", "MyObjectBuilder_Component/EngineerPlushie", null ) },
            { Item.Explosives,          new ItemConstant(Item.Explosives, "Explosives", "MyObjectBuilder_Component/Explosives", "MyObjectBuilder_BlueprintDefinition/ExplosivesComponent" ) },
            { Item.Girder,              new ItemConstant(Item.Girder, "Girder", "MyObjectBuilder_Component/Girder", "MyObjectBuilder_BlueprintDefinition/GirderComponent" ) },
            { Item.GravityComp,         new ItemConstant(Item.GravityComp, "Gravity Comp.", "MyObjectBuilder_Component/GravityGenerator", "MyObjectBuilder_BlueprintDefinition/GravityGeneratorComponent" ) },
            { Item.InteriorPlate,       new ItemConstant(Item.InteriorPlate, "Interior Plate", "MyObjectBuilder_Component/InteriorPlate", "MyObjectBuilder_BlueprintDefinition/InteriorPlate" ) },
            { Item.LargeSteelTube,      new ItemConstant(Item.LargeSteelTube, "Large Steel Tube", "MyObjectBuilder_Component/LargeTube", "MyObjectBuilder_BlueprintDefinition/LargeTube" ) },
            { Item.MedicalComp,         new ItemConstant(Item.MedicalComp, "Medical Comp.", "MyObjectBuilder_Component/Medical", "MyObjectBuilder_BlueprintDefinition/MedicalComponent" ) },
            { Item.MetalGrid,           new ItemConstant(Item.MetalGrid, "Metal Grid", "MyObjectBuilder_Component/MetalGrid", "MyObjectBuilder_BlueprintDefinition/MetalGrid" ) },
            { Item.Motor,               new ItemConstant(Item.Motor, "Motor", "MyObjectBuilder_Component/Motor", "MyObjectBuilder_BlueprintDefinition/MotorComponent" ) },
            { Item.PowerCell,           new ItemConstant(Item.PowerCell, "Power Cell", "MyObjectBuilder_Component/PowerCell", "MyObjectBuilder_BlueprintDefinition/PowerCell" ) },
            { Item.RadioCommComp,       new ItemConstant(Item.RadioCommComp, "Radio - comm Comp.", "MyObjectBuilder_Component/RadioCommunication", "MyObjectBuilder_BlueprintDefinition/RadioCommunicationComponent" ) },
            { Item.ReactorComp,         new ItemConstant(Item.ReactorComp, "Reactor Comp.", "MyObjectBuilder_Component/Reactor", "MyObjectBuilder_BlueprintDefinition/ReactorComponent" ) },
            { Item.SaberoidPlushie,     new ItemConstant(Item.SaberoidPlushie, "Saberoid Plushie", "MyObjectBuilder_Component/SabiroidPlushie", null ) },
            { Item.SmallSteelTube,      new ItemConstant(Item.SmallSteelTube, "Small Steel Tube", "MyObjectBuilder_Component/SmallTube", "MyObjectBuilder_BlueprintDefinition/SmallTube" ) },
            { Item.SolarCell,           new ItemConstant(Item.SolarCell, "Solar Cell", "MyObjectBuilder_Component/SolarCell", "MyObjectBuilder_BlueprintDefinition/SolarCell" ) },
            { Item.SteelPlate,          new ItemConstant(Item.SteelPlate, "Steel Plate", "MyObjectBuilder_Component/SteelPlate", "MyObjectBuilder_BlueprintDefinition/SteelPlate" ) },
            { Item.Superconductor,      new ItemConstant(Item.Superconductor, "Superconductor", "MyObjectBuilder_Component/Superconductor", "MyObjectBuilder_BlueprintDefinition/Superconductor" ) },
            { Item.ThrusterComp,        new ItemConstant(Item.ThrusterComp, "Thruster Comp.", "MyObjectBuilder_Component/Thrust", "MyObjectBuilder_BlueprintDefinition/ThrustComponent" ) },
            { Item.ZoneChip,            new ItemConstant(Item.ZoneChip, "Zone Chip", "MyObjectBuilder_Component/ZoneChip", "MyObjectBuilder_BlueprintDefinition/ZoneChip" ) },

            { Item.Cobalt, new ItemConstant(Item.Cobalt, "Cobalt", "MyObjectBuilder_Ingot/Cobalt", "MyObjectBuilder_BlueprintDefinition/CobaltOreToIngot") },
            { Item.Gold, new ItemConstant(Item.Gold, "Gold", "MyObjectBuilder_Ingot/Gold", "MyObjectBuilder_BlueprintDefinition/GoldOreToIngot") },
            { Item.Ice, new ItemConstant(Item.Ice, "Ice", "MyObjectBuilder_Ingot/Ice", null) },
            { Item.Iron, new ItemConstant(Item.Iron, "Iron", "MyObjectBuilder_Ingot/Iron", "MyObjectBuilder_BlueprintDefinition/IronOreToIngot") },
            { Item.Magnesium, new ItemConstant(Item.Magnesium, "Magnesium", "MyObjectBuilder_Ingot/Magnesium", "MyObjectBuilder_BlueprintDefinition/MagnesiumOreToIngot") },
            { Item.Nickel, new ItemConstant(Item.Nickel, "Nickel", "MyObjectBuilder_Ingot/Nickel", "MyObjectBuilder_BlueprintDefinition/NickelOreToIngot") },
            //{ Item.Organic, new ItemConstant(Item.Organic, "Organic", "MyObjectBuilder_Ingot/Organic", null) },
            { Item.Platinum, new ItemConstant(Item.Platinum, "Platinum", "MyObjectBuilder_Ingot/Platinum", "MyObjectBuilder_BlueprintDefinition/PlatinumOreToIngot") },
            { Item.Scrap, new ItemConstant(Item.Scrap, "Scrap", "MyObjectBuilder_Ingot/Scrap", "MyObjectBuilder_BlueprintDefinition/ScrapOreToIngot") },
            { Item.Silicon, new ItemConstant(Item.Silicon, "Silicon", "MyObjectBuilder_Ingot/Silicon", "MyObjectBuilder_BlueprintDefinition/SiliconOreToIngot") },
            { Item.Silver, new ItemConstant(Item.Silver, "Silver", "MyObjectBuilder_Ingot/Silver", "MyObjectBuilder_BlueprintDefinition/SilverOreToIngot") },
            //{ Item.Stone, new ItemConstant(Item.Stone, "Stone", "MyObjectBuilder_Ingot/Stone", "MyObjectBuilder_BlueprintDefinition/StoneOreToIngot") },
            { Item.Uranium, new ItemConstant(Item.Uranium, "Uranium", "MyObjectBuilder_Ingot/Uranium", "MyObjectBuilder_BlueprintDefinition/UraniumOreToIngot") },
        };
    }
}