package metreos.core.net.nio.test;

import java.io.IOException;
import java.nio.channels.SocketChannel;

import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.core.net.ipc.IpcMessage;
import metreos.core.net.nio.SuperSelector;

/**
 * Description of FlatmapIpcStreamHandler.
 */
abstract public class FlatmapIpcStreamHandler extends IpcStreamHandler
{
	/**
	 * Constructs the FlatmapIpcStreamHandler.
	 *
	 * @param superSelector
	 * @param channel
	 * @param maxOutputQueueSize
	 */
	public FlatmapIpcStreamHandler( SuperSelector superSelector,
		SocketChannel channel, int maxOutputQueueSize )
	{
		super( superSelector, channel, maxOutputQueueSize );
	}

	@Override
	protected void handleMessage( IpcMessage msg ) throws IOException
	{
		handleMessage( new FlatmapIpcMessage( msg.getBytes(), null, null ) );
	}
	
	/**
	 * @param msg
	 * @throws IOException
	 */
	abstract protected void handleMessage( FlatmapIpcMessage msg )
		throws IOException;
	
	/**
	 * @param fmsg
	 * @throws IOException
	 */
	public void send( FlatmapIpcMessage fmsg ) throws IOException
	{
		send( 0, 0, fmsg.getBytes() );
	}
}