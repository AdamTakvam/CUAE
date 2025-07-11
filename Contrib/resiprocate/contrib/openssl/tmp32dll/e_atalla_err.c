/* e_atalla_err.c */
/* ====================================================================
 * Copyright (c) 1999-2005 The OpenSSL Project.  All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer. 
 *
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in
 *    the documentation and/or other materials provided with the
 *    distribution.
 *
 * 3. All advertising materials mentioning features or use of this
 *    software must display the following acknowledgment:
 *    "This product includes software developed by the OpenSSL Project
 *    for use in the OpenSSL Toolkit. (http://www.OpenSSL.org/)"
 *
 * 4. The names "OpenSSL Toolkit" and "OpenSSL Project" must not be used to
 *    endorse or promote products derived from this software without
 *    prior written permission. For written permission, please contact
 *    openssl-core@OpenSSL.org.
 *
 * 5. Products derived from this software may not be called "OpenSSL"
 *    nor may "OpenSSL" appear in their names without prior written
 *    permission of the OpenSSL Project.
 *
 * 6. Redistributions of any form whatsoever must retain the following
 *    acknowledgment:
 *    "This product includes software developed by the OpenSSL Project
 *    for use in the OpenSSL Toolkit (http://www.OpenSSL.org/)"
 *
 * THIS SOFTWARE IS PROVIDED BY THE OpenSSL PROJECT ``AS IS'' AND ANY
 * EXPRESSED OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE OpenSSL PROJECT OR
 * ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 * ====================================================================
 *
 * This product includes cryptographic software written by Eric Young
 * (eay@cryptsoft.com).  This product includes software written by Tim
 * Hudson (tjh@cryptsoft.com).
 *
 */

/* NOTE: this file was auto generated by the mkerr.pl script: any changes
 * made to it will be overwritten when the script next updates this file,
 * only reason strings will be preserved.
 */

#include <stdio.h>
#include <openssl/err.h>
#include "e_atalla_err.h"

/* BEGIN ERROR CODES */
#ifndef OPENSSL_NO_ERR

#define ERR_FUNC(func) ERR_PACK(0,func,0)
#define ERR_REASON(reason) ERR_PACK(0,0,reason)

static ERR_STRING_DATA ATALLA_str_functs[]=
	{
{ERR_FUNC(ATALLA_F_ATALLA_CTRL),	"ATALLA_CTRL"},
{ERR_FUNC(ATALLA_F_ATALLA_FINISH),	"ATALLA_FINISH"},
{ERR_FUNC(ATALLA_F_ATALLA_INIT),	"ATALLA_INIT"},
{ERR_FUNC(ATALLA_F_ATALLA_MOD_EXP),	"ATALLA_MOD_EXP"},
{ERR_FUNC(ATALLA_F_ATALLA_RSA_MOD_EXP),	"ATALLA_RSA_MOD_EXP"},
{0,NULL}
	};

static ERR_STRING_DATA ATALLA_str_reasons[]=
	{
{ERR_REASON(ATALLA_R_ALREADY_LOADED)     ,"already loaded"},
{ERR_REASON(ATALLA_R_BN_CTX_FULL)        ,"bn ctx full"},
{ERR_REASON(ATALLA_R_BN_EXPAND_FAIL)     ,"bn expand fail"},
{ERR_REASON(ATALLA_R_CTRL_COMMAND_NOT_IMPLEMENTED),"ctrl command not implemented"},
{ERR_REASON(ATALLA_R_MISSING_KEY_COMPONENTS),"missing key components"},
{ERR_REASON(ATALLA_R_NOT_LOADED)         ,"not loaded"},
{ERR_REASON(ATALLA_R_REQUEST_FAILED)     ,"request failed"},
{ERR_REASON(ATALLA_R_UNIT_FAILURE)       ,"unit failure"},
{0,NULL}
	};

#endif

#ifdef ATALLA_LIB_NAME
static ERR_STRING_DATA ATALLA_lib_name[]=
        {
{0	,ATALLA_LIB_NAME},
{0,NULL}
	};
#endif


static int ATALLA_lib_error_code=0;
static int ATALLA_error_init=1;

static void ERR_load_ATALLA_strings(void)
	{
	if (ATALLA_lib_error_code == 0)
		ATALLA_lib_error_code=ERR_get_next_error_library();

	if (ATALLA_error_init)
		{
		ATALLA_error_init=0;
#ifndef OPENSSL_NO_ERR
		ERR_load_strings(ATALLA_lib_error_code,ATALLA_str_functs);
		ERR_load_strings(ATALLA_lib_error_code,ATALLA_str_reasons);
#endif

#ifdef ATALLA_LIB_NAME
		ATALLA_lib_name->error = ERR_PACK(ATALLA_lib_error_code,0,0);
		ERR_load_strings(0,ATALLA_lib_name);
#endif
		}
	}

static void ERR_unload_ATALLA_strings(void)
	{
	if (ATALLA_error_init == 0)
		{
#ifndef OPENSSL_NO_ERR
		ERR_unload_strings(ATALLA_lib_error_code,ATALLA_str_functs);
		ERR_unload_strings(ATALLA_lib_error_code,ATALLA_str_reasons);
#endif

#ifdef ATALLA_LIB_NAME
		ERR_unload_strings(0,ATALLA_lib_name);
#endif
		ATALLA_error_init=1;
		}
	}

static void ERR_ATALLA_error(int function, int reason, char *file, int line)
	{
	if (ATALLA_lib_error_code == 0)
		ATALLA_lib_error_code=ERR_get_next_error_library();
	ERR_PUT_error(ATALLA_lib_error_code,function,reason,file,line);
	}
