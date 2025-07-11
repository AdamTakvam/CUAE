#include <cppunit/TestSuite.h>
#include <qlistview.h>
#include <qmessagebox.h>
#include "TestBrowserDlgImpl.h"
#include "TestListViewItem.h"


/* 
 *  Constructs a TestBrowser which is a child of 'parent', with the 
 *  name 'name' and widget flags set to 'f' 
 *
 *  The dialog will by default be modeless, unless you set 'modal' to
 *  TRUE to construct a modal dialog.
 */
TestBrowser::TestBrowser( QWidget* parent,  const char* name, bool modal, WFlags fl )
    : TestBrowserBase( parent, name, modal, fl ),
    _selectedTest( NULL )
{
  _listTests->setRootIsDecorated( TRUE );
}

/*  
 *  Destroys the object and frees any allocated resources
 */
TestBrowser::~TestBrowser()
{
    // no need to delete child widgets, Qt does it all for us
}


void 
TestBrowser::setRootTest( CppUnit::Test *rootTest )
{
  QListViewItem *dummyRoot = new QListViewItem( _listTests );

  insertItemFor( rootTest, dummyRoot );
  
  dummyRoot->firstChild()->moveItem( dummyRoot );
  delete dummyRoot;

  _listTests->firstChild()->setOpen( TRUE );
  _listTests->triggerUpdate();
}


void 
TestBrowser::insertItemFor( CppUnit::Test *test,
                            QListViewItem *parentItem )
{
  CppUnit::TestSuite *suite = dynamic_cast<CppUnit::TestSuite *>(test);
  QListViewItem *item = new TestListViewItem( test, parentItem );
  QString testName = QString::fromLatin1( test->getName().c_str() );
  item->setText( 0, testName );
  if ( suite != NULL )
  {
    for ( int index =0; index < suite->getTests().size(); ++index )
      insertItemFor( suite->getTests()[index], item );
  }
}


CppUnit::Test *
TestBrowser::selectedTest()
{
  return _selectedTest;
}


void 
TestBrowser::accept()
{
  TestListViewItem *item = (TestListViewItem *)_listTests->selectedItem();
  if ( item == NULL )
  {
    QMessageBox::information( this, tr("Selected test"),
                              tr( "You must select a test." ) );
    return;
  }

  _selectedTest = item->test();

  TestBrowserBase::accept();
}
