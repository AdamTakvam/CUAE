////////////////////////////////////////////////////////////////
// MSDN Magazine -- April 2005
// If this code works, it was written by Paul DiLascia.
// If not, I don't know who wrote it.
// Compiles with Visual Studio .NET 2003 (V7.1) on Windows XP. Tab size=3.
//
#pragma once
#ifdef _MANAGED
#using <mscorlib.dll>
#using <System.dll>
using namespace System;
#endif

#include <vector>
using namespace std;

////////////////////////////////////////////////////////////////
// ManWrap managed wrapper toolkit. Lets you wrap .NET classes in purely
// native C++ wrappers.
//

//////////////////
// Macros to control DLL. Use BUILD_MANWRAP_DLL when you're writing your own
// wrapper. Use USE_MANWRAP_DLL if you're writing an app that links to
// RegexWrap.dll. Don't use anything if you link the code directly into your
// app, but then you have to compile this and ManWrap.cpp with /clr.
//
#if defined(BUILD_MANWRAP_DLL)
#	define DLLEXPORT __declspec ( dllexport )
#elif defined(USE_MANWRAP_DLL)
#	define DLLEXPORT __declspec ( dllimport )
#else
#	define DLLEXPORT
#endif

//////////////////
// Handle to managed object.
// Appears as gcroot<T> wrapper to managed code, intptr_t to native code.
//
#ifdef _MANAGED
# define GCHANDLE(T) gcroot<T>
#else
# define GCHANDLE(T) intptr_t
#endif

//////////////////
// Wrapper for Object, base of .NET hierarchy.
// CMObject doesn't use DECLARE_WRAPPER because it has no base class.
//
class DLLEXPORT CMObject {
protected:
	GCHANDLE(Object*) m_handle; // handle to underlying managed object
public:

#ifdef _MANAGED
	// visible to managed clients only: anything that deals with __gc objects
	CMObject(Object* o) : m_handle(o) {	}
	Object* ThisObject() const { return (Object*)m_handle; }
	Object* operator->() const { return ThisObject(); }
#endif

	// visible to all clients
	CMObject();
	CMObject(const CMObject& o);
	CMObject& operator=(const CMObject& r);

	BOOL operator==(const CMObject& r) const;
	BOOL operator!=(const CMObject& r) const { return ! operator==(r); }
	CString ToString() const;
	CString TypeName() const;
	BOOL IsNull() const;
};

#ifdef _MANAGED

//////////////////
// template fn converts Object* __gc[] array to native STL vector<NT>. This
// only works with classes derived from from Object, not managed primitive
// types like int, double, etc. See below.
//
template <typename NT, typename MT>
vector<NT> wrap_array(MT* pmarr[])
{
	vector<NT> narr;
	size_t count = pmarr->Count;
	narr.reserve(count); // always reserve space if you know the size!
	for (UINT i=0; i<count; i++) {
		NT val = pmarr[i];
		narr.push_back(val);
	}
	return narr;
}

//////////////////
// Specialization for int __gc[] arrays. Managed Extensions don't support
// templates yet, so can't pass "int gcarr __gc[]" as template param, need to
// code it as function arg. (Actually, this is not really a specialization but
// a different template, parameterized by the native type.) If you want to
// wrap a different managed primitive type, copy this and change "int gcarr
// __gc[]" to something else like "byte gcarr __gc[]".
//
template<typename T>
vector<int> wrap_array(int gcarr __gc[])
{
	vector<T> narr;
	size_t count = gcarr->Count;
	narr.reserve(count); // always reserve space if you know the size!
	for (UINT i=0; i<count; i++) {
		T val(gcarr[i]);
		narr.push_back(val);
	}
	return narr;
}

// This declares the managed part of DECLARE_WRAPPER. For internal use
// only. Use DECLARE_WRAPPER.
//
#define DECLARE_WRAPPER_M(MT,BT)										\
public:																		\
	CM##MT(MT* o) : CM##BT(o) { }										\
	MT* ThisObject() const												\
	{																			\
		return static_cast<MT*>((Object*)m_handle);				\
	}																			\
	MT* operator->() const												\
	{																			\
		return ThisObject();												\
	}																			\
	CM##MT& operator=(MT* o)											\
	{																			\
		m_handle = o;														\
		return *this;														\
	}																			\

// Implement managed wrapper functions. For internal use only. Use
// IMPLEMENT_WRAPPER.
//
#define IMPLEMENT_WRAPPER_M(MT,BT)									\

#else // NOT _MANAGED

// native code doesn't see the managed stuff:
#define DECLARE_WRAPPER_M(MT,BT)				// nothing
#define IMPLEMENT_WRAPPER_M(MT,BT)			// nothing

#endif // _MANAGED

//////////////////
// Use this to declare a wrapper class. Declares the basic ctors and
// operators needed for type-safeness usage. MT=managed type, BT=base
// (managed) type. You must follow the naming convention CMFoo = wrapper
// for Foo.
//
#define DECLARE_WRAPPER(MT,BT)										\
public:																		\
	CM##MT() { }															\
	CM##MT(const CM##MT& o) : CM##BT(o) { }						\
	CM##MT& operator=(const CM##MT &r)								\
	{																			\
		CM##BT::operator=(r);											\
		return *this;														\
	}																			\
	DECLARE_WRAPPER_M(MT,BT)											\

//////////////////
// Use this to implement a wrapper class. Implements the fns declared in
// DECLARE_WRAPPER.
//
#define IMPLEMENT_WRAPPER(MT,BT)										\
	IMPLEMENT_WRAPPER_M(MT,BT)											\

//////////////////
// Use this to wrap collections. Eg, if your're wrapping the FooCollection,
// use this instead of DECLARE_WRAPPER. It adds Count, IsReadOnly and
// IsSynchronized, which all collections have.
//
#define DECLARE_COLLECTION(MT,BT)									\
	DECLARE_WRAPPER(MT,BT)												\
	int Count();															\
	BOOL IsReadOnly();													\
	BOOL IsSynchronized();												\
																				
//////////////////
// Use this with DECLARE_COLLECTION to implement fns declared by
// DECLARE_COLLECTION.
//
#define IMPLEMENT_COLLECTION(MT,BT)									\
	IMPLEMENT_WRAPPER(MT,BT)											\
	int  CM##MT::Count()													\
	{																			\
		return (*this)->Count;											\
	}																			\
	BOOL CM##MT::IsReadOnly()											\
	{																			\
		return (*this)->IsReadOnly;									\
	}																			\
	BOOL CM##MT::IsSynchronized()										\
	{																			\
		return (*this)->IsSynchronized;								\
	}																			\

//////////////////
// Base Exception class. Derive from this to wrap more types. I only
// needed two for RegexWrap.
//
class DLLEXPORT CMException : public CMObject {
	DECLARE_WRAPPER(Exception, Object);
public:
	CString HelpLink() const;
	CString Message() const;
	CString Source() const;
};

//////////////////
// Wrapper for ArgumentException.
//
class DLLEXPORT CMArgumentException : public CMException {
	DECLARE_WRAPPER(ArgumentException, Exception);
public:
	CString Message() const;
	CString ParamName() const;
};
