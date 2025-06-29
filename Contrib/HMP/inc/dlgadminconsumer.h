/**************************************************************
    Copyright (C) 2000-2002.  Intel Corporation.

    All Rights Reserved.  All names, products,
    and services mentioned herein are the trademarks
    or registered trademarks of their respective organizations
    and are the sole property of their respective owners.
 **************************************************************/

/*
*    AUTO-VERSIONING HEADER  DO NOT HAND MODIFY
*    ===================================================================
*    %name:          dlgadminconsumer.h %
*    %version:       15 %
*    %instance:      hsw_1 %
*    %created_by:    klotzw %
*    %date_modified: Wed Oct 16 16:14:22 2002 %
*    ===================================================================
*/

#ifndef __DLG_ADMIN_CONSUMER_H__
#define __DLG_ADMIN_CONSUMER_H__


#if defined DLG_WIN32_OS
#pragma warning( disable: 4251 )
#pragma warning( disable: 4275 )
#endif

#include "dlgeventproxydef.h"
#include "adminconsumer.h"

namespace DlgEventService 
{
   
   class DlgAdminConsumerInternal;
   

   class ADMINCONSUMERFW_API DlgAdminConsumer  {
        public:
        DlgAdminConsumer ( /*in*/ const char*          szChannelName, 
                           /*in*/ const wchar_t*       szConsumerName,
                           /*in*/ AdminConsumer::FilterCallbackAssoc* pFilters,
                           /*in*/ int                  iFilterCnt,
                           /*in*/ const char*          szServerIpAddr = NULL,
                           /*in*/ int                  iServerPort = 0 );



        virtual ~DlgAdminConsumer ( );

        virtual bool StartListening ( );
        virtual bool StopListening ( );

        virtual bool IsFilterEnable ( unsigned long ulFilter );
        virtual void EnableFilters ( const unsigned long* pFilters,
                                     int iCount );
        virtual void DisableFilters ( const unsigned long* pFilters,
                                      int iCount );

        virtual const char* getChannelName ( );
        virtual const wchar_t* getConsumerName ( );
        virtual long getStatus ( );

        private:
        DlgAdminConsumerInternal *m_pConsumer_;        

    };


} //end of namespace DlgEventService


#endif
