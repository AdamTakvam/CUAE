/* $Id: DeviceMap.java 7119 2005-06-10 19:54:11Z wert $
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
	 */
	public DeviceMap( int deviceType, JTapiServer server,
		IpcConnection connection, CallMonitorMap cmMap )
	{
		this.deviceType = deviceType;
		this.server = server;
		this.connection = connection;
		this.cmMap = cmMap;
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
	 * @param regAddr 
	 * @param regPort 
	 */
	public void add( String deviceName, List<String> managers, String username,
		String password, InetAddress regAddr, int regPort )
	{
		// you might think there could be a race condition here,
		// but we only speak to a single client which completely
		// owns this data. therefore the data is not shared, and
		// there is no need for synchronization.
		
		drop( deviceName );
		Device device = mkDevice( deviceName, managers, username, password, regAddr, regPort );
		add( device );
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
		Iterator<Device> i = getAll().iterator();
		while (i.hasNext())
		{
			Device device = i.next();
			i.remove();
			device.close();
		}
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
	 * @param regAddr 
	 * @param regPort 
	 * 
	 * @return an appropriate device for this map.
	 */
	abstract protected Device mkDevice( String deviceName, List<String> managers,
		String username, String password, InetAddress regAddr, int regPort );

	/////////////
	// devices //
	/////////////
	
	/**
	 * Gets a list of all devices.
	 * 
	 * @return a list of all devices.
	 */
	private List<Device> getAll()
	{
		synchronized (devices)
		{
			return new Vector<Device>( devices.values() );
		}
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
		return devices.get( deviceName );
	}

	/**
	 * Adds a device to this map.
	 * 
	 * @param device
	 */
	private void add( Device device )
	{
		Device old = devices.put( device.getDeviceName(), device );
		
		if (old != null)
			old.close();
		
		device.open();
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
		return devices.remove( deviceName );
	}
	
	private final Map<String,Device> devices = new Hashtable<String,Device>();
}
