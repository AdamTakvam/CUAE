/* $Id: Flatmap.java 19261 2006-01-12 17:21:45Z wert $
 *
 * Created by wert on Jan 25, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.Iterator;

import metreos.core.ipc.flatmaps.FlatmapList.MapEntry;
import metreos.util.Assertion;
import metreos.util.Convert;

/**
 * Static methods and structures for binary flatmap manipulation.
 */
abstract public class Flatmap
{
	private static final String UNKNOWN_DATA_TYPE_MSG = "unknown data type";

	/**
	 * Converts a FlatmapList to a binary flatmap.
	 *
	 * @param list
	 * @return the bytes rep of a FlatmapList
	 * @throws FlatmapException
	 */
	public static byte[] toFlatmap( FlatmapList list ) throws FlatmapException
	{
		return toFlatmap( list, null );
	}

	/**
	 * Converts a FlatmapList to a binary flatmap.
	 *
	 * @param list
	 * @param headerExtension
	 * @return the bytes rep of a FlatmapList
	 * @throws FlatmapException
	 */
	public static byte[] toFlatmap( FlatmapList list, byte[] headerExtension )
			throws FlatmapException
	{
		try
		{
			return toFlatmap0( list, headerExtension );
		}
		catch ( Exception x )
		{
			throw new FlatmapException( x.getMessage(), x );
		}
	}

	/**
	 * Converts a binary flatmap to a FlatmapList.
	 *
	 * @param flatmap
	 * @return the FlatmapList of the bytes
	 * @throws FlatmapException
	 */
	public static FlatmapList fromFlatmap( byte[] flatmap )
			throws FlatmapException
	{
		return fromFlatmap( flatmap, null );
	}

	/**
	 * Convert a binary flatmap to a FlatmapList.
	 *
	 * @param flatmap
	 * @param fl
	 * @return the FlatmapList of the bytes
	 * @throws FlatmapException
	 */
	public static FlatmapList fromFlatmap( byte[] flatmap, FlatmapList fl )
			throws FlatmapException
	{
		if (!isFlatmap( flatmap ))
			throw new FlatmapException( BAD_MAP_MSG );

		try
		{
			return fromFlatmap0( flatmap, fl );
		}
		catch ( Exception x )
		{
			throw new FlatmapException( x.getMessage(), x );
		}
	}

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// ToFlatmap support
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	/**
	 * Convert FlatmapList collection to binary flatmap.
	 * @param list 
	 * @param extraHeader 
	 * @return byte array representation of flatmap
	 * @throws FlatmapException 
	 * @throws IOException 
	 */
	private static byte[] toFlatmap0( FlatmapList list, byte[] extraHeader )
			throws FlatmapException, IOException
	{
		ByteArrayOutputStream indexStream = new ByteArrayOutputStream();
		ByteArrayOutputStream bodyStream = new ByteArrayOutputStream();

		IndexEntry indexI = new IndexEntry();
		MapEntryHeader entryI = new MapEntryHeader();

		list.sort();
		Iterator<MapEntry> entries = list.iterator();
		while (entries.hasNext())
		{
			// Write index slot
			
			MapEntry entry = entries.next();
			indexI.set( entry.key, bodyStream.size() );
			indexStream.write( indexI.toArray(), 0, IndexEntry.LENGTH );

			// Write value header and entry to body
			
			int dataType = entry.dataType;
			switch (dataType)
			{
				case FlatmapList.VT_INT:
				{
					int nval = ((Integer) entry.dataValue).intValue();
					entryI.set( dataType, 4 );
					bodyStream.write( entryI.toArray(), 0, MapEntryHeader.LENGTH );
					byte[] bytes = new byte[4];
					Convert.putInt( nval, bytes, 0 );
					bodyStream.write( bytes );
					break;
				}

				case FlatmapList.VT_STRING:
				{
					String sval = (String) entry.dataValue;
					byte[] bval = stringToCstring( sval );
					entryI.set( dataType, bval.length );
					bodyStream.write( entryI.toArray(), 0, MapEntryHeader.LENGTH );
					bodyStream.write( bval, 0, bval.length );
					break;
				}

				case FlatmapList.VT_LONG:
				{					
					long lval = ((Long) entry.dataValue).longValue();
					entryI.set( dataType, 8 );
					bodyStream.write( entryI.toArray(), 0, MapEntryHeader.LENGTH );
					byte[] bytes = new byte[8];
					Convert.putLong( lval, bytes, 0 );
					bodyStream.write( bytes );
					break;
				}
				
				case FlatmapList.VT_DOUBLE:
				{					
					double dval = ((Double) entry.dataValue).doubleValue();
					entryI.set( dataType, 8 );
					bodyStream.write( entryI.toArray(), 0, MapEntryHeader.LENGTH );
					byte[] bytes = new byte[8];
					Convert.putDouble( dval, bytes, 0 );
					bodyStream.write( bytes );
					break;
				}
				
				case FlatmapList.VT_BYTE:
				{					
					byte[] bval = (byte[]) entry.dataValue;
					entryI.set( dataType, bval.length );
					bodyStream.write( entryI.toArray(), 0, MapEntryHeader.LENGTH );
					bodyStream.write( bval, 0, bval.length );
					break;
				}
				
				case FlatmapList.VT_FLATMAP:
				{
					if (entry.dataValue instanceof FlatmapList)
					{
						FlatmapList fml = (FlatmapList) entry.dataValue;
						byte[] bval = fml.toFlatmap( null );
						entryI.set( dataType, bval.length );
						bodyStream.write( entryI.toArray(), 0, MapEntryHeader.LENGTH );
						bodyStream.write( bval, 0, bval.length );
					}
					else
					{
						byte[] bval = (byte[]) entry.dataValue;
						entryI.set( dataType, bval.length );
						bodyStream.write( entryI.toArray(), 0, MapEntryHeader.LENGTH );
						bodyStream.write( bval, 0, bval.length );
					}
					break;
				}
			}
		}
		
		// Populate map header
		MapHeader mapheader = new MapHeader( list.count() );
		int extraLength = extraHeader == null ? 0 : extraHeader.length;
		mapheader.length += extraLength;
		int indexSize = mapheader.length+indexStream.size();
		mapheader.bodyOffset = indexSize;
		mapheader.bodyLength = bodyStream.size();
		mapheader.mapLength = (indexSize+mapheader.bodyLength);
		// Write header, index, and body to map
		ByteArrayOutputStream mapStream = new ByteArrayOutputStream();
		mapStream.write( mapheader.toArray(), 0, MapHeader.LENGTH );
		if (extraLength > 0)
			mapStream.write( extraHeader, 0, extraLength );
		indexStream.writeTo( mapStream );
		indexStream.close();
		bodyStream.writeTo( mapStream );
		bodyStream.close();

		byte[] flatmap = mapStream.toByteArray();
		mapStream.close();

		return flatmap;
	}

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// FromFlatmap support
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	/// <summary>Convert prevalidated flatmap bytes to FlatmapList</summary>
	/// <exception cref="FlatmapException">If map is invalid</exception>
	private static FlatmapList fromFlatmap0( byte[] bytes, FlatmapList fl )
			throws FlatmapException
	{
		MapHeader mapHeader = new MapHeader( bytes, 0 );
//		System.out.println( "mapHeader.length = "+mapHeader.length );
//		System.out.println( "mapHeader.sig = "+mapHeader.sig );
//		System.out.println( "mapHeader.mapLength = "+mapHeader.mapLength );
//		System.out.println( "mapHeader.count = "+mapHeader.count );
//		System.out.println( "mapHeader.bodyOffset = "+mapHeader.bodyOffset );
//		System.out.println( "mapHeader.bodyLength = "+mapHeader.bodyLength );
		mapHeader.validate( bytes.length );
		
		IndexEntry indexI = new IndexEntry();
		MapEntryHeader entryI = new MapEntryHeader();

		FlatmapList list = fl == null ? new FlatmapList() : fl;
		// Store custom header if any
		list.setHeaderExtension( getExtension( bytes, mapHeader ) );
		// Point at first index entry
		int currIndexOffset = mapHeader.length;

		// For each index entry ...
		for (int i = 0; i < mapHeader.count; i++, currIndexOffset += IndexEntry.LENGTH)
		{
//			System.out.println( "currIndexOffset = "+currIndexOffset );
			
			// Interpret index slot
			indexI.set( bytes, currIndexOffset );
//			System.out.println( "indexI.key = "+indexI.key );
//			System.out.println( "indexI.offset = "+indexI.offset );
			indexI.validate( mapHeader );
			
			// Point at data item header
			int currDataOffset = mapHeader.bodyOffset+indexI.offset;
//			System.out.println( "currDataOffset = "+currDataOffset );
			
			// Interpret data item header
			entryI.set( bytes, currDataOffset );
//			System.out.println( "entryI.dataType = "+entryI.dataType );
//			System.out.println( "entryI.dataLength = "+entryI.dataLength );
//			System.out.println( "entryI.sig = "+entryI.sig );
			entryI.validate( mapHeader );
			
			// Bump past data item header
			currDataOffset += MapEntryHeader.LENGTH;
			
			// Interpret data type
			switch (entryI.dataType)
			{
				case FlatmapList.VT_INT:
				{
					Assertion.check( entryI.dataLength == 4, "entryI.dataLength == 4" );
					int nval = Convert.getInt( bytes, currDataOffset );
					list.add( indexI.key, nval );
					break;
				}

				case FlatmapList.VT_STRING:
				{
					String sval = cstringToString( bytes, currDataOffset,
							entryI.dataLength );
					list.add( indexI.key, sval );
					break;
				}
								
				case FlatmapList.VT_LONG:
				{
					Assertion.check( entryI.dataLength == 8, "entryI.dataLength == 8" );
					long lval = Convert.getLong( bytes, currDataOffset );
					list.add( indexI.key, lval );
					break;
				}
				
				case FlatmapList.VT_DOUBLE:
				{
					Assertion.check( entryI.dataLength == 8, "entryI.dataLength == 8" );
					double dval = Convert.getDouble( bytes, currDataOffset );
					list.add( indexI.key, dval );
					break;
				}

				case FlatmapList.VT_BYTE:
				{
					byte[] bval = new byte[entryI.dataLength];
					System.arraycopy( bytes, currDataOffset, bval, 0,
							bval.length );
					list.add( indexI.key, bval );
					break;
				}
				
				case FlatmapList.VT_FLATMAP:
				{
					byte[] bval = new byte[entryI.dataLength];
					System.arraycopy( bytes, currDataOffset, bval, 0,
							bval.length );
					FlatmapList fml = new FlatmapList( bval );
					list.add( indexI.key, fml );
					break;
				}

				default:
					throw new FlatmapException( UNKNOWN_DATA_TYPE_MSG );
			}
		}

		return list;
	}

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// Public utilities
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	/**
	 * Precalculates byte length of a flatmap created from specified list.
	 * @param list
	 * @param extraHeaderLength
	 * @return calculated length.
	 */
	public static int flatmapLengthFromList( FlatmapList list,
			int extraHeaderLength )
	{
		int hdrlength = MapHeader.LENGTH+extraHeaderLength;
		int count = list.count();
		int indexLength = count * IndexEntry.LENGTH;
		int bodyLength = count * MapEntryHeader.LENGTH;

		Iterator<MapEntry> i = list.iterator();
		while (i.hasNext())
		{
			MapEntry entry = i.next();
			switch (entry.dataType)
			{
				case FlatmapList.VT_INT:
					bodyLength += 4;
					break;
					
				case FlatmapList.VT_LONG:
				case FlatmapList.VT_DOUBLE:
					bodyLength += 8;
					break;

				case FlatmapList.VT_STRING:
					bodyLength += ((String) entry.dataValue).length()+1;
					break;

				case FlatmapList.VT_BYTE:
					bodyLength += ((byte[]) entry.dataValue).length;
					break;
				
				case FlatmapList.VT_FLATMAP:
					if (entry.dataValue instanceof FlatmapList)
					{
						FlatmapList fml = (FlatmapList) entry.dataValue;
						bodyLength += flatmapLengthFromList( fml, 0 );
					}
					else
					{
						bodyLength += ((byte[]) entry.dataValue).length;
					}
					break;
			}
		}

		return hdrlength+indexLength+bodyLength;
	}

