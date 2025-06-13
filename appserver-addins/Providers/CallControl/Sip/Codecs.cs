namespace Metreos.CallControl.Sip
{
	//Payload Type based on RFC 3551
	public enum CodecPayloadType
	{
		G711u		= 0,	//PCMU
		GSM			= 3,
		G723		= 4,
		G711a		= 8,	//PCMA
		G722		= 9,
		G728		= 15,
		G729		= 18,
		Unspecified = 10000,
	}

}