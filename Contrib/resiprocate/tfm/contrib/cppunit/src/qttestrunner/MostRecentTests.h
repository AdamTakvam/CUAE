// //////////////////////////////////////////////////////////////////////////
// Header file MostRecentTests.h for class MostRecentTests
// (c)Copyright 2000, Baptiste Lepilleur.
// Created: 2001/09/20
// //////////////////////////////////////////////////////////////////////////
#ifndef MOSTRECENTTESTS_H
#define MOSTRECENTTESTS_H

#include <cppunit/Test.h>
#include <qstring.h>
#include <qlist.h>
#include <qobject.h>


/*! \class MostRecentTests
 * \brief This class represents the list of the recent tests.
 */
class MostRecentTests : public QObject
{
  Q_OBJECT
public:
  /*! Constructs a MostRecentTests object.
   */
  MostRecentTests();

  /*! Destructor.
   */
  virtual ~MostRecentTests();

  void setTestToRun( CppUnit::Test *test );
  CppUnit::Test *testToRun();

  int testCount();
  QString testNameAt( int index );
  CppUnit::Test *testAt( int index );

signals:
  void listChanged();
  void testToRunChanged( CppUnit::Test *testToRun );

public slots:
  void selectTestToRun( int index );

private:
  /// Prevents the use of the copy constructor.
  MostRecentTests( const MostRecentTests &copy );

  /// Prevents the use of the copy operator.
  void operator =( const MostRecentTests &copy );

private:
  QList<CppUnit::Test> m_tests;
};



// Inlines methods for MostRecentTests:
// ------------------------------------



#endif  // MOSTRECENTTESTS_H
