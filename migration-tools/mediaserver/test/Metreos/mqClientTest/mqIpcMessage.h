// mqIpcMessage.h

#ifndef _MQIPCMESSAGE_H_
#define _MQIPCMESSAGE_H_

#include "ace/Message_Block.h"

class mqIpcMessage : public ACE_Message_Block
{
public:
	mqIpcMessage(const int iLen, const char* pData);
	~mqIpcMessage();

  int len;
	char* data;
};

#endif