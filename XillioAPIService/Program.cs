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
            var api = new XillioApi("http://tenant.localhost:8080/");
            var auth = api.Authenticate("user", "password", "client", "secret");
            var configs = api.GetConfigurations(auth);
            var children = api.GetChildren(auth, configs[0], "");
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new TestService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
