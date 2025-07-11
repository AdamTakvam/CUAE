#include "tfm/ActionBase.hxx"

ActionBase::ActionBase() : mNext(0)
{
}
      
ActionBase::~ActionBase()
{
   delete mNext;
}
      
void 
ActionBase::addNext(ActionBase* action)
{
   if (mNext)
   {
      mNext->addNext(action);
   } 
   else
   {
      mNext = action;
   }
}
      
void 
ActionBase::exec(boost::shared_ptr<Event> event)
{
   (*this)(event);
   if (mNext != 0)
   {
      mNext->exec(event);
   }
}
      
void 
ActionBase::exec()
{
   (*this)();
   if (mNext != 0)
   {
      mNext->exec();
   }
}

resip::Data 
ActionBase::toString() const 
{
   return ""; 
}

ActionBase*
chain(ActionBase* action1,
      ActionBase* action2)
{
   action1->addNext(action2);
   return action1;
}   

ActionBase*
chain(ActionBase* action1,
      ActionBase* action2,
      ActionBase* action3)
{
   action1->addNext(action2);
   action1->addNext(action3);
   return action1;
}

ActionBase*
chain(ActionBase* action1,
      ActionBase* action2,
      ActionBase* action3,
      ActionBase* action4)
{
   action1->addNext(action2);
   action1->addNext(action3);
   action1->addNext(action4);
   return action1;
}

ActionBase*
chain(ActionBase* action1,
      ActionBase* action2,
      ActionBase* action3,
      ActionBase* action4,
      ActionBase* action5)
{
   action1->addNext(action2);
   action1->addNext(action3);
   action1->addNext(action4);
   action1->addNext(action5);
   return action1;
}

ActionBase*
chain(ActionBase* action1,
      ActionBase* action2,
      ActionBase* action3,
      ActionBase* action4,
      ActionBase* action5,
      ActionBase* action6)
{
   action1->addNext(action2);
   action1->addNext(action3);
   action1->addNext(action4);
   action1->addNext(action5);
   action1->addNext(action6);
   return action1;
}

ActionBase*
chain(ActionBase* action1,
      ActionBase* action2,
      ActionBase* action3,
      ActionBase* action4,
      ActionBase* action5,
      ActionBase* action6,
      ActionBase* action7)
{
   action1->addNext(action2);
   action1->addNext(action3);
   action1->addNext(action4);
   action1->addNext(action5);
   action1->addNext(action6);
   action1->addNext(action7);
   return action1;
}

ActionBase*
chain(ActionBase* action1,
      ActionBase* action2,
      ActionBase* action3,
      ActionBase* action4,
      ActionBase* action5,
      ActionBase* action6,
      ActionBase* action7,
      ActionBase* action8)
{
   action1->addNext(action2);
   action1->addNext(action3);
   action1->addNext(action4);
   action1->addNext(action5);
   action1->addNext(action6);
   action1->addNext(action7);
   action1->addNext(action8);
   return action1;
}
/*
  Copyright (c) 2005, PurpleComm, Inc. 
  All rights reserved.

  Redistribution and use in source and binary forms, with or without modification,
  are permitted provided that the following conditions are met:

  * Redistributions of source code must retain the above copyright notice, this
    list of conditions and the following disclaimer.
  * Redistributions in binary form must reproduce the above copyright notice,
    this list of conditions and the following disclaimer in the documentation
    and/or other materials provided with the distribution.
  * Neither the name of PurpleComm, Inc. nor the names of its contributors may
    be used to endorse or promote products derived from this software without
    specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
  AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
  FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
  DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
  CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
  OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
