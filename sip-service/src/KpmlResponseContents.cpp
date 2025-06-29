#include "KpmlResponseContents.h"

#include "rutil/ParseBuffer.hxx"

using namespace Metreos::Sip;
using namespace std;

static bool invokeKpmlResponseContentsInit = KpmlResponseContents::init();

const char* KpmlResponseContents::contentsPrefix = 
	"<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>\n<kpml-response code=\"200\" digits=\"";
const char* KpmlResponseContents::contentsPostfix = 
	"\" forced_flush=\"false\" suppressed=\"false\" tag=\"dtml\" text=\"success\" version=\"1.0\"/>\n\r\n";


bool
KpmlResponseContents::init()
{
   static ContentsFactory<KpmlResponseContents> factory;
   (void)factory;
   return true;
}

KpmlResponseContents::KpmlResponseContents()
   : Contents(getStaticType())
{
}

KpmlResponseContents::KpmlResponseContents(const Data& digits)
   : Contents(getStaticType()),
     mDigits(digits)
{
}

KpmlResponseContents::KpmlResponseContents(const Data& digits, const Mime& contentsType)
   : Contents(contentsType),
     mDigits(digits)
{
}
 
KpmlResponseContents::KpmlResponseContents(HeaderFieldValue* hfv, const Mime& contentsType)
   : Contents(hfv, contentsType),
     mDigits()
{
}
 
KpmlResponseContents::KpmlResponseContents(const KpmlResponseContents& rhs)
   : Contents(rhs),
     mDigits(rhs.mDigits)
{
}

KpmlResponseContents::~KpmlResponseContents()
{
}

KpmlResponseContents&
KpmlResponseContents::operator=(const KpmlResponseContents& rhs)
{
   if (this != &rhs)
   {
      Contents::operator=(rhs);
      mDigits = rhs.mDigits;
   }
   return *this;
}

Contents* 
KpmlResponseContents::clone() const
{
   return new KpmlResponseContents(*this);
}

const Mime& 
KpmlResponseContents::getStaticType() 
{
   static Mime type("application", "kpml-response+xml");
   return type;
}

std::ostream& 
KpmlResponseContents::encodeParsed(std::ostream& str) const
{
   //DebugLog(<< "KpmlResponseContents::encodeParsed " << mDigits);
	str << contentsPrefix <<mDigits <<contentsPostfix;
   return str;
}


void 
KpmlResponseContents::parse(ParseBuffer& pb)
{
	const char *psz = pb.skipToChars("digits");
	psz = pb.skipToChar('\"');
	psz = pb.skipN(1);
	psz = pb.skipWhitespace();
	pb.skipToChar('\"');
	pb.data(mDigits, psz);
}

Data 
KpmlResponseContents::getBodyData() const
{
	checkParsed();

	Data d = contentsPrefix;
	d += mDigits;
	d += contentsPostfix;

	return d;
}


