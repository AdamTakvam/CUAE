/* -*- C++ -*- */
// POSIX_CB_Proactor.i,v 4.1 2002/04/25 19:48:19 shuston Exp

ACE_INLINE 
ACE_POSIX_Proactor::Proactor_Type ACE_POSIX_CB_Proactor::get_impl_type (void)
{
  return PROACTOR_CB;
} 
