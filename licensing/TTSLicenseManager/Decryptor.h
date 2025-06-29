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

class Decryptor {
	byte pass[ SHA256::DIGESTSIZE ]; 
    byte iv[ AES::BLOCKSIZE ]; 
    byte head_file[ 3 * AES::BLOCKSIZE ]; 
    byte head_file_dec[ 3 * AES::BLOCKSIZE ]; 
	FileSource *source; 
	CBC_Mode<AES>::Decryption *decryption; 
	string file, file_out, password;
public:
	Decryptor(string file_out);
	Decryptor(string file, string file_out, string password);
	~Decryptor();
	void decryptToHiddenFile();
	void decrypt();
};