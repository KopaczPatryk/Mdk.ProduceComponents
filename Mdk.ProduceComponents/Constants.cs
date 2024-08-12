using System.Collections.Generic;
using VRage.Game;


namespace IngameScript {
    public enum ItemType {
        Ore,
        Ice,
        Component,
        Ingot,
    }
    public enum Item {
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

    public class ItemConstant {
        public Item Item { get; }
        public string DisplayName { get; }
        public ItemType ItemType { get; }
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
                            string recipeDefId,
                            ItemType itemType) {
            Item = item;
            DisplayName = displayName;
            ItemType = itemType;
            DefinitionId = MyDefinitionId.Parse(definitionId);
            if(recipeDefId != null) {
                RecipeDefId = MyDefinitionId.Parse(recipeDefId);
            }
        }
    }

    public class DefinitionConstants {
        private DefinitionConstants() { }

        public static Dictionary<Item, ItemConstant> Components = new Dictionary<Item, ItemConstant> {
            { Item.BulletproofGlass,    new ItemConstant(Item.BulletproofGlass, "Bulletproof Glass", "MyObjectBuilder_Component/BulletproofGlass", "MyObjectBuilder_BlueprintDefinition/BulletproofGlass", itemType:ItemType.Component ) },
            { Item.Canvas,              new ItemConstant(Item.Canvas, "Canvas", "MyObjectBuilder_Component/Canvas", "MyObjectBuilder_BlueprintDefinition/Position0030_Canvas", itemType: ItemType.Component )},
            { Item.Computer,            new ItemConstant(Item.Computer, "Computer", "MyObjectBuilder_Component/Computer", "MyObjectBuilder_BlueprintDefinition/ComputerComponent", itemType:ItemType.Component ) },
            { Item.ConstructionComp,    new ItemConstant(Item.ConstructionComp, "Construction Comp.", "MyObjectBuilder_Component/Construction", "MyObjectBuilder_BlueprintDefinition/ConstructionComponent", itemType:ItemType.Component ) },
            { Item.DetectorComp,        new ItemConstant(Item.DetectorComp, "Detector Comp.", "MyObjectBuilder_Component/Detector", "MyObjectBuilder_BlueprintDefinition/DetectorComponent", itemType:ItemType.Component ) },
            { Item.Display,             new ItemConstant(Item.Display, "Display", "MyObjectBuilder_Component/Display", "MyObjectBuilder_BlueprintDefinition/Display", itemType:ItemType.Component ) },
            { Item.EngineerPlushie,     new ItemConstant(Item.EngineerPlushie, "Engineer Plushie", "MyObjectBuilder_Component/EngineerPlushie", null, itemType:ItemType.Component ) },
            { Item.Explosives,          new ItemConstant(Item.Explosives, "Explosives", "MyObjectBuilder_Component/Explosives", "MyObjectBuilder_BlueprintDefinition/ExplosivesComponent", itemType:ItemType.Component ) },
            { Item.Girder,              new ItemConstant(Item.Girder, "Girder", "MyObjectBuilder_Component/Girder", "MyObjectBuilder_BlueprintDefinition/GirderComponent", itemType:ItemType.Component ) },
            { Item.GravityComp,         new ItemConstant(Item.GravityComp, "Gravity Comp.", "MyObjectBuilder_Component/GravityGenerator", "MyObjectBuilder_BlueprintDefinition/GravityGeneratorComponent", itemType:ItemType.Component ) },
            { Item.InteriorPlate,       new ItemConstant(Item.InteriorPlate, "Interior Plate", "MyObjectBuilder_Component/InteriorPlate", "MyObjectBuilder_BlueprintDefinition/InteriorPlate", itemType:ItemType.Component ) },
            { Item.LargeSteelTube,      new ItemConstant(Item.LargeSteelTube, "Large Steel Tube", "MyObjectBuilder_Component/LargeTube", "MyObjectBuilder_BlueprintDefinition/LargeTube", itemType:ItemType.Component ) },
            { Item.MedicalComp,         new ItemConstant(Item.MedicalComp, "Medical Comp.", "MyObjectBuilder_Component/Medical", "MyObjectBuilder_BlueprintDefinition/MedicalComponent", itemType:ItemType.Component ) },
            { Item.MetalGrid,           new ItemConstant(Item.MetalGrid, "Metal Grid", "MyObjectBuilder_Component/MetalGrid", "MyObjectBuilder_BlueprintDefinition/MetalGrid", itemType:ItemType.Component ) },
            { Item.Motor,               new ItemConstant(Item.Motor, "Motor", "MyObjectBuilder_Component/Motor", "MyObjectBuilder_BlueprintDefinition/MotorComponent", itemType:ItemType.Component ) },
            { Item.PowerCell,           new ItemConstant(Item.PowerCell, "Power Cell", "MyObjectBuilder_Component/PowerCell", "MyObjectBuilder_BlueprintDefinition/PowerCell", itemType:ItemType.Component ) },
            { Item.RadioCommComp,       new ItemConstant(Item.RadioCommComp, "Radio - comm Comp.", "MyObjectBuilder_Component/RadioCommunication", "MyObjectBuilder_BlueprintDefinition/RadioCommunicationComponent", itemType:ItemType.Component ) },
            { Item.ReactorComp,         new ItemConstant(Item.ReactorComp, "Reactor Comp.", "MyObjectBuilder_Component/Reactor", "MyObjectBuilder_BlueprintDefinition/ReactorComponent", itemType:ItemType.Component ) },
            { Item.SaberoidPlushie,     new ItemConstant(Item.SaberoidPlushie, "Saberoid Plushie", "MyObjectBuilder_Component/SabiroidPlushie", null, itemType:ItemType.Component ) },
            { Item.SmallSteelTube,      new ItemConstant(Item.SmallSteelTube, "Small Steel Tube", "MyObjectBuilder_Component/SmallTube", "MyObjectBuilder_BlueprintDefinition/SmallTube", itemType:ItemType.Component ) },
            { Item.SolarCell,           new ItemConstant(Item.SolarCell, "Solar Cell", "MyObjectBuilder_Component/SolarCell", "MyObjectBuilder_BlueprintDefinition/SolarCell", itemType:ItemType.Component ) },
            { Item.SteelPlate,          new ItemConstant(Item.SteelPlate, "Steel Plate", "MyObjectBuilder_Component/SteelPlate", "MyObjectBuilder_BlueprintDefinition/SteelPlate", itemType:ItemType.Component ) },
            { Item.Superconductor,      new ItemConstant(Item.Superconductor, "Superconductor", "MyObjectBuilder_Component/Superconductor", "MyObjectBuilder_BlueprintDefinition/Superconductor", itemType:ItemType.Component ) },
            { Item.ThrusterComp,        new ItemConstant(Item.ThrusterComp, "Thruster Comp.", "MyObjectBuilder_Component/Thrust", "MyObjectBuilder_BlueprintDefinition/ThrustComponent", itemType:ItemType.Component ) },
            { Item.ZoneChip,            new ItemConstant(Item.ZoneChip, "Zone Chip", "MyObjectBuilder_Component/ZoneChip", "MyObjectBuilder_BlueprintDefinition/ZoneChip", itemType:ItemType.Component ) },

            { Item.Cobalt, new ItemConstant(Item.Cobalt, "Cobalt", "MyObjectBuilder_Ingot/Cobalt", "MyObjectBuilder_BlueprintDefinition/CobaltOreToIngot", itemType: ItemType.Ingot) },
            { Item.Gold, new ItemConstant(Item.Gold, "Gold", "MyObjectBuilder_Ingot/Gold", "MyObjectBuilder_BlueprintDefinition/GoldOreToIngot", itemType: ItemType.Ingot) },
            { Item.Ice, new ItemConstant(Item.Ice, "Ice", "MyObjectBuilder_Ingot/Ice", null, itemType: ItemType.Ingot) },
            { Item.Iron, new ItemConstant(Item.Iron, "Iron", "MyObjectBuilder_Ingot/Iron", "MyObjectBuilder_BlueprintDefinition/IronOreToIngot", itemType: ItemType.Ingot) },
            { Item.Magnesium, new ItemConstant(Item.Magnesium, "Magnesium", "MyObjectBuilder_Ingot/Magnesium", "MyObjectBuilder_BlueprintDefinition/MagnesiumOreToIngot", itemType: ItemType.Ingot) },
            { Item.Nickel, new ItemConstant(Item.Nickel, "Nickel", "MyObjectBuilder_Ingot/Nickel", "MyObjectBuilder_BlueprintDefinition/NickelOreToIngot", itemType: ItemType.Ingot) },
            //{ Item.Organic, new ItemConstant(Item.Organic, "Organic", "MyObjectBuilder_Ingot/Organic", null) },
            { Item.Platinum, new ItemConstant(Item.Platinum, "Platinum", "MyObjectBuilder_Ingot/Platinum", "MyObjectBuilder_BlueprintDefinition/PlatinumOreToIngot", itemType: ItemType.Ingot) },
            { Item.Scrap, new ItemConstant(Item.Scrap, "Scrap", "MyObjectBuilder_Ingot/Scrap", "MyObjectBuilder_BlueprintDefinition/ScrapOreToIngot", itemType: ItemType.Ingot) },
            { Item.Silicon, new ItemConstant(Item.Silicon, "Silicon", "MyObjectBuilder_Ingot/Silicon", "MyObjectBuilder_BlueprintDefinition/SiliconOreToIngot", itemType: ItemType.Ingot) },
            { Item.Silver, new ItemConstant(Item.Silver, "Silver", "MyObjectBuilder_Ingot/Silver", "MyObjectBuilder_BlueprintDefinition/SilverOreToIngot", itemType: ItemType.Ingot) },
            { Item.Stone, new ItemConstant(Item.Stone, "Stone", "MyObjectBuilder_Ingot/Stone", "MyObjectBuilder_BlueprintDefinition/StoneOreToIngot", itemType: ItemType.Ore) },
            { Item.Uranium, new ItemConstant(Item.Uranium, "Uranium", "MyObjectBuilder_Ingot/Uranium", "MyObjectBuilder_BlueprintDefinition/UraniumOreToIngot", itemType: ItemType.Ingot) },
        };
    }
}