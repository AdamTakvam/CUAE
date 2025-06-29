/* $Id: RoutePointMap.java 29371 2007-01-31 19:18:34Z sccomer $
 *
 * Created by wert on Jan 31, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.net.InetAddress;
import java.util.List;

import metreos.core.net.ipc.IpcConnection;

/**
 * A device map of route points.
 */
public class RoutePointMap extends DeviceMap
{
	/**
	 * Constructs a device map of route points.
	 * 
	 * @param deviceType the device type for this map.
	 * 
	 * @param server the server instance that owns this device map.
	 * 
	 * @param connection the connection of the server instance.
	 * 
	 * @param cmMap the call monitor map.
	 * 
	 * @param fakeIp 
	 * 
	 * @param fakePort 
	 */
	public RoutePointMap( int deviceType, JTapiServer server,
		IpcConnection connection, CallMonitorMap cmMap, InetAddress fakeIp,
		int fakePort )
	{
		super( deviceType, server, connection, cmMap, fakeIp, fakePort );
	}

	@Override
	protected Device mkDevice( String deviceName, List<String> managers, String username,
		String password, boolean thirdParty )
	{
		return new RoutePointDevice( this, deviceName, managers, username, password, thirdParty );
	}
}
