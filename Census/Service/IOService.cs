using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Census.Service
{ }

    internal class IOService
    {
        private string path;

        private static IOService instance = new IOService();

        public static IOService Instance { 
            get {
                return instance;
            }
        }
        private IOService()
        {
        Census.Service.DebugService.Log(Census.Service.Debug.DebugState.info, "Begin initializing IO.");
        path = ColossalFramework.IO.DataLocation.applicationBase;
        try { 
            Census.Service.DebugService.Log(Census.Service.Debug.DebugState.error, path);
        } catch(Exception e)
        {
            Census.Service.DebugService.Log(Census.Service.Debug.DebugState.error, "ERROR: " + e.ToString());
        }
        }

    public void WriteInFile(string[] lines, string filename)
    {
        Census.Service.DebugService.Log(Census.Service.Debug.DebugState.info, "Begin writing file " + filename);

        if (!filename.StartsWith(@"\"))
        {
            filename = @"\" + filename;
        }

        using (StreamWriter sw = new StreamWriter(path + filename))
        {
            foreach(string line in lines)
            {
                Census.Service.DebugService.Log(Census.Service.Debug.DebugState.info, line);
                sw.WriteLine(line);
            }
        }
    }
}


