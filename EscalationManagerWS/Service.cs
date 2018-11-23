using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EscalationManagerWS.Logic;
using System.Configuration;

namespace EscalationManagerWS
{
    public partial class Service : ServiceBase
    {
        private Timer _timer = null;
        private EscalationManager manager;

        public Service()
        {
            InitializeComponent();
            manager = new EscalationManager();
        }

        protected override void OnStart(string[] args)
        {
            // Pass in the time you want to start and the interval
            // Run the service every 24hrs at 3AM

            StartTimer(new TimeSpan(3, 0, 0), new TimeSpan(24, 0, 0));
        }

        protected override void OnStop()
        {
            _timer = null;
            manager = null;
        }

        protected void StartTimer(TimeSpan scheduledRunTime, TimeSpan timeBetweenEachRun)
        {
            // Initialize timer
            double current = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double scheduledTime = scheduledRunTime.TotalMilliseconds;
            double intervalPeriod = timeBetweenEachRun.TotalMilliseconds;

            // calculates the first execution of the method, either its today at the scheduled time or tomorrow (if scheduled time has already occurred today)
            double firstExecution = current > scheduledTime ? intervalPeriod - (current - scheduledTime) : scheduledTime - current;

            // create callback - this is the method that is called on every interval
            var callback = new TimerCallback(ExecuteService);

            // create timer
            _timer = new Timer(callback, null, Convert.ToInt32(firstExecution), Convert.ToInt32(intervalPeriod));
        }

        private void ExecuteService(object state)
        {
            manager.EscalateIfNeeded();
        }
    }
}
