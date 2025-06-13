using System;

namespace ServerInterface
{
	public interface IRemotingServer
	{
        void SetName(string name);

        string GetName();

        void UploadData(byte[] data);

        void Cleanup();
	}
}
