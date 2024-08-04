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
    public enum Component
    {
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
    }
    public enum Ingot
    {
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

    public class ItemConstant<T>
    {
        public T Item { get; }
        public string DisplayName { get; }
        public string TypeId { get; }
        public string SubtypeId { get; }

        public string DefinitionIdString
        {
            get
            {
                return $"{TypeId}/{SubtypeId}";
            }
        }

        public MyDefinitionId MyDefinitionId
        {
            get
            {
                return MyDefinitionId.Parse(DefinitionIdString);
            }
        }

        public ItemConstant(T item,
                            string displayName,
                            string typeId,
                            string subtypeId)
        {
            Item = item;
            DisplayName = displayName;
            TypeId = typeId;
            SubtypeId = subtypeId;

        }
    }

    public class DefinitionConstants
    {
        private const string componentTypeId = "MyObjectBuilder_Component";
        private const string ingotTypeId = "MyObjectBuilder_Ingot";

        public static Dictionary<Component, ItemConstant<Component>> components = new Dictionary<Component, ItemConstant<Component>> {
            { Component.BulletproofGlass, new ItemConstant<Component>( Component.BulletproofGlass, "Bulletproof Glass", componentTypeId, "BulletproofGlass") },
            { Component.Canvas, new ItemConstant<Component>( Component.Canvas, "Canvas", componentTypeId, "Canvas")},
            { Component.Computer, new ItemConstant<Component>( Component.Computer, "Computer", componentTypeId, "Computer") },
            { Component.ConstructionComp, new ItemConstant<Component>( Component.ConstructionComp, "Construction Comp.", componentTypeId, "Construction") },
            { Component.DetectorComp, new ItemConstant<Component>( Component.DetectorComp, "Detector Comp.", componentTypeId, "Detector") },
            { Component.Display, new ItemConstant<Component>( Component.Display, "Display", componentTypeId, "Display") },
            { Component.EngineerPlushie, new ItemConstant<Component>( Component.EngineerPlushie, "Engineer Plushie", componentTypeId, "EngineerPlushie") },
            { Component.Explosives, new ItemConstant<Component>( Component.Explosives, "Explosives", componentTypeId, "Explosives") },
            { Component.Girder, new ItemConstant<Component>( Component.Girder, "Girder", componentTypeId, "Girder") },
            { Component.GravityComp, new ItemConstant<Component>( Component.GravityComp, "Gravity Comp.", componentTypeId, "GravityGenerator") },
            { Component.InteriorPlate, new ItemConstant<Component>( Component.InteriorPlate, "Interior Plate", componentTypeId, "InteriorPlate") },
            { Component.LargeSteelTube, new ItemConstant<Component>( Component.LargeSteelTube, "Large Steel Tube", componentTypeId, "LargeTube") },
            { Component.MedicalComp, new ItemConstant<Component>( Component.MedicalComp, "Medical Comp.", componentTypeId, "Medical") },
            { Component.MetalGrid, new ItemConstant<Component>( Component.MetalGrid, "Metal Grid", componentTypeId, "MetalGrid") },
            { Component.Motor, new ItemConstant<Component>( Component.Motor, "Motor", componentTypeId, "Motor") },
            { Component.PowerCell, new ItemConstant<Component>( Component.PowerCell, "Power Cell", componentTypeId, "PowerCell") },
            { Component.RadioCommComp, new ItemConstant<Component>( Component.RadioCommComp, "Radio - comm Comp.", componentTypeId, "RadioCommunication") },
            { Component.ReactorComp, new ItemConstant<Component>( Component.ReactorComp, "Reactor Comp.", componentTypeId, "Reactor") },
            { Component.SaberoidPlushie, new ItemConstant<Component>( Component.SaberoidPlushie, "Saberoid Plushie", componentTypeId, "SabiroidPlushie") },
            { Component.SmallSteelTube, new ItemConstant<Component>( Component.SmallSteelTube, "Small Steel Tube", componentTypeId, "SmallTube") },
            { Component.SolarCell, new ItemConstant<Component>( Component.SolarCell, "Solar Cell", componentTypeId, "SolarCell") },
            { Component.SteelPlate, new ItemConstant<Component>( Component.SteelPlate, "Steel Plate", componentTypeId, "SteelPlate") },
            { Component.Superconductor, new ItemConstant<Component>( Component.Superconductor, "Superconductor", componentTypeId, "Superconductor") },
            { Component.ThrusterComp, new ItemConstant<Component>( Component.ThrusterComp, "Thruster Comp.", componentTypeId, "Thrust") },
            { Component.ZoneChip, new ItemConstant<Component>( Component.ZoneChip, "Zone Chip", componentTypeId, "ZoneChip") },
        };

        public static Dictionary<Ingot, ItemConstant<Ingot>> ingots = new Dictionary<Ingot, ItemConstant<Ingot>> {
            { Ingot.Cobalt, new ItemConstant<Ingot>(Ingot.Cobalt, "Cobalt", ingotTypeId, "Cobalt") },
            { Ingot.Gold, new ItemConstant<Ingot>(Ingot.Gold, "Gold", ingotTypeId, "Gold") },
            { Ingot.Ice, new ItemConstant<Ingot>(Ingot.Ice, "Ice", ingotTypeId, "Ice") },
            { Ingot.Iron, new ItemConstant<Ingot>(Ingot.Iron, "Iron", ingotTypeId, "Iron") },
            { Ingot.Magnesium, new ItemConstant<Ingot>(Ingot.Magnesium, "Magnesium", ingotTypeId, "Magnesium") },
            { Ingot.Nickel, new ItemConstant<Ingot>(Ingot.Nickel, "Nickel", ingotTypeId, "Nickel") },
            { Ingot.Organic, new ItemConstant<Ingot>(Ingot.Organic, "Organic", ingotTypeId, "Organic") },
            { Ingot.Platinum, new ItemConstant<Ingot>(Ingot.Platinum, "Platinum", ingotTypeId, "Platinum") },
            { Ingot.Scrap, new ItemConstant<Ingot>(Ingot.Scrap, "Scrap", ingotTypeId, "Scrap") },
            { Ingot.Silicon, new ItemConstant<Ingot>(Ingot.Silicon, "Silicon", ingotTypeId, "Silicon") },
            { Ingot.Silver, new ItemConstant<Ingot>(Ingot.Silver, "Silver", ingotTypeId, "Silver") },
            { Ingot.Stone, new ItemConstant<Ingot>(Ingot.Stone, "Stone", ingotTypeId, "Stone") },
            { Ingot.Uranium, new ItemConstant<Ingot>(Ingot.Uranium, "Uranium", ingotTypeId, "Uranium") },
        };
    }
}