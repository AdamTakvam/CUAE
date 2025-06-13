using System;

namespace StressTesting
{
	/// <summary>
	/// Summary description for RangeQueue.
	/// </summary>
	public class RangeQueue
	{
        public string[] lowerBound;
        public string[] upperBound;
        bool[] checkedIn;
        string[] numbersToCall;     
        int accumulatedRanges;
        public object rangeQueueLock;

		public RangeQueue(int numberOfBounds, string[] lowerBound, string[] upperBound)
		{
            rangeQueueLock = new object();
            accumulatedRanges = 0;

            for(int i = 0; i < numberOfBounds; i++)
            {
                accumulatedRanges += Int32.Parse(upperBound[i]) - Int32.Parse(lowerBound[i]);
            }

            checkedIn = new bool[accumulatedRanges];
            numbersToCall = new string[accumulatedRanges];

            int incrementer = 0;

            for(int i = 0; i < numberOfBounds; i++)
            {
                for(int j = Int32.Parse(lowerBound[i]); j < Int32.Parse(upperBound[i]) ; j++)
                {
                    numbersToCall[incrementer] = j.ToString();;
                    checkedIn[incrementer] = true;
                    incrementer++;
                }              
            }
            
		}

        public bool GrabANumber(out string number)
        {
            number = "-1";

            for(int i = 0; i < accumulatedRanges; i++)
            {
                lock(rangeQueueLock)
                {
                    if(checkedIn[i])
                    {
                        number = numbersToCall[i];
                        checkedIn[i] = false;
                        return true;
                    }
                }
            }
    
            return false;
        }

        public bool ReserveANumber(string number)
        {
            if(number == null)
                return true;

            for(int i = 0; i < accumulatedRanges;i++)
            {
                lock(rangeQueueLock)
                {
                    if(number == numbersToCall[i])
                    {
                        checkedIn[i] = false;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckNumber(string number)
        {
            if(number == null)
                return true;

            for(int i = 0; i < accumulatedRanges;i++)
            {
                lock(rangeQueueLock)
                {
                    if(number == numbersToCall[i])
                    {
                        if(checkedIn[i] == false)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }      
                }
            }

            return true;
        }
        public void ReturnANumber(string number)
        {
            if(number == null)
                return;

            for(int i = 0; i < accumulatedRanges; i++)
            {
                lock(rangeQueueLock)
                {
                    if(numbersToCall[i] == number)
                    {
                        checkedIn[i] = true;
                        return;
                    }
                }
            }
        }
	}
}
