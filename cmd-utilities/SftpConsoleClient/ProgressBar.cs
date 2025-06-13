using System;

using Metreos.Interfaces;

namespace Metreos.SftpConsoleClient
{
	public class ProgressBar : IProgressIndicator
	{
        private int _value = 0;

        public int Value 
        { 
            get { return _value; }
            set 
            {
                if(_value < value && value <= Maximum)
                {
                    int diff = value - _value;
                    for(int i=0; i<diff; i++)
                        Console.Write(".");

                    _value = value; 
                }
            }
        }

        public bool Canceled { get { return false; } }

        public int Maximum { get { return 70; } }
    
        public ProgressBar()
        {
            Console.Write("Progress: ");
        }
    }
}
