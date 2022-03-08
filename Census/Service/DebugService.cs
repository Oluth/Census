using Census.Service.Debug;
using System;
using UnityEngine;

namespace Census.Service
{
    /// <summary>
    /// Communicates with the in-game Unity debug engine to broadcast
    /// debug messages. 
    /// <br/>
    /// Debug messages can either be read in output_log.txt after terminating Cities: Skylines
    /// or in the debug console of <a href="https://steamcommunity.com/sharedfiles/filedetails/?id=450877484">ModTools (third-party mod)</a>.
    /// <br/>
    /// <br/>
    /// It is preferred to be used instead of in-game managers due to SoC. 
    /// </summary>
    internal static class DebugService
    {

        /// <summary>
        /// Broadcast a string to the in-game debug console.
        /// </summary>
        /// <param name="state">Priority of the message.</param>
        /// <param name="msg">Message.</param>
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
                case DebugState.fine:
                case DebugState.finest:
                    // Only output in debug mode.
                    break;
                default:
                    UnityEngine.Debug.Log("[Census] " + msg);
                    break;
            }
        }
    }
}
