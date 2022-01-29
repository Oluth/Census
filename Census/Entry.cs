using ICities;
using Census.Service;
using Census.Service.Debug;
using Census.Manager;

namespace Census
{

    /// <summary>
    /// This is the entry point of <b>Census</b> once it's loaded in game. 
    /// </summary>
    public sealed class Entry : IUserMod, ILoadingExtension
    {
        string IUserMod.Name => "Census";

        string IUserMod.Description => "[Pre-Alpha] A Cities: Skylines demography tool.";

        public void OnCreated(ILoading loading) { }

        public void OnLevelLoaded(LoadMode mode) {
            DebugService.Log(DebugState.error, "Inhabitants: " + InternalCitizenManager.GetInhabitantCitizens());
        }

        public void OnLevelUnloading() { }

        public void OnReleased() { }
    }
}
