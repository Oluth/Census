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
                    UnityEngine.Debug.LogError(msg);
                    break;
                case DebugState.warning:
                    UnityEngine.Debug.LogWarning(msg);
                    break;
                case DebugState.info:
                    UnityEngine.Debug.Log(msg);
                    break;
                default:
                    UnityEngine.Debug.Log(msg);
                    break;
            }
        }
    }
}
