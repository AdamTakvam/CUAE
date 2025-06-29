/* $Id: RoutePointDevice.java 29371 2007-01-31 19:18:34Z sccomer $
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

import com.cisco.jtapi.extensions.CiscoJtapiException;
import com.cisco.jtapi.extensions.CiscoMediaCapability;
import com.cisco.jtapi.extensions.CiscoRTPHandle;
import com.cisco.jtapi.extensions.CiscoRTPParams;
import com.cisco.jtapi.extensions.CiscoRegistrationException;
import com.cisco.jtapi.extensions.CiscoRouteTerminal;

/**
 * A route point device.
 */
public class RoutePointDevice extends Device
{
	/**
	 * Constructs a route point device.
	 * 
	 * @param map the route point map which owns this device.
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
	public RoutePointDevice( RoutePointMap map, String deviceName, List<String> managers,
		String username, String password, boolean thirdParty )
	{
		super( map, deviceName, managers, username, password, thirdParty );
	}
	
	@Override
	public String toString()
	{
		if (toString0 == null)
			toString0 =  "route-point( "+getDeviceName()+" )";
		return toString0;
	}
	
	private String toString0;

	@Override
	protected void registerMediaCaps( Terminal t, CiscoMediaCapability[] caps )
		throws CiscoRegistrationException
	{
		((CiscoRouteTerminal) t).register( caps,
			CiscoRouteTerminal.DYNAMIC_MEDIA_REGISTRATION );
		report( Trace.m( "registered media caps" ) );
	}
	
	@Override
	protected void unregisterMediaCaps( Terminal t )
	{
		try
		{
			((CiscoRouteTerminal) t).unregister();
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
			((CiscoRouteTerminal) getTerminal()).setRTPParams( handle, params );
			//report( Trace.m( "called setRTPParams( " ).a( getTerminalName() ).a( ", " ).a( handle ).a( ", " ).a( params ).a( " )" ) );
			return true;
		}
		catch (PlatformException e )
		{
			String s = "";
			if (e instanceof CiscoJtapiException)
				s = ((CiscoJtapiException) e).getErrorName()+" "+((CiscoJtapiException) e).getErrorDescription();
			report( Trace.m( "*** setRTPParams caught PlatformException " ).a( e ).a( "; " ).a( s ) );
			return false;
		}
	}
}
