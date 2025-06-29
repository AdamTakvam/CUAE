/* $Id: FlatmapList.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Jan 25, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

import java.util.Collections;
import java.util.Comparator;
import java.util.Iterator;
import java.util.List;
import java.util.Vector;

/**
 * List of key / value pairs.
 */
public class FlatmapList // : ICollection, IDictionary, IEnumerable 
{
	/**
	 * Constructs an empty FlatmapList.
	 */
	public FlatmapList()
	{
		// nothing to do.
	}

	/**
	 * Constructs a read-only FlatmapList from a binary flatmap.
	 * @param flatmap
	 * @throws FlatmapException
	 */
	public FlatmapList( byte[] flatmap ) throws FlatmapException
	{
		Flatmap.fromFlatmap( flatmap, this );
		immutable = true;
	}

	/**
	 * Constructs a possibly modifiable FlatmapList from a binary flatmap.
	 * @param flatmap
	 * @param isModifiable
	 * @throws FlatmapException
	 */
	public FlatmapList( byte[] flatmap, boolean isModifiable )
			throws FlatmapException
	{
		Flatmap.fromFlatmap( flatmap, this );
		immutable = !isModifiable;
	}

	/**
	 * Converts this list to a binary flatmap with no header extension.
	 * @return the bytes of this flatmap.
	 * @throws FlatmapException
	 */
	public byte[] toFlatmap() throws FlatmapException
	{
		return toFlatmap( null );
	}

	/**
	 * Converts this list to a binary flatmap with the specified header
	 * extension.
	 * @param newHeaderExtension
	 * @return the bytes of this flatmap.
	 * @throws FlatmapException
	 */
	public byte[] toFlatmap( byte[] newHeaderExtension ) throws FlatmapException
	{
		return Flatmap.toFlatmap( this, newHeaderExtension );
	}

	/**
	 * Precalculate byte length of a flatmap created from current list.
	 * @param extraHeaderLength If a header extension is to be included 
	 * in the calculation, the length of that extra header; otherwise zero.
	 * @return length of the resulting flatmap
	 */
	public int binaryFlatmapLength( int extraHeaderLength )
	{
		return Flatmap.flatmapLengthFromList( this, extraHeaderLength );
	}

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// List maintenance 
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	/**
	 * Adds map entry for specified key and integer value.
	 * @param key
	 * @param nval
	 * @return this flatmap list
	 */
	public FlatmapList add( int key, int nval )
	{
		add( key, new Integer( nval ) );
		return this;
	}
	
	/**
	 * Adds map entry for specified key and long value.
	 * @param key
	 * @param val
	 * @return this flatmap list
	 */
	public FlatmapList add( int key, long val )
	{
		add( key, new Long( val ) );
		return this;
	}
	
	/**
	 * Adds map entry for specified key and double value.
	 * @param key
	 * @param val
	 * @return this flatmap list
	 */
	public FlatmapList add( int key, double val )
	{
		add( key, new Double( val ) );
		return this;
	}
	
	/**
	 * Adds map entry for specified key and object value.
	 * @param key
	 * @param xval
	 * @return this flatmap list
	 */
	public FlatmapList add( int key, Object xval )
	{
		checkMutable();

		int dataType = validateValue( xval );
		if (dataType == VT_NONE)
			throw new IllegalArgumentException( "unrecognized value type" );

		MapEntry entry = new MapEntry( key, dataType, xval );
		list.add( entry );
		sorted = false;
		return this;
	}
	
	/**
	 * Determines flatmap type of supplied value Object.
	 * @param xval
	 * @return flatmap type of supplied value Object. VT_NONE if
	 * the object type is not supported.
	 */
	public static int validateValue( Object xval )
	{
		int type = VT_NONE;

		if (xval instanceof Integer)
			type = VT_INT;
		else if (xval instanceof String)
			type = VT_STRING;
		else if (xval instanceof Long)
			type = VT_LONG;
		else if (xval instanceof Double)
			type = VT_DOUBLE;
		else if (xval instanceof FlatmapList)
			type = VT_FLATMAP;
		else if (xval instanceof byte[])
			type = Flatmap.isFlatmap( (byte[]) xval ) ? VT_FLATMAP : VT_BYTE;

		return type;
	}

