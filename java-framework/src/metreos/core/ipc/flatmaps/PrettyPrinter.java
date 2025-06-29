/* $Id: PrettyPrinter.java 5136 2005-02-09 22:40:03Z wert $
 *
 * Created by wert on Feb 9, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

/**
 * Pretty prints a flatmap ipc message.
 */
public interface PrettyPrinter
{
	/**
	 * Formats a message for easier reading.
	 * 
	 * @param message
	 * 
	 * @return the formatted message.
	 */
	public String format( FlatmapIpcMessage message );
}
