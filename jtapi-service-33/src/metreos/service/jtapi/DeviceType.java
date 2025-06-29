/* $Id: DeviceType.java 7113 2005-06-10 16:00:16Z wert $
 *
 * Created by wert on Feb 1, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

/**
 * The device type.
 */
public interface DeviceType
{
	/**
	 * The device is a cti port device.
	 */
	public final static int CtiPort = 2;
	
	/**
	 * The device is a route point.
	 */
	public final static int RoutePoint = 3;
}
