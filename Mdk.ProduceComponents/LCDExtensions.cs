using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    internal static class LCDExtensions
    {
        internal static void Clear(this IMyTextSurface myTextSurface)
        {
            myTextSurface.WriteText("");
        }
        internal static void WriteLine(this IMyTextSurface myTextSurface, string line)
        {
            myTextSurface.WriteText($"{line}\n", append: true);
        }
    }
}
