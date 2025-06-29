/* $Id: MediaCap.java 30152 2007-03-06 21:47:50Z wert $
 *
 * Created by wert on Feb 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import com.cisco.jtapi.extensions.CiscoMediaCapability;

/**
 * Media capabilities associated with a media terminal or route point.
 */
public class MediaCap
{
	/**
	 * Constructs the MediaCaps.
	 * 
	 * @param codec a specification of the audio media format.
	 * @param framesize the amount of audio data per packet in milliseconds.
	 * @param capability the corresponding cisco media capability.
	 * 
	 * @see Codec
	 */
	public MediaCap( int codec, int framesize, CiscoMediaCapability capability )
	{
		this.codec = codec;
		this.framesize = framesize;
		this.capability = capability;
	}
	
	/**
	 * A specification of the audio media format.
	 */
	public final int codec;
	
	/**
	 * The amount of audio data per packet in milliseconds.
	 */
	public final int framesize;
	
	/**
	 * The corresponding cisco media capability.
	 */
	public final CiscoMediaCapability capability;
	
	@Override
	public String toString()
	{
		if (toString0 == null)
			toString0 =  "mediaCap( "+codec+"/"+framesize+" )";
		return toString0;
	}
	
	private String toString0;
	
	@Override
	public int hashCode()
	{
		return codec*37 ^ framesize;
	}
	
	@Override
	public boolean equals( Object obj )
	{
		if (obj == this)
			return true;
		
		if (!(obj instanceof MediaCap))
			return false;
		
		MediaCap other = (MediaCap) obj;
		return other.codec == codec && other.framesize == framesize;
	}
}
