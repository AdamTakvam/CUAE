/* $Id: FlatmapHeaderExtension.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Jan 27, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

import metreos.util.Convert;

/**
 * Flatmap header extension for use with IPC.
 */
public class FlatmapHeaderExtension
{
	/**
	 * The length of the flatmap header extension.
	 */
	public static final int LENGTH = 4;

	/**
	 * Constructs a flatmap header extension specifying the message type.
	 * 
	 * @param messageType the ipc message type.
	 */
	public FlatmapHeaderExtension( int messageType )
	{
		this.messageType = messageType;
	}

	/**
	 * Constructs a flatmap header extension from the bytes.
	 * 
	 * @param bytes the bytes of the flatmap header extension.
	 */
	public FlatmapHeaderExtension( byte[] bytes )
	{
		this( Convert.getInt( bytes, 0 ) );
	}
	
	/**
	 * Get the bytes of the flatmap header extension.
	 * 
	 * @return an array of bytes.
	 */
	public byte[] toArray()
	{
		byte[] bytes = new byte[LENGTH];
		Convert.putInt( messageType, bytes, 0 );
		return bytes;
	}
	
	/**
	 * The ipc message type.
	 */
	public final int messageType;
}
