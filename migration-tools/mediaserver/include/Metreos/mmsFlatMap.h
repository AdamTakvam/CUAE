// 
// mmsFlatMap.h  
//
// A pointer-free integer-to-bytestring multimap class. A flatmap can be 
// created in one process and read in another. Create a MmsFlatMapWriter and 
// invoke its insert methods to add entries to the map. Invoke its marshal() 
// method to commit the map to your buffer, the buffer size obtained from 
// MmsFlatMapWriter::length(). To subsequently read the map, instantiate a  
// MmsFlatMapReader, passing the marshaled map to the constructor, and invoke
// MmsFlatMapReader::find to access the desired map entries randomly, or  
// MmsFlatMapReader::get to access the map entries sequentially. 
// Note that once the map is committed, there is no further copying of map
// data; that is, the map reader uses the map as is, in its flat format.                     
// Flatmaps may embedded within flatmaps; to do so, (a) create the submap;
// (b) MmsFlatMapWriter::format an entry in the metamap, of submap.length(), 
// and of datatype FLATMAP; (c) commit the submap into that formatted area.
// Note that at commit time, an extra custom map header extension can be 
// supplied, permitting application-specific header data to be embedded,
// Of course this information can be accessed without instantiating a reader.
// 
#ifndef MMS_FLATMAP_H
#define MMS_FLATMAP_H                      
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include <set>                               
#include <assert.h>                         // Some macros to peek at flatmap
                                            // without instantiating a reader
#define getFlatmapLength(p) (((MmsFlatMap::MapHeader*)p)->maplength)
#define getFlatmapCount(p)  (((MmsFlatMap::MapHeader*)p)->count)
#define getFlatmapHeaderExtension(p) \
  (((MmsFlatMap::MapHeader*)p)->length == sizeof(MmsFlatMap::MapHeader)? 0: \
   ((MmsFlatMap::MapHeader*)p)->length  - sizeof(MmsFlatMap::MapHeader))
#define isValidFlatmap(p) (p)
#define isValidFlatmapSignature(p) \
  (((MmsFlatMap::MapHeader*)p)->sig == MmsFlatMap::MapHeader::signature)

#define INITIAL_BODYBUFSIZE 512  



class MmsFlatMap                            // Base class        
{
  public: 
  enum datatype {INT=1, BYTE=2, STRING=3, FLATMAP=4, LONG=5, DOUBLE=6};

  struct MapHeader                          // Header for entire map
  { 
    int  length;                            // Header length incl any extension
    unsigned int sig;                       // Header signature
    int  maplength;                         // Total length of flatmap object
    int  count;                             // Number of map entries
    int  bodyoffset;                        // Offset to data block
    int  bodylength;                        // Length of data block
    enum{signature=0xdeadcafe};             // Signature magic number
    MapHeader() 
    { memset(this, 0, sizeof(MapHeader)); 
      sig    = signature;
      length = sizeof(MapHeader);
    }
  };


  struct EntryHeader                        // Header for map body entry
  {
    int  datatype;   
    int  datalength; 
    unsigned int sig; 
    enum{signature=0xfeedface};
    EntryHeader(int t,int l)  {datatype=t; datalength=l; sig = signature;}
    EntryHeader() {memset(this, 0, sizeof(EntryHeader)); sig = signature;}
  };


  struct IndexEntry                         // Index entry[i]
  { 
    int key;                                 
    int offset;
    IndexEntry(int k, int o) {key = k; offset = o;} 
    bool operator<(const IndexEntry& that) const {return this->key < that.key;}
  }; 

  int size()  { return m_entries; }
  
  protected:
  char* m_body;                             // Map data area buffer
  int   m_bodybufsize;                      // Current size of data buffer
  int   m_entries;                          // Current number of map entries
};


                                            
class MmsFlatMapWriter: public MmsFlatMap   // Constructs a flatmap
{
  public:  
                                            // Insert object-valued pair
  int insert(const int key, const int datatype, const int length, const char* value);
                                            // Insert 32-bit integer-valued pair 
  int insert(const int key, const int value);
                                            // Insert 64-bit integer-valued pair 
  int insert(const int key, const __int64 value);
                                            // Insert 64-bit float-valued pair 
  int insert(const int key, const double value);
                                            // Format a block, return pointer
  char* format(const int key, const int datatype, const int length);
                                            // Return size of a marshaled map
  int length();                             //(any hdr extension not included 
                                            // Commit map to supplied buffer
  int marshal(char* buf, int extensionlength=0, char* headerextension=0);  

  int verify(const int length);             // Ensure sufficient buffer space
                                            
  int reallocateBody(const int size = 0);   // Reallocate map data buffer
                                            // Return current data length
  inline int bodysize() { return m_bodyptr - m_body; }
                                            // Ctor
  MmsFlatMapWriter(const int initialBodySize = 0);

 ~MmsFlatMapWriter();                            

  protected:                                // We use a std::multiset during
  std::multiset<IndexEntry> m_index;        // build step to sort the index 
  char* m_bodyptr;                          // Current data buffer position
  int   m_initialBodySize;                         
                                            // Insert data or preformat paramater
  char* insertx(const int key, const int datatyp, const int len, const char* val);
};



                                            
class MmsFlatMapReader: public MmsFlatMap   // Lightweight flatmap reader 
{
  public:
                                             
  MmsFlatMapReader(const char* flatmap);    // Ctor 1 accepts a flatmap
                                            
  MmsFlatMapReader();                       // Ctor 2 creates an empty reader

  int load(const char* flatmap);            // Load a flatmap into the reader
                                            // Random access
  int find(const int key, char** value, int* datatype=0, int occurrence=0);
                                            // Sequential access
  int get(const int i, int* key, char** value, int* datatype);

  MapHeader* header() { return m_header; }

  void clear() { m_index = NULL; m_header = NULL; m_bodysize = m_entries = 0; }

  protected:
  int m_bodysize;
  MapHeader*  m_header;
  IndexEntry* m_index;                                            
};



#endif
