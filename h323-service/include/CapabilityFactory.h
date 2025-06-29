#ifndef CAPABILITY_FACTORY_H
#define CAPABILITY_FACTORY_H

#include "H323Common.h"

namespace Metreos
{
namespace H323
{

class CapabilityFactory
{
public:
    static CapabilityFactory* Instance();

    H323Capability* CreateG711uCapability(unsigned int frameSize);
    H323Capability* CreateG711aCapability(unsigned int frameSize);
    H323Capability* CreateG729Capability();
    H323Capability* CreateG723Capability();

protected:
    CapabilityFactory();

    H323PluginCodecManager*     m_codecMgr;
    static ACE_Thread_Mutex     m_instanceLock;
    static CapabilityFactory*   m_instance;
};

} // namespace H323
} // namespace Metreos

#endif // CAPABILITY_FACTORY_H
