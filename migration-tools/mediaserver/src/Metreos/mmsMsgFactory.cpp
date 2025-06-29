// 
// mmsMsgFactory.cpp
// 
#include "StdAfx.h"
#include "mmsMsgFactory.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

mmsMsgFactory* mmsMsgFactory::singleton = 0;


MmsMsg* mmsMsgFactory::get(MmsMsg::MMSMSGPARAMS* params)
{
  msgFactoryGuard x(this->getMutex);

  MmsMsg* msg = new MmsMsg();
  return this->setPriority(msg, params);
}


MmsMsg* mmsMsgFactory::get(MmsMsg::MMSMSGPARAMS* params, const char *data, size_t size)
{
  msgFactoryGuard x(this->getMutex);

  MmsMsg* msg = new MmsMsg(data, size);
  return this->setPriority(msg, params);
}



MmsMsg* mmsMsgFactory::get(MmsMsg::MMSMSGPARAMS* params, size_t size, ACE_Message_Block::ACE_Message_Type type, 
        ACE_Message_Block* cont, const char* data)
{
  msgFactoryGuard x(this->getMutex);

  MmsMsg* msg = new MmsMsg(size, type, cont, data);
  return this->setPriority(msg, params);
}



MmsMsg* mmsMsgFactory::get(MmsMsg::MMSMSGPARAMS* params, const ACE_Message_Block &mb, size_t align)
{
  msgFactoryGuard x(this->getMutex);

  MmsMsg* msg = new MmsMsg(mb, align);
  return this->setPriority(msg, params);
}



MmsMsg* mmsMsgFactory::release(MmsMsg* msg)
{
  msgFactoryGuard x(this->relMutex);

  if  (msg->isPersistent()) return msg; 
  if  (msg->signature() != MmsMsg::SIGNATURE_VALUE)
  {    ACE_ERROR((LM_EMERGENCY, ">>>> (%t) MMSMSG SIGNATURE bad (synch problem)\n"));
       ACE_ASSERT(msg->signature() == MmsMsg::SIGNATURE_VALUE);
       return msg;
  }
  if  (msg->reference_count() < 1) 
  {    ACE_ERROR((LM_EMERGENCY, ">>>> (%t) REFCOUNT not positive (synch problem)\n"));
       ACE_ASSERT(msg->reference_count() > 0);
       return msg;
  }

  return (MmsMsg*)(msg->release());
}


MmsMsg* mmsMsgFactory::setPriority(MmsMsg* msg, MmsMsg::MMSMSGPARAMS* params)
{ 
  if  (msg == NULL) return msg;
  msg->type (params->type);
  msg->param(params->param);

  int  priorityIncrement = this->getMessagePriority(msg->type());
  if  (priorityIncrement)
       msg->msg_priority(ACE_DEFAULT_MESSAGE_BLOCK_PRIORITY + priorityIncrement);

  return msg;
}



inline int mmsMsgFactory::getMessagePriority(const int msgnum) const
// Returns a priority delta indicating the relative priority of the message
// whose type is supplied. Zero is baseline, 3 is max. Messages will be queued
// according to their priority, for example, MMSM_TIMER, indicating a timer
// has fired, should be removed from a queue prior to some other messaage 
// which may already exist in the queue.
{
  switch(msgnum)
  { case MMSM_TIMER:
         return 2; 
  }

  return 0;
}

