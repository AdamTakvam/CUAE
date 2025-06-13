// c-wrapper.h

#ifndef _CWRAPPER_H_
#define _CWRAPPER_H_

#ifdef WIN32
#   ifdef CWRAPPER_EXPORTS
#       define CWRAPPER_API __declspec(dllexport)
#   else
#       define CWRAPPER_API __declspec(dllimport)
#   endif
#else
#   define CWRAPPER_API
#endif // WIN32

#endif