// -*- C++ -*-
// OS_NS_math.inl,v 1.2 2003/11/01 11:15:15 dhinton Exp

ACE_INLINE double
ACE_OS::floor (double x)
{
  // This method computes the largest integral value not greater than x.
  return double (ACE_static_cast (long, x));
}

ACE_INLINE double
ACE_OS::ceil (double x)
{
  // This method computes the smallest integral value not less than x.
  double floor = ACE_OS::floor (x);
  if (floor == x)
    return floor;
  else
    return floor + 1;
}
