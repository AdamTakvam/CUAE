using System;
using System.Threading;

namespace Metreos.Providers.Http
{
    public class SessionData
    {
        private int expMinutes;
        private DateTime expirationDateTime;

        public bool IsExpired
        {
            get
            {
                lock(this)
                {
                    if(DateTime.Now >= expirationDateTime) return true;
                    return false;
                }
            }
        }


        public SessionData(int expirationMinutes)
        {
            expMinutes = expirationMinutes;
            ResetExpirationTime();
        }


        public void ForceExpiration()
        {
            // Forcefully mark this session for expiration
        }


        public void ResetExpirationTime()
        {
            lock(this)
            {
                expirationDateTime = DateTime.Now.AddMinutes(expMinutes);
            }
        }
    }
}
