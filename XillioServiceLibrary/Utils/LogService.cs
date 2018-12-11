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
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(DateTime.Now.ToLongTimeString() + " " + content);
                sw.Flush();
            }
        }

        public static void Clear()
        {
            if (File.Exists(logLocation))
            {
                File.Delete(logLocation);
            }
        }

        public static void Log(Exception exception)
        {
            Log(exception.GetType() + " at " + exception.Source + " caused by: " + exception.Message);
            Log(exception.StackTrace);
            if (exception.InnerException != null)
            {
                Log("Caused by:");
                Log(exception.InnerException);
            }
        }
    }
}