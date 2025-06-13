using System;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Diagnostics;

namespace TimeStampAnalysis
{
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			string directory;
			int min;
			int max;
			if (GetArguments(args, out directory, out min, out max))
			{
				string dataFile = "tmp3565718.txt";	// Arbitrary temp file name that taa.plt expects

				GenerateDataFile(dataFile, directory, min, max);

				Process process = Process.Start("c:/Program Files/gnuplot/bin/wgnuplot.exe",
					"../../taa.plt -");

				process.WaitForExit();
				File.Delete(dataFile);
			}
		}

		private static bool GetArguments(string[] args, out string directory, out int min, out int max)
		{
			bool gotten;

			// Default is current directory, all time stamps
			directory = ".";
			min = 0;
			max = int.MaxValue;

			if (args == null || (args.Length > 0 && (args[0] == "-h" || args[0] == "-?")))
			{
				Console.WriteLine("usage: TimeStampAnaylsis <directory> [ [ <min milliseconds> ] <max milliseconds> ]");

				gotten = false;
			}
			else
			{
				if (args.Length > 0)
				{
					directory = args[0];
				}

				if (args.Length > 1)
				{
					try { min = Convert.ToInt32(args[1]); } catch { }
				}

				if (args.Length > 2)
				{
					try { max = Convert.ToInt32(args[2]); } catch { }
				}

				gotten = true;
			}

			return gotten;
		}

		private static void GenerateDataFile(string dataFile, string directory, int min, int max)
		{
			DateTime currentDate = DateTime.Now;	// GnuPlot needs date, doesn't matter which
			DateTime previous = DateTime.MaxValue;

			TextWriter writer = new StreamWriter(dataFile);

			DirectoryInfo dir = new DirectoryInfo(directory);
			foreach (FileInfo file in dir.GetFiles("*.log"))
			{
				using (StreamReader reader = new StreamReader(file.FullName))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						try
						{
							DateTime next = new DateTime(
								currentDate.Year, currentDate.Month, currentDate.Day,
								Convert.ToInt32(line.Substring(0, 2)),
								Convert.ToInt32(line.Substring(3, 2)),
								Convert.ToInt32(line.Substring(6, 2)),
								Convert.ToInt32(line.Substring(9, 3)));

							// Skip first time so "previous" gets primed
							if (previous != DateTime.MaxValue)
							{
								// Go to next day when hour wraps from 23 to 0
								if (next.Hour < previous.Hour)
								{
									currentDate = currentDate.AddDays(1);
									next = next.AddDays(1);
								}

								TimeSpan delta = next - previous;
								if (delta > TimeSpan.FromMilliseconds(min) &&
									delta < TimeSpan.FromMilliseconds(max))
								{
									writer.WriteLine("{0} {1}",
										next.ToString("G", DateTimeFormatInfo.InvariantInfo),
										delta.TotalSeconds);
								}
							}

							previous = next;
						}
						catch
						{
							// Skip lines that don't start with a timestamp.
						}
					}
				}
			}

			writer.Close();
		}
	}
}
