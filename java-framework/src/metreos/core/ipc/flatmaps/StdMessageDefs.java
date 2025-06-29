/* $Id: StdMessageDefs.java 6808 2005-05-20 17:59:29Z wert $
 *
 * Created by wert on May 2, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

/**
 * Standard messages and fields for any flatmap ipc listener.
 */
public interface StdMessageDefs
{
	//////////////
	// MESSAGES //
	//////////////
	
	/**
	 * Description of Error.
	 */
	public int Error = 1000001001;

	/////////////////////
	// FAILURE REASONS //
	/////////////////////
	
	/**
	 * Description of GeneralFailure.
	 */
	public int GeneralFailure = 1000000001;
	
	/**
	 * Description of MissingField.
	 */
	public int MissingField = 1000000002;
	
	/**
	 * Description of UnknownMessageType.
	 */
	public int UnknownMessageType = 1000000003;

	////////////
	// FIELDS //
	////////////
	
	/**
	 * Description of Args.
	 */
	public int Args = 1000000101;
	
	/**
	 * Description of Message.
	 */
	public int ErrorMessage = 1000000102;
	
	/**
	 * Description of FailReason.
	 */
	public int FailReason = 1000000103;
	
	/**
	 * Description of MessageField.
	 */
	public int MessageField = 1000000104;
	
	/**
	 * Description of MessageType.
	 */
	public int MessageType = 1000000105;

	/**
	 * Description of RequestId.
	 */
	public int RequestId = 1000000106;
}
