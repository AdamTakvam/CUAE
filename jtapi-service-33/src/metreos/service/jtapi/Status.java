/* $Id: Status.java 7113 2005-06-10 16:00:16Z wert $
 * 
 * Created by achaney on Jan 28, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

/**
 * Describes the set of status changes which may occur at any
 * time in the JTapiServer.
 */
public interface Status 
{
	/**
	 * Device is offline and cannot be accessed.
	 */
	public final static int DeviceOffline = 0;
	
	/**
	 * Device is online and available for access.
	 */
	public final static int DeviceOnline = 1;
}
