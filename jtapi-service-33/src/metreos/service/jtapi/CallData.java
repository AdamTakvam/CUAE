/* $Id: CallData.java 7113 2005-06-10 16:00:16Z wert $
 *
 * Created by wert on Feb 20, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

/**
 * Information about a made call which will be needed later
 * during call processing.
 * 
 * @author wert
 */
public class CallData
{
	/**
	 * Constructs the CallData.
	 *
	 * @param fromCallId the name of the "from" side of the call.
	 * @param toCallId the name of the "to" side of the call.
	 * @param rxIP the address to receive audio data.
	 * @param rxPort the port to receive audio data.
	 */
	public CallData( String fromCallId, String toCallId, String rxIP, int rxPort )
	{
		this.fromCallId = fromCallId;
		this.toCallId = toCallId;
		this.rxIP = rxIP;
		this.rxPort = rxPort;
	}
	
	/**
	 * The name of the "from" side of the call.
	 */
	public String fromCallId;
	
	/**
	 * The name of the "to" side of the call.
	 */
	public String toCallId;
	
	/**
	 * The address to receive audio data.
	 */
	public String rxIP;
	
	/**
	 * The port to receive audio data.
	 */
	public int rxPort;
}
