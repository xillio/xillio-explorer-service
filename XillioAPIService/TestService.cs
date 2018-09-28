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
    public partial class TestService : ServiceBase
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
        
        int count;
        private XillioApi api;
        private UpdateService update;
        private PingService ping;

        public TestService()
        {
            const string syncFolder = "C:\\Users\\Dwight.Peters\\Documents\\XillioProjects";
            InitializeComponent();
            fileSystemWatcher1.Path = syncFolder;
            fileSystemWatcher1.Changed += new FileSystemEventHandler(OnChange);
            
        }

        /// <summary>
        /// Called when a file or folder in the watched folder is changed.
        /// </summary>
        /// <param name="sender">the object that send the event</param>
        /// <param name="e">the event</param>
        private void OnChange(object sender, FileSystemEventArgs e)
        {
            update.HandleFileChanges(sender, e);
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            
            LogService.Log("starting up the service.");

            fileSystemWatcher1.EnableRaisingEvents = true;

            api = new XillioApi("http://tenant.localhost:8080/", true);
            update = new UpdateService();
            ping = new PingService(api);

            ping.Start();
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            LogService.Log("service started.");
        }

        protected override void OnStop()
        {
            // Update the service state to Stop Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            
            ping.Stop();

            fileSystemWatcher1.EnableRaisingEvents = false;

            LogService.Clear();

            // Update the service state to Stopped.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
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