using System.IO;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System;
using XillioEngineSDK;

namespace XillioAPIService
{
    
    /// <summary>
    /// To run this service call: C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe .\XillioAPIService.exe
    /// in the bin/debug folder in admin mode. and uninstall using /u
    /// </summary>
    public partial class XillioWindowsService : ServiceBase
    {
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }
        
        private XillioApi api;
        private WatcherService watcher = new WatcherService();
        private PingService ping = new PingService();
        ServiceStatus serviceStatus;

        public XillioWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(ServiceHandle, ref serviceStatus);
            
            LogService.Clear();
            LogService.Log("starting up the service.");

            api = new XillioApi("http://tenant.localhost:8080/", true);
            RunAuthentication();
            
            //Setup other services
            watcher.api = api;
            watcher.Start();
            ping.api = api;
            try
            {
                ping.Start();
            }
            catch (Exception e)
            {
                LogService.Log(e);
                throw;
            }

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            LogService.Log("service started.");
        }

        protected override void OnPause()
        {
            // Update service status
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSE_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(ServiceHandle, ref serviceStatus);
       
            // pause other services
            watcher.Pause();
            ping.Pause();
            
            // Update service status
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSED;
            SetServiceStatus(ServiceHandle, ref serviceStatus);
        }

        protected override void OnContinue()
        {
            // Update service status
            serviceStatus.dwCurrentState = ServiceState.SERVICE_CONTINUE_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(ServiceHandle, ref serviceStatus);
       
            // pause other services
            watcher.Resume();
            ping.Resume();
            
            // Update service status
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            // Update the service state to Stop Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            
            ping.Stop();
            watcher.Stop();

            LogService.Clear();

            // Update the service state to Stopped.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        
        
        
        private void RunAuthentication()
        {
            LogService.Log("authenticating");
            api.Authenticate("user", "password", "client", "secret");
            LogService.Log("Authentication Complete");
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };
    }
}