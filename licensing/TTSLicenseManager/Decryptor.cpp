#include "Decryptor.h"
#include "LicenseLock.h"

template <typename T> std::ostream& bin(T& value, std::ostream &o);

void Decryptor::decryptToHiddenFile() {
	HANDLE hOut;
	DWORD nOut;

	printf("\ndecryting....\n");

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
	
	License += "JW";

	std::string RecoveredLicense = "";
    // Remove Appendage for Pretty Printing
    License = License.substr( 0, License.length() - 2 );

    CryptoPP::StringSource( License, true,
        new CryptoPP::Base32Decoder(
            new CryptoPP::StreamTransformationFilter( Decryptor,
                new CryptoPP::StringSink( RecoveredLicense )
            ) // StreamTransformationFilter
        ) // Base32Decoder
    ); // StringSource
	
	DWORD dwSize;
	hOut = CreateFile(file_out.c_str(), GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_HIDDEN, NULL);
	WriteFile(hOut, RecoveredLicense.c_str(), RecoveredLicense.size(), &dwSize, NULL);  

	CloseHandle(hOut);

    printf("\nDone...\n");
}

void Decryptor::decrypt() {
    
    source = new FileSource(file.c_str(), false); 

    delete new StringSource( password, true, 
            new HashFilter(*(new SHA256), new ArraySink(pass, SHA256::DIGESTSIZE)) ); 

    decryption = new  CBC_Mode<AES>::Decryption(pass, AES::DEFAULT_KEYLENGTH, iv); 

    // checking password 
    // get and encrypt 3 blocks 
    source->Pump(3 * AES::BLOCKSIZE); 
    source->Get(head_file, 3 * AES::BLOCKSIZE); 
    decryption->ProcessBlocks(head_file_dec, head_file, 3); 
    // compare block2 and block3 
    if(0 != memcmp(&head_file_dec[AES::BLOCKSIZE], &head_file_dec[2*AES::BLOCKSIZE], 
                    AES::BLOCKSIZE)) { 
                    printf("\nBad password...\n"); 
                    return; 
    } 

    // "bind" decryptor to output file 
    source->Attach( new StreamTransformationFilter(*decryption, 
            new FileSink(file_out.c_str()) ) ); 

    // push the rest data 
    source->PumpAll(); 

    printf("\nDone...\n");
}

template <typename T> inline T highbit(T& t) {
	return t = (((T)(-1)) >> 1) +1;
}

template <typename T> std::ostream& bin(T& value, std::ostream &o) {
	for (T bit = highbit(bit);bit;bit >>= 1) {
		o << ((value & bit) ? '1' : '0' );
	}
	return 0;
}

Decryptor::Decryptor(string file_out) {
    memset(iv, 0, AES::BLOCKSIZE ); 
	this->file_out = file_out;
}

Decryptor::Decryptor(string file, string file_out, string password) {

    memset(iv, 0, AES::BLOCKSIZE ); 

	this->file = file;
	this->file_out = file_out;
	this->password = password;

}

Decryptor::~Decryptor() {
	// need to be destructed. Otherwise, the decrypted license files are locked
	//delete decryption;
	//delete source;
}