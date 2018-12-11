using System;
using System.ServiceProcess;
using XillioServiceLibrary;

namespace XillioAPIService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                StartService();
            }
            catch (Exception e)
            {
                LogService.Log(e);
                throw;
            }
        }

        private static void StartService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new XillioWindowsService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}