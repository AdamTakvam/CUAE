/* $Id: PlainSocketAddressGen.java 6817 2005-05-20 22:06:30Z wert $
 *
 * Created by wert on May 20, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.SocketAddress;

/**
 * Description of PlainSocketAddressGen.
 */
public class PlainSocketAddressGen implements SocketAddressGen
{
	/**
	 * Constructs the PlainSocketAddressGen.
	 * @param host 
	 * @param port 
	 */
	public PlainSocketAddressGen( String host, int port )
	{
		this.sa = new InetSocketAddress( host, port );
	}
	
	/**
	 * Constructs the PlainSocketAddressGen.
	 * @param addr 
	 * @param port 
	 */
	public PlainSocketAddressGen( InetAddress addr, int port )
	{
		this.sa = new InetSocketAddress( addr, port );
	}
	
	/**
	 * Constructs the PlainSocketAddressGen.
	 *
	 * @param sa
	 */
	public PlainSocketAddressGen( SocketAddress sa )
	{
		this.sa = sa;
	}
	
	private final SocketAddress sa;
	
	public int count()
	{
		return 1;
	}

	public SocketAddress next()
	{
		return sa;
	}
}
