/*
 * main.cxx
 *
 * PWLib application source file for threadex
 *
 * Main program entry point.
 *
 * Copyright (c) 2003 Equivalence Pty. Ltd.
 *
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.0 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See
 * the License for the specific language governing rights and limitations
 * under the License.
 *
 * The Original Code is Portable Windows Library.
 *
 * The Initial Developer of the Original Code is Equivalence Pty. Ltd.
 *
 * Contributor(s): ______________________________________.
 *
 * $Log: main.cxx,v $
 * Revision 1.5  2005/07/28 00:25:03  dereksmithies
 * Update console reading code so it works via a ssh connection to a remote machine.
 * Add D option to console, so report the average time taken by each iteration of the thread.
 *
 * Revision 1.4  2005/07/26 02:52:39  dereksmithies
 * Use different console handling code. Still get "console gone errors" when on
 * remote box.
 *
 * Revision 1.3  2005/07/26 01:43:05  dereksmithies
 * Set up so that only H or h or ? generate a help message.
 *
 * Revision 1.2  2005/07/26 00:46:22  dereksmithies
 * Commit code to provide two examples of waiting for a thread to terminate.
 * The busy wait method provides a method of testing PWLIB processes for closing
 * a thread. With a delay of 20ms, SMP box, we found some pwlib methods that
 * needed fixing. At the time of committing this change, the pwlib code was correct.
 *
 * Revision 1.1  2004/09/13 01:13:26  dereksmithies
 * Initial release of VERY simple program to test PThread::WaitForTermination
 *
 * Revision 1.3  2004/09/10 22:33:31  dereksmithies
 * Calculate time required to do the decoding of the dtmf symbol.
 *
 * Revision 1.2  2004/09/10 04:31:57  dereksmithies
 * Add code to calculate the detection rate.
 *
 * Revision 1.1  2004/09/10 01:59:35  dereksmithies
 * Initial release of program to test Dtmf creation and detection.
 *
 *
 */

#include "precompile.h"
#include "main.h"
#include "version.h"


PCREATE_PROCESS(Threadex);

#include  <ptclib/dtmf.h>
#include  <ptclib/random.h>



Threadex::Threadex()
  : PProcess("Derek Smithies code factory", "threadex", MAJOR_VERSION, MINOR_VERSION, BUILD_TYPE, BUILD_NUMBER)
{
}

/*Note: This program uses either a busy wait system to check for
   thread termination, or the WaitForTermination method supplied by
   pwlib. It was found that with sufficient number of iterations,
   busywaiting, and a delay of 20ms on a SMP machine that segfaults
   occurred. This program was used to verify the correct close down
   and creation process of a thread */

void Threadex::Main()
{
  PArgList & args = GetArguments();

  args.Parse(
             "h-help."               "-no-help."
	     "d-delay:"              "-no-delay."
	     "b-busywait."           "-no-busywait."
#if PTRACING
             "o-output:"             "-no-output."
             "t-trace."              "-no-trace."
#endif
	     "v-version."
	     );

#if PTRACING
  PTrace::Initialise(args.GetOptionCount('t'),
                     args.HasOption('o') ? (const char *)args.GetOptionString('o') : NULL,
         PTrace::Blocks | PTrace::Timestamp | PTrace::Thread | PTrace::FileAndLine);
#endif

  if (args.HasOption('v')) {
    cout << "Product Name: " << GetName() << endl
	 << "Manufacturer: " << GetManufacturer() << endl
	 << "Version     : " << GetVersion(TRUE) << endl
	 << "System      : " << GetOSName() << '-'
	 << GetOSHardware() << ' '
	 << GetOSVersion() << endl;
    return;
  }

  if (args.HasOption('h')) {
    PError << "Available options are: " << endl         
	   << "-h  or --help        :print this help" << endl
	   << "-v  or --version      print version info" << endl
	   << "-d  or --delay      X where X specifies how many milliseconds the created thread waits for" << endl
	   << "-b  or --busywait     where if specified will cause the created thread to be tested for termination using a busy wait." << endl
#if PTRACING
	   << "o-output              output file name for trace" << endl
	   << "t-trace.              trace level to use." << endl
#endif
	   << endl
	   << endl << endl;
    return;
  }

  PINDEX delay = 2000;;
  if (args.HasOption('d'))
    delay = args.GetOptionString('d').AsInteger();

  delay = PMIN(1000000, PMAX(1, delay));
  cout << "Created thread will wait for " << delay << " milliseconds before ending" << endl;
 
  BOOL doBusyWait = args.HasOption('b');

  UserInterfaceThread ui(delay, doBusyWait);
  ui.WaitForTermination();
}


void LauncherThread::Main()
{
  if (useBusyWait) {
    while (keepGoing) {
      DelayThread thread(delay);
      while (!thread.IsTerminated());
      iteration++;
    }
  } else {
    while (keepGoing) {
      DelayThread thread(delay);
      thread.WaitForTermination();
      iteration++;
    }
  }
}

void UserInterfaceThread::Main()
{
  PConsoleChannel console(PConsoleChannel::StandardInput);
  cout << "This program will repeatedly create and destroy a thread until terminated from the console" << endl;

  PStringStream help;
  help << "Press : " << endl
       << "         D      average Delay time of each thread" << endl
       << "         H or ? help"                              << endl
       << "         R      report count of threads done"      << endl
       << "         T      time elapsed"                      << endl
       << "         X or Q exit "                             << endl;
 
  cout << endl << help;

  LauncherThread launch(delay, useBusyWait);

  console.SetReadTimeout(P_MAX_INDEX);
  for (;;) {
    char ch = console.ReadChar();

    switch (tolower(ch)) {
    case 'd' :
      {
	int i = launch.GetIteration();
	if (i == 0) {
	  cout << "Have not completed an iteration yet, so time per iteration is unavailable" << endl;
	} else {
	  cout << "Average time per iteration is " << (launch.GetElapsedTime().GetMilliSeconds()/((double) i)) 
	       << " milliseconds" << endl;
	}
	cout << "Command ? " << flush;
	break;
      }
    case 'r' :
      cout << "\nHave completed " << launch.GetIteration() << " iterations" << endl;
      cout << "Command ? " << flush;
      break;
    case 't' :
      cout << "\nElapsed time is " << launch.GetElapsedTime() << " (Hours:mins:seconds.millseconds)" << endl;
      cout << "Command ? " << flush;
      break;

    case 'x' :
    case 'q' :
      cout << "Exiting." << endl;
      launch.Terminate();
      launch.WaitForTermination();
      return;
      break;
    case '?' :
    case 'h' :
      cout << help << endl;
      cout << "Command ? " << flush;
    default:
      break;
                                                                                                                                            
    } // end switch
  } // end for
}


// End of File ///////////////////////////////////////////////////////////////
