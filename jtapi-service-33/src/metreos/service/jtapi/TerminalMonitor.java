/* $Id: TerminalMonitor.java 7119 2005-06-10 19:54:11Z wert $
 *
 * Created by wert on Feb 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import javax.telephony.Terminal;
import javax.telephony.TerminalObserver;

import metreos.core.ipc.flatmaps.FlatmapIpcMessage;

/**
 * blah.
 */
public interface TerminalMonitor extends TerminalObserver
{
	/**
	 * @param monitor
	 * @param active
	 */
	public void addCallMonitor( CallMonitor monitor, boolean active );

	/**
	 * @param monitor
	 */
	public void removeCallMonitor( CallMonitor monitor );

	/**
	 * @return the terminal being monitored.
	 */
	public Terminal getTerminal();

	/**
	 * @param messageType
	 * @return a new flatmap ipc message
	 */
	public FlatmapIpcMessage newMessage( int messageType );
}
