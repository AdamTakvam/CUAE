/* $Id: CtiPortDevice.java 30152 2007-03-06 21:47:50Z wert $
 *
 * Created by wert on Jan 31, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.net.InetAddress;
import java.util.List;

import javax.telephony.Terminal;

import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoMediaCapability;
import com.cisco.jtapi.extensions.CiscoMediaTerminal;
import com.cisco.jtapi.extensions.CiscoRegistrationException;

/**
 * A cti port device.
 */
public class CtiPortDevice extends Device
{
	/**
	 * Constructs a cti port device.
	 * 
	 * @param map the cti port map which owns this device.
	 * 
	 * @param deviceName the name of the device.
	 * 
	 * @param managers the list of cti managers for this device.
	 * 
	 * @param username the username used to access this device.
	 * 
	 * @param password the password for the username.
	 * @param regAddr 
	 * @param regPort 
	 */
	public CtiPortDevice( CtiPortMap map, String deviceName, List<String> managers,
		String username, String password, InetAddress regAddr, int regPort )
	{
		super( map, deviceName, managers, username, password, regAddr, regPort );
	}
	
	@Override
	public String toString()
	{
		if (toString0 == null)
			toString0 =  "cti-port( "+getDeviceName()+" )";
		return toString0;
	}
	
	private String toString0;

	/* (non-Javadoc)
	 * @see service.Device#registerMediaCaps(javax.telephony.Terminal, com.cisco.jtapi.extensions.CiscoMediaCapability[])
	 */
	@Override
	protected void registerMediaCaps( Terminal t, InetAddress addr, int port,
		CiscoMediaCapability[] caps ) throws CiscoRegistrationException
	{
		if (t instanceof CiscoMediaTerminal)
		{
			((CiscoMediaTerminal) t).register( null, 0, caps );
			report( Trace.m( "registered media caps" ) );
		}
		else
		{
			report( Trace.m( "did not register media caps, terminal class is " )
				.a( t.getClass() ) );
		}
	}
	
	/* (non-Javadoc)
	 * @see service.Device#unregisterMediaCaps(javax.telephony.Terminal)
	 */
	@Override
	protected void unregisterMediaCaps( Terminal t )
	{
		try
		{
			((CiscoMediaTerminal) t).unregister();
		}
		catch ( Exception e )
		{
			report( e );
		}
	}
}
