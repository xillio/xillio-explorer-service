using System;
using System.Collections.Generic;
using System.Timers;
using XillioEngineSDK.model;
using XillioEngineSDK.responses;

namespace XillioAPIService
{
    public static class InfoHolder
    {
        public static string syncFolder = "C:\\Users\\Dwight.Peters\\XillioAPI";
        public static List<Tuple<Configuration, Timer>> Configurations = new List<Tuple<Configuration, Timer>>();
        public static AuthenticationInfo auth;
    }
}