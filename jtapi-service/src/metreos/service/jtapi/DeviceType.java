/* $Id: DeviceType.java 26649 2006-08-31 16:55:01Z adchaney $
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
	
	/**
	 * The device is a phone we're monitoring.
	 */
	public final static int MonitoredDevice = 6;
}
