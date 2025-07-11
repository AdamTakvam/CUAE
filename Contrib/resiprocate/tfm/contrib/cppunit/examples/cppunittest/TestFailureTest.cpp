#include "CoreSuite.h"
#include "TestFailureTest.h"
#include <cppunit/TestFailure.h>
#include <cppunit/Exception.h>


CPPUNIT_TEST_SUITE_NAMED_REGISTRATION( TestFailureTest,
                                       CppUnitTest::coreSuiteName() );


TestFailureTest::TestFailureTest()
{
}


TestFailureTest::~TestFailureTest()
{
}


void 
TestFailureTest::setUp()
{
  m_exceptionDestroyed = false;
}


void 
TestFailureTest::tearDown()
{
}


void 
TestFailureTest::testConstructorAndGetters()
{
  CppUnit::TestCase test;
  CppUnit::Exception *error = new ObservedException( this );
  checkTestFailure( &test, error, false );
  CPPUNIT_ASSERT( m_exceptionDestroyed );
}


void 
TestFailureTest::testConstructorAndGettersForError()
{
  CppUnit::TestCase test;
  CppUnit::Exception *error = new ObservedException( this );
  checkTestFailure( &test, error, true );
  CPPUNIT_ASSERT( m_exceptionDestroyed );
}


void 
TestFailureTest::exceptionDestroyed()
{
  m_exceptionDestroyed = true;
}


void 
TestFailureTest::checkTestFailure( CppUnit::Test *test, 
                                   CppUnit::Exception *error,
                                   bool isError )
{
  CppUnit::TestFailure failure( test, error, isError );
  CPPUNIT_ASSERT_EQUAL( test, failure.failedTest() );
  CPPUNIT_ASSERT_EQUAL( error, failure.thrownException() );
  CPPUNIT_ASSERT_EQUAL( isError, failure.isError() );
}
