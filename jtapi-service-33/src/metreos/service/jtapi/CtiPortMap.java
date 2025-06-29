/* $Id: CtiPortMap.java 30152 2007-03-06 21:47:50Z wert $
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
 * A device map of cti ports.
 */
public class CtiPortMap extends DeviceMap
{
	/**
	 * Constructs a device map for cti ports.
	 * 
	 * @param deviceType the device type for this map.
	 * 
	 * @param server the server instance that owns this device map.
	 * 
	 * @param connection the connection of the server instance.
	 * 
	 * @param cmMap the call monitor map.
	 */
	public CtiPortMap( int deviceType, JTapiServer server, IpcConnection connection,
		CallMonitorMap cmMap )
	{
		super( deviceType, server, connection, cmMap );
	}
	
	/* (non-Javadoc)
	 * @see service.DeviceMap#mkDevice(java.lang.String, java.util.List, java.lang.String, java.lang.String)
	 */
	@Override
	protected Device mkDevice( String deviceName, List<String> managers, String username,
		String password, InetAddress regAddr, int regPort )
	{
		return new CtiPortDevice( this, deviceName, managers, username,
			password, regAddr, regPort );
	}
}
