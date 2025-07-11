#include "XmlUniformiser.h"

namespace CppUnitTest
{



int 
notEqualIndex( std::string expectedXml,
               std::string actualXml )
{
  int index = 0;
  while ( index < actualXml.length()  &&  
          index < expectedXml.length()  &&
          actualXml[index] == expectedXml[index] )
    ++index;

  return index;
}


/// Asserts that two XML string are equivalent.
void 
checkXmlEqual( std::string expectedXml,
               std::string actualXml,
               CppUnit::SourceLine sourceLine )
{
  std::string expected = XmlUniformiser( expectedXml ).stripped();
  std::string actual = XmlUniformiser( actualXml ).stripped();

  if ( expected == actual )
    return;

  int index = notEqualIndex( expected, actual );
  CppUnit::OStringStream message;
  message  <<  "differ at index: "  <<  index  << "\n"
           <<  "expected: "  <<  expected.substr(index) << "\n"
           <<  "but was : "  <<  actual.substr( index );
  ::CppUnit::Asserter::failNotEqual( expected,
                                     actual,
                                     sourceLine,
                                     message.str() );
}



XmlUniformiser::XmlUniformiser( const std::string &xml ) :
    m_xml( xml ),
    m_index( 0 )
{
}


std::string 
XmlUniformiser::stripped()
{
  while ( isValidIndex() )
  {
    skipSpaces();
    if ( startsWith( "<?" ) )
      skipProcessed();
    else if ( startsWith( "<!--" ) )
      skipComment();
    else if ( startsWith( "<" ) )
      copyElement();
    else
      copyElementContent();
  }
  return m_stripped;
}


void 
XmlUniformiser::skipSpaces()
{
  while ( isSpace() )
    skipNext();
}


bool 
XmlUniformiser::isSpace( char c )
{
  return c < 33;
}


bool 
XmlUniformiser::isSpace()
{
  return isValidIndex()  &&  isSpace( m_xml[m_index] );
}


bool 
XmlUniformiser::isValidIndex()
{
  return m_index < m_xml.length();
}


void 
XmlUniformiser::skipNext( int count )
{
  while ( count-- > 0 )
    ++m_index;
}


void 
XmlUniformiser::copyNext( int count )
{
  while ( count-- > 0  &&  isValidIndex() )
    m_stripped += m_xml[ m_index++ ];
}


bool 
XmlUniformiser::startsWith( std::string expected )
{
  std::string actual = m_xml.substr( m_index, expected.length() );
  return actual == expected;
}


void 
XmlUniformiser::skipProcessed()
{
  while ( isValidIndex()  &&  !startsWith( "?>" ) )
    skipNext();
  if ( isValidIndex() )
    skipNext( 2 );
}


void 
XmlUniformiser::skipComment()
{
  while ( isValidIndex()  &&  !startsWith( "-->" ) )
    skipNext();
  if ( isValidIndex() )
    skipNext( 3 );
}


void 
XmlUniformiser::copyElement()
{
  copyElementName();
  copyElementAttributes();
}


void 
XmlUniformiser::copyElementName()
{
  while ( isValidIndex()  &&
          !( isSpace()  ||  startsWith( ">" ) ) )
    copyNext();
}


void 
XmlUniformiser::copyElementAttributes()
{
  do
  {
    skipSpaces();
    if ( startsWith( ">" ) )
      break;
    m_stripped += ' ';

    copyAttributeName();
    skipSpaces();
    if ( startsWith( "=" ) )
    {
      copyNext();
      copyAttributeValue();
    }
    else    // attribute should always be valued, ne ?
      m_stripped += ' ';
  }
  while ( isValidIndex() );
  copyNext();
}


void 
XmlUniformiser::copyAttributeName()
{
  while ( isValidIndex()  &&  !isEndOfAttributeName() )
    copyNext();
}


bool 
XmlUniformiser::isEndOfAttributeName()
{
  return isSpace()  ||  startsWith( ">" )  ||  startsWith( "=" );
}


void 
XmlUniformiser::copyAttributeValue()
{
  skipSpaces();
  copyUntilDoubleQuote();
  copyUntilDoubleQuote();
}


void 
XmlUniformiser::copyUntilDoubleQuote()
{
  while ( isValidIndex()  &&  !startsWith("\"") )
    copyNext();
  copyNext();   // '"'
}


void 
XmlUniformiser::copyElementContent()
{
  while ( isValidIndex()  &&  !startsWith( "<" ) )
    copyNext();
}


}  // namespace CppUnitTest
