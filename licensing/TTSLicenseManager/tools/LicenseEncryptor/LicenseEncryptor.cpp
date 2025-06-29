#include "StdAfx.h"

// Runtime Includes
#include <iostream>

// Crypto++ Includes
#include "cryptlib.h"
#include "osrng.h"      // PRNG
#include "Base32.h"     // Base32
#include "aes.h"        // AES
#include "modes.h"      // CBC_Mode< >
#include "filters.h"    // StringSource and
                        // StreamTransformation

int main(int argc, char* argv[]) {
    
    // Key and IV setup
    byte key[ CryptoPP::AES::DEFAULT_KEYLENGTH ], 
          iv[ CryptoPP::AES::BLOCKSIZE ];

    ::memset( key, 0x01, CryptoPP::AES::DEFAULT_KEYLENGTH );
    ::memset(  iv, 0x01, CryptoPP::AES::BLOCKSIZE );
	
	// The TTS license Key
	//const std::string PlainText = 
	//"License:0048f40078ee6cc981e8a43300d5cf002d1dd9b9a0d0776b01fc653fc4283923c01e5522000240007c7000bbfa80a74e:000EA6CA7459:VoiceText:20920606:128:Cisco (CA)_Neospeech:WindowsNT2KXP:[V00] CDKEY_92B8-2R5M-ZYZZ-Z5UF-YVUZ:";
	// The HMP License Key
	/*
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
		0x9A57DC17
	*/
	// the above key needs to be changed to the below. It needs to be flat as below into one heaxdecimal.
	//const std::string PlainText =
	//"0x8A76C8230xFD3999AF0x13210EA10x6B29E7A70x8B2D8FCA0x66355CB10x28DFC5200x5AB0AEDD0xD5B4BBB40x8E5FA62C0x6CCBEAE00xFFC35E070x34936F700xF7A5D80E0x02CD2E6D0x6C64BAB10xE4340DE90xA3919FA50xE5E8EEE70xCEB63AD70x9CBA71E80x2AABB45B0xBCA87AD40x67E072F70xA9DC9C040xCDA1EEC90xA51BE73C0x40C711ED0xC725EF930x7B4EFBDD0xD7BFE8720x9A57DC17";
	/*
		0xD12C46BF,
		0x91F946CD,
		0x701CB14F,
		0xAC5E89FF,
		0xAAD77EDF,
		0x7D6AD3C0,
		0x0907A900,
		0xB27030EC,
		0x446ABA1E,
		0x6D5E79C9,
		0x4519E59D,
		0x16AB7A28,
		0xD12DD9F7,
		0xF81214ED,
		0x3E9FF34D,
		0x1D2E4E03,
		0x590C550B,
		0xAED4515A,
		0x272C49B9,
		0xD28C1A3C,
		0x0CD27275,
		0x89589D12,
		0x27EAC39F,
		0xE41F84E1,
		0xA1CADD7D,
		0x94778678,
		0xADE65F67,
		0xADD38E36,
		0xEF4315A8,
		0x9C174AB2,
		0xA779257B,
		0xD8ACC440
	*/
	const std::string PlainText =
	"0xD12C46BF0x91F946CD0x701CB14F0xAC5E89FF0xAAD77EDF0x7D6AD3C00x0907A9000xB27030EC0x446ABA1E0x6D5E79C90x4519E59D0x16AB7A280xD12DD9F70xF81214ED0x3E9FF34D0x1D2E4E030x590C550B0xAED4515A0x272C49B90xD28C1A3C0x0CD272750x89589D120x27EAC39F0xE41F84E10xA1CADD7D0x947786780xADE65F670xADD38E360xEF4315A80x9C174AB20xA779257B0xD8ACC440";

    // Pseudo Random Number Generator
    CryptoPP::AutoSeededRandomPool rng;

    // Encryptor
    CryptoPP::CBC_Mode<CryptoPP::AES>::Encryption
        Encryptor( key, sizeof(key), iv );

    // Decryptior
    CryptoPP::CBC_Mode<CryptoPP::AES>::Decryption
        Decryptor( key, sizeof(key), iv );

    //////////////////////////////////////////
    //                Output                //
    //////////////////////////////////////////
    
    /*std::cout << "Algorithm:" << std::endl;
    std::cout << "  " << Encryptor.AlgorithmName() << std::endl;
    std::cout << std::endl;
    
    std::cout << "Plain Text (" << PlainText.length() << " bytes)" << std::endl;
    std::cout << "  '" << PlainText << "'" << std::endl;
    std::cout << std::endl;*/

    ///////////////////////////////////////////
    //            Generation Loop            //
    ///////////////////////////////////////////

    //unsigned int ITERATIONS = 1;
    //for( unsigned int i = 0; i < ITERATIONS; i++ )
    //{
        //std::string EncodedText = "TCGN-3CZK-6HXK-XDGU-Q7TJ-D7QP-T66X-U2ME-J9Q9-554S-3WGJ-CGZJ-WWGZ-498C-QAIE-TRD4-KXGT-C3JX-5M98-K7MP-HJ6R-BSPS-FM7C-UI67-NAVU-EPBX-3BFV-VPVQ-5CPP-BZPF-DQEV-DCPD-2B4N-HGYT-I6AA-J5Z4-9QGW-WDJW-464U-IJD9-MTWC-2J56-XM44-THYQ-SRWV-RU57-GTVF-MJA2-MKSU-JPQ3-FYIE-ZD9W-WWRR-8E9W-ABH4-R5P4-I39C-CX2I-9XJ9-9NS7-PM84-ZHQY-NSFN-WN7U-NRH8-PKFC-XWQD-5SET-KI77-PM8M-ZVFA-XRPK-W7WX-IWMM-RTR6-AVX8-SJTF-X6AJ-Q5YB-N5Z9-QE4M-68T3-XIZR-VUTQ-C275-Q3GT-US72-FDXM-7RB8-EZS";
    std::string EncodedText = "";    
	std::string SaltText = "";
    Encryptor.Resynchronize( iv );
    Decryptor.Resynchronize( iv );

    // Salt
    CryptoPP::RandomNumberSource( rng, 4, true,
        new CryptoPP::StringSink( SaltText )
    ); // RandomNumberSource
	//printf("/n%s=/n", SaltText.c_str());
    // Encryption
    CryptoPP::StringSource( SaltText + PlainText, true,
        new CryptoPP::StreamTransformationFilter( Encryptor,
            new CryptoPP::Base32Encoder(
                new CryptoPP::StringSink( EncodedText ),
            true, 4, "-") // Base32Encoder
        ) // StreamTransformationFilter
    ); // StringSource
    
    // Add Appendage for Pretty Printing
    EncodedText += "JW";

    //////////////////////////////////////////
    //                Output                //
    //////////////////////////////////////////
    std::cout << EncodedText << std::endl;

    //////////////////////////////////////////
    //                  DMZ                 //
    //////////////////////////////////////////

    // Recovered Text Sink
    std::string RecoveredText = "";

    // Remove Appendage for Pretty Printing
    EncodedText = EncodedText.substr( 0, EncodedText.length() - 2 );

    CryptoPP::StringSource( EncodedText, true,
        new CryptoPP::Base32Decoder(
            new CryptoPP::StreamTransformationFilter( Decryptor,
                new CryptoPP::StringSink( RecoveredText )
            ) // StreamTransformationFilter
        ) // Base32Decoder
    ); // StringSource

    // Step over Salt
    RecoveredText = RecoveredText.substr( 4 );

    //////////////////////////////////////////
    //                Output                //
    //////////////////////////////////////////
    std::cout << "  '" << RecoveredText << "'" << std::endl;

    //} // for( ITERATIONS )
   
    return 0;
}