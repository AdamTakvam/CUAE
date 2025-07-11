// -*- C++ -*- Env.i,v 1.3 2002/01/27 18:13:57 nanbor Exp

ACEXML_INLINE int
ACEXML_Env::exception_occured (void) const
{
  return this->exception_ != 0;
}

ACEXML_INLINE ACEXML_Exception *
ACEXML_Env::exception (void) const
{
  return this->exception_;
}

ACEXML_INLINE void
ACEXML_Env::exception (ACEXML_Exception *ex)
{
  delete this->exception_;
  this->exception_ = ex;
}

ACEXML_INLINE void
ACEXML_Env::reset (void)
{
  this->exception (0);
}
