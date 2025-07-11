#ifndef CPPUNIT_MSVC_TESTRUNNER_H
#define CPPUNIT_MSVC_TESTRUNNER_H


#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#include <cppunit/ui/mfc/TestRunner.h>

/*! \brief MFC test runner (DEPRECATED)
 * \ingroup ExecutingTest
 * \deprecated Use CppUnit::MfcUi::TestRunner instead.
 */
typedef CppUnit::MfcUi::TestRunner TestRunner;

#endif  // CPPUNIT_MSVC_TESTRUNNER_H
