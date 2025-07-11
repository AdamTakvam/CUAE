#ifndef __CHESSTEST_H__
#define __CHESSTEST_H__

#include "BoardGameTest.h"

template<typename GAMECLASS> 
class ChessTest : public BoardGameTest<GAMECLASS> 
{
  CPPUNIT_TEST_SUB_SUITE( ChessTest, BoardGameTest<GAMECLASS> );
  CPPUNIT_TEST( testNumberOfPieces );
  CPPUNIT_TEST_SUITE_END();
public:
  ChessTest() 
  {
  }
  
  void testNumberOfPieces()
  { 
    CPPUNIT_ASSERT( m_game->getNumberOfPieces () == 32 );
  }
};



#endif
