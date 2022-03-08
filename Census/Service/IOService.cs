using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Census.Service
{

    /// <summary>
    /// Manages the IO functionality of Census.
    /// <br/>
    /// <br/>
    /// It is preferred to be used instead of in-game managers due to SoC. 
    /// </summary>
    internal class IOService
    {

        /// <summary>
        /// The base path of Cities: Skylines within the /steamapps/ subfolder.
        /// </summary>
        private string appBase;

        private static IOService instance = new IOService();

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static IOService Instance
        {
            get
            {
                return instance;
            }
        }

        private IOService()
        {
            Census.Service.DebugService.Log(Census.Service.Debug.DebugState.info, "Begin initializing IO.");
            appBase = ColossalFramework.IO.DataLocation.applicationBase;
            try
            {
                Census.Service.DebugService.Log(Census.Service.Debug.DebugState.error, appBase);
            }
            catch (Exception e)
            {
                Census.Service.DebugService.Log(Census.Service.Debug.DebugState.error, "ERROR: " + e.ToString());
            }
        }

        /// <summary>
        /// Writes a new file with the specified lines. Existing files are overwritten.
        /// </summary>
        /// <param name="lines">Array of lines.</param>
        /// <param name="filename">Path relative to appBase.</param>
        public void WriteFile(string[] lines, string filename)
        {
            Census.Service.DebugService.Log(Census.Service.Debug.DebugState.info, "Begin writing file " + filename);

            if (!filename.StartsWith(@"\"))
            {
                filename = @"\" + filename;
            }

            using (StreamWriter sw = new StreamWriter(appBase + filename))
            {
                foreach (string line in lines)
                {
                    Census.Service.DebugService.Log(Census.Service.Debug.DebugState.info, line);
                    sw.WriteLine(line);
                }
            }
        }
    }
}


