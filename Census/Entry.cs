using ICities;
using Census.Service;
using Census.Service.Debug;

namespace Census
{

    /// <summary>
    /// This is the entry point of <b>Census</b> once it's loaded in game. 
    /// </summary>
    public sealed class Entry : IUserMod, ILoadingExtension
    {
        string IUserMod.Name => "Census";

        string IUserMod.Description => "[Pre-Alpha] A Cities: Skylines demography tool.";

        // INFO - DEBUG OUTPUTS ARE PLACEHOLDERS!
        public void OnCreated(ILoading loading)
        {
            DebugService.Log(DebugState.error, "Mod created!");
        }

        public void OnLevelLoaded(LoadMode mode)
        {
            DebugService.Log(DebugState.warning, "Level loaded!");
        }

        public void OnLevelUnloading()
        {
            DebugService.Log(DebugState.warning, "Level unloaded!");
        }

        public void OnReleased()
        {
            DebugService.Log(DebugState.info, "Level released!");
        }
    }
}
