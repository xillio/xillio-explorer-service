using System;
using System.IO;

namespace XillioAPIService
{
    public static class LogService
    {
        private static String logLocation = @"C:\TestServiceLog.txt";
        public static void Log(string content)
        {
            FileStream fs = new FileStream(logLocation, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
        }

        public static void Clear()
        {
            if(File.Exists(logLocation))
            {
                File.Delete(logLocation);
            }
        }
    }
}