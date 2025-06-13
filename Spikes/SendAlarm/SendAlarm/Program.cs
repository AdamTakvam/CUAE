using System;

using Metreos.Stats;
using Metreos.Interfaces;

namespace SendAlarm
{
    class Program
    {
        static void Main(string[] args)
        {
            StatsClient client = StatsClient.Instance;
            client.OnAlarmAck += new AlarmAckDelegate(OnAlarmAck);
            client.OnStatAck += new StatAckDelegate(OnStatAck);
            client.OnStatNack += new StatNackDelegate(OnStatNack);

            string input = "";
            while(input != "q")
            {
                Console.WriteLine("Press 'a' to trigger an alarm, 's' for a stat or 'q' to quit");
                input = Console.ReadLine();

                if(input == "a")
                {
                    client.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.Licensing.AppSessionsExceeded,
                        "App Sessions exceeded: 1234");
                }
                else if(input == "s")
                {
                    client.SetStatistic(IStats.Statistics.AppSessions, 1234);
                }
            }
        }

        static void OnStatNack(string errorText)
        {
            Console.WriteLine("Stat rejected: " + errorText);
        }

        static void OnStatAck(int oid)
        {
            Console.WriteLine("Stat accepted: " + oid);
        }

        static void OnAlarmAck(string alarmCode)
        {
            Console.WriteLine("Alarm ACK: " + alarmCode);
        }
    }
}
