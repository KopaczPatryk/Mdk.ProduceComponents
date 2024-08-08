using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    internal class BlockProvider
    {
        private readonly IMyGridTerminalSystem gridTerminalSystem;
        private readonly IMyProgrammableBlock me;

        public BlockProvider(IMyGridTerminalSystem gridTerminalSystem, IMyProgrammableBlock me)
        {
            this.gridTerminalSystem = gridTerminalSystem;
            this.me = me;
        }

        public void GetAllInventories(out List<IMyInventory> inventories, bool connected = false)
        {
            var blocks = new List<IMyTerminalBlock>();
            gridTerminalSystem.GetBlocks(blocks);

            var blocksOnThisGrid = blocks
                .Where(block => block.CubeGrid == me.CubeGrid);

            var blocksWithInventories = blocksOnThisGrid
                .Where(block => block.HasInventory);

            var outputInventories = blocksWithInventories
                .Where(block => block is IMyProductionBlock)
                .Cast<IMyProductionBlock>()
                .Select(block => block.OutputInventory);

            inventories = blocksWithInventories
                .Select(block => block.GetInventory())
                .Concat(outputInventories).ToList();
        }
    }
}
