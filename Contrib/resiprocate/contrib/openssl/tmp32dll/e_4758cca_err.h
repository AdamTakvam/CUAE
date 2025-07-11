/* ====================================================================
 * Copyright (c) 2001 The OpenSSL Project.  All rights reserved.
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
 *    for use in the OpenSSL Toolkit. (http://www.openssl.org/)"
 *
 * 4. The names "OpenSSL Toolkit" and "OpenSSL Project" must not be used to
 *    endorse or promote products derived from this software without
 *    prior written permission. For written permission, please contact
 *    openssl-core@openssl.org.
 *
 * 5. Products derived from this software may not be called "OpenSSL"
 *    nor may "OpenSSL" appear in their names without prior written
 *    permission of the OpenSSL Project.
 *
 * 6. Redistributions of any form whatsoever must retain the following
 *    acknowledgment:
 *    "This product includes software developed by the OpenSSL Project
 *    for use in the OpenSSL Toolkit (http://www.openssl.org/)"
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

#ifndef HEADER_CCA4758_ERR_H
#define HEADER_CCA4758_ERR_H

/* BEGIN ERROR CODES */
/* The following lines are auto generated by the script mkerr.pl. Any changes
 * made after this point may be overwritten when the script is next run.
 */
static void ERR_load_CCA4758_strings(void);
static void ERR_unload_CCA4758_strings(void);
static void ERR_CCA4758_error(int function, int reason, char *file, int line);
#define CCA4758err(f,r) ERR_CCA4758_error((f),(r),__FILE__,__LINE__)

/* Error codes for the CCA4758 functions. */

/* Function codes. */
#define CCA4758_F_CCA_RSA_SIGN				 105
#define CCA4758_F_CCA_RSA_VERIFY			 106
#define CCA4758_F_IBM_4758_CCA_CTRL			 100
#define CCA4758_F_IBM_4758_CCA_FINISH			 101
#define CCA4758_F_IBM_4758_CCA_INIT			 102
#define CCA4758_F_IBM_4758_LOAD_PRIVKEY			 103
#define CCA4758_F_IBM_4758_LOAD_PUBKEY			 104

/* Reason codes. */
#define CCA4758_R_ALREADY_LOADED			 100
#define CCA4758_R_ASN1_OID_UNKNOWN_FOR_MD		 101
#define CCA4758_R_COMMAND_NOT_IMPLEMENTED		 102
#define CCA4758_R_DSO_FAILURE				 103
#define CCA4758_R_FAILED_LOADING_PRIVATE_KEY		 104
#define CCA4758_R_FAILED_LOADING_PUBLIC_KEY		 105
#define CCA4758_R_NOT_LOADED				 106
#define CCA4758_R_SIZE_TOO_LARGE_OR_TOO_SMALL		 107
#define CCA4758_R_UNIT_FAILURE				 108
#define CCA4758_R_UNKNOWN_ALGORITHM_TYPE		 109

#ifdef  __cplusplus
}
#endif
#endif
