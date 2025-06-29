/* $Id: DeviceMap.java 29371 2007-01-31 19:18:34Z sccomer $
 *
 * Created by wert on Jan 31, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.net.InetAddress;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Vector;

import metreos.core.net.ipc.IpcConnection;
import metreos.util.Trace;

/**
 * A type-specific map of devices.
 */
abstract public class DeviceMap
{
	/**
	 * Constructs a device map.
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
	public DeviceMap( int deviceType, JTapiServer server,
		IpcConnection connection, CallMonitorMap cmMap, InetAddress fakeIp,
		int fakePort )
	{
		this.deviceType = deviceType;
		this.server = server;
		this.connection = connection;
		this.cmMap = cmMap;
		this.fakeIp = fakeIp;
		this.fakePort = fakePort;
	}
	
	/**
	 * The device type.
	 */
	public final int deviceType;
	
	/**
	 * The server instance that owns this device map.
	 */
	public final JTapiServer server;
	
	/**
	 * The connection of the server instance.
	 */
	public final IpcConnection connection;
	
	/**
	 * The call monitor map.
	 */
	public final CallMonitorMap cmMap;

	/**
	 * Description of fakeIp.
	 */
	public final InetAddress fakeIp;

	/**
	 * Description of fakePort.
	 */
	public final int fakePort;

	////////////////////
	// map management //
	////////////////////
	
	/**
	 * Adds the specified device to the map. If there is already a device
	 * here with the same name, it is dropped first.
	 * 
	 * @param deviceName the name of the device in this map.
	 * 
	 * @param managers the list of cti managers for this device.
	 * 
	 * @param username the username used to access this device.
	 * 
	 * @param password the password for the username.
	 * 
	 * @param thirdParty monitor for third party call control.
	 * 
	 * @throws Exception 
	 */
	public void add( String deviceName, List<String> managers, String username,
		String password, boolean thirdParty ) throws Exception
	{
		// you might think there could be a race condition here,
		// but we only speak to a single client which completely
		// owns this data. therefore the data is not shared, and
		// there is no need for synchronization.
		
		Device device = mkDevice( deviceName, managers, username, password, thirdParty );
		
		Device oldDevice = add( device );
		if (oldDevice != null)
		{
			Trace.report( this, Trace.m( "replaced device " ).a( oldDevice ) );
			oldDevice.close();
		}
		
		try
		{
			device.open();
		}
		catch ( Exception e )
		{
			drop( deviceName );
			throw e;
		}
	}

	/**
	 * Drops the specified device from the map.
	 * 
	 * @param deviceName the name of the device in this map.
	 */
	public void drop( String deviceName )
	{
		Device device = remove( deviceName );
		if (device != null)
			device.close();
	}

	/**
	 * Drops all devices from the map.
	 */
	public void dropAll()
	{
		Iterator<String> i = getAllNames().iterator();
		while (i.hasNext())
			drop( i.next() );
	}

	/////////////////////
	// personalization //
	/////////////////////

	/**
	 * Constructs an appropriate device for this map.
	 * 
	 * @param deviceName the name of the device in this map.
	 * 
	 * @param managers the list of cti managers for this device.
	 * 
	 * @param username the username used to access this device.
	 * 
	 * @param password the password for the username.
	 * 
	 * @param thirdParty monitor for third party call control.
	 * 
	 * @return an appropriate device for this map.
	 */
	abstract protected Device mkDevice( String deviceName, List<String> managers,
		String username, String password, boolean thirdParty );

	/////////////
	// devices //
	/////////////
	
	/**
	 * Gets a list of all device names.
	 * 
	 * @return a list of all device names.
	 */
	private List<String> getAllNames()
	{
		return new Vector<String>( devices.keySet() );
	}
	
	/**
	 * Gets the specified device.
	 * 
	 * @param deviceName the name of the device in this map.
	 * 
	 * @return the specified device or null.
	 */
	public Device get( String deviceName )
	{
		return devices.get( deviceName.toLowerCase() );
	}

	/**
	 * Adds a device to this map.
	 * 
	 * @param device
	 */
	private Device add( Device device )
	{
		return devices.put( device.getDeviceName().toLowerCase(), device );
	}

	/**
	 * Removes a device from this map.
	 * 
	 * @param deviceName the name of the device in this map.
	 * 
	 * @return the device if it was in the map, or null.
	 */
	private Device remove( String deviceName )
	{
		return devices.remove( deviceName.toLowerCase() );
	}
	
	private final Map<String,Device> devices = new Hashtable<String,Device>();
}
