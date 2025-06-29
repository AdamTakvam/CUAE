#ifndef KpmlResponseContents_H_LOADED
#define KpmlResponseContents_H_LOADED

#include "resip/stack/Contents.hxx"
#include "rutil/Data.hxx"

using namespace resip;

namespace Metreos
{
namespace Sip
{
class KpmlResponseContents : public Contents
{
   public:
      KpmlResponseContents();
      KpmlResponseContents(const Data& digits);
      KpmlResponseContents(HeaderFieldValue* hfv, const Mime& contentType);
      KpmlResponseContents(const Data& data, const Mime& contentType);
      KpmlResponseContents(const KpmlResponseContents& rhs);
      virtual ~KpmlResponseContents();
      KpmlResponseContents& operator=(const KpmlResponseContents& rhs);

      virtual Contents* clone() const;

      virtual Data getBodyData() const;

      static const Mime& getStaticType() ;

      virtual std::ostream& encodeParsed(std::ostream& str) const;
      virtual void parse(ParseBuffer& pb);

      Data& digits() {checkParsed(); return mDigits;}

      static bool init();
      
   private:
      Data mDigits;

	  static const char* contentsPrefix;
	  static const char* contentsPostfix;

};

static bool invokeKpmlResponseContentsInit = KpmlResponseContents::init();

}
}

#endif

