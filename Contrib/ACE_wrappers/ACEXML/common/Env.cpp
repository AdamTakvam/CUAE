// -*- C++ -*-  Env.cpp,v 1.3 2002/02/14 06:28:57 nanbor Exp

#include "ACEXML/common/Env.h"

#if !defined (__ACEXML_INLINE__)
# include "ACEXML/common/Env.i"
#endif /* __ACEXML_INLINE__ */

ACEXML_Env::ACEXML_Env (void)
  : exception_ (0)
{
}

ACEXML_Env::ACEXML_Env (const ACEXML_Env &ev)
  : exception_ (ev.exception_->duplicate ())
{
}

ACEXML_Env::~ACEXML_Env (void)
{
  delete this->exception_;
}
