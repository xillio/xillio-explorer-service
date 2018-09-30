using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using XillioEngineSDK.model;

namespace XillioAPIService
{
    public static class InfoHolder
    {
        public static string syncFolder = Path.GetFullPath("C:\\Users\\Dwight.Peters\\XillioAPI");

        public static Dictionary<string, Tuple<Configuration, Timer>> Configurations =
            new Dictionary<string, Tuple<Configuration, Timer>>();
    }
}