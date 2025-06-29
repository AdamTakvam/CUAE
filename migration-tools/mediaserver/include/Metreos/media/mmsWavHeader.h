//
// mmsWavHeader.h
//
// Standard and portable WAV file headers  
//
#ifndef MMS_WAVHDR_H
#define MMS_WAVHDR_H
#pragma once



#pragma pack(1)

struct MmsWavHeader               // Header 1: file header
{                                 // Header part:
  char riff[4];                   // 4 4  "RIFF"
  long wavFileLength;             // 4 8  Length of entire file
  char wave[4];                   // 4 12 "WAVE"
                                  // Header 2: Format header
  char fmt[4];                    // 4 16 "fmt "
  long fmtHeaderLength;           // 4 20 Length of format part (16)
  unsigned short formatTag;       // 2 22 Always 1 (PCM)
  unsigned short channels;        // 2 24 1=mono 2=stereo
  unsigned int   sampleRate;      // 4 28 4670 hz to 44100 hz
  unsigned int   bytesPerSec;     // 4 32 (channels*sampleFrequency*SampleSize+7)/8
  unsigned short blockAlign;      // 2 34 (channels*samplesize+7)/8                                 
  unsigned short bitsPerSample;   // 2 36 (sampleSize) 8 for mono, 16 for stereo
                                  // Header 3: data header
  char data[4];                   // 4 40 "data"
  long dataLength;                // n nn length of raw data

  MmsWavHeader(int KHz, long dataLength)
  {
    memset(this, 0, sizeof(MmsWavHeader));
    memcpy(this->riff, "RIFF", 4);
    this->dataLength    = dataLength;
    this->wavFileLength = dataLength + (8 + 16 + 12) + 8;  // Padded to a/l 8 bytes
    memcpy(this->wave, "WAVE", 4);
    memcpy(this->fmt,  "fmt ", 4);
    this->fmtHeaderLength = 16;
    this->formatTag  = 1;
    this->channels   = 1;  
    this->sampleRate = KHz * 1000;  
    this->bitsPerSample = 16;  
    this->bytesPerSec = KHz == 16? 16000: 8000;  
    this->blockAlign  = 2; // 1=8bit mono; 2=8bit stereo or 16bit mono; 4=16bit stereo

    memcpy(this->data, "data", 4);
  }
};

#pragma pack()

#endif // MMS_WAVHDR_H
