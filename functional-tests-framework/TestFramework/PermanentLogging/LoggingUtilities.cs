using System;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.LoggingFramework;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Provides needed grunt work functions
	/// </summary>
	public class LoggingUtilities
	{
        private static LogWriter log;
        
		public LoggingUtilities()
		{
			log = new LogWriter(TraceLevel.Error, "PermanentLoggingUtilities");
		}

        public static PointF[] GeneratePoints(StringCollection x, StringCollection y)
        {        
            if( x.Count != y.Count )
            {
                PointF[] returnValue = {new PointF(0,0)};
            }

            PointF[] points = new PointF[x.Count];

            for(int i = 0; i < x.Count; i++)
            {
                try
                {
                    points[i] = new PointF(float.Parse(x[i]), float.Parse(y[i]));
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, e.Message);
                }
            }

            return points;
        }
	}
}
