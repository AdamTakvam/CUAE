/* $Id: CtiPortDevice.java 32224 2007-05-16 21:26:24Z wert $
 *
 * Created by wert on Jan 31, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.util.List;

import javax.telephony.PlatformException;
import javax.telephony.Terminal;

import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoMediaCapability;
import com.cisco.jtapi.extensions.CiscoMediaTerminal;
import com.cisco.jtapi.extensions.CiscoRTPHandle;
import com.cisco.jtapi.extensions.CiscoRTPParams;
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
	 * 
	 * @param thirdParty monitor for third party call control.
	 */
	public CtiPortDevice( CtiPortMap map, String deviceName, List<String> managers,
		String username, String password, boolean thirdParty )
	{
		super( map, deviceName, managers, username, password, thirdParty );
	}
	
	@Override
	public String toString()
	{
		if (toString0 == null)
		{
			if (isThirdParty())
				toString0 =  "mon-dev( "+getDeviceName()+" )";
			else
				toString0 =  "cti-port( "+getDeviceName()+" )";
		}
		return toString0;
	}
	
	private String toString0;

	@Override
	protected void registerMediaCaps( Terminal t, CiscoMediaCapability[] caps )
		throws CiscoRegistrationException
	{
		if (t instanceof CiscoMediaTerminal)
		{
			((CiscoMediaTerminal) t).register( caps );
			report( Trace.m( "registered media caps" ) );
		}
		else
		{
			report( Trace.m( "did not register media caps, terminal class is " )
				.a( t.getClass() ) );
		}
	}
	
	@Override
	protected void unregisterMediaCaps( Terminal t )
	{
		try
		{
			if (t instanceof CiscoMediaTerminal)
				((CiscoMediaTerminal) t).unregister();
			else
				report( Trace.m( "did not unregister media caps, terminal class is " )
					.a( t.getClass() ) );
		}
		catch ( Exception e )
		{
			report( e );
		}
	}
	
	@Override
	public boolean setRTPParams( CiscoRTPHandle handle, CiscoRTPParams params )
		throws Exception
	{
		try
		{
			//report( Trace.m( "calling setRTPParams( " ).a( getTerminalName() ).a( ", " ).a( handle ).a( ", " ).a( params ).a( " )" ) );
			((CiscoMediaTerminal) getTerminal()).setRTPParams( handle, params);
			//report( Trace.m( "called setRTPParams( " ).a( getTerminalName() ).a( ", " ).a( handle ).a( ", " ).a( params ).a( " )" ) );
			return true;
		}
		catch ( PlatformException e )
		{
			report( Trace.m( "*** setRTPParams caught PlatformException " ).a( e ) );
			return false;
		}
	}
}
