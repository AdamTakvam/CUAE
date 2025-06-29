// LicKeyDll.cpp : Defines the entry point for the DLL application.
//


#include "stdafx.h"
#include "LicKeyDll.h"

#include "stdafx.h" 

//#include "filters.h" 
#include "files.h" 
#include "sha.h" 
//#include "modes.h" 
//#include "aes.h" 
#include "mars.h" 
#include "osrng.h" 

// Runtime Includes
#include <iostream>
#include <cstring>
#include <iomanip>
#include <tchar.h>

#include <stdio.h>
#include <windows.h>

// Crypto++ Includes
#include "cryptlib.h"
#include "Base32.h"     // Base32
#include "aes.h"        // AES
#include "modes.h"      // CBC_Mode< >
#include "filters.h"    // StringSource

USING_NAMESPACE(CryptoPP) 
USING_NAMESPACE(std) 

void test(unsigned long* code);
void decrypt(unsigned long* code);
int hex2int(char c);

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    return TRUE;
}

// the 1st real segnment
std::string getSeg2345() {
	return "VKGH-9GJY-FM93-7APZ-GS8Y-A5UW-CYVQ-XTRH-WBPN-";
}

// the 2nd real segnment
std::string getSeg8972() {
	return "VAZ4-DZUX-HPKR-S6MY-3GD7-Q2Y4-W8VH-R5HF-J9YY-XTNA-Q9VD-FE35-";
}

// fake license segment
std::string getSeg7234() {
	return "93B9-U8CF-72PK-WBCH-PXJI-TAAP-8HZ4-FBWQ-";
}

// the 3rd real segnment
std::string getSeg0137() {
	return "7R8R-EDBY-Q7EI-36JJ-93B9-U8CF-72PK-WBCH-PXJI-TAAP-8HZ4-FBWQ-QENI-HN4W-FC64-3BJU-";
}

void decrypt(unsigned long* code) {
	// obfustigated
	// 
	std::string License = getSeg2345()+getSeg8972()+getSeg0137()+"TBKA-SWHZ-63ZX-89U2-DMU4-YJGQ-CESB-4SM6-D82W-FMWG-QI4R-EIU2-3XRG-S6B2-4T3A-8JQE-PPGS-NRB4-JZQ5-S6T6-26YC-927Y-PGDA-FAW3-ZC6N-RIBD-EYNM-EAEA-AFCG-WI2S-8NHF-G4GP-GX39-UCAU-EG3A-H245-6R2G-JXM8-JGBF-UFUC-2V5H-ICPM-5US5-DZC2-V6DK-M2JE-67CX-H94Z-8EXH-THUR-FBTK-4GKV-Z4WG-8ITM-7RV3-25XY-HE9J-TZDR-GSUM-RG37-EYHW-GP9M-DZQR-FKCQ-I3TA-AV4A-58IZ-CFNA-26AB-PHH7-66PV-GH3F-W8H6-4X5Q-CAC5-FCR2-UNXS-J8RU-VQCU-WW9K-A2XJ-MAAM-RP9V-GCAV-XT5E-3GBF-FAUX-ZHEQ-H64A-6HNY-4UHW-FSGN-76QM-X98R-2BK4-VU9P-KFI5-WWJW";

	// Recovered Text Sink
    std::string RecoveredText;

	// Key and IV setup
    byte key[ CryptoPP::AES::DEFAULT_KEYLENGTH ], 
          iv[ CryptoPP::AES::BLOCKSIZE ];

    ::memset( key, 0x01, CryptoPP::AES::DEFAULT_KEYLENGTH );
    ::memset(  iv, 0x01, CryptoPP::AES::BLOCKSIZE );  

    // Pseudo Random Number Generator
    CryptoPP::AutoSeededRandomPool rng;

    // Encryptor
    CryptoPP::CBC_Mode<CryptoPP::AES>::Encryption
        Encryptor( key, sizeof(key), iv );

    // Decryptior
    CryptoPP::CBC_Mode<CryptoPP::AES>::Decryption
        Decryptor( key, sizeof(key), iv );

    std::string SaltText = "";
    Encryptor.Resynchronize( iv );
	Decryptor.Resynchronize( iv );

	std::string RecoveredLicense = "";

    License = License.substr( 0, License.length() - 2 );

    CryptoPP::StringSource( License, true,
        new CryptoPP::Base32Decoder(
            new CryptoPP::StreamTransformationFilter( Decryptor,
                new CryptoPP::StringSink( RecoveredLicense )
            ) // StreamTransformationFilter
        ) // Base32Decoder
    ); // StringSource

	RecoveredLicense = RecoveredLicense.substr( 4 );

	std::string baseNumber;
	for (int i=0;i<32;i++) {
		baseNumber.assign(RecoveredLicense,  i*10+2, 8);
		char c[9];
		strcpy(c, baseNumber.c_str());
		code[i] = 0;
		for (int j=0;j<8;j++) {
			code[i] = code[i]*16 + hex2int(*(c+j));
			//std::cout << "code = " << code[i] << '\n';
		}
	}
}

int hex2int(char c) 
{
	int value = 0;
	switch (c) 
	{
case '0':
case '1':
case '2':
case '3':
case '4':
case '5':
case '6':
case '7':
case '8':
case '9':
value=atoi(&c);
break;
case 'A':
case 'a':
value=10;
break;
case 'B':
case 'b':
value=11;
break;
case 'C':
value=12;
break;
case 'D':
case 'd':
value=13;
break;
case 'E':
case 'e':
value=14;
break;
case 'F':
case 'f':
value=15;
break;

	}
	return value;
}

void test(unsigned long* code) 
{
/*unsigned long key[] = {
0x8A76C823,
0xFD3999AF,
0x13210EA1,
0x6B29E7A7,
0x8B2D8FCA,
0x66355CB1,
0x28DFC520,
0x5AB0AEDD,
0xD5B4BBB4,
0x8E5FA62C,
0x6CCBEAE0,
0xFFC35E07,
0x34936F70,
0xF7A5D80E,
0x02CD2E6D,
0x6C64BAB1,
0xE4340DE9,
0xA3919FA5,
0xE5E8EEE7,
0xCEB63AD7,
0x9CBA71E8,
0x2AABB45B,
0xBCA87AD4,
0x67E072F7,
0xA9DC9C04,
0xCDA1EEC9,
0xA51BE73C,
0x40C711ED,
0xC725EF93,
0x7B4EFBDD,
0xD7BFE872,
0x9A57DC17};*/
for (int i=0;i<32;i++) 
{
	code[i] = 0x00000000;//key[i];
}

}
