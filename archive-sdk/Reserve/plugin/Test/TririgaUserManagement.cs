using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using Metreos.LoggingFramework;
using Metreos.Common.Reserve;
using Metreos.Utilities;
namespace Test
{
	/// <summary>
	/// Summary description for TririgaUserManagament.
	/// </summary>
	public class UserTest
	{
        delegate void CheckInDelegate(string user, int delay);

        delegate void StressDelegate();
        WorkRequestDelegate worker;
        LogWriter log;
        Metreos.Utilities.ThreadPool pool;

        public UserTest()
		{
            Logger.Instance.verboseMessageSink += new LoggerWriteDelegate(Instance_verboseMessageSink);
            Logger.Instance.warningMessageSink += new LoggerWriteDelegate(Instance_warningMessageSink);
            Logger.Instance.infoMessageSink += new LoggerWriteDelegate(Instance_infoMessageSink);
            Logger.Instance.errorMessageSink += new LoggerWriteDelegate(Instance_errorMessageSink);
			log = new LogWriter(TraceLevel.Verbose, "UserTest");

            
		}

        #region FunctionalTest
        public void FunctionalTest()
        {
            string[][] users = new string[1][];

            // Define one user
            users[0] = new string[2];
            users[0][0] = "user1";
            users[0][1] = "pass1";

            TririgaUsersManagement.Instance.UpdateUsers(log, users); 
            TririgaUsersManagement.Instance.UpdateUsers(log, users); 

            string user;
            string pass;
            if(TririgaUsersManagement.Instance.CheckOut(5000, log, out user, out pass))
            {
                if(user == "user1" && pass == "pass1")
                {
                    
                }
                else
                {
                    log.Write(TraceLevel.Error, "Failed to get the correct user and pass values");
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "Functional test failed--couldn't retrieve a user");
            }

            TririgaUsersManagement.Instance.CheckIn(user, log);

            if(TririgaUsersManagement.Instance.CheckOut(5000, log, out user, out pass))
            {
                if(user == "user1" && pass == "pass1")
                {
                    
                }
                else
                {
                    log.Write(TraceLevel.Error, "Failed to get the correct user and pass values");
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "Functional test failed--couldn't retrieve a user");
            }

            if(TririgaUsersManagement.Instance.CheckOut(5000, log, out user, out pass))
            {
                if(user == "user1" && pass == "pass1")
                {
                    
                }
                else
                {
                    log.Write(TraceLevel.Error, "Failed to get the correct user and pass values");
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "Functional test failed--couldn't retrieve a user");
            }

            TririgaUsersManagement.Instance.UpdateUsers(log, users);
           
            if(TririgaUsersManagement.Instance.CheckOut(5000, log, out user, out pass))
            {
                if(user == "user1" && pass == "pass1")
                {
                    
                }
                else
                {
                    log.Write(TraceLevel.Error, "Failed to get the correct user and pass values");
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "Functional test failed--couldn't retrieve a user");
            }

            // Launch CheckIn thread
            
            CheckInDelegate checkInDelayed = new CheckInDelegate(DelayedCheck);
            checkInDelayed.BeginInvoke("user1", 2500, new AsyncCallback(Done), null);
            if(TririgaUsersManagement.Instance.CheckOut(5000, log, out user, out pass))
            {
                if(user == "user1" && pass == "pass1")
                {
                    
                }
                else
                {
                    log.Write(TraceLevel.Error, "Failed to get the correct user and pass values");
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "Functional test failed--couldn't retrieve a user");
           } 
        }

        public void DelayedCheck(string user, int delay)
        {
            Thread.Sleep(delay);
            TririgaUsersManagement.Instance.CheckIn(user, log);
        }

        public void Done(IAsyncResult asy)
        {
        }
        #endregion

        #region StressTest
        public void StressTest()
        {
            pool = new Metreos.Utilities.ThreadPool(5 , 500, this.GetType().FullName ) ;
            pool.Priority = ThreadPriority.Normal;
            pool.NewThreadTrigger = 400;

            System.Threading.Thread.Sleep(1000);

            pool.Start();

            System.Threading.Thread.Sleep(5000);
            Random rand = new Random();
            worker = new WorkRequestDelegate(Session);

            // Go wild!
            for(int i = 0; i < 10000; i++)
            {
                int request = rand.Next(50); //~25k BHCA
                System.Threading.Thread.Sleep(request);
                pool.PostRequest(worker);
            }
        }


        public void Session(object obj)
        {
            string[][] users = new string[5][];

            // Define one user
            users[0] = new string[2];
            users[0][0] = "user1";
            users[0][1] = "pass1";

            users[1] = new string[2];
            users[1][0] = "user2";
            users[1][1] = "pass2";

            users[2] = new string[2];
            users[2][0] = "user3";
            users[2][1] = "pass3";

            users[3] = new string[2];
            users[3][0] = "user4";
            users[3][1] = "pass4";

            users[4] = new string[2];
            users[4][0] = "user5";
            users[4][1] = "pass5";

            TririgaUsersManagement.Instance.UpdateUsers(log, users); 

            string user;
            string pass;

            bool checkedOut = TririgaUsersManagement.Instance.CheckOut(5000, log, out user, out pass);

            if(!checkedOut)
            {
                log.Write(TraceLevel.Error, "Unable to get a username in Session");
                return;
            }

            Random rand = new Random();
            int signOnTime = rand.Next(1000);
            int saveBoTime = rand.Next(1500);
            int signOutTime = rand.Next(500);

            // Simulate SignOn
            System.Threading.Thread.Sleep(signOnTime);

            // Simulate SaveBO
            System.Threading.Thread.Sleep(saveBoTime);

            // Simulate SignOut
            System.Threading.Thread.Sleep(signOutTime);

            TririgaUsersManagement.Instance.CheckIn(user, log);

            log.Write(TraceLevel.Info, "Done with session");
        }

        #endregion

        private void Instance_verboseMessageSink(DateTime timeStamp, TraceLevel errorLevel, string message)
        {
            //Console.WriteLine(String.Format("{0} {1}: {2}", timeStamp.ToString("HH:mm:ss.fff"), errorLevel.ToString()[0], message));
        }
         

        private void Instance_warningMessageSink(DateTime timeStamp, TraceLevel errorLevel, string message)
        {
            Console.WriteLine(String.Format("{0} {1}: {2}", timeStamp.ToString("HH:mm:ss.fff"), errorLevel.ToString()[0], message));
        }

        private void Instance_infoMessageSink(DateTime timeStamp, TraceLevel errorLevel, string message)
        {
            Console.WriteLine(String.Format("{0} {1}: {2}", timeStamp.ToString("HH:mm:ss.fff"), errorLevel.ToString()[0], message));

        }

        private void Instance_errorMessageSink(DateTime timeStamp, TraceLevel errorLevel, string message)
        {
            Console.WriteLine(String.Format("{0} {1}: {2}", timeStamp.ToString("HH:mm:ss.fff"), errorLevel.ToString()[0], message));
        }
    }
}
