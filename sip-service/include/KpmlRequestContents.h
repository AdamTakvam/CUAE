#ifndef KpmlRequestContents_H_LOADED
#define KpmlRequestContents_H_LOADED

#include "resip/stack/Contents.hxx"
#include "rutil/Data.hxx"

using namespace resip;

namespace Metreos
{
namespace Sip
{
class KpmlRequestContents : public Contents
{
   public:
      KpmlRequestContents();
      KpmlRequestContents(HeaderFieldValue* hfv, const Mime& contentType);
      KpmlRequestContents(const Data& data, const Mime& contentType);
      KpmlRequestContents(const KpmlRequestContents& rhs);
      virtual ~KpmlRequestContents();
      KpmlRequestContents& operator=(const KpmlRequestContents& rhs);

      virtual Contents* clone() const;

      virtual Data getBodyData() const;

      static const Mime& getStaticType() ;

      virtual std::ostream& encodeParsed(std::ostream& str) const;
      virtual void parse(ParseBuffer& pb);

      Data& digits() {checkParsed(); return mDigits;}

      static bool init();
      
   private:
      Data mDigits;

	  static const char* contents;
};

static bool invokeKpmlRequestContentsInit = KpmlRequestContents::init();

}
}

#endif

