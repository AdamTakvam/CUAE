/* $Id: AlarmListener.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Mar 23, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

/**
 * Classes wanting alarms must implement this interface.
 */
public interface AlarmListener
{
	/**
	 * Reports that the listener's alarm went off.
	 * @param manager the manager of the alarm.
	 * @param due the time the alarm was set to go off.
	 * @return the snooze time in ms for this alarm, or
	 * 0 to shutoff the alarm.
	 */
	public int wakeup( AlarmManager manager, long due );
}