	/**
	 * Extracts flatmap header from binary flatmap.
	 * @param flatmap
	 * @return map header.
	 */
	public static MapHeader getFlatmapHeader( byte[] flatmap )
	{
		return isFlatmap( flatmap ) ? new MapHeader(
				flatmap, 0 ) : null;
	}

	/**
	 * Extracts flatmap header extension if any.
	 * @param flatmap
	 * @return header extension bytes.
	 */
//	public static byte[] getFlatmapHeaderExtension( byte[] flatmap )
//	{
//		return isFlatmap( flatmap )
//			? getExtension( flatmap ) : null;
//	}

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// General support
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


	/**
	 * Determines if byte array looks like a flatmap.
	 * @param bytes
	 * @return true if it looks like a flatmap, false otherwise.
	 */
	public static boolean isFlatmap( byte[] bytes )
	{
		return isFlatmap( bytes, 0, bytes.length );
	}

	/**
	 * Indicates if byte blob at buffer offset looks like a flatmap.
	 * @param buf
	 * @param offset
	 * @param indicatedMapLength
	 * @return true if byte array looks like a flatmap, false otherwise.
	 */
	public static boolean isFlatmap( byte[] buf, int offset,
			int indicatedMapLength )
	{
//		System.out.println( "isFlatmap: checking indicatedMapLength" );

//		System.out.println( "indicatedMapLength = "+indicatedMapLength );
		if (indicatedMapLength < Flatmap.MapHeader.LENGTH)
			return false;

//		System.out.println( "isFlatmap: checking calculatedMapLength" );
		
		int calculatedMapLength = buf.length - offset;
//		System.out.println( "calculatedMapLength = "+calculatedMapLength );
		if (calculatedMapLength < Flatmap.MapHeader.LENGTH)
			return false;
		
//		System.out.println( "isFlatmap: checking actualMapLength" );

		// Check presumed header length against blob size
		int actualMapLength = Convert.getInt( buf, offset+MapHeader.LEN_OFFSET );
//		System.out.println( "actualMapLength = "+actualMapLength );
		if (actualMapLength != calculatedMapLength)
			return false;

//		System.out.println( "isFlatmap: checking signature" );
		
		// Determine if blob contains a flatmap signature in expected position
		int signature = Convert.getInt( buf, offset+MapHeader.SIG_OFFSET );
//		System.out.println( "signature = "+signature );
		return (signature == MapHeader.SIGNATURE);
	}

