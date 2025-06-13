//
// Flatmap.cpp
//
#include "stdafx.h"
#include "Flatmap.h"

using namespace Metreos;

                                            // Insert object-valued pair
int FlatMapWriter::insert(const int key, const int datatype, const int length, const char* value)
{
  assert(value);
  this->insertx(key, datatype, length, value);
  return 0;
}


                                            // Insert integer-valued pair 
int FlatMapWriter::insert(const int key, const int value)
{
  int intval = value;
  this->insertx(key, datatype::INT, sizeof(int), (char*)&intval);
  return 0;
}


                                            // Insert 64-bit integer-valued pair 
int FlatMapWriter::insert(const int key, const __int64 value)
{
  __int64 int64val = value;
  this->insertx(key, datatype::LONG, sizeof(__int64), (char*)&int64val);
  return 0;
}


                                            // Insert 64-bit float-valued pair 
int FlatMapWriter::insert(const int key, const double value)
{
  double float64val = value;
  this->insertx(key, datatype::DOUBLE, sizeof(double), (char*)&float64val);
  return 0;
}


                                            // Format a block, return pointer
char* FlatMapWriter::format(const int key, const int datatype, const int length)
{
  return this->insertx(key, datatype, length, NULL);
}



int FlatMapWriter::length()                 // Return size of a marshaled map 
{                                           //(any hdr extension not included)
  int indexsize = sizeof(MapHeader) + (sizeof(IndexEntry) * m_entries);
  return indexsize + this->bodysize();
}


                                            // Commit map to supplied buffer
int FlatMapWriter::marshal(char* buf, int extensionlength, char* headerextension)           
{
  char* p = buf;

  MapHeader header;                         // Build & copy map header
  header.count  = m_entries;              
  int indexsize = sizeof(MapHeader)  + extensionlength
                +(sizeof(IndexEntry) * m_entries);
  header.bodyoffset = indexsize;
  header.bodylength = this->bodysize();
  header.maplength  = indexsize + header.bodylength;

  if  (extensionlength)
  {    
       assert(headerextension); 
       header.length += extensionlength;
  }

  memcpy(p, &header, sizeof(MapHeader));
  p += sizeof(MapHeader);                  

  if  (extensionlength)                     // Copy header extension
  {                                         // if one was supplied
       memcpy(p, headerextension, extensionlength);
       p += extensionlength;
  }

  std::multiset<IndexEntry>::iterator i;    // Copy map index
  for(i = m_index.begin(); i != m_index.end(); i++) 
  {
    IndexEntry& entry = *i;
    memcpy(p, &entry, sizeof(IndexEntry));
    p += sizeof(IndexEntry);
  }

  memcpy(p, m_body, header.bodylength);     // Copy map body (data)
  p += header.bodylength;
  delete[] m_body;
  m_body = 0;
  return p - buf;                           // Return external length
}


                                            // Ensure sufficient buffer space
int FlatMapWriter::verify(const int length)    
{
  int  newsize  = this->bodysize() + length;
  if  (newsize <= m_bodybufsize) return 0;  // Not enough, so increase body
                                            // buffer size by initial size
  int  delta = length > m_initialBodySize? length: m_initialBodySize;
  reallocateBody(m_bodybufsize + delta);
  return 1;
}


                                            // Reallocate map data buffer
int FlatMapWriter::reallocateBody(const int size)    
{
  int currentbodyoffset = m_bodyptr - m_body;
  int newsize = size? size: m_bodybufsize + m_initialBodySize;
  char*  newbody  = new char[newsize];
  memset(newbody, 0, newsize);
  int  copylength = newsize < m_bodybufsize? newsize: m_bodybufsize;
  memcpy(newbody, m_body, copylength); 
  delete[] m_body;
  m_body    = newbody;
  m_bodyptr = m_body + currentbodyoffset;
  m_bodybufsize = newsize; 
  assert(m_bodybufsize > 0 && m_bodybufsize < 65536); 
  return newsize;
}


                                            // Ctor
FlatMapWriter::FlatMapWriter(const int initialBodySize)
{
  m_initialBodySize = initialBodySize? initialBodySize: INITIAL_BODYBUFSIZE;
  m_bodybufsize = m_initialBodySize;  
  m_body = new char[m_bodybufsize];
  memset(m_body, 0, m_bodybufsize);

  m_entries = 0;
  m_bodyptr = m_body;
}



FlatMapWriter::~FlatMapWriter()                            
{                                            
  if (m_body) delete[] m_body;             
}


                                            // Insert data or preformat paramater
