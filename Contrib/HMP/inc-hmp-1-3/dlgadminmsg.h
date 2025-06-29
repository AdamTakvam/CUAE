/**************************************************************
    Copyright (C) 2000-2002.  Intel Corporation.

    All Rights Reserved.  All names, products,
    and services mentioned herein are the trademarks
    or registered trademarks of their respective organizations
    and are the sole property of their respective owners.
 **************************************************************/

/*
    AUTO-VERSIONING HEADER  DO NOT HAND MODIFY
    ===================================================================
    %name:          dlgadminmsg.h %
    %version:       9 %
    %instance:      hsw_1 %
    %created_by:    klotzw %
    %date_modified: Wed Oct 16 16:14:26 2002 %
    ===================================================================
*/
 
#ifdef DLG_WIN32_OS

#pragma warning( disable: 4251 )
#pragma warning( disable: 4275 )

#endif

#ifndef _DLG_ADMIN_MSG_H
#define _DLG_ADMIN_MSG_H


#include "dlgeventproxydef.h"

namespace DlgEventService
{
    /*
        The C++ "Consumer" clients must extend this class and override the HandleEvent method. 
        This is the callback object that is invoked by the consumer when an event is dispatched. 
        Clients creating consumers only use this class. Client creating suppliers do not care 
        for this class.

        Note: When a CEventHandlerAdaptor object is shared among different consumer objects. 
        The developer must make sure that the HandleEvent callback method is thread safe. 
        Remember that a consumer component runs on its own thread; therefore, having an 
        EventHandlerAdaptor shared by multiple threads can have unpredictable behavior if 
        not thread safe.

        Since:
            10/04/2000

        Version:
            1.0
        Author:  Dialogic Admin Software Group
    */
	class CEventHandlerAdaptor {

	public:


        /*
            virtual int HandleEvent( const DlgEventMsgTypePtr evMsg,
			                         ClientDataType clientData )
            C++ Consumer client must override this virtual method in order to receive events 
            from the Event Framework.

            Since:
                10/04/2000

            Parameter:
                [in]const DlgEventMsgTypePtr	evMsg : This is the actual event message 
                                                        that is sent by the supplier component. The actual client message is 
                                                        found under the payload field. The supplier and consumers must agree on the payload message format. The consumer uses the msgId to correctly typecast the message payload. 
                                                        The AdminCallbackMsg also contains the supplier name and node IP address.

            [in]const ClientDataType clientData : 	A void pointer value that is passed in during the filter registration by the consumer client. This value is
                                                    returned back to the client in the callback object. 

            Return :
                Return zero for no error
                non-zero - error		 Note: At this time the return value has no meaning.

            Version:
                1.0

            throws:

            See Also:

            Author:
                Dialogic Admin Software Group
        */
		virtual int HandleEvent( const DlgEventMsgTypePtr evMsg,
			         ClientDataType clientData ) = 0; 
		
	private:

	};

}

#endif