	/**
	 * Extracts a flatmap from specified buffer at specified offset.
	 * @param buf
	 * @param offset
	 * @param indicatedMapLength
	 * @return extracted bytes
	 * @throws FlatmapException
	 */
	public static byte[] getFlatmap( byte[] buf, int offset,
			int indicatedMapLength ) throws FlatmapException
	{
		byte[] flatmap = null;
		boolean result = false;

		try
		{
			if (isFlatmap( buf, offset, indicatedMapLength ))
			{
				flatmap = new byte[buf.length - offset];
				System.arraycopy( buf, offset, flatmap, 0, indicatedMapLength );
				result = true;
			}
		}
		catch ( Exception e )
		{
			throw new FlatmapException( BAD_MAP_MSG, e );
		}
		
		if (!result)
			throw new FlatmapException( BAD_MAP_MSG );

		return flatmap;
	}

	/**
	 * Converts a String to an 8-bit ascii C String.
	 *
	 * @param s a String
	 *
	 * @return an 8-bit ascii C String.
	 */
	public static byte[] stringToCstring( String s )
	{
		byte[] sbytes = s.getBytes();
		int n = sbytes.length;
		byte[] bytes = new byte[n+1];
		System.arraycopy( sbytes, 0, bytes, 0, n );
		bytes[n] = 0;
		return bytes;
	}

