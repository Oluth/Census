using ICities;
using Census.Service;
using Census.Service.Debug;
using Census.Manager;
using Census.Util.Demography;
using Census.UI;

namespace Census
{

    /// <summary>
    /// This is the entry point of <b>Census</b> once it's loaded in game. 
    /// </summary>
    public sealed class Entry : IUserMod, ILoadingExtension, IThreadingExtension
    {
        private int frameBuffer = 120;
        private int frameBufferCount = 0;

        string IUserMod.Name => "Census";

        string IUserMod.Description => "[Alpha Preview] A Cities: Skylines demography tool. FOR TESTING PURPOSES ONLY!";

        public void OnCreated(ILoading loading) { }

        public void OnLevelLoaded(LoadMode mode) {
            InternalUIManager.Instance.OpenTestWindow();
        }

        public void OnLevelUnloading() { }

        public void OnReleased() { }

        void IThreadingExtension.OnAfterSimulationFrame()
        {
            
        }

        void IThreadingExtension.OnAfterSimulationTick()
        {
        }

        void IThreadingExtension.OnBeforeSimulationFrame()
        {
            frameBufferCount++;
            if ((frameBufferCount + 1) % frameBuffer == 0 && CUIPopPyramid.Instance != null)
            {
                DebugService.Log(DebugState.error, "PING");
                 CUIPopPyramid.Instance.PrintMakeshiftPopGraph();
                frameBufferCount = 0;
            }
        }

        void IThreadingExtension.OnBeforeSimulationTick()
        {
        }

        void IThreadingExtension.OnCreated(IThreading threading)
        {
        }

        void IThreadingExtension.OnReleased()
        {
        }

        void IThreadingExtension.OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
        }
    }
}
