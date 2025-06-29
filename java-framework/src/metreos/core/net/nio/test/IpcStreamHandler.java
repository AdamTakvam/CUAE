package metreos.core.net.nio.test;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.channels.SocketChannel;
import java.util.List;
import java.util.Vector;

import metreos.core.net.ipc.BasicIpcMessage;
import metreos.core.net.ipc.IpcConnection;
import metreos.core.net.ipc.IpcMessage;
import metreos.core.net.nio.StreamHandler;
import metreos.core.net.nio.SuperSelector;
import metreos.util.Assertion;

/**
 * Description of IpcStreamHandler.
 */
abstract public class IpcStreamHandler extends StreamHandler
{
	/**
	 * Constructs the IpcStreamHandler.
	 *
	 * @param superSelector
	 * @param channel
	 * @param maxOutputQueueSize
	 */
	public IpcStreamHandler( SuperSelector superSelector,
		SocketChannel channel, int maxOutputQueueSize )
	{
		super( superSelector, channel );
		this.maxOutputQueueSize = maxOutputQueueSize;
	}
	
	private final int maxOutputQueueSize;
	
	private ByteBuffer flagInput = ByteBuffer.allocate( 4 );
	
	private ByteBuffer flag2Input = ByteBuffer.allocate( 4 );
	
	private ByteBuffer input = flagInput;

	private static final int MAX_INPUT_BUFFER = 4096;

	@Override
	protected boolean wantsRead()
	{
		return !inputIsBlocked() && needsInput();
	}
	
	private boolean inputIsBlocked()
	{
		return outputQueue.size() > maxOutputQueueSize;
	}
	
	private boolean needsInput()
	{
		return input != null && input.remaining() > 0;
	}

	@Override
	protected void doRead() throws IOException
	{
		while (needsInput())
		{
			while (input.remaining() > 0)
			{
				int n = read( input, true );
				if (n == 0)
					return;
			}
			
			processInput();
		}
	}

	private void processInput() throws IOException
	{
//		report( "processInput: "+input.position() );
		Assertion.check( input.remaining() == 0, "input.remaining() == 0" );
		
		if (input == flagInput)
		{
			input.flip();
			flag = input.getInt();
			input.clear();
			
			length = flag & IpcConnection.LENGTH_MASK; 
			flag = flag >>> IpcConnection.FLAG_SHIFT;
			
//			report( "length = "+length+", flag = "+flag );
			
			if ((flag & IpcConnection.HAS_ADDITIONAL_FLAG) != 0)
			{
				input = flag2Input;
			}
			else
			{
				flag2 = 0;
				
//				report( "flag2 = "+flag2 );
				
				index = 0;
				bytes = new Vector<byte[]>();
				input = getInputBuffer( index, length );
			}
		}
		else if (input == flag2Input)
		{
			input.flip();
			flag2 = input.getInt();
			input.clear();
			
//			report( "flag2 = "+flag2 );

			index = 0;
			bytes = new Vector<byte[]>();
			input = getInputBuffer( index, length );
		}
		else
		{
			int k = input.position();
			index += k;
			
			Assertion.check( input.hasArray(), "input.hasArray()" );
			bytes.add( input.array() );
			
			input = getInputBuffer( index, length );
			if (input == null)
			{
				// TODO we can remove an extra copy of the data
				// and also a potentially large buffer allocation
				// by passing an input stream to BasicIpcMessage.
				byte[] buf = flattenBytes( length, bytes );
				bytes = null;
				input = flagInput;
				handleMessage( new BasicIpcMessage( flag, flag2, buf ) );
			}
		}
	}

	private static ByteBuffer getInputBuffer( int index, int length )
	{
		int remaining = length - index;
		
		if (remaining == 0)
			return null;
		
		return ByteBuffer.allocate( Math.min( remaining, MAX_INPUT_BUFFER ) );
	}
	
	private static byte[] flattenBytes( int length, List<byte[]> bytes )
	{
		if (bytes.size() == 1)
			return bytes.get( 0 );
		
		throw new UnsupportedOperationException();
	}

	private int flag;
	
	private int flag2;
	
	private int length;
	
	private int index;
	
	private List<byte[]> bytes;
	
	/**
	 * @param msg
	 * @throws IOException
	 */
	abstract protected void handleMessage( IpcMessage msg ) throws IOException;

	@Override
	protected boolean wantsWrite()
	{
		return outputQueue.size() > 0;
	}
	
	/**
	 * @param flg
	 * @param flg2
	 * @param bts
	 * @throws IOException 
	 */
	public void send( int flg, int flg2, byte[] bts ) throws IOException
	{
//		report( "sending "+flg+", "+flg2+", "+bts.length );
		
		if ((flg & ~IpcConnection.FLAG_MASK) != 0)
			throw new IllegalArgumentException( "illegal bits in flag" );
		
		int len = bts.length;
		if ((len & ~IpcConnection.LENGTH_MASK) != 0)
			throw new IllegalArgumentException( "bytes too long" );
		
		if (flg2 == 0)
			flg &= ~IpcConnection.HAS_ADDITIONAL_FLAG;
		else
			flg |= IpcConnection.HAS_ADDITIONAL_FLAG;

		int k = len + 4;
		if (flg2 != 0)
			k += 4;
		
		ByteBuffer bb = ByteBuffer.allocate( k );
		
		bb.putInt( (flg << IpcConnection.FLAG_SHIFT) | len );
		
		if (flg2 != 0)
			bb.putInt( flg2 );
		
		bb.put( bts );
		
		bb.flip();
		
		addToOutputQueue( bb );
	}
	
	/**
	 * @param msg
	 * @throws IOException
	 */
	public void send( IpcMessage msg ) throws IOException
	{
		send( msg.getFlag(), msg.getFlag2(), msg.getBytes() );
	}
	
	private void addToOutputQueue( ByteBuffer buffer ) throws IOException
	{
		outputQueue.add( buffer );
		if (myDoWrite())
			resetInterestOps();
	}

	private List<ByteBuffer> outputQueue = new Vector<ByteBuffer>();

	@Override
	protected void doWrite() throws IOException
	{
		myDoWrite();
	}
	
	private boolean myDoWrite() throws IOException
	{
		synchronized (writeSync)
		{
			while (outputQueue.size() > 0)
			{
				ByteBuffer output = outputQueue.get( 0 );
				int n = write( output );
				if (n == 0)
					return true;
				if (output.remaining() == 0)
					outputQueue.remove( 0 );
			}
			return false;
		}
	}
	
	private final Object writeSync = new Object();
}