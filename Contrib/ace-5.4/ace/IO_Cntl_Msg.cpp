// IO_Cntl_Msg.cpp
// IO_Cntl_Msg.cpp,v 4.4 2002/05/27 05:50:17 jwillemsen Exp

#if 0
// This is not meant to be used, it's just a place holder...

ACE_RCSID(ace, IO_Cntl_Msg, "IO_Cntl_Msg.cpp,v 4.4 2002/05/27 05:50:17 jwillemsen Exp")

// Forward decl
template <class SYNCH> class ACE_Module;

/**
 * @class ACE_Module_Link
 *
 * @brief Data structure used to link two modules together
 */
class ACE_Module_Link
{
public:
  ACE_Module_Link (ACE_Module *m1, ACE_Module *m2): mod_upper_ (m1), mod_lower_ (m2), count_ (0) {}

  ACE_Module *upper (void) { return this->mod_upper_; }
  void   upper (ACE_Module *u) { this->mod_upper_ = u; }

  ACE_Module *lower (void) { return this->mod_lower_; }
  void   lower (ACE_Module *l) { this->mod_lower_ = l; }

  int    count (void) { return this->count_; }
  void   count (int c) { this->count_ = c; }

private:
  ACE_Module *mod_upper_;
  ACE_Module *mod_lower_;
  int    count_;
};
#endif

