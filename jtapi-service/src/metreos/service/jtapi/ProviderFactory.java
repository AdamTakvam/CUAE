/* $Id: ProviderFactory.java 8447 2005-08-09 22:38:23Z wert $
 *
 * Created by wert on Feb 1, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.util.Iterator;

import javax.telephony.ProviderUnavailableException;

import com.cisco.jtapi.extensions.CiscoJtapiPeer;
import com.cisco.jtapi.extensions.CiscoProvider;

/**
 * A factory for javax.telephony.Provider instances.
 */
public class ProviderFactory
{
	/**
	 * Constructs a provider factory.
	 * @param jtapiPeer
	 */
	public ProviderFactory( CiscoJtapiPeer jtapiPeer )
	{
		this.jtapiPeer = jtapiPeer;
	}

	private final CiscoJtapiPeer jtapiPeer;

	/**
	 * Gets the provider string for this provider factory.
	 * @param managers
	 * @param username
	 * @param password
	 * @return the provider string for this provider factory.
	 */
	public String getProviderString( Iterator<String> managers, String username, String password )
	{
		StringBuffer sb = new StringBuffer();
		
		while (managers.hasNext())
		{
			if (sb.length() > 0)
				sb.append( ',' );
			sb.append( managers.next() );
		}
		
		sb.append( ";login=" );
		sb.append( username );
		
		sb.append( ";passwd=" );
		sb.append( password );
		
		return sb.toString();
	}

	/**
	 * Gets the provider for the specified provider string.
	 * @param providerString
	 * @return the provider for the specified provider string.
	 * @throws ProviderUnavailableException
	 */
	public CiscoProvider getProvider( String providerString )
		throws ProviderUnavailableException
	{
		return (CiscoProvider) jtapiPeer.getProvider( providerString );
	}
}
