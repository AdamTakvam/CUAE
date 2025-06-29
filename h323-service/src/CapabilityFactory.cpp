#include "stdafx.h"

#include "CapabilityFactory.h"
#include "msgs/MetreosH323MessageTypes.h"

using namespace Metreos::H323;

ACE_Thread_Mutex   CapabilityFactory::m_instanceLock;
CapabilityFactory* CapabilityFactory::m_instance = 0;

CapabilityFactory::CapabilityFactory()
{
    // Get an instance of the plugin codec factory
    m_codecMgr = (H323PluginCodecManager *)PFactory<PPluginModuleManager>::CreateInstance("H323PluginCodecManager");
}

CapabilityFactory* CapabilityFactory::Instance()
{
    if(m_instance == 0)
    {
        m_instanceLock.acquire();
        if(m_instance == 0)
            m_instance = new CapabilityFactory();
        m_instanceLock.release();
    }

    return m_instance;
}

H323Capability* CapabilityFactory::CreateG711uCapability(unsigned int frameSize)
{
    H323Capability* cap;
    cap = m_codecMgr->CreateCapability(
        OPAL_G711_ULAW_64K,                     // must match an existing OpalMediaFormat
        Codecs::G711uStr,                       // String used to identify the codec
        30,                                     // maximum number of frames per packet
        20,                                     // suggested frames per packet
        PluginCodec_H323AudioCodec_g711Ulaw_64k // capability type - see opalplugin.h for values
        );    
    cap->SetTxFramesInPacket(frameSize);
    return cap;
}

H323Capability* CapabilityFactory::CreateG711aCapability(unsigned int frameSize)
{
    H323Capability* cap;
    cap = m_codecMgr->CreateCapability(
        OPAL_G711_ALAW_64K,                     // must match an existing OpalMediaFormat
        Codecs::G711aStr,                       // String used to identify the codec
        30,                                     // maximum number of frames per packet
        20,                                     // suggested frames per packet
        PluginCodec_H323AudioCodec_g711Alaw_64k // capability type - see opalplugin.h for values
        );    
    cap->SetTxFramesInPacket(frameSize);
    return cap;
}

H323Capability* CapabilityFactory::CreateG729Capability()
{
    return m_codecMgr->CreateCapability(
        OPAL_G729A,                             // must match an existing OpalMediaFormat
        Codecs::G729aStr,                       // String used to identify the codec
        4,                                      // maximum number of frames per packet
        2,                                      // suggested frames per packet
        PluginCodec_H323AudioCodec_g729AnnexA   // capability type - see opalplugin.h for values
        );
}

H323Capability* CapabilityFactory::CreateG723Capability()
{
    return m_codecMgr->CreateCapability(
        OPAL_G7231,                             // must match an existing OpalMediaFormat
        Codecs::G7231Str,                       // String used to identify the codec
        4,                                      // maximum number of frames per packet
        2,                                      // suggested frames per packet
        PluginCodec_H323AudioCodec_g7231        // capability type - see opalplugin.h for values
        );
}