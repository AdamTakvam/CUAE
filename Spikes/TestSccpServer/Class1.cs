using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Metreos.LoggingFramework;
using Metreos.Utilities;
using Metreos.Utilities.Selectors;

namespace TestSccpServer
{
	class Class1
	{
		static void Main( string[] args )
		{
			Log log = new Log( TraceLevel.Info );
			log.Write( TraceLevel.Info, "starting..." );

			SelectorBase selector = new SuperSelector( null, null );
			selector.Start();

			SccpPolicy policy = new SccpPolicy();

			SccpListener listener = new SccpListener( log, selector, policy );
			listener.Start( 2000 );

			while (true)
			{
				Console.WriteLine( "hit q to quit" );
				String s = Console.ReadLine();
				if (s.Equals( "q" ))
					break;
			}
		}
	}

	class SccpPolicy
	{
		public bool OkToAccept()
		{
			return true;
		}

		private RegInfo GetRegInfo( SccpServer server )
		{
			String deviceName = server.DeviceName;
			if (deviceName == null)
				return null;

			RegInfo ri = (RegInfo) regs[deviceName];
			if (ri == null)
			{
				ri = new RegInfo( this, deviceName );
				regs.Add( deviceName, ri );
			}

			return ri;
		}

		public bool OkToRegister( SccpServer server )
		{
			RegInfo ri = GetRegInfo( server );
			Assertion.Check( ri != null, "ri != null" );
			return ri.OkToRegister( server );
		}

		public LineStat GetLineStat( SccpServer server, int lineNumber )
		{
			RegInfo ri = GetRegInfo( server );
			if (ri == null)
				return null;

			return ri.GetLineStat( lineNumber );
		}

		public int GetNextAddr()
		{
			lock (this)
			{
				return nextAddr++;
			}
		}

		private int nextAddr = 1000000000;

		public int Unregister( SccpServer server )
		{
			RegInfo ri = GetRegInfo( server );
			if (ri == null)
				return 1;

			return ri.Unregister( server );
		}

		private IDictionary regs = Hashtable.Synchronized( new Hashtable() );
	}

	class RegInfo
	{
		public RegInfo( SccpPolicy policy, String deviceName )
		{
			this.policy = policy;
			this.deviceName = deviceName;
		}

		private readonly SccpPolicy policy;

		private readonly String deviceName;

		public bool OkToRegister( SccpServer server )
		{
			lock (this)
			{
				if (currentServer != null)
					return false;

				if (regLockoutEnd > HPTimer.Now())
					return false;
				
				currentServer = server;
				lastRegTime = HPTimer.Now();

				// set registration to expire in 30 seconds...

				TimerManager.StaticAdd( 30000, new WakeupDelegate( WakeupAndDie ), server );

				return true;
			}
		}
		
		private long WakeupAndDie( TimerHandle th, Object state )
		{
			if (currentServer == state)
				currentServer.Stop();
			return 0;
		}

		public LineStat GetLineStat( int lineNumber )
		{
			int addr = policy.GetNextAddr();
			String addrStr = addr.ToString();
			return new LineStat( addrStr, addrStr, addrStr, 0 );
		}

		public int Unregister( SccpServer server )
		{
			lock (this)
			{
				if (currentServer != server)
					return 1;

				// when deregistered, disallow register until 60 seconds have gone by.
				regLockoutEnd = HPTimer.Now() + 60*1000*1000*1000L;

				currentServer = null;
				return 0;
			}
		}

		private SccpServer currentServer;

		private long regLockoutEnd;

		private long lastRegTime;
	}

	class SccpConn
	{
		public SccpConn( Log log, SelectorBase selector, SccpPolicy policy )
		{
			this.log = log;
			this.selector = selector;
			this.policy = policy;
		}

		protected readonly Log log;

		protected readonly SelectorBase selector;

		protected readonly SccpPolicy policy;

		protected Metreos.Utilities.Selectors.SelectionKey key;

		protected void Selected( Metreos.Utilities.Selectors.SelectionKey key )
		{
			if (key != this.key)
			{
				log.Write( TraceLevel.Error, "{0} selected key mismatch; closing", this );
				Close();
				if (key != null)
					key.Close();
				return;
			}

			if (key.IsSelectedForRead || key.IsSelectedForWrite)
			{
				if (key.IsSelectedForRead)
					SelectedForRead();

				if (key.IsSelectedForWrite)
					SelectedForWrite();

				return;
			}

			if (key.IsSelectedForAccept)
			{
				SelectedForAccept();
				return;
			}

			if (key.IsSelectedForConnect)
			{
				SelectedForConnect();
				return;
			}

			if (key.IsSelectedForError)
			{
				SelectedForError();
				return;
			}

			log.Write( TraceLevel.Error, "{0} selected for what i dunno; closing", this );
			Close();
		}

		virtual protected void SelectedForAccept()
		{
			log.Write( TraceLevel.Error, "{0} selected for accept; declined", this );
			key.WantsAccept = false;
		}

		virtual protected void SelectedForConnect()
		{
			log.Write( TraceLevel.Error, "{0} selected for connect; declined", this );
			key.WantsConnect = false;
		}

		virtual protected void SelectedForError()
		{
			log.Write( TraceLevel.Error, "{0} selected for error; closing", this );
			Close();
		}

		virtual protected void SelectedForRead()
		{
			log.Write( TraceLevel.Error, "{0} selected for read; declined", this );
			key.WantsRead = false;
		}

		virtual protected void SelectedForWrite()
		{
			log.Write( TraceLevel.Error, "{0} selected for write; declined", this );
			key.WantsWrite = false;
		}

		protected void SelectedException( Metreos.Utilities.Selectors.SelectionKey key, Exception e )
		{
			log.Write( TraceLevel.Error, "{0} caught exception; closing {1}", this, e );
			Close();
		}

		virtual protected bool Close()
		{
			if (key != null)
			{
				key.Close();
				key = null;
				return true;
			}
			return false;
		}

		override public String ToString()
		{
			return key != null ? key.ToString() : "(null)";
		}
	}

	class SccpListener: SccpConn
	{
		public SccpListener( Log log, SelectorBase selector, SccpPolicy policy )
			: base( log, selector, policy )
		{
			// nothing else to do.
		}

		public void Start( int port )
		{
			if (key != null)
				throw new InvalidOperationException( "already started" );

			key = selector.Register( Metreos.Utilities.Selectors.SelectionKey.NewTcpSocket( false ),
				null, new Metreos.Utilities.Selectors.SelectedDelegate( Selected ),
				new Metreos.Utilities.Selectors.SelectedExceptionDelegate( SelectedException ) );
			
			key.Listen( new IPEndPoint( IPAddress.Any, port ), 250 );
			
			log.Write( TraceLevel.Info, "SccpListener listening on {0}", key );
		}

		public void Stop()
		{
			Close();
		}

		override protected void SelectedForAccept()
		{
			if (!OkToAccept())
			{
				key.WantsAccept = false;
				return;
			}

			Socket s = key.Accept();
			log.Write( TraceLevel.Info, "{0} accepted {1}", this, s.RemoteEndPoint );
			new SccpServer( log, selector, policy ).Start( s );
		}

		private bool OkToAccept()
		{
			return policy.OkToAccept();
		}
	}

	abstract class SccpReadWrite: SccpConn
	{
		public SccpReadWrite( Log log, SelectorBase selector, SccpPolicy policy )
			: base( log, selector, policy )
		{
			// nothing else to do.
		}

		override protected void SelectedForRead()
		{
			int n = key.Receive( inputBuf, length, inputBuf.Length-length );
			if (n <= 0)
			{
				log.Write( TraceLevel.Info, "{0} got eof; closing", this );
				Close();
				return;
			}

			//log.Write( TraceLevel.Verbose, "{0} received {1} bytes", this, n );
			length += n;

			while (length - offset >= wantedLength)
			{
				Assertion.Check( wantedLength > 0, "wantedLength > 0" );

				int newWantedLength = ProcessData( inputBuf, offset, wantedLength );
				if (newWantedLength == 0)
				{
					log.Write( TraceLevel.Info, "{0} done; closing", this );
					Close();
					return;
				}
				
				offset += wantedLength;
				wantedLength = newWantedLength;

				if (offset >= MAX_BODY_LEN/2)
				{
					int len = length-offset;
					Array.Copy( inputBuf, offset, inputBuf, 0, len );
					offset = 0;
					length = len;
				}
			}
		}

		private int ProcessData( byte[] buf, int offset, int length )
		{
			if (wantBody)
			{
				if (!ProcessPacket( buf, offset, length ))
					return 0;

				wantBody = false;
				return HEADER_LEN;
			}
			else
			{
				Assertion.Check( length >= HEADER_LEN, "length >= HEADER_LEN" );
				int k = BitConverter.ToInt32( buf, offset );
				//log.Write( TraceLevel.Verbose, "{0} got length {1}", this, k );
				
				if (k == 0)
					return HEADER_LEN;

				Assertion.Check( k >= 1 && k <= MAX_BODY_LEN, "k >= 1 && k <= MAX_BODY_LEN" );

				wantBody = true;
				return k;
			}
		}

		private bool ProcessPacket( byte[] buf, int offset, int length )
		{
			PacketReader pr = new PacketReader( buf, offset, length );

			int cmd = pr.GetInt32();
			//log.Write( TraceLevel.Verbose, "{0} got cmd {1}", this, cmd );

			return ProcessCommand( cmd, pr );
		}

		virtual protected bool ProcessCommand( int cmd, PacketReader pr )
		{
			log.Write( TraceLevel.Warning,
				"{0} unknown cmd 0x{1}",
				this, cmd.ToString( "x" ) );
			return false;
		}

		private byte[] inputBuf = new byte[MAX_BODY_LEN*4];

		private int offset;

		private int length;

		private bool wantBody;

		private int wantedLength = HEADER_LEN;

		private const int HEADER_LEN = 8;

		private const int MAX_BODY_LEN = 2048;
	}

	class Cmd
	{

		public const int Keepalive = 0x00;

		public const int Register = 0x01;

		public const int Alarm = 0x20;

		public const int Unregister = 0x27;

		public const int RegisterAck = 0x81;

		public const int RegisterReject = 0x9d;

		public const int UnregisterAck = 0x118;

		public const int HeadsetStatus = 0x2b;

		public const int KeepaliveAck = 0x100;

		public const int TimeDateReq = 0x0d;

		public const int ButtonTemplateReq = 0x0e;

		public const int CapabilitiesReq = 0x9b;

		public const int CapabilitiesRes = 0x10;

		public const int ButtonTemplate = 0x97;

		public const int SoftkeyTemplateReq = 0x28;

		public const int SoftkeyTemplateRes = 0x108;

		public const int SoftkeySetReq = 0x25;

		public const int SoftkeySetRes = 0x109;

		public const int LineStatReq = 0x0b;

		public const int LineStat = 0x92;

		public const int SpeeddialStatReq = 0x0a;

		public const int SpeeddialStat = 0x91;

		public const int RegisterAvailableLines = 0x2d;

		public const int DefineTimeDate = 0x94;

		public const int IpPort = 0x02;

		public const int Onhook = 0x7;
	}

	abstract class SccpPacket: SccpReadWrite
	{
		public SccpPacket( Log log, SelectorBase selector, SccpPolicy policy )
			: base( log, selector, policy )
		{
			// nothing else to do.
		}

		protected bool ProcessRegister( PacketReader pr )
		{
			String deviceName = pr.GetString( RegisterDeviceNameLen );
			int reserved1 = pr.GetInt32();
			int instance = pr.GetInt32();
			int ipAddr = pr.GetInt32();
			int deviceType = pr.GetInt32();
			int maxStreams = pr.GetInt32();
			int activeStreams = pr.GetInt32();
			int protocolVersion = pr.GetInt32();
			int maxConferences = pr.GetInt32();
			int activeConferences = pr.GetInt32();
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessRegister( deviceName, instance, ipAddr, deviceType,
				maxStreams, activeStreams, protocolVersion, maxConferences,
				activeConferences );
		}

		private const int RegisterDeviceNameLen = 16;

		virtual protected bool DoProcessRegister( String deviceName, int instance,
			int ipAddr, int deviceType, int maxStreams, int activeStreams,
			int protocolVersion, int maxConferences, int activeConferences )
		{
			throw new InvalidOperationException( "DoProcessRegister not implemented" );
		}

		protected bool ProcessUnregister( PacketReader pr )
		{
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessUnregister();
		}

		virtual protected bool DoProcessUnregister()
		{
			throw new InvalidOperationException( "DoProcessUnregister not implemented" );
		}

		protected bool ProcessKeepalive( PacketReader pr )
		{
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessKeepalive();
		}

		virtual protected bool DoProcessKeepalive()
		{
			throw new InvalidOperationException( "DoProcessKeepalive not implemented" );
		}

		protected bool ProcessButtonTemplateReq( PacketReader pr )
		{
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessButtonTemplateReq();
		}

		virtual protected bool DoProcessButtonTemplateReq()
		{
			throw new InvalidOperationException( "DoProcessButtonTemplateReq not implemented" );
		}

		protected bool ProcessSoftkeyTemplateReq( PacketReader pr )
		{
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessSoftkeyTemplateReq();
		}

		virtual protected bool DoProcessSoftkeyTemplateReq()
		{
			throw new InvalidOperationException( "DoProcessSoftkeyTemplateReq not implemented" );
		}

		protected bool ProcessSoftkeySetReq( PacketReader pr )
		{
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessSoftkeySetReq();
		}

		virtual protected bool DoProcessSoftkeySetReq()
		{
			throw new InvalidOperationException( "DoProcessSoftkeySetReq not implemented" );
		}

		protected bool ProcessLineStatReq( PacketReader pr )
		{
			int lineNumber = pr.GetInt32();
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessLineStatReq( lineNumber );
		}

		virtual protected bool DoProcessLineStatReq( int lineNumber )
		{
			throw new InvalidOperationException( "DoProcessLineStatReq not implemented" );
		}

		protected bool ProcessSpeeddialStatReq( PacketReader pr )
		{
			int number = pr.GetInt32();
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessSpeeddialStatReq( number );
		}

		virtual protected bool DoProcessSpeeddialStatReq( int number )
		{
			throw new InvalidOperationException( "DoProcessSpeeddialStatReq not implemented" );
		}

		protected bool ProcessRegisterAvailableLines( PacketReader pr )
		{
			int number = pr.GetInt32();
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessRegisterAvailableLines( number );
		}

		virtual protected bool DoProcessRegisterAvailableLines( int number )
		{
			throw new InvalidOperationException( "DoProcessRegisterAvailableLines not implemented" );
		}

		protected bool ProcessTimeDateReq( PacketReader pr )
		{
			Assertion.Check( pr.Remaining == 0, "pr.Remaining == 0" );

			return DoProcessTimeDateReq();
		}

		virtual protected bool DoProcessTimeDateReq()
		{
			throw new InvalidOperationException( "DoProcessTimeDateReq not implemented" );
		}

		public void SendRegisterAck( int primaryAckInterval, String dateFormat,
			int secondaryAckInterval, uint flags )
		{
			log.Write( TraceLevel.Verbose, "{0} sending RegisterAck", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.RegisterAck );
			pw.PutInt32( primaryAckInterval );
			pw.PutString( dateFormat, RegisterAckDateFormatLen );
			pw.PutBytes(new byte[RegisterAckDateFormatLen % 4]);	// Force 32-bit alignment on the following 32-bit integer.
			pw.PutInt32( secondaryAckInterval );
			pw.PutInt32( flags );
			Send( pw );
		}

		private const int RegisterAckDateFormatLen = 6;

		public void SendCapabilitiesReq()
		{
			log.Write( TraceLevel.Verbose, "{0} sending CapabilitiesReq", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.CapabilitiesReq );
			Send( pw );
		}

		public void SendRegisterReject( String msg )
		{
			log.Write( TraceLevel.Verbose, "{0} sending RegisterReject", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.RegisterReject );
			pw.PutString( msg, RegisterRejectMsgLen );
			Send( pw );
		}

		private const int RegisterRejectMsgLen = 32;

		public void SendKeepaliveAck()
		{
			log.Write( TraceLevel.Verbose, "{0} sending KeepaliveAck", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.KeepaliveAck );
			Send( pw );
		}

		public void SendUnregisterAck( int sts )
		{
			log.Write( TraceLevel.Verbose, "{0} sending UnregisterAck", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.UnregisterAck );
			pw.PutInt32( sts ); // ok=0, error=1, nak=2
			Send( pw );
		}

		public void SendButtonTemplate( int offset, int count, ButtonTemplate[] bts )
		{
			log.Write( TraceLevel.Verbose, "{0} sending ButtonTemplate", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.ButtonTemplate );
			pw.PutInt32( offset );
			pw.PutInt32( count );
			pw.PutInt32( bts.Length );
			foreach (ButtonTemplate bt in bts)
			{
				if (pw.Length >= ButtonTemplatePacketLen)
					break;

				bt.Put( pw );
			}
			while (pw.Length < ButtonTemplatePacketLen)
				ButtonTemplate.PutEmpty( pw );
			Send( pw );
		}

		private const int ButtonTemplatePacketLen = 100; // max 42 definitions

		public void SendSoftkeyTemplateRes( int offset, int count, SoftkeyTemplate[] sks )
		{
			log.Write( TraceLevel.Verbose, "{0} sending SoftkeyTemplateRes", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.SoftkeyTemplateRes );
			pw.PutInt32( offset );
			pw.PutInt32( count );
			pw.PutInt32( sks.Length );
			foreach (SoftkeyTemplate sk in sks)
			{
				if (pw.Length >= SoftkeyTemplateResPacketLen)
					break;

				sk.Put( pw );
			}
			while (pw.Length < SoftkeyTemplateResPacketLen)
				SoftkeyTemplate.PutEmpty( pw );
			Send( pw );
		}

		private const int SoftkeyTemplateResPacketLen = 656; // max 32 definitions

		public void SendSoftkeySetRes( int offset, int count, SoftkeySet[] sss )
		{
			log.Write( TraceLevel.Verbose, "{0} sending SoftkeySetRes", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.SoftkeySetRes );
			pw.PutInt32( offset );
			pw.PutInt32( count );
			pw.PutInt32( sss.Length );
			foreach (SoftkeySet ss in sss)
			{
				if (pw.Length >= SoftkeySetResPacketLen)
					break;

				ss.Put( pw );
			}
			while (pw.Length < SoftkeySetResPacketLen)
				SoftkeySet.PutEmpty( pw );
			Send( pw );
		}

		private const int SoftkeySetResPacketLen = 784;

		public void SendLineStat( int lineNumber, String directoryNumber, String displayName,
			String label, int options )
		{
			log.Write( TraceLevel.Verbose, "{0} sending LineStat", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.LineStat );
			pw.PutInt32( lineNumber );
			pw.PutString( directoryNumber, LineStatDirectoryNumberLen );
			pw.PutString( displayName, LineStatDisplayNameLen );
			pw.PutString( label, LineStatLabelLen );
			pw.PutInt32( options );
			Send( pw );
		}

		private const int LineStatDirectoryNumberLen = 24;

		private const int LineStatDisplayNameLen = 40;

		private const int LineStatLabelLen = 40;

		public void SendSpeeddialStat( int number, String directoryNumber, String displayName )
		{
			log.Write( TraceLevel.Verbose, "{0} sending SpeeddialStat", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.SpeeddialStat );
			pw.PutInt32( number );
			pw.PutString( directoryNumber, SpeeddialStatDirectoryNumberLen );
			pw.PutString( displayName, SpeeddialStatDisplayNameLen );
			Send( pw );
		}

		private const int SpeeddialStatDirectoryNumberLen = 24;

		private const int SpeeddialStatDisplayNameLen = 40;

		public void SendDefineTimeDate( int year, int month, int dayOfWeek, int day,
			int hour, int minute, int second, int millisecond, int systemTime )
		{
			log.Write( TraceLevel.Verbose, "{0} sending DefineTimeDate", this );
			PacketWriter pw = new PacketWriter();
			pw.PutInt32( Cmd.DefineTimeDate );
			pw.PutInt32( year );
			pw.PutInt32( month );
			pw.PutInt32( dayOfWeek );
			pw.PutInt32( day );
			pw.PutInt32( hour );
			pw.PutInt32( minute );
			pw.PutInt32( second );
			pw.PutInt32( millisecond );
			pw.PutInt32( systemTime );
			Send( pw );
		}

		public void Send( PacketWriter pw )
		{
			byte[] msg = pw.ToByteArray();

			pw = new PacketWriter();
			pw.PutInt32( msg.Length );
			pw.PutInt32( 0 ); // reserved
			pw.PutBytes( msg );

			key.Send( pw.ToByteArray() );
		}
	}

	class ButtonTemplate
	{
		public ButtonTemplate( int id, int type )
		{
			this.id = id; // 1, 2, ...
			this.type = type; // 9 = line, 2 = speed dial
		}

		private int id;
		
		private int type;

		public void Put( PacketWriter pw )
		{
			pw.PutByte( id );
			pw.PutByte( type );
		}

		public static void PutEmpty( PacketWriter pw )
		{
			pw.PutByte( 0 ); // id
			pw.PutByte( 255 ); // type
		}
	}

	class SoftkeyTemplate
	{
		public SoftkeyTemplate( String label, uint evnt )
		{
			this.label = label;
			this.evnt = evnt;
		}

		public String label;
		
		public uint evnt;

		public void Put( PacketWriter pw )
		{
			pw.PutString( label, LabelSize );
			pw.PutInt32( evnt );
		}

		public static void PutEmpty( PacketWriter pw )
		{
			pw.PutString( "", LabelSize );
			pw.PutInt32( 0 );
		}

		private const int LabelSize = 16;
	}

	class SoftkeySet
	{
		public SoftkeySet( byte[] templateIndex, byte[] infoIndex )
		{
			this.templateIndex = templateIndex;
			this.infoIndex = infoIndex;
		}

		public byte[] templateIndex;
		
		public byte[] infoIndex;

		public void Put( PacketWriter pw )
		{
			pw.PutBytes( templateIndex, IndexLen );
			pw.PutBytes( infoIndex, IndexLen );
		}

		public static void PutEmpty( PacketWriter pw )
		{
			byte[] b = new byte[0];
			pw.PutBytes( b, IndexLen );
			pw.PutBytes( b, IndexLen );
		}

		public const int IndexLen = 16;
	}

	class SccpServer: SccpPacket
	{
		public SccpServer( Log log, SelectorBase selector, SccpPolicy policy )
			: base( log, selector, policy )
		{
			// nothing else to do.
		}

		public void Start( Socket socket )
		{
			if (key != null)
				throw new InvalidOperationException( "already started" );

			key = selector.Register( socket, null,
				new Metreos.Utilities.Selectors.SelectedDelegate( Selected ),
				new Metreos.Utilities.Selectors.SelectedExceptionDelegate( SelectedException ) );

			key.WantsRead = true;
		}

		public void Stop()
		{
			Close();
		}

		override protected bool Close()
		{
			if (base.Close())
			{
				policy.Unregister( this );
				return true;
			}
			return false;
		}


		override protected bool ProcessCommand( int cmd, PacketReader pr )
		{
			switch (cmd)
			{
				case Cmd.Register:               return ProcessRegister( pr );
				case Cmd.Unregister:             return ProcessUnregister( pr );
				case Cmd.Keepalive:              return ProcessKeepalive( pr );
				case Cmd.ButtonTemplateReq:      return ProcessButtonTemplateReq( pr );
				case Cmd.SoftkeyTemplateReq:     return ProcessSoftkeyTemplateReq( pr );
				case Cmd.SoftkeySetReq:          return ProcessSoftkeySetReq( pr );
				case Cmd.LineStatReq:            return ProcessLineStatReq( pr );
				case Cmd.SpeeddialStatReq:       return ProcessSpeeddialStatReq( pr );
				case Cmd.RegisterAvailableLines: return ProcessRegisterAvailableLines( pr );
				case Cmd.TimeDateReq:            return ProcessTimeDateReq( pr );
				case Cmd.Alarm:                  return true;
				case Cmd.HeadsetStatus:          return true;
				case Cmd.CapabilitiesRes:        return true;
				case Cmd.IpPort:                 return true;
				case Cmd.Onhook:                 return true;
				default:						 return base.ProcessCommand( cmd, pr );
			}
		}

		override protected bool DoProcessRegister( String deviceName, int instance, int ipAddr,
			int deviceType, int maxStreams, int activeStreams, int protocolVersion,
			int maxConferences, int activeConferences)
		{
			log.Write( TraceLevel.Verbose,
				"{0} Register {1} instance {3} ipAddr {4} deviceType {5} maxStreams {6} activeStreams {7} protocolVersion {8} maxConferences {9} activeConferences {10}",
				this, deviceName, null, instance, ipAddr.ToString( "x" ), deviceType, maxStreams,
				activeStreams, protocolVersion.ToString( "x" ), maxConferences, activeConferences );

			if (OkToRegister( deviceName, ipAddr ))
			{
				SendRegisterAck( 30, "M/D/Y", 60, 0xfc000005 );
				SendCapabilitiesReq();
				return true;
			}

			SendRegisterReject( "hate ya, neener neener!" );
			return false;
		}

		private bool OkToRegister( String deviceName, int ipAddr )
		{
			this.deviceName = deviceName;
			this.ipAddr = ipAddr;
			return policy.OkToRegister( this );
		}

		public String DeviceName
		{
			get { return deviceName; }
		}

		private String deviceName;

		public int IpAddr
		{
			get { return ipAddr; }
		}

		private int ipAddr;

		override protected bool DoProcessUnregister()
		{
			int sts = policy.Unregister( this );
			this.deviceName = null;
			this.ipAddr = 0;
			SendUnregisterAck( sts );
			return false;
		}

		override protected bool DoProcessButtonTemplateReq()
		{
			SendButtonTemplate( 0, 1, new ButtonTemplate[] { new ButtonTemplate( 1, 9 ) } );
			return true;
		}

		override protected bool DoProcessKeepalive()
		{
			SendKeepaliveAck();
			return true;
		}

		override protected bool DoProcessLineStatReq(int lineNumber)
		{
			LineStat ls = policy.GetLineStat( this, lineNumber );
			SendLineStat( lineNumber, ls.directoryNumber, ls.displayName, ls.label, ls.flags );
			return true;
		}

		override protected bool DoProcessSoftkeySetReq()
		{
			SendSoftkeySetRes( 0, 0, new SoftkeySet[] {} );
			return true;
		}

		override protected bool DoProcessSoftkeyTemplateReq()
		{
			SendSoftkeyTemplateRes( 0, 0, new SoftkeyTemplate[] {} );
			return true;
		}

		override protected bool DoProcessSpeeddialStatReq( int number )
		{
			SendSpeeddialStat( number, (9000+number).ToString(), "sd"+number );
			return true;
		}

		override protected bool DoProcessRegisterAvailableLines( int number )
		{
			return true;
		}

		override protected bool DoProcessTimeDateReq()
		{
			DateTime now = DateTime.Now;
			SendDefineTimeDate( now.Year, now.Month, (int) now.DayOfWeek,
				now.Day, now.Hour, now.Minute, now.Second, now.Millisecond,
				(int)(DateTime.Now.Ticks/10000) );
			return true;
		}
	}

	class LineStat
	{
		public LineStat( String directoryNumber, String displayName, String label, int flags )
		{
			this.directoryNumber = directoryNumber;
			this.displayName = displayName;
			this.label = label;
			this.flags = flags;
		}

		public String directoryNumber;
		public String displayName;
		public String label;
		public int flags;
	}

	class PacketReader
	{
		public PacketReader( byte[] buf, int offset, int length )
		{
			this.buf = buf;
			this.offset = offset;
			this.length = length;
		}

		private byte[] buf;
		
		private int offset;
		
		private int length;

		private int index;

		public int Remaining
		{
			get { return length - index; }
		}

		public int GetByte()
		{
			Assertion.Check( Remaining >= 1, "Remaining >= 1" );
			int value = buf[offset+index] & 0xff;
			index += 1;
			return value;
		}

		public byte[] GetBytes( int len )
		{
			Assertion.Check( Remaining >= len, "Remaining >= len" );
			byte[] value = new byte[len];
			Array.Copy( buf, offset+index, value, 0, len );
			index += len;
			return value;
		}

		public int GetInt32()
		{
			Assertion.Check( Remaining >= 4, "Remaining >= 4" );
			int value = BitConverter.ToInt32( buf, offset + index );
			index += 4;
			return value;
		}

		public String GetString( int len )
		{
			Assertion.Check( Remaining >= len, "Remaining >= len" );

			StringBuilder sb = new StringBuilder();
			
			int c;
			while (len-- > 0 && (c = GetByte()) != 0)
				sb.Append( (char) c );
			
			while (len-- > 0)
				GetByte();

			return sb.ToString();
		}
	}

	class PacketWriter
	{
		private MemoryStream ms = new MemoryStream();

		public int Length { get { return (int) ms.Length; } }

		public void PutByte( int value )
		{
			ms.WriteByte( (byte)(value & 255) );
		}

		public void PutBytes( byte[] bytes )
		{
			ms.Write( bytes, 0, bytes.Length );
		}

		public void PutBytes( byte[] value, int len )
		{
			Assertion.Check( value.Length <= len, "value.Length <= len" );
			foreach (byte b in value)
			{
				PutByte( b );
				len--;
			}

			while (len-- > 0)
				PutByte( 0 );
		}

		public void PutInt32( int value )
		{
			PutBytes( BitConverter.GetBytes( value ) );
		}

		public void PutInt32( uint value )
		{
			PutBytes( BitConverter.GetBytes( value ) );
		}

		public void PutString( String value, int len )
		{
			Assertion.Check( value.Length <= len, "value.Length <= len" );
			foreach (char c in value)
			{
				PutByte( c );
				len--;
			}

			while (len-- > 0)
				PutByte( 0 );
		}

		public byte[] ToByteArray()
		{
			return ms.ToArray();
		}
	}

	class Log
	{
		public Log( TraceLevel level )
		{
			this.level = level;
		}

		private TraceLevel level;

		public bool Enabled( TraceLevel msgLevel )
		{
			return msgLevel.CompareTo( level ) <= 0;
		}

		public void Write( TraceLevel msgLevel, String msg )
		{
			if (Enabled( msgLevel ))
				WriteMsg( msgLevel, msg );
		}

		public void Write( TraceLevel msgLevel, String format, params Object[] args )
		{
			if (Enabled( msgLevel ))
			{
				String msg = String.Format( format, args );
				WriteMsg( msgLevel, msg );
			}
		}

		private void WriteMsg( TraceLevel msgLevel, String msg )
		{
			lock (this)
			{
				DateTime dt = DateTime.Now;
				Console.WriteLine( "{0} {1} {2}",
					DateTime.Now.ToString( "HH:mm:ss.fff" ),
					msgLevel.ToString().Substring( 0, 1 ),
					msg );
			}
		}
	}
}
