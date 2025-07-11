/*
 * main.h
 *
 * PWLib application header file for dtmftest
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
 * Revision 1.1  2004/09/10 01:59:35  dereksmithies
 * Initial release of program to test Dtmf creation and detection.
 *
 *
 */

#ifndef _Dtmftest_MAIN_H
#define _Dtmftest_MAIN_H




class DtmfTest : public PProcess
{
  PCLASSINFO(DtmfTest, PProcess)

  public:
    DtmfTest();
    virtual void Main();

 protected:

};



#endif  // _Dtmftest_MAIN_H


// End of File ///////////////////////////////////////////////////////////////
