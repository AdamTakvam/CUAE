/* $Id: TerminalMonitor.java 11571 2005-10-07 16:12:41Z wert $
 *
 * Created by wert on Feb 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import javax.telephony.Terminal;

import metreos.core.ipc.flatmaps.FlatmapIpcMessage;

import com.cisco.jtapi.extensions.CiscoRTPHandle;
import com.cisco.jtapi.extensions.CiscoRTPParams;
import com.cisco.jtapi.extensions.CiscoSynchronousObserver;
import com.cisco.jtapi.extensions.CiscoTerminalObserver;

/**
 * blah.
 */
public interface TerminalMonitor extends CiscoTerminalObserver, CiscoSynchronousObserver
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
	 * @return the name of the terminal being monitored.
	 */
	public String getTerminalName();

	/**
	 * @param messageType
	 * @return a new flatmap ipc message
	 */
	public FlatmapIpcMessage newMessage( int messageType );
	
	/**
	 * @param handle
	 * @param params
	 * @return true if it worked, false if it failed
	 * @throws Exception
	 */
	public boolean setRTPParams( CiscoRTPHandle handle, CiscoRTPParams params )
		throws Exception;
}