	private void checkMutable()
	{
		if (immutable)
			throw new UnsupportedOperationException( "immutable" );
	}

	/**
	 * Adds map entry for specified key and value.
	 * @param xkey
	 * @param xval
	 * @return this flatmap list
	 */
	public FlatmapList add( Object xkey, Object xval )
	{
		add( obj2Int( xkey ), xval );
		return this;
	}

	/**
	 * Removes first map entry for specified key.
	 * @param key
	 */
	public void remove( int key )
	{
		remove( key, 1 );
	}

	/**
	 * Removes first map entry for specified key.
	 * @param xkey
	 */
	public void remove( Object xkey )
	{
		remove( obj2Int( xkey ) );
	}

	/**
	 * Removes map entry at specified index.
	 * @param index
	 */
	public void removeAt( int index )
	{
		checkMutable();

		MapEntry entry = getAt( index );
		if (entry.dataType == VT_NONE)
			throw new IndexOutOfBoundsException();

		list.remove( entry );
		// sorted = false; remove from a sorted list leaves the list sorted
	}

	/**
	 * Removes map entry for specified key and occurrence.
	 * @param key
	 * @param occurrence
	 */
	public void remove( int key, int occurrence )
	{
		checkMutable();

		MapEntry entry = find( key, occurrence );
		if (entry.dataType == VT_NONE)
			throw new IndexOutOfBoundsException();

		list.remove( entry );
		// sorted = false; remove from a sorted list leaves the list sorted
	}

	/**
	 * Removes map entry for specified key, value, and occurrence.
	 * @param key
	 * @param xval
	 * @param occurrence
	 */
	public void remove( int key, Object xval, int occurrence )
	{
		checkMutable();

		MapEntry entry = find( key, xval, occurrence );
		if (entry.dataType == VT_NONE)
			throw new IndexOutOfBoundsException();

		list.remove( entry );
		// sorted = false; remove from a sorted list leaves the list sorted
	}

	/**
	 * Clears the entire list.
	 */
	public void clear()
	{
		checkMutable();
		list.clear();
		// sorted = false; a cleared list is sorted!
	}

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// List content query 
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	/**
	 * Indicates if list contains specified value.
	 * @param xval
	 * @return true if value exists for any key.
	 */
	public boolean contains( Object xval )
	{
		Iterator<MapEntry> i = list.iterator();
		while (i.hasNext())
		{
			MapEntry entry = i.next();
			if (entry.dataValue.equals( xval ))
				return true;
		}
		return false;
	}

	/**
	 * Indicates if list contains specified key.
	 * @param key
	 * @return true if key exists.
	 */
	public boolean containsKey( int key )
	{
		return containsKey( key, 1 );
	}

	/**
	 * Indicates if list contains specified occurrence of specified key.
	 * @param key
	 * @param occurrence
	 * @return true if key exists.
	 */
	public boolean containsKey( int key, int occurrence )
	{
		return find( key, occurrence ).dataType != VT_NONE;
	}

	/**
	 * Indicates if list contains occurrence of specified key and value.
	 * @param key
	 * @param xval
	 * @param occurrence
	 * @return true if key/value/occurrence exists.
	 */
	public boolean contains( int key, Object xval, int occurrence )
	{
		return find( key, xval, occurrence ).dataType != VT_NONE;
	}

	/**
	 * Returns map entry for specified key and occurrence.
	 * @param key
	 * @param occurrence
	 * @return the entry for the specified key/occurrence, or null.
	 */
	public MapEntry find( int key, int occurrence )
	{
		int occurrencesThisKey = 0;
		int desiredOccurrence = occurrence > 0 ? occurrence : 1;
		sort();

		Iterator<MapEntry> i = list.iterator();
		while (i.hasNext())
		{
			MapEntry entry = i.next();
			if (entry.key < key)
				continue;
			if (entry.key > key)
				break;
			if (++occurrencesThisKey < desiredOccurrence)
				continue;
			return entry;
		}

		return null;
	}

