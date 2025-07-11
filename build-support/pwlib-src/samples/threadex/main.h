/*
 * main.h
 *
 * PWLib application header file for threadex
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
 * $Log: main.h,v $
 * Revision 1.2  2005/07/26 00:46:22  dereksmithies
 * Commit code to provide two examples of waiting for a thread to terminate.
 * The busy wait method provides a method of testing PWLIB processes for closing
 * a thread. With a delay of 20ms, SMP box, we found some pwlib methods that
 * needed fixing. At the time of committing this change, the pwlib code was correct.
 *
 * Revision 1.1  2004/09/13 01:13:26  dereksmithies
 * Initial release of VERY simple program to test PThread::WaitForTermination
 *
 * Revision 1.1  2004/09/10 01:59:35  dereksmithies
 * Initial release of program to test Dtmf creation and detection.
 *
 *
 */

#ifndef _Threadex_MAIN_H
#define _Threadex_MAIN_H

/**This class is a simple simple thread that just creates, waits a
   period of time, and exits.It is designed to test the PwLib methods
   for reporting the status of a thread. This class will be created
   over and over- millions of times is possible if left long
   enough. If the pwlib thread status functions are broken, a segfault
   will result. Past enxperience has found a fault in pwlib with the
   BusyWait option on, with SMP machines and a delay period of 20ms */
class DelayThread : public PThread
{
  PCLASSINFO(DelayThread, PThread);
  
public:
  DelayThread(PINDEX _delay)
    : PThread(1000, NoAutoDeleteThread), delay(_delay)
    { Resume(); }
  
  void Main() { PThread::Sleep(delay); }
    
 protected:
  PINDEX delay;
};

////////////////////////////////////////////////////////////////////////////////
/**This thread handles the Users console requests to query the status of 
   the launcher thread. It provides a means for the user to close down this
   program - without having to use Ctrl-C*/
class UserInterfaceThread : public PThread
{
  PCLASSINFO(UserInterfaceThread, PThread);
  
public:
  UserInterfaceThread(PINDEX _delay, BOOL _useBusyWait)
    : PThread(1000, NoAutoDeleteThread), delay(_delay), useBusyWait(_useBusyWait)
    { Resume(); }
  
  void Main();
    
 protected:
  PINDEX delay;
  BOOL useBusyWait;
};

///////////////////////////////////////////////////////////////////////////////
/**This thread launches multiple instances of the BusyWaitThread. Each
   thread launched is busy monitored for termination. When the thread
   terminates, the thread is deleted, and a new one is created. This
   process repeats until segfault or termination by the user */
class LauncherThread : public PThread
{
  PCLASSINFO(LauncherThread, PThread);
  
public:
  LauncherThread(PINDEX _delay, BOOL  _useBusyWait)
    : PThread(1000, NoAutoDeleteThread), delay(_delay), useBusyWait(_useBusyWait)
    { Resume(); iteration = 0; keepGoing = TRUE; }
  
  void Main();
    
  PINDEX GetIteration() { return iteration; }

  virtual void Terminate() { keepGoing = FALSE; }

  PTimeInterval GetElapsedTime() { return PTime() - startTime; }
  
 protected:
  PINDEX delay;
  PINDEX iteration;
  PTime startTime;
  BOOL  keepGoing;
  BOOL useBusyWait;
};

////////////////////////////////////////////////////////////////////////////////


class Threadex : public PProcess
{
  PCLASSINFO(Threadex, PProcess)

  public:
    Threadex();
    virtual void Main();

 protected:

};



#endif  // _Threadex_MAIN_H


// End of File ///////////////////////////////////////////////////////////////
