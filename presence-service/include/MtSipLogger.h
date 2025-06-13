#ifndef MtSipLogger_H_LOADED
#define MtSipLogger_H_LOADED

#include "rutil/Logger.hxx"

using namespace resip;

namespace Metreos
{
namespace Presence
{

class MtSipLogger : public ExternalLogger
{
   public:
      virtual ~MtSipLogger();
      /** return true to also do default logging, false to supress default logging. */
      virtual bool operator()(Log::Level level,
                              const Subsystem& subsystem, 
                              const Data& appName,
                              const char* file,
                              int line,
                              const Data& message,
			      const Data& messageWithHeaders);
};

}
}

#endif