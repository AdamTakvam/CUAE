package metreos.service.jtapi;

import java.io.IOException;
import java.net.Socket;

import metreos.core.ipc.flatmaps.PrettyPrinter;
import metreos.core.net.Server;
import metreos.core.net.ipc.IpcConnection;

/**
 * Description of MyIpcConnection.
 */
public class MyIpcConnection extends IpcConnection
{
	/**
	 * Constructs the MyIpcConnection.
	 *
	 * @param server
	 * @param socket
	 * @param printer 
	 * @throws IOException
	 */
	public MyIpcConnection( Server server, Socket socket, PrettyPrinter printer )
		throws IOException
	{
		super( server, socket );
		this.printer = printer;
	}
	
	private final PrettyPrinter printer;

	/**
	 * @return the printer for this connection.
	 */
	public PrettyPrinter getPrinter()
	{
		return printer;
	}
}
