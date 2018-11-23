//using EscalationManagerWS.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EscalationManagerWS;
using EscalationManagerWS.Logic;

namespace ConsoleApp1
{
    class Program
    {
        private static Timer _timer = null;
        private static EscalationManager manager;

        static void Main(string[] args)
        {
            manager = new EscalationManager();

            // Pass in the time you want to start and the interval
            // Run the service every 24hrs at 3AM
            var startHour = Convert.ToInt32(ConfigurationManager.AppSettings["StartHour"]);
            var startMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["StartMinutes"]);
            var startSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["StartSeconds"]);

            var runEveryHours = Convert.ToInt32(ConfigurationManager.AppSettings["RunEveryHours"]);
            var runEveryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["RunEveryMinutes"]);
            var runEverySeconds = Convert.ToInt32(ConfigurationManager.AppSettings["RunEverySeconds"]);

            StartTimer(new TimeSpan(startHour, startMinutes, startSeconds), new TimeSpan(runEveryHours, runEveryMinutes, runEverySeconds));
        }

        private static void StartTimer(TimeSpan scheduledRunTime, TimeSpan timeBetweenEachRun)
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

            for (; ; )
            {
                // add a sleep for 100 mSec to reduce CPU usage
                Thread.Sleep(100);
            }
        }

        private static void ExecuteService(object state)
        {
            manager.EscalateIfNeeded();
        }
    }
}
