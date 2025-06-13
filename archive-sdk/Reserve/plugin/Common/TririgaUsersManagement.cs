using System;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using Metreos.LoggingFramework;

namespace Metreos.Common.Reserve
{
    /// <summary>
    ///     Used to manage users for the Tririga Web Service.
    ///     In the version of Tririga we are interacting with, only one user can be 
    ///     performing a SaveBo actoin at a time.  To get around this concurrency issue,
    ///     we use this singleton to manage these users--applications request that
    ///     they be given a username (CheckOut), and when they are done, they give the
    ///     username back (CheckIn).
    ///     
    ///     CheckOut takes a ms wait time because obviously there is still a chance for
    ///     this process to block the script.
    ///     
    ///     UpdateUsers allows a script to tell this manager to add or remove user names
    ///     from the list.  Because there is no 'RefreshConfiguration' concept in applications,
    ///     this method will have to be called every time a script first runs. 
    ///     
    ///     This does not support an administrator making different usernames/passwords
    ///     accross different partitions--this is a per-application concurrency controller,
    ///     so it should not be needed to configure different users per partition.
    ///     
    ///     This model is based on an application and all script instances living in 
    ///     their own appdomain, hence making the static 
    ///     singleton accessible across all scripts 'for free'.  If one wants to ensure that
    ///     our system doesn't break the Tririga-concurrency rule, then distinct usernames
    ///     must be given across all users.
    /// </summary>
	public class TririgaUsersManagement
	{
        private Hashtable availableUsers;
        private TimeSpan reservationTimeout;

        #region Singleton
        private static TririgaUsersManagement instance;
        private static object instanceLock = new object();
        public static TririgaUsersManagement Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (instanceLock)
                    {
                        if(instance == null)
                        {
                            instance = new TririgaUsersManagement();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion Singleton

        private TririgaUsersManagement()
		{
		    availableUsers = new Hashtable();
            reservationTimeout = new TimeSpan(0, 0, 10, 0, 0); // 10 minutes
		}

        public void CheckIn(string username, LogWriter log)
        {
            
            lock(this)
            {
                lock(availableUsers.SyncRoot)
                {
                    if(availableUsers.Contains(username))
                    {
                        User user = availableUsers[username] as User;
                        user.InUse = false;
                    }
                    else
                    {
                        // The user must be removed from the collection due
                        // to an update to the application config
                        log.Write(TraceLevel.Verbose, "User not found in list on CheckIn--administrator must have changed the usernames in the application configuration");
                    }
                }
                // Set the checkin event
                Monitor.Pulse(this);
            }
        }

        public bool CheckOut(int wait, LogWriter log, out string username, out string password)
        {
            // Find an available username

            bool success = false;
            username = null;
            password = null;

            User foundUser = null;

            lock(this)
            {
                lock(availableUsers.SyncRoot)
                {
                    foreach(User user in availableUsers.Values)
                    {
                        if(!user.InUse)
                        {
                            // This user will be reserved--flag it as in use while in the lock
                            foundUser = user;
                            foundUser.InUse = true;
                            foundUser.Reserved = DateTime.Now;
                            break;
                        }
                    }
                }

                if(foundUser != null) // Found the user, assign values
                {
                    username = foundUser.Username;
                    password = foundUser.Password;
                    success = true;
                }
                else
                {
                    log.Write(TraceLevel.Warning, "No users are available for CheckOut.  Waiting in line for {0}ms", wait);
                
   
                    bool acquired = System.Threading.Monitor.Wait(this, wait);
  
                    if(acquired)
                    {
                        lock(availableUsers.SyncRoot)
                        {
                            foreach(User user in availableUsers.Values)
                            {
                                if(!user.InUse)
                                {
                                    // This user will be reserved--flag it as in use while in the lock
                                    foundUser = user;
                                    foundUser.Reserved = DateTime.Now;
                                    foundUser.InUse = true;
                                    break;
                                }
                                else if(DateTime.Now.Subtract(user.Reserved) > reservationTimeout)
                                {
                                    log.Write(TraceLevel.Warning, "Expiring a reservation for user {0}", user.Username);
                                    // This user will be reserved--flag it as in use while in the lock
                                    foundUser = user;
                                    foundUser.Reserved = DateTime.Now;
                                    foundUser.InUse = true;

                                }
                            }
                        }

                        if(foundUser != null)  // Found the user, assign values
                        {
                            username = foundUser.Username;
                            password = foundUser.Password;
                            success = true;
                            log.Write(TraceLevel.Verbose, "Retrieved user for CheckOut after waiting in line");
                        }
                        else
                        {
                            log.Write(TraceLevel.Error, "Unable to acquire a user even after CheckIn");
                        }
                    }
                    else
                    {
                        log.Write(TraceLevel.Error, "Unable to acquire a user after {0}ms", wait);
                    }
                }
            }

            return success;
        }

        public void UpdateUsers(LogWriter log, params string[][] userPassPairs)
        {
            foreach(string[] userPassPair in userPassPairs)
            {
                if(userPassPair == null || userPassPair.Length != 2)
                {
                    continue;
                }

                string username = userPassPair[0] as string;
                string password = userPassPair[1] as string;

                if( (username == null || username == String.Empty) &&
                    (password == null || password == String.Empty) )
                {
                    continue;
                }

                lock(availableUsers.SyncRoot)
                {
                    if(!availableUsers.Contains(username))
                    {
                        // We need to add this username to our partitioned user collection
                        User user = new User(username, password, false);
                        availableUsers[username] = user;
                        log.Write(TraceLevel.Verbose, "Adding user {0} to reservation list", username);
                    }
                    else
                    {
                        // User is already contained in the collection, so leave it be
                        log.Write(TraceLevel.Verbose, "Skipping user {0} for addition to reservation list--already added", username);
                    }
                }
            }

            ArrayList toRemove = new ArrayList();
            // Then iterate through users, and check for leftovers
            lock(availableUsers.SyncRoot)
            {
                foreach(string username in availableUsers.Keys)
                {
                    bool found = false;
                    foreach(string[] userPassPair in userPassPairs)
                    {
                        if(userPassPair == null || userPassPair.Length != 2)
                        {
                            continue;
                        }

                        string checkAgainstUser = userPassPair[0] as string;
                        if(username == checkAgainstUser)
                        {
                            // Found them!  
                            found = true;
                            break;
                        }
                    }

                    if(!found)
                    {
                        toRemove.Add(username);
                    }
                }
         
                foreach(string user in toRemove)
                {
                    log.Write(TraceLevel.Verbose, "Removing user {0} from reservation list", user);
                    availableUsers.Remove(user);
                }
            }
        }

        public class User
        {
            public string Username;
            public string Password;
            public bool InUse;
            public DateTime Reserved;

            public User (string name, string pass, bool inUse)
            {
                this.Username = name;
                this.Password = pass;
                this.InUse = inUse;
            }
        }

	}
}
