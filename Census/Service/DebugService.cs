using Census.Service.Debug;
using System;
using UnityEngine;

namespace Census.Service
{
    /// <summary>
    /// Service providing the interfaces to handle debug messages from the mod.
    /// </summary>
    internal static class DebugService
    {

        internal static void Log(DebugState state, string msg)
        {
            switch(state)
            {
                case DebugState.error:
                    UnityEngine.Debug.LogError("[Census] " + msg);
                    break;
                case DebugState.warning:
                    UnityEngine.Debug.LogWarning("[Census] " + msg);
                    break;
                case DebugState.info:
                    UnityEngine.Debug.Log("[Census] " + msg);
                    break;
                default:
                    UnityEngine.Debug.Log("[Census] " + msg);
                    break;
            }
        }
    }
}
