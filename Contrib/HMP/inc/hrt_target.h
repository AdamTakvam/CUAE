/*
 * INTEL CONFIDENTIAL	
 * Copyright 2004 Intel Corporation All Rights Reserved.
 * 
 * The source code contained or described herein and all documents related to the
 * source code ("Material") are owned by Intel Corporation or its suppliers or
 * licensors.  Title to the Material remains with Intel Corporation or its suppliers
 * and licensors.  The Material contains trade secrets and proprietary and
 * confidential information of Intel or its suppliers and licensors.  The Material is 
 * protected by worldwide copyright and trade secret laws and treaty provisions. No
 * part of the Material may be used, copied, reproduced, modified, published,
 * uploaded, posted, transmitted, distributed, or disclosed in any way without Intel's
 * prior express written permission.
 * 
 * No license under any patent, copyright, trade secret or other intellectual property
 * right is granted to or conferred upon you by disclosure or delivery of the
 * Materials,  either expressly, by implication, inducement, estoppel or otherwise.
 * Any license under such intellectual property rights must be express and approved by
 * Intel in writing.
 * 
 * Unless otherwise agreed by Intel in writing, you may not remove or alter this notice
 * or any other notice embedded in Materials by Intel or Intel's suppliers or licensors
 * in any way.
 */

#ifndef _HRT_TARGET_H_
#define _HRT_TARGET_H_

/*
 * Guess what is the target OS platform
*/
#if defined(__linux__) || defined(__CYGWIN__)
	#define HRT_TARGET_LINUX
#elif defined(_WIN32) || defined(WIN32) || defined(__WINDOWS__)
	#define HRT_TARGET_WIN32
#else
    #define HRT_TARGET_UNKNOWN
#endif


/*
 * Detect Compiler
*/
#if defined (__GNUG__)
	/* GNU compiler */
    #define HRT_COMPILER_GCC
#elif defined(_MSC_VER)
	/* Intel or MS compiler for Win32 */
    #define HRT_COMPILER_INTEL_WIN32
#else
	/* Unknown compiler */
    #define HRT_COMPILER_UNKNOWN
#endif

/*
 * Definitions
 */
#if defined(HRT_TARGET_WIN32)
#if defined(HRT_COMPILER_GCC)
	#define HRT_EXPORT __attribute__((dllexport))
	#define HRT_IMPORT __attribute__((dllimport))
	#define HRT_CDECL /*__attribute__((cdecl))*/
#elif defined(HRT_COMPILER_INTEL_WIN32)
	#define HRT_EXPORT __declspec(dllexport)
	#define HRT_IMPORT __declspec(dllimport)
	#define HRT_CDECL __cdecl
#else
	#define HRT_EXPORT
	#define HRT_IMPORT
	#define HRT_CDECL
#endif
#else
#define HRT_EXPORT
#define HRT_IMPORT
#if defined(HRT_COMPILER_GCC)
	#define HRT_CDECL /*__attribute__((cdecl))*/
#elif defined(HRT_COMPILER_INTEL_WIN32)
	#define HRT_CDECL __cdecl
#else
	#define HRT_CDECL
#endif
#endif

#define HRT_CONV HRT_CDECL

/*
 * Project definitions
 */
#if defined(HRT_EXPORTS)
#define HRT_API HRT_EXPORT
#else
#define HRT_API HRT_IMPORT
#endif

#endif /* _HRT_TARGET_H_ */
