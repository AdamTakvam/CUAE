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
*    %name:          adminconsumer.h %
*    %version:       15 %
*    %instance:      hsw_1 %
*    %created_by:    klotzw %
*    %date_modified: Wed Oct 16 16:14:11 2002 %
*    ===================================================================
*/

#ifndef __ADMIN_CONSUMER_H__
#define __ADMIN_CONSUMER_H__

#if defined DLG_WIN32_OS
#pragma warning( disable: 4251 )
#pragma warning( disable: 4275 )
#endif

#include "adminconsumerfw.h"
#include "dlgadminmsg.h"
#include "dlgeventproxydef.h"

using DlgEventService::CEventHandlerAdaptor;

namespace DlgEventService
{
   class AdminConsumer
      {
        public:
         struct FilterCallbackAssoc {

            CEventHandlerAdaptor* callback;
            ClientDataType		clientData;
            DlgFilterType		filter;		//only one filter allow at this time
                                          //in this case msgid.
            DlgEvent_FilterControl		enable;		// true if filter enable 
         };

      };
   

} //end of namespace 


#endif
