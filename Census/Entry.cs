using ICities;
using Census.Manager;
using Census.UI;

namespace Census
{

    /// <summary>
    /// This is the entry point of <b>Census</b> once it's loaded in game. 
    /// </summary>
    public sealed class Entry : IUserMod, ILoadingExtension, IThreadingExtension
    {
        private static string ver = "0.0.0-alpha.2";
        private int frameBuffer = 120;
        private int frameBufferCount = 0;

        string IUserMod.Name => string.Format("Census [{0}]", ver);

        string IUserMod.Description => "[Alpha Preview] A Cities: Skylines demography tool. FOR TESTING PURPOSES ONLY!";

        public void OnCreated(ILoading loading) {
            Service.DebugService.Log(Service.Debug.DebugState.info, "Created.");
        }

        public void OnLevelLoaded(LoadMode mode) {
            InternalUIManager.Instance.AddWindowToGameView(typeof(PopPyramidWindow));
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
            bool preCondition = (frameBufferCount + 1) % frameBuffer == 0;
            Service.DebugService.Log(Service.Debug.DebugState.finest, "Frame: " + frameBufferCount + ", " + preCondition);
            frameBufferCount++;
            if ((frameBufferCount + 1) % frameBuffer == 0)
            {
                Service.DebugService.Log(Service.Debug.DebugState.finest, "Refresh population window...");
                PopPyramidWindow instance = PopPyramidWindow.Instance;
                Service.DebugService.Log(Service.Debug.DebugState.finest, "Fetched PopPyramidWindow instance.");
                instance.PrintPopGraph();
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