	/**
	 * Returns map entry for specified key, value, and occurrence.
	 * @param key
	 * @param xval
	 * @param occurrence
	 * @return the entry for the specified key/value/occurrence, or null.
	 */
	public MapEntry find( int key, Object xval, int occurrence )
	{
		int occurrencesThisKey = 0;
		int desiredOccurrence = occurrence > 0 ? occurrence : 1;
		sort();

		Iterator<MapEntry> i = list.iterator();
		while (i.hasNext())
		{
			MapEntry entry = i.next();
			if (entry.key < key)
				continue;
			if (entry.key > key)
				break;
			if (!entry.dataValue.equals( xval ))
				continue;
			if (++occurrencesThisKey < desiredOccurrence)
				continue;
			return entry;
		}

		return null;
	}

	/**
	 * Returns map entry at specified index.
	 * @param index
	 * @return the entry for the specified index, or null.
	 */
	public MapEntry getAt( int index )
	{
		sort();
		
		Iterator<MapEntry> i = list.iterator();
		int j = 0;
		while (i.hasNext())
		{
			MapEntry entry = i.next();
			if (j++ != index)
				continue;
			return entry;
		}

		return null;
	}

	@Override
	public String toString()
	{
		sort();
		return list.toString();
	}

	/**
	 * A FlatmapList map entry.
	 */
	public static class MapEntry
	{
		/**
		 * The key of the entry.
		 */
		public final int key;
	
		/**
		 * The data type of the entry.
		 */
		public final int dataType;
	
		/**
		 * The value of the entry.
		 */
		public final Object dataValue;
	
		/**
		 * Constructs a dummy map entry.
		 */
		public MapEntry()
		{
			this( 0, VT_NONE, null );
		}
		
		/**
		 * Constructs a map entry.
		 * 
		 * @param key
		 * 
		 * @param dataType
		 * 
		 * @param dataValue
		 */
		public MapEntry( int key, int dataType, Object dataValue )
		{
			this.key = key;
			this.dataType = dataType;
			this.dataValue = dataValue;
		}

		@Override
		public String toString()
		{
			return ""+key+"("+dataType2String(dataType)+")="+dataValue;
		}

		/**
		 * @return the string value of this map entry.
		 */
		public String stringValue()
		{
			return (String) dataValue;
		}

		/**
		 * @return the integer value of this map entry.
		 */
		public Integer integerValue()
		{
			return (Integer) dataValue;
		}

		/**
		 * @return the flat map list value of this map entry.
		 */
		public FlatmapList flatmapListValue()
		{
			return (FlatmapList) dataValue;
		}
	}

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// List sort 
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	/**
	 * Comparator for map entries.
	 */
	public static class MapEntryComparer implements Comparator<MapEntry>
	{
		public int compare( MapEntry a, MapEntry b )
		{
			if (a.key < b.key)
				return -1;
			if (a.key > b.key)
				return 1;
			return 0;
		}
	}

	/**
	 * Sorts the list if not currently sorted.
	 */
	void sort()
	{
		if (!sorted)
			Collections.sort( list, new MapEntryComparer() );
		sorted = true;
	}

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// Support methods
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	private int obj2Int( Object obj )
	{
		return ((Integer) obj).intValue();
	}

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// Properties
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	private List<MapEntry> list = new Vector<MapEntry>();

	private boolean immutable;
	
	private boolean sorted;
	
	private byte[] headerExtension;

	/**
	 * Returns a name for a given data type.
	 * 
	 * @param dataType a data type.
	 * 
	 * @return the data type's name.
	 */
	public static String dataType2String( int dataType )
	{
		switch (dataType)
		{
			case VT_NONE:
				return "None";
			case VT_STRING:
				return "String";
			case VT_BYTE:
				return "Byte";
			case VT_FLATMAP:
				return "Flatmap";
			case VT_INT:
				return "Int";
			case VT_LONG:
				return "Long";
			case VT_DOUBLE:
				return "Double";
			default:
				return "Unknown";
		}
	}

	/**
	 * Minimum defined data type code.
	 */
	public static final int VT_MIN = 0;

	/**
	 * Data type code for an unknown value type (0).
	 */
	public static final int VT_NONE = 0;

	/**
	 * Data type code for a 32-bit integer (1).
	 */
	public static final int VT_INT = 1;

	/**
	 * Data type code for a byte array (2).
	 */
	public static final int VT_BYTE = 2;

	/**
	 * Data type code for a string (3).
	 */
	public static final int VT_STRING = 3;

	/**
	 * Data type code for a flatmap (4).
	 */
	public static final int VT_FLATMAP = 4;
	
	/**
	 * Data type code for a 64-bit integer (5).
	 */
	public static final int VT_LONG = 5;
	
	/**
	 * Data type code for a 64-bit IEEE floating (6).
	 */
	public static final int VT_DOUBLE = 6;

	/**
	 * Maximum defined data type code.
	 */
	public static final int VT_MAX = 6;
	
	/**
	 * Returns an iterator over all the list entries.
	 * @return an iterator over all the list entries.
	 */
	public Iterator<MapEntry> iterator()
	{
		return list.iterator();
	}

	/**
	 * Counts the number of map entries altogether.
	 * 
	 * @return the number of map entries altogether.
	 */
	public int count()
	{
		return list.size();
	}

	/**
	 * Returns the headerExtension.
	 * 
	 * @return returns the headerExtension.
	 */
	public byte[] getHeaderExtension()
	{
		return headerExtension;
	}

	/**
	 * Sets the headerExtension.
	 * 
	 * @param headerExtension The headerExtension to set.
	 */
	public void setHeaderExtension( byte[] headerExtension )
	{
		this.headerExtension = headerExtension;
	}

	/**
	 * Gets the map entries for the specified key.
	 * 
	 * @param key
	 * 
	 * @return an iterator over the map entries for the
	 * specified key.
	 */
	public Iterator<MapEntry> getValues( int key )
	{
		List<MapEntry> v = new Vector<MapEntry>();
		Iterator<MapEntry> i = list.iterator();
		while (i.hasNext())
		{
			MapEntry me = i.next();
			if (me.key == key)
				v.add( me );
		}
		return v.iterator();
	}

	/**
	 * Gets the first string value of the specified key.
	 * @param key
	 * @return the specified string value or null.
	 */
	public String getString( int key )
	{
		return getString( key, 1 );
	}

	/**
	 * Gets the string value of the specified key/occurrence.
	 * @param key
	 * @param occurrence
	 * @return the specified string value or null.
	 */
	public String getString( int key, int occurrence )
	{
		MapEntry me = find( key, occurrence );
		if (me == null)
			return null;
		return me.stringValue();
	}

	/**
	 * Gets the first int value of the specified key.
	 * @param key
	 * @return the specified int value or null.
	 */
	public Integer getInteger( int key )
	{
		return getInteger( key, 1 );
	}

	/**
	 * Gets the int value of the specified key/occurrence.
	 * @param key
	 * @param occurrence
	 * @return the specified int value or null.
	 */
	public Integer getInteger( int key, int occurrence )
	{
		MapEntry me = find( key, occurrence );
		if (me == null)
			return null;
		return me.integerValue();
	}

	/**
	 * Gets the first int value of the specified key.
	 * @param key
	 * @return the specified int value or null.
	 */
	public FlatmapList getFlatmapList( int key )
	{
		return getFlatmapList( key, 1 );
	}

	/**
	 * Gets the int value of the specified key/occurrence.
	 * @param key
	 * @param occurrence
	 * @return the specified int value or null.
	 */
	public FlatmapList getFlatmapList( int key, int occurrence )
	{
		MapEntry me = find( key, occurrence );
		if (me == null)
			return null;
		return me.flatmapListValue();
	}

	/**
	 * @param key
	 * @param dfltValue
	 * @return the value of the key, or the default value if key has no
	 * value.
	 */
	public int getInt( int key, int dfltValue )
	{
		Integer i = getInteger( key );
		if (i == null)
			return dfltValue;
		return i.intValue();
	}
}
