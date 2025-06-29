/* $Id: IpcMessage.java 4949 2005-01-28 17:15:10Z wert $
 *
 * Created by wert on Jan 28, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.ipc;

/**
 * Interface for ipc messages.
 */
public interface IpcMessage
{
	/**
	 * Gets the 8-bit flag.
	 * @return the 8-bit flag.
	 */
	public int getFlag();
	
	/**
	 * Gets the 32-bit flag2;
	 * @return the 32-bit flag2;
	 */
	public int getFlag2();
	
	/**
	 * Gets the bytes.
	 * @return the bytes.
	 */
	public byte[] getBytes();
}