char* FlatMapWriter::insertx
( const int key, const int datatype, const int length, const char* value)
{
  IndexEntry xh(key, this->bodysize()); 
  m_index.insert(xh);                       // Insert index node to index set

  EntryHeader eh(datatype, length);
  this->verify(sizeof(EntryHeader) + length);

  memcpy(m_bodyptr, &eh, sizeof(EntryHeader));
  m_bodyptr += sizeof(EntryHeader);
  char* dataaddr = m_bodyptr;

  if  (value)                               // If data supplied ...
       memcpy(m_bodyptr, value, length);    // ... insert data block to body      
  else memset(m_bodyptr, 0, length);        // otherwise preformat with zeros

  m_bodyptr += length;     
  m_entries++;
  return dataaddr;
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// FlatMapReader
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

                                            // Ctor 1 accepts a flatmap
FlatMapReader::FlatMapReader(const char* flatmap): m_index(0), m_bodysize(0)
{
  this->load(flatmap);
}


                                            // Ctor 2 creates an empty reader
FlatMapReader::FlatMapReader(): m_index(0), m_bodysize(0) 
{ 
}


                 
                                           // Load a flatmap into the reader 
int FlatMapReader::load(const char* marshaledFlatmap)    
{                   
  // NOTE: load() will return 0 on an empty flat map.
  m_body = NULL;
  m_bodybufsize = 0;
  m_entries = 0;

  char* p  = (char*)marshaledFlatmap;
  if  (!p) return 0;
  m_body   = 0;
  m_header = (MapHeader*)p;          
  p += sizeof(MapHeader);
                                            // Interpret map header
  if  (m_header->sig != MapHeader::signature) return 0;
  m_entries  = m_header->count;
  m_body     = (char*)marshaledFlatmap + m_header->bodyoffset;
  m_bodysize = m_header->bodylength;
  assert(m_entries < 2048 && m_bodysize < 65536);
                                            // Skip header extension if any
  if  (m_header->length > sizeof(MapHeader))
       p += (m_header->length - sizeof(MapHeader));    

  m_index = (IndexEntry*)p;                 // Point at index
  return m_entries;
}


                                            // Random access
int FlatMapReader::find
( const int key, char** value, int* datatype, int occurrence)
{
  // Locate and return the map entry whose key is key, and whose occurrence
  // of key is occurrence. Caller supplies pointers to char* map entry and
  // optionally to the int entry datatype. Map entry length is returned. 
  // If an error is encountered, the returned length is zero. 
  // This is a sequential search of a sorted index, making it reasonably
  // efficient used as intended, with maps with relatively few entries.

  if  (!m_index) return 0;
  IndexEntry* indexi = m_index; 
  int  occurrencesThisKey = 0, length = 0;
  int  desiredOccurrence  = occurrence? occurrence: 1;
  
  for(int i=0; i < m_entries; i++, indexi++)
  {
    if  (indexi->key  < key) continue;
    if  (indexi->key  > key) break; 
    if  (++occurrencesThisKey < desiredOccurrence) continue; 

    char* p = m_body + indexi->offset;      // Index entry found:   
    EntryHeader* header =(EntryHeader*)p;   // Read data header                                            

    if  (header->sig != EntryHeader::signature) break;
    p += sizeof(EntryHeader);               // Skip over header ...
    *value = p;                             // ... & return parameter data 
    if  (datatype)                          // Return datatype ...
        *datatype = header->datatype;       // ... if requested
    length = header->datalength;            // Return data length
    break;
  }

  return length;
}


                                            // Sequential access
int FlatMapReader::get(const int i, int* key, char** value, int* datatype)
{
  // Locate and return map entry[i]. Keys are stored in ascending order,
  // not in the order they were inserted. Caller supplies pointers to int 
  // key entry, char* map entry, and int entry datatype. Map entry length 
  // is returned. If an error is encountered, the returned length is zero. 

  if  (i >= m_entries || i < 0 || !m_index) return 0;
  IndexEntry* indexi  = &m_index[i]; 
  char* p = m_body + indexi->offset; 
  EntryHeader* header = (EntryHeader*)p;
  p += sizeof(EntryHeader);
  *key = indexi->key;
  *datatype = header->datatype;
  *value = p;
  return header->datalength;
}

ostream& FlatMapReader::show(ostream& os, int key, int len, int type, char* val)
{
  if  (len == 0) { os <<"key " <<key << " not found" <<endl; return os; }
  char    buf[512]; 
  int     ival; 
  __int64 lval;
  double  dval; 
  switch(type)
  { case FlatMap::datatype::BYTE:
         memset(buf,0,sizeof(buf));
         memcpy(buf,val,len);
         os<<"key " << key <<" value "<<buf <<endl;
         break;

    case FlatMap::datatype::INT:
         ival = *((int*)val);
         os <<"key " <<key << " value " <<ival <<endl;
         break;

    case FlatMap::datatype::STRING:
         os <<"key " <<key << " value " <<val <<endl;
         break;

    case FlatMap::datatype::FLATMAP:
		{
			os <<"Key " <<key <<" is embedded flatmap, recursing ..." <<endl;
			FlatMapReader reader(val);
			reader.dump(os);
		}
        
		break;

    case FlatMap::datatype::LONG:
         lval = *((__int64*)val);
         os <<"key " <<key << " value " <<lval <<endl;
         break;

    case FlatMap::datatype::DOUBLE:
         dval = *((double*)val);
         os <<"key " <<key << " value " <<dval <<endl;
         break;
  }

  return os;
}

ostream& FlatMapReader::dump(ostream& os)
{
  char* data;
  int   type, len, key;
                                            
  for(int i=0; i < size(); i++)
  {
    len = get(i, &key, &data, &type);
    show(os, key, len, type, data);
  }

  return os;
}