	/**
	 * Converts an 8-bit ascii C String to a String.
	 *
	 * @param bytes an 8-bit ascii C String.
	 *
	 * @return a String.
	 */
	public static String cstringToString( byte[] bytes )
	{
		return cstringToString( bytes, 0, bytes.length );
	}

	/**
	 * Converts an 8-bit ascii C String to a String.
	 *
	 * @param bytes an 8-bit ascii C String.
	 *
	 * @param offset the offset in bytes of the start of the string.
	 *
	 * @param length value passed is total length including null term.
	 *
	 * @return a String.
	 */
	public static String cstringToString( byte[] bytes, int offset, int length )
	{
		Assertion.check( length > 0, "length > 0" );
		Assertion.check( bytes[offset+length - 1] == 0,
				"byte[offset+length-1]" );
		return new String( bytes, offset, length - 1 );
	}

	/**
	 * Extracts and return flatmap header extension if any.
	 *
	 * @param flatmap the bytes of the flatmap.
	 * @param header the header of the map
	 *
	 * @return the bytes of the header extension, or null if there is none.
	 */
	private static byte[] getExtension( byte[] flatmap, MapHeader header )
	{
		int extLength = header.length - MapHeader.LENGTH;
		if (extLength < 1)
			return null;

		byte[] extension = new byte[extLength];
		System.arraycopy( flatmap, MapHeader.LENGTH, extension, 0, extLength );
		return extension;
	}

	/**
	 * Header struct for flatmap.
	 */
	public static class MapHeader
	{
		/**
		 * Header length including any extension.
		 */
		public int length;

		/**
		 * Header signature.
		 */
		public final int sig;

		/**
		 * Total length of flatmap.
		 */
		public int mapLength;

		/**
		 * Number of map entries.
		 */
		public final int count;

		/**
		 * Offset to data block.
		 */
		public int bodyOffset;

		/**
		 * Length of data block.
		 */
		public int bodyLength;

		/**
		 * Constructs a map header with the specified number of map entries.
		 *
		 * @param count the number of map entries.
		 */
		public MapHeader( int count )
		{
			this.count = count;
			length = LENGTH;
			sig = SIGNATURE;
		}

		/**
		 * Validates the map header.
		 * 
		 * @param bytesLen the actual length of the flatmap bytes.
		 * 
		 * @throws FlatmapException if there is a problem validating.
		 */
		public void validate( int bytesLen ) throws FlatmapException
		{
			if (sig != SIGNATURE)
				throw new FlatmapException( BAD_MAP_HEADER_SIG_MSG );
			
			if (length < LENGTH)
				throw new FlatmapException( BAD_MAP_HEADER_LEN_MSG );
			
			if (mapLength != bytesLen)
				throw new FlatmapException( BAD_MAP_HEADER_MAPLEN_MSG );
			
			if (count < 0)
				throw new FlatmapException( BAD_MAP_HEADER_COUNT_MSG );
			
			int indexLen = count * IndexEntry.LENGTH;
			
			if (bodyOffset != length + indexLen)
				throw new FlatmapException( BAD_MAP_HEADER_BODY_OFFSET_MSG );
			
			if (bodyLength != bytesLen - bodyOffset)
				throw new FlatmapException( BAD_MAP_HEADER_BODY_LENGTH_MSG );
		}

		/**
		 * Constructs a map header from the specified bytes and offset.
		 *
		 * @param bytes the bytes of the flatmap.
		 *
		 * @param offset the offset to the first byte of the flat map.
		 */
		public MapHeader( byte[] bytes, int offset )
		{
			length = Convert.getInt( bytes, offset );
			sig = Convert.getInt( bytes, offset+4 );
			mapLength = Convert.getInt( bytes, offset+8 );
			count = Convert.getInt( bytes, offset+12 );
			bodyOffset = Convert.getInt( bytes, offset+16 );
			bodyLength = Convert.getInt( bytes, offset+20 );
		}

