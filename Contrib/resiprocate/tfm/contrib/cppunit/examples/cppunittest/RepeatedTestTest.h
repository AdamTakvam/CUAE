#ifndef REPEATEDTESTTEST_H
#define REPEATEDTESTTEST_H

#include <cppunit/extensions/HelperMacros.h>


class RepeatedTestTest : public CppUnit::TestFixture
{
  CPPUNIT_TEST_SUITE( RepeatedTestTest );
  CPPUNIT_TEST( testRun );
  CPPUNIT_TEST_SUITE_END();

public:
  RepeatedTestTest();
  virtual ~RepeatedTestTest();

  virtual void setUp();
  virtual void tearDown();

  void testRun();

private:
  class RunCountTest : public CppUnit::TestCase
  {
  public:
    RunCountTest() : m_runCount( 0 ) {}

    void runTest()
    {
      ++m_runCount;
    }

    int m_runCount;
  };

  RepeatedTestTest( const RepeatedTestTest &copy );
  void operator =( const RepeatedTestTest &copy );

private:
  RunCountTest *m_test;
  CppUnit::Test *m_repeatedTest;
  const int m_repeatCount;
};



#endif  // REPEATEDTESTTEST_H
