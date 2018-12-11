using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using XillioEngineSDK;

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
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new XillioWindowsService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception e)
            {
                LogService.Log(e);
                throw;
            }
        }
    }
}