		/**
		 * Converts a map header into bytes.
		 *
		 * @return the bytes of the map header
		 */
		public byte[] toArray()
		{
			byte[] bytes = new byte[LENGTH];
			Convert.putInt( length, bytes, 0 );
			Convert.putInt( sig, bytes, 4 );
			Convert.putInt( mapLength, bytes, 8 );
			Convert.putInt( count, bytes, 12 );
			Convert.putInt( bodyOffset, bytes, 16 );
			Convert.putInt( bodyLength, bytes, 20 );
			return bytes;
		}

		/**
		 * The signature of a map header.
		 */
		public final static int SIGNATURE = 0xdeadcafe;

		/**
		 * Size of map header struct.
		 */
		public final static int LENGTH = 24;

		/**
		 * Byte offset to signature.
		 */
		public final static int SIG_OFFSET = 4;

		/**
		 * Byte offset to map length.
		 */
		public final static int LEN_OFFSET = 8;

		private static final String BAD_MAP_HEADER_SIG_MSG = "bad map header sig";

		private static final String BAD_MAP_HEADER_LEN_MSG = "bad map header len";

		private static final String BAD_MAP_HEADER_MAPLEN_MSG = "bad map header map len";

		private static final String BAD_MAP_HEADER_COUNT_MSG = "bad map header count";

		private static final String BAD_MAP_HEADER_BODY_OFFSET_MSG = "bad map header body offset";

		private static final String BAD_MAP_HEADER_BODY_LENGTH_MSG = "bad map header body length";
	}

	/**
	 * Struct for a map entry header.
	 */
	public static class MapEntryHeader
	{
		/**
		 * Data type of map entry.
		 */
		public int dataType;

		/**
		 * Length of map entry data.
		 */
		public int dataLength;

		/**
		 * Signature of map entry header.
		 */
		public int sig;

		/**
		 * Constructs a new map entry header.
		 */
		public MapEntryHeader()
		{
			dataType = FlatmapList.VT_NONE;
			dataLength = 0;
			sig = 0;
		}

		/**
		 * Constructs a new map entry header.
		 *
		 * @param bytes the bytes of the flat map.
		 *
		 * @param offset the offset of this map entry header.
		 */
		public MapEntryHeader( byte[] bytes, int offset )
		{
			dataType = Convert.getInt( bytes, offset );
			dataLength = Convert.getInt( bytes, offset+4 );
			sig = Convert.getInt( bytes, offset+8 );
		}

		/**
		 * Constructs a new map entry header.
		 *
		 * @param dataType the data type of the value.
		 *
		 * @param dataLength the length of the value.
		 */
		public MapEntryHeader( int dataType, int dataLength )
		{
			this.dataType = dataType;
			this.dataLength = dataLength;
			sig = SIGNATURE;
		}

		/**
		 * Sets the data type and data length.
		 *
		 * @param dataType the data type of the value.
		 *
		 * @param dataLength the length of the value.
		 */
		public void set( int dataType, int dataLength )
		{
			this.dataType = dataType;
			this.dataLength = dataLength;
			sig = SIGNATURE;
		}

		/**
		 * Sets the data type and data length.
		 *
		 * @param bytes the bytes of a flatmap.
		 *
		 * @param offset the offset of this map entry header.
		 */
		public void set( byte[] bytes, int offset )
		{
			dataType = Convert.getInt( bytes, offset );
			dataLength = Convert.getInt( bytes, offset+4 );
			sig = Convert.getInt( bytes, offset+8 );
		}

