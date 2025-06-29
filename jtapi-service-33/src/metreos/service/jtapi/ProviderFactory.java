/* $Id: ProviderFactory.java 7113 2005-06-10 16:00:16Z wert $
 *
 * Created by wert on Feb 1, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import javax.telephony.JtapiPeer;
import javax.telephony.Provider;
import javax.telephony.ProviderUnavailableException;

/**
 * A factory for javax.telephony.Provider instances.
 */
public class ProviderFactory
{
	/**
	 * Constructs a provider factory.
	 * @param jtapiPeer
	 */
	public ProviderFactory( JtapiPeer jtapiPeer )
	{
		this.jtapiPeer = jtapiPeer;
	}

	private final JtapiPeer jtapiPeer;

	/**
	 * Gets the provider string for this provider factory.
	 * @param manager
	 * @param username
	 * @param password
	 * @return the provider string for this provider factory.
	 */
	public String getProviderString( String manager, String username, String password )
	{
		return manager + ";login=" + username + ";passwd=" + password;
	}

	/**
	 * Gets the provider for the specified provider string.
	 * @param providerString
	 * @return the provider for the specified provider string.
	 * @throws ProviderUnavailableException
	 */
	public Provider getProvider( String providerString )
		throws ProviderUnavailableException
	{
		return jtapiPeer.getProvider( providerString );
	}
}
