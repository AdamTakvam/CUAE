#include "KpmlRequestContents.h"

#include "rutil/ParseBuffer.hxx"

using namespace Metreos::Sip;
using namespace std;

static bool invokeKpmlRequestContentsInit = KpmlRequestContents::init();

const char* KpmlRequestContents::contents = 
"<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>\n<kpml-request version=\"1.0\">\n\n<pattern interdigittime=\"500000\" persist=\"persist\">\n<regex tag=\"dtmf\">[x*#ABCD]</regex>\n</pattern>\n\n</kpml-request>\n\r\n";


bool
KpmlRequestContents::init()
{
   static ContentsFactory<KpmlRequestContents> factory;
   (void)factory;
   return true;
}

KpmlRequestContents::KpmlRequestContents()
   : Contents(getStaticType())
{
}

KpmlRequestContents::KpmlRequestContents(const Data& d, const Mime& contentsType)
   : Contents(contentsType)
{
}
 
KpmlRequestContents::KpmlRequestContents(HeaderFieldValue* hfv, const Mime& contentsType)
   : Contents(hfv, contentsType)
{
}
 
KpmlRequestContents::KpmlRequestContents(const KpmlRequestContents& rhs)
   : Contents(rhs)
{
}

KpmlRequestContents::~KpmlRequestContents()
{
}

KpmlRequestContents&
KpmlRequestContents::operator=(const KpmlRequestContents& rhs)
{
   if (this != &rhs)
   {
      Contents::operator=(rhs);
   }
   return *this;
}

Contents* 
KpmlRequestContents::clone() const
{
   return new KpmlRequestContents(*this);
}

const Mime& 
KpmlRequestContents::getStaticType() 
{
   static Mime type("application", "kpml-request+xml");
   return type;
}

std::ostream& 
KpmlRequestContents::encodeParsed(std::ostream& str) const
{
   //DebugLog(<< "KpmlRequestContents::encodeParsed " << mDigits);
	str << contents;
   return str;
}


void 
KpmlRequestContents::parse(ParseBuffer& pb)
{
}

Data 
KpmlRequestContents::getBodyData() const
{
	checkParsed();

	Data d = contents;

	return d;
}


