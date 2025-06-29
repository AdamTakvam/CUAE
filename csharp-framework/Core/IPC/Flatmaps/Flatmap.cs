using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;


namespace Metreos.Core.IPC.Flatmaps
{
	/// <summary>Static methods and structures for binary flatmap manipulation</summary>
	public abstract class Flatmap
	{
        /// <summary>Convert a FlatmapList to a binary flatmap</summary>
        /// <exception cref="FlatmapException">If list cannot be converted</exception>
        public static byte[] ToFlatmap(FlatmapList list)
        {            
            return Flatmap.ToFlatmap(list, null);             
        }


        /// <summary>Convert a FlatmapList to a binary flatmap</summary>
        /// <exception cref="FlatmapException">If list cannot be converted</exception>
        public static byte[] ToFlatmap(FlatmapList list, byte[] headerExtension)
        {
            try 
            { 
                return Flatmap.ConvertList(list, headerExtension); 
            }
            catch(Exception x) 
            { 
                throw new FlatmapException(x.Message); 
            }
        }


        /// <summary>Convert a binary flatmap to a FlatmapList</summary>
        /// <exception cref="FlatmapException">If bytes cannot be converted</exception>
        public static FlatmapList FromFlatmap(byte[] flatmap)
        {        
            return Flatmap.FromFlatmap(flatmap, null);           
        }


