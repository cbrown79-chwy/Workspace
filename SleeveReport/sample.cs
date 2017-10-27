using System;
using log4net;
using log4net.Config;
using Topshelf;
namespace SampleWindowsService
{
    class Program
    {
        public const string Description = "Sample Windows Service";
        public const string DisplayName = "PPA.SampleWindowsService";
        public const string ServiceName = "PPA.SampleWindowsService";
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(); // <-- This is important
            var logger = LogManager.GetLogger(typeof(Program));
            var argsList = string.Join(" ", args);
            logger.InfoFormat("Starting service with args: {0}", argsList);            
  
            var exitCode = StartService();
            Environment.Exit((int)exitCode);
        }
        private static TopshelfExitCode StartService()
        {
            return HostFactory.Run(x =>
            {
                x.Service<PpaWindowsService>(s =>
                {
                    s.ConstructUsing(name => new PpaWindowsService());
                    s.WhenStarted((tc, hostControl) => tc.Start(hostControl));
                    s.WhenStopped((tc, hostcontrol) => tc.Stop());
                });
                x.RunAsLocalService();
                x.StartAutomatically();
                x.SetDescription(Description);
                x.SetDisplayName(DisplayName);
                x.SetServiceName(ServiceName);
            });
        }
    }
    public class PpaWindowsService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (PpaWindowsService));
        public bool Start(HostControl hostControl)
        {
            try
            {
                Log.Info("Received Start Command");
                // Note: All the configuration and startup code needs to occur in the try() so that if any exceptions do occur, they are logged
                StartupTheApplication();
  
                Log.Info("Service Started");
                return true;
            }
            catch (Exception exception)
            {
                Log.Error("An unhandled error occurred when trying to start the service", exception);
                hostControl.Stop(); 
                return false;
            }
        }
        private void StartupTheApplication()
        {
            // This is where application specific startup items occur
        }
        public bool Stop()
        {
            Log.Info("Received Stop Command");
            // Do work
            Log.Info("Service Stopped");
  
            LogManager.Shutdown();
            return true;
        }
    }
    
    public class PpaServiceHostControl : IServiceHostControl
    {
        private readonly HostControl _hostControl;
        public PpaServiceHostControl(HostControl hostControl)
        {
            _hostControl = hostControl;
        }
        public void Stop()
        {
            _hostControl.Stop();
        }
        public void Restart()
        {
            _hostControl.Restart();
        }
    }
    public interface IServiceHostControl
    {
        void Stop();
        void Restart();
    }
}