/* $Id: BasicIpcMessage.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Jan 27, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.ipc;

/**
 * An IPC message.
 */
public class BasicIpcMessage implements IpcMessage
{
	/**
	 * Constructs an IPC message.
	 * 
	 * @param flag an 8-bit flag
	 * 
	 * @param flag2 a 32-bit flag.
	 * 
	 * @param bytes the bytes of the message.
	 */
	public BasicIpcMessage( int flag, int flag2, byte[] bytes )
	{
		this.flag = flag;
		this.flag2 = flag2;
		this.bytes = bytes;
	}
	
	/**
	 * An 8-bit flag.
	 */
	public final int flag;
	
	/**
	 * A 32-bit flag.
	 */
	public final int flag2;
	
	/**
	 * The bytes of the message.
	 */
	public final byte[] bytes;

	@Override
	public String toString()
	{
		StringBuffer sb = new StringBuffer();
		
		sb.append( "flag=0x" );
		sb.append( Integer.toString( flag, 16 ) );
		sb.append( ", flag2=0x" );
		sb.append( Integer.toString( flag2, 16 ) );
		sb.append( ", length=" );
		sb.append( bytes.length );
		sb.append( ", bytes=" );
		int n = bytes.length;
		for (int i = 0; i < n; i++)
		{
			sb.append( " " );
			sb.append( bytes[i] & 255 );
		}
		
		return sb.toString();
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcMessage#getFlag()
	 */
	public int getFlag()
	{
		return flag;
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcMessage#getFlag2()
	 */
	public int getFlag2()
	{
		return flag2;
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcMessage#getBytes()
	 */
	public byte[] getBytes()
	{
		return bytes;
	}
}