		/**
		 * Validates the map entry.
		 * 
		 * @param mapHeader the map header.
		 * 
		 * @throws FlatmapException if there is a problem with the map entry.
		 */
		public void validate( MapHeader mapHeader )
			throws FlatmapException
		{
			if (sig != MapEntryHeader.SIGNATURE)
				throw new FlatmapException( BAD_MAP_ENTRY_SIG_MSG );
			
			if (dataType < FlatmapList.VT_MIN || dataType > FlatmapList.VT_MAX)
				throw new FlatmapException( BAD_MAP_ENTRY_DATA_TYPE_MSG );
			
			if (dataLength < 0)
				throw new FlatmapException( BAD_MAP_ENTRY_LEN_MSG );
			
			int end = MapEntryHeader.LENGTH+dataLength;
//			System.out.println( "end = "+end );
			if (end > mapHeader.bodyLength)
				throw new FlatmapException( BAD_MAP_ENTRY_LEN_MSG );
		}

		/**
		 * Converts the map entry header to bytes.
		 *
		 * @return the bytes of the map entry header.
		 */
		public byte[] toArray()
		{
			byte[] bytes = new byte[LENGTH];
			Convert.putInt( dataType, bytes, 0 );
			Convert.putInt( dataLength, bytes, 4 );
			Convert.putInt( sig, bytes, 8 );
			return bytes;
		}

		/**
		 * Map entry header length.
		 */
		public final static int LENGTH = 12;

		/**
		 * Map entry header signature.
		 */
		public final static int SIGNATURE = 0xfeedface;

		private static final String BAD_MAP_ENTRY_SIG_MSG = "bad map entry sig";

		private static final String BAD_MAP_ENTRY_DATA_TYPE_MSG = "bad map entry data type";

		private static final String BAD_MAP_ENTRY_LEN_MSG = "bad map entry len";
	}

	/**
	 * Struct for an index entry.
	 */
	public static class IndexEntry
	{
		/**
		 * Item key.
		 */
		public int key;

		/**
		 * Data offset from start of body.
		 */
		public int offset;

		/**
		 * Constructs an index entry.
		 *
		 */
		public IndexEntry()
		{
			key = 0;
			offset = 0;
		}

		/**
		 * Constructs an index entry.
		 * 
		 * @param key item key.
		 * 
		 * @param offset data offset from start of body.
		 */
		public IndexEntry( int key, int offset )
		{
			this.key = key;
			this.offset = offset;
		}

		/**
		 * Sets an index entry.
		 * 
		 * @param key item key.
		 * 
		 * @param offset data offset from start of body.
		 */
		public void set( int key, int offset )
		{
			this.key = key;
			this.offset = offset;
		}

		/**
		 * Sets an index entry.
		 * 
		 * @param bytes the bytes of the flatmap.
		 * 
		 * @param offset the offset of this index entry
		 */
		public void set( byte[] bytes, int offset )
		{
			this.key = Convert.getInt( bytes, offset );
			this.offset = Convert.getInt( bytes, offset+4 );
		}

		/**
		 * Ensures index entry points within map bounds.
		 *
		 * @param mapHeader map header.
		 *
		 * @throws FlatmapException if index entry is invalid.
		 */
		public void validate( MapHeader mapHeader ) throws FlatmapException
		{
			if (offset < 0)
				throw new FlatmapException( BAD_INDEX_ENTRY_OFFSET_MSG );
			
			int end = offset+MapEntryHeader.LENGTH;
//			System.out.println( "end = "+end );
			if (end > mapHeader.bodyLength)
				throw new FlatmapException( BAD_INDEX_ENTRY_OFFSET_MSG );
		}

		/**
		 * Converts a index entry to bytes.
		 * 
		 * @return the bytes of the index entry.
		 */
		public byte[] toArray()
		{
			byte[] bytes = new byte[LENGTH];
			Convert.putInt( key, bytes, 0 );
			Convert.putInt( offset, bytes, 4 );
			return bytes;
		}

		/**
		 * The length of an index entry.
		 */
		public final static int LENGTH = 8;

		private static final String BAD_INDEX_ENTRY_OFFSET_MSG = "bad index entry offset";
	}

	/**
	 * Exception message describing a bad map.
	 */
	public final static String BAD_MAP_MSG = "error reading flatmap header";
}