        /// <summary>Convert a binary flatmap to a FlatmapList</summary>
        /// <exception cref="FlatmapException">If bytes cannot be converted</exception>
        public static FlatmapList FromFlatmap(byte[] flatmap, FlatmapList fl)
        {        
            if (Flatmap.InterpretBytes(flatmap) != Flatmap.ValueType.Flatmap)
                throw new FlatmapException(Flatmap.BadMapMsg);

            try 
            { 
                return Flatmap.ConvertBytes(flatmap, fl); 
            }
            catch(Exception x) 
            { 
                throw new FlatmapException(x.Message); 
            }
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // ToFlatmap support
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Convert FlatmapList collection to binary flatmap</summary>
        protected static byte[] ConvertList(FlatmapList list, byte[] extraHeader)
        {
            MemoryStream indexStream = new MemoryStream();
            MemoryStream bodyStream  = new MemoryStream();

            IndexEntry indexI  = new IndexEntry();
            EntryHeader entryI = new EntryHeader();

            uint   nval = 0;
            long   lval = 0;
            double dval = 0.0;
            string sval = null;
            byte[] bval = null;

            list.Sort();
                                             
            foreach(Flatmap.MapEntry entry in list)
            {                               // Write index slot
                indexI.Set(entry.key, (uint)bodyStream.Position);                 
                indexStream.Write(indexI.ToArray(), 0, IndexEntryLength);
                
                Flatmap.ValueType valueType = entry.dataType;

                switch(valueType)           // Write value header and entry to body
                {
                  case ValueType.Int:
                       nval = Convert.ToUInt32(entry.dataValue);   
                       entryI.Set((uint)valueType, 4); 
                       bodyStream.Write(entryI.ToArray(), 0, EntryHeaderLength);
                       bodyStream.Write(BitConverter.GetBytes(nval), 0, 4);
                       break; 

                  case ValueType.String:  
                       sval = (string) entry.dataValue;
                       bval = Flatmap.StringToCstring(sval);
                       entryI.Set((uint)valueType, (uint)bval.Length);
                       bodyStream.Write(entryI.ToArray(), 0, EntryHeaderLength);
                       bodyStream.Write(bval, 0, bval.Length);
                       break;

                  case ValueType.Long:
                       lval = Convert.ToInt64 (entry.dataValue);                               
                       entryI.Set((uint)valueType, 8); 
                       bodyStream.Write(entryI.ToArray(), 0, EntryHeaderLength);
                       bodyStream.Write(BitConverter.GetBytes(lval), 0, 8);
                       break;

                  case ValueType.Double:
                       dval = Convert.ToDouble (entry.dataValue);                               
                       entryI.Set((uint)valueType, 8); 
                       bodyStream.Write(entryI.ToArray(), 0, EntryHeaderLength);
                       bodyStream.Write(BitConverter.GetBytes(dval), 0, 8);
                       break;

                  case ValueType.Byte:
                  case ValueType.Flatmap:
                       bval = (byte[]) entry.dataValue;
                       entryI.Set((uint)valueType, (uint)bval.Length);
                       bodyStream.Write(entryI.ToArray(), 0, EntryHeaderLength);
                       bodyStream.Write(bval, 0, bval.Length);
                       break; 
                }
            }
                                            // Populate map header          
            Flatmap.MapHeader mapheader = new Flatmap.MapHeader(list.Count);
            int extraLength   = extraHeader == null? 0: extraHeader.Length;
            mapheader.length += (uint)extraLength;
            int indexSize = (int)mapheader.length + (int)indexStream.Position;
            mapheader.bodyoffset = (uint) indexSize;
            mapheader.bodylength = (uint) bodyStream.Position;
            mapheader.maplength  = (uint)(indexSize + mapheader.bodylength);
                                            // Write header, index, and body to map
            MemoryStream mapStream = new MemoryStream();
            mapStream.Write(mapheader.ToArray(), 0, MapHeaderLength); 
            if (extraLength > 0) mapStream.Write(extraHeader, 0, extraLength);
            indexStream.WriteTo(mapStream);
            indexStream.Close();
            bodyStream.WriteTo (mapStream);
            bodyStream.Close(); 
                                    
            byte[] flatmap = mapStream.ToArray();
            mapStream.Close();
            
            return flatmap;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // FromFlatmap support
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Convert prevalidated flatmap bytes to FlatmapList</summary>
        /// <exception cref="FlatmapException">If map is invalid</exception>
        protected static FlatmapList ConvertBytes(byte[] bytes, FlatmapList fl)
        {
            MapHeader mapHeader = new MapHeader(bytes, 0);
            IndexEntry indexI   = new IndexEntry();
            EntryHeader entryI  = new EntryHeader();

            FlatmapList list = fl == null? new FlatmapList(): fl;
                                            // Store custom header if any
            list.HeaderExtension = Flatmap.GetExtension(bytes);
                                            // Point at first index entry
            int  currIndexOffset = (int)mapHeader.length;

                                            // For each index entry ...
            for(int i = 0; i < mapHeader.count; i++, currIndexOffset += IndexEntryLength)
            {                               // Interpret index slot
                indexI.Set(bytes, currIndexOffset);
                ValidateIndexEntry(indexI, mapHeader);
                                            // Point at data item header
                int currDataOffset = (int)(mapHeader.bodyoffset + indexI.offset);
                                            // Interpret data item header
                entryI.Set(bytes, currDataOffset);
                ValidateEntryHeader(entryI, mapHeader, currDataOffset);

                uint   nval = 0;
                long   lval = 0;
                double dval = 0.0;
                string sval = null;
                byte[] bval = null;         // Bump past data item header
                currDataOffset += Flatmap.EntryHeaderLength;
                                            // Interpret data type
                Flatmap.ValueType valueType = entryI.datatype <= (uint)Flatmap.ValueType.Double? 
                    (Flatmap.ValueType) entryI.datatype: Flatmap.ValueType.None;                

                switch(valueType)           // Interpret data item
                {
                    case ValueType.Int:
                         nval = BitConverter.ToUInt32(bytes, currDataOffset); 
                         list.Add(indexI.key, nval);                         
                         break; 

                    case ValueType.String:  
                         sval = Flatmap.CstringToString
                                 (bytes, currDataOffset, (int)entryI.datalength);
                         list.Add(indexI.key, sval);
                         break;

                    case ValueType.Long:
                         lval = (long)BitConverter.ToUInt64(bytes, currDataOffset); 
                         list.Add(indexI.key, lval);                         
                         break;

                    case ValueType.Double:
                         dval = BitConverter.ToDouble(bytes, currDataOffset); 
                         list.Add(indexI.key, dval);                         
                         break; 

                    case ValueType.Byte:
                    case ValueType.Flatmap:
                         bval = new byte[entryI.datalength];
                         Buffer.BlockCopy(bytes, currDataOffset, bval, 0, bval.Length);
                         list.Add(indexI.key, bval);
                         break; 

                    default: 
                         throw new FlatmapException(Flatmap.BadNdxMsg); 
                }
            }    

            return list;
        }    


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Public utilities
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

        /// <summary>Precalculate byte length of a flatmap created from specified list</summary>      
        public static int FlatmapLengthFromList(FlatmapList list, int extraHeaderLength)
        {
            int hdrlength   = Flatmap.MapHeaderLength + extraHeaderLength;
            int indexLength = list.Count * Flatmap.IndexEntryLength;
            int bodyLength  = list.Count * Flatmap.EntryHeaderLength;

            foreach(Flatmap.MapEntry entry in list)
            {                                 
                switch(entry.dataType)            
                {
                    case Flatmap.ValueType.Int:
                         bodyLength += 4;                         
                         break; 

                    case Flatmap.ValueType.Long:
                    case Flatmap.ValueType.Double:
                         bodyLength += 8;                         
                         break; 

                    case Flatmap.ValueType.String:  
                         bodyLength += ((string)entry.dataValue).Length + 1;
                         break;

                    case Flatmap.ValueType.Byte:
                    case Flatmap.ValueType.Flatmap:
                         bodyLength += ((byte[])entry.dataValue).Length;                    
                         break; 
                }
            }

            return hdrlength + indexLength + bodyLength;
        }


        /// <summary>Extract and return flatmap header from binary flatmap</summary>
        public static Flatmap.MapHeader GetFlatmapHeader(byte[] flatmap)
        {
            return Flatmap.ValueType.Flatmap == Flatmap.InterpretBytes(flatmap)?
                   new MapHeader(flatmap, 0): null;
        }


        /// <summary>Extract and return flatmap header extension if any</summary>
        public static byte[] GetFlatmapHeaderExtension(byte[] flatmap)
        {
            return Flatmap.ValueType.Flatmap == Flatmap.InterpretBytes(flatmap)?
                   Flatmap.GetExtension(flatmap): null;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // General support 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    
        /// <summary>Determine flatmap type of supplied value object</summary>
        public static Flatmap.ValueType ValidateValue(object x)
        {
            Flatmap.ValueType thistype = 0;
            
            if (x is Int32 || x is UInt32 || x is Int64 || x is UInt64 ||
                x is Int16 || x is UInt16 || x is Enum)  
                thistype = Flatmap.ValueType.Int;     
            else
            if (x is string) 
                thistype = Flatmap.ValueType.String; 
            else
            if (x is long)
                thistype = Flatmap.ValueType.Long;
            else
            if (x is double)
                thistype = Flatmap.ValueType.Double;
            else
            if (x is byte[]) 
                thistype = Flatmap.InterpretBytes(x as byte[]);

            return thistype; 
        }


        /// <summary>Determine if byte array is a flatmap or something else</summary>
        /// <returns>Flatmap value type indicating flatmap or other byte array</returns>
        public static ValueType InterpretBytes(byte[] bytes)
        {                                    
            return IsFlatmap(bytes, 0, bytes.Length)? 
                   Flatmap.ValueType.Flatmap: Flatmap.ValueType.Byte;
        }


        /// <summary>Indicate if byte blob at buffer offset is a flatmap</summary>
        public static bool IsFlatmap(byte[] buf, int offset, int indicatedMapLength)
        {
            if  (indicatedMapLength  < Flatmap.MapHeaderLength) return false;
            int  calculatedMapLength = buf.Length - offset;
            if  (calculatedMapLength < Flatmap.MapHeaderLength) return false;
  
            // Check presumed header length against blob size
            uint actualMapLength  = BitConverter.ToUInt32(buf, offset + Flatmap.MapLengthOffset);
            if  (actualMapLength != calculatedMapLength) return false;

            // Determine if blob contains a flatmap signature in expected position
            uint signature = BitConverter.ToUInt32(buf, offset + Flatmap.MapHeaderSigOffset);
            return (signature == Flatmap.MapHeaderSignature);
        }  


        /// <summary>Extract a flatmap from specified buffer at specified offset</summary>
        /// /// <exception cref="FlatmapException">If map is invalid</exception>
        public static byte[] GetFlatmap(byte[] buf, int offset, int indicatedMapLength)
        {
            byte[] flatmap = null;
            bool   result  = false;

            try
            {   if (IsFlatmap(buf, offset, indicatedMapLength))  
                {  
                    flatmap = new byte[buf.Length - offset];  
                    Buffer.BlockCopy(buf, offset, flatmap, 0, indicatedMapLength);
                    result = true;
                }
            }
            catch { }

            if (!result) throw new FlatmapException(BadMapMsg);

            return flatmap;      
        }      


        /// <summary>Convert a C# string to an 8-bit ascii C string</summary>
        public static byte[] StringToCstring(string s)
        {
            byte[] unibytes = Encoding.Unicode.GetBytes(s);
            byte[] ascbytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unibytes);
            byte[] strbytes = new byte[ascbytes.Length + 1];
            ascbytes.CopyTo(strbytes,0);
            return strbytes;
        }


        /// <summary>Convert an 8-bit ascii C string to a C# string</summary>
        public static string CstringToString(byte[] ascbytes)
        {
            byte[] unibytes = Encoding.Convert
                  (Encoding.ASCII, Encoding.Unicode, ascbytes, 0, ascbytes.Length-1);
 
            return Encoding.Unicode.GetString(unibytes);
        }


        /// <summary>Convert an 8-bit ascii C string to a C# string</summary>
        /// <remarks>Length passed is total C string length including null term</remarks>
        public static string CstringToString(byte[] bytes, int offset, int length)
        {
            byte[] unibytes = Encoding.Convert
                (Encoding.ASCII, Encoding.Unicode, bytes, offset, length-1);
 
            return Encoding.Unicode.GetString(unibytes);
        }


        /// <summary>Extract and return flatmap header extension if any</summary>
        protected static byte[] GetExtension(byte[] flatmap)
        {
            MapHeader header = Flatmap.GetFlatmapHeader(flatmap);

            int extLength = (int)header.length - Flatmap.MapHeaderLength;
            if (extLength < 1) return null;

            byte[] extension = new byte[extLength];
            Buffer.BlockCopy(flatmap, Flatmap.MapHeaderLength, extension, 0, extLength);
            return extension;
        }


        /// <summary>Ensure index entry points within map bounds</summary>
        /// <exception cref="FlatmapException">If index entry is invalid</exception>
        protected static void ValidateIndexEntry(IndexEntry entry, MapHeader header)
        {
            if (header.bodyoffset + entry.offset > header.maplength - EntryHeaderLength)
                throw new FlatmapException(Flatmap.BadNdxMsg);
        }


        /// <summary>Ensure data item length lies within map bounds</summary>
        /// <exception cref="FlatmapException">If index entry is invalid</exception>
        protected static void ValidateEntryHeader(EntryHeader entry, MapHeader hdr, int offset)
        {
            uint lastbyte  = (uint)offset + Flatmap.EntryHeaderLength + entry.datalength;

            if (entry.sig != Flatmap.EntryHeaderSignature || lastbyte > hdr.maplength)
                throw new FlatmapException(Flatmap.BadNdxMsg);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Inner classes, structures, constants
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Type constants for value of {key,value} pair</summary>
        public enum ValueType 
        { None=0, Int=1, Byte=2, String=3, Flatmap=4, Long=5, Double=6 
        }


        /// <summary>Header struct for entire flatmap</summary>
        public class MapHeader                     
        {                                    
            public uint length;             // Header length incl any extension
            public uint sig;                // Header signature
            public uint maplength;          // Total length of flatmap object
            public uint count;              // Number of map entries
            public uint bodyoffset;         // Offset to data block
            public uint bodylength;         // Length of data block  

            public MapHeader(int count)
            { 
                this.count  = (uint)count;
                this.length = Flatmap.MapHeaderLength;
                this.sig    = Flatmap.MapHeaderSignature;
            }

            public MapHeader(byte[] bytes, int offset)
            { 
                this.FromArray(bytes, offset);
            }  

            public byte[] ToArray()
            {
                byte[] buf = new byte[Flatmap.MapHeaderLength];
                Buffer.BlockCopy(BitConverter.GetBytes(length),    0, buf, 0, 4); 
                Buffer.BlockCopy(BitConverter.GetBytes(sig),       0, buf, 4, 4); 
                Buffer.BlockCopy(BitConverter.GetBytes(maplength), 0, buf, 8, 4);  
                Buffer.BlockCopy(BitConverter.GetBytes(count),     0, buf, 12, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(bodyoffset),0, buf, 16, 4); 
                Buffer.BlockCopy(BitConverter.GetBytes(bodylength),0, buf, 20, 4); 
                return buf;
            }  

            public void FromArray(byte[] array, int offset)
            {
                this.length     = BitConverter.ToUInt32(array, offset);
                this.sig        = BitConverter.ToUInt32(array, offset+4); 
                this.maplength  = BitConverter.ToUInt32(array, offset+8); 
                this.count      = BitConverter.ToUInt32(array, offset+12); 
                this.bodyoffset = BitConverter.ToUInt32(array, offset+16); 
                this.bodylength = BitConverter.ToUInt32(array, offset+20);                
            }                   
        }


        /// <summary>Header struct for a map body entry</summary>
        protected class EntryHeader                       
        {                                   
            public uint datatype;   
            public uint datalength; 
            public uint sig; 

            public EntryHeader() { }

            public EntryHeader(byte[] bytes, int offset)
            { 
                this.FromArray(bytes, offset);
            }

            public EntryHeader(uint dt, uint dl)
            { 
                datatype = dt; datalength = dl; sig = Flatmap.EntryHeaderSignature;;
            } 

            public void Set(uint dt, uint dl)
            { 
                datatype = dt; datalength = dl; sig = Flatmap.EntryHeaderSignature;
            } 

            public void Set(byte[] bytes, int offset)
            { 
                this.FromArray(bytes, offset);
            }

            public byte[] ToArray()
            {
                byte[] buf = new byte[Flatmap.EntryHeaderLength];
                Buffer.BlockCopy(BitConverter.GetBytes(datatype),  0, buf, 0, 4); 
                Buffer.BlockCopy(BitConverter.GetBytes(datalength),0, buf, 4, 4); 
                Buffer.BlockCopy(BitConverter.GetBytes(sig),       0, buf, 8, 4);  
                return buf;
            } 

            public void FromArray(byte[] array, int offset)
            {
                this.datatype   = BitConverter.ToUInt32(array, offset);
                this.datalength = BitConverter.ToUInt32(array, offset+4); 
                this.sig        = BitConverter.ToUInt32(array, offset+8);                              
            }        
        }


        /// <summary>Index entry[i]</summary>
        protected class IndexEntry                         
        {                                   
            public uint key;                // Item key                 
            public uint offset;             // Data offset from start of body

            public IndexEntry() { } 
            public IndexEntry(uint k, uint o) {key = k; offset = o;} 
            public void Set  (uint k, uint o) {key = k; offset = o;}   

            public void Set(byte[] bytes, int offset)
            { 
                this.FromArray(bytes, offset);
            }

            public byte[] ToArray()
            {
                byte[] buf = new byte[Flatmap.IndexEntryLength];
                Buffer.BlockCopy(BitConverter.GetBytes(key),   0, buf, 0, 4); 
                Buffer.BlockCopy(BitConverter.GetBytes(offset),0, buf, 4, 4); 
                return buf;
            } 

            public void FromArray(byte[] array, int offset)
            {
                this.key    = BitConverter.ToUInt32(array, offset);
                this.offset = BitConverter.ToUInt32(array, offset+4); 
            }          
        } 


        /// <summary>A FlatmapList list entry</summary>
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct MapEntry
        {
            public uint   key;
            public object dataValue;
            public Flatmap.ValueType dataType;

            public MapEntry(uint k, int t, object v)
            {
                key = k; dataValue = v; 
                dataType = t > 0 && t <= 4? (ValueType)t: ValueType.None;
            }

            public MapEntry(uint k, Flatmap.ValueType t, object v)
            {
                key = k; dataValue = v; dataType = t;
            }
        }


        public  const uint MapHeaderSignature   = 0xdeadcafe;
        public  const uint EntryHeaderSignature = 0xfeedface;

        public  const int  MapHeaderLength    = 24; // Size of map header struct
        public  const int  EntryHeaderLength  = 12; // Size of entry header struct
        public  const int  IndexEntryLength   = 8;  // Size of index entry struct

        public  const int  MapHeaderSigOffset = 4;  // Byte offset to signature 
        public  const int  MapLengthOffset    = 8;  // Byte offset to map length   

        protected static string BadMapMsg = "invalid flatmap header";
        protected static string BadNdxMsg = "invalid flatmap index";
   
        public class FlatmapException: ApplicationException
        {
            public FlatmapException(string msg): base(msg) { }
        }

    }   // class Flatmap

}       // namespace
