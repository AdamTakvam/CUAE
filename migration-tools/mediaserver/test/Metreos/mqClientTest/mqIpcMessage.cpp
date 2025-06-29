// mqIpcMessage.cpp

#include "StdAfx.h"
#include <stdlib.h>
#include <string.h>
#include "mqIpcMessage.h"

mqIpcMessage::mqIpcMessage(const int iLen, const char* pData)
{
  len = iLen;

	if (pData)
	{
    data = (char*)malloc(strlen(pData)+1);
    strcpy(data, pData);
	}
	else
		data = 0;
}

mqIpcMessage::~mqIpcMessage()
{
	if (data)
    free(data);
}
