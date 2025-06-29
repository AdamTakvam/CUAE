/* $Id: SocketAddressGen.java 6817 2005-05-20 22:06:30Z wert $
 *
 * Created by wert on May 20, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.net.SocketAddress;

/**
 * Generate socket addresses for bind attempts. Applications can use
 * this to restrict port values or ranges.
 */
public interface SocketAddressGen
{
	/**
	 * @return the number of socket addresses in this generator.
	 */
	public int count();

	/**
	 * @return the next socket address in sequence.
	 */
	public SocketAddress next();
}
