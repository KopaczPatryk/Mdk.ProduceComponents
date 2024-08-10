using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript {
    class BlockManager {
        public static void GetAllInventories(
            IList<IMyTerminalBlock> lookAmong,
            out IList<IMyInventory> inventories
        ) {

            var blocksWithInventories = lookAmong
                .Where(block => block.HasInventory);

            var outputInventories = blocksWithInventories
                .Where(block => block is IMyProductionBlock)
                .Cast<IMyProductionBlock>()
                .Select(block => block.OutputInventory);

            inventories = blocksWithInventories
                .Select(block => block.GetInventory())
                .Concat(outputInventories).ToList();
        }
        public static void GetAllAssemblers(
            IList<IMyTerminalBlock> lookAmong,
            out IList<IMyAssembler> assemblers
        ) {
            assemblers = lookAmong
                .Where(block => block is IMyAssembler)
                .Cast<IMyAssembler>().ToList();
        }
    }
}
