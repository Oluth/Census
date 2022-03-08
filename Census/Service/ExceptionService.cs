using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Census.Service
{

    /// <summary>
    /// Prepares internal exceptions and communicates with the game. 
    /// <br/>
    /// <br/>
    /// It is preferred to be used instead of in-game managers due to SoC. 
    /// </summary>
    internal class ExceptionService
    {

        public enum Type
        {
            Severe,
            Warning,
            Information
        }

        private static ExceptionService instance;

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static ExceptionService Instance {
            get
            {
                if (Instance == null)
                {
                    instance = new ExceptionService();
                }
                DebugService.Log(Debug.DebugState.finest, "Return instance of ExceptionService.");
                return instance;
            }
        }

        /// <summary>
        /// Throws an in-game exceptions and logs it.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ex"></param>
        /// <exception cref="ModException">Exception thrown in-game.</exception>
        public void ThrowException(ExceptionService.Type type, Exception ex)
        {
            if (type == null)
            {
                type = Type.Severe;
                ex = new ArgumentNullException(nameof(type), ex);
            }

            switch(type)
            {
                case Type.Severe:
                default:
                    DebugService.Log(Debug.DebugState.error, "ERROR: " + ex.Message);
                    throw new ModException("[Census] " + ex.Message, ex);
                    break;
                case Type.Warning:
                    DebugService.Log(Debug.DebugState.warning, "Warning: " + ex.Message);
                    break;
                case Type.Information:
                    DebugService.Log(Debug.DebugState.info, ex.Message);
                    break;
            }
        }
    }
}
