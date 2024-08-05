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
    public class InventoryManager
    {
        private IList<IMyInventory> inventories;

        public InventoryManager(IList<IMyInventory> inventories)
        {
            this.inventories = inventories;
        }

        public MyFixedPoint GetAvailableCount(Item item)
        {
            var def = DefinitionConstants.Components[item].DefinitionId;

            var count = inventories
             .Select(inv => inv.GetItemAmount(def))
             .Aggregate((a, b) => a + b);

            return count;
        }
    }
}