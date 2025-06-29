// $ANTLR 2.7.4: "MMSScript.g" -> "MMSScriptLexer.cs"$

    // gets inserted in the C# source file before any
    // generated namespace declarations
    // hence -- can only be using directives
	using System.IO;
	using System.Collections;
	using Metreos.MMSTestTool.Commands;
	using Metreos.MMSTestTool.Sessions;
	
	using AST                      = antlr.collections.AST;
	using CommonAST				   = antlr.CommonAST;

namespace Metreos.MMSTestTool.Parser
{
	// Generate header specific to lexer CSharp file
	using System;
	using Stream                          = System.IO.Stream;
	using TextReader                      = System.IO.TextReader;
	using Hashtable                       = System.Collections.Hashtable;
	using Comparer                        = System.Collections.Comparer;
	using CaseInsensitiveHashCodeProvider = System.Collections.CaseInsensitiveHashCodeProvider;
	using CaseInsensitiveComparer         = System.Collections.CaseInsensitiveComparer;
	
	using TokenStreamException            = antlr.TokenStreamException;
	using TokenStreamIOException          = antlr.TokenStreamIOException;
	using TokenStreamRecognitionException = antlr.TokenStreamRecognitionException;
	using CharStreamException             = antlr.CharStreamException;
	using CharStreamIOException           = antlr.CharStreamIOException;
	using ANTLRException                  = antlr.ANTLRException;
	using CharScanner                     = antlr.CharScanner;
	using InputBuffer                     = antlr.InputBuffer;
	using ByteBuffer                      = antlr.ByteBuffer;
	using CharBuffer                      = antlr.CharBuffer;
	using Token                           = antlr.Token;
	using CommonToken                     = antlr.CommonToken;
	using SemanticException               = antlr.SemanticException;
	using RecognitionException            = antlr.RecognitionException;
	using NoViableAltForCharException     = antlr.NoViableAltForCharException;
	using MismatchedCharException         = antlr.MismatchedCharException;
	using TokenStream                     = antlr.TokenStream;
	using LexerSharedInputState           = antlr.LexerSharedInputState;
	using BitSet                          = antlr.collections.impl.BitSet;
	
	public 	class MMSScriptLexer : antlr.CharScanner	, TokenStream
	 {
		public const int EOF = 1;
		public const int NULL_TREE_LOOKAHEAD = 3;
		public const int NFixture = 4;
		public const int NScript = 5;
		public const int NCommand = 6;
		public const int NParameter = 7;
		public const int NReqParameter = 8;
		public const int NParameterList = 9;
		public const int NAssert = 10;
		public const int NProvisionalAssert = 11;
		public const int NAssertList = 12;
		public const int NFile = 13;
		public const int NCommandOption = 14;
		public const int NScriptOption = 15;
		public const int TEST_FIXTURE_LITERAL = 16;
		public const int ASSIGN = 17;
		public const int ID = 18;
		public const int FWDCURLY = 19;
		public const int BACKCURLY = 20;
		public const int TESTSCRIPT_LITERAL = 21;
		public const int COMMAND_TIMEOUT = 22;
		public const int WAIT_FOR_FINAL = 23;
		public const int TRUE = 24;
		public const int FALSE = 25;
		public const int PROVASSERT_LITERAL = 26;
		public const int RESULT_CODE = 27;
		public const int COMPARE_OPERATOR = 28;
		public const int CONNECT_LITERAL = 29;
		public const int IP_ADDRESS = 30;
		public const int PORT = 31;
		public const int CONNECTION_ID = 32;
		public const int CONFERENCE_ID = 33;
		public const int SESSION_TIMEOUT = 34;
		public const int TRANSACTION_ID = 35;
		public const int CLIENT_ID = 36;
		public const int FINALASSERTS_LITERAL = 37;
		public const int DISCONNECT_LITERAL = 38;
		public const int PLAY_LITERAL = 39;
		public const int RECORD_LITERAL = 40;
		public const int FILENAME = 41;
		public const int EXPIRES = 42;
		public const int AUDIO_FILE_ATTRIBUTE = 43;
		public const int TERMINATION_CONDITION = 44;
		public const int MAXTIME = 45;
		public const int DIGIT = 46;
		public const int DIGITLIST = 47;
		public const int MAXDIGITS = 48;
		public const int SILENCE = 49;
		public const int NONSILENCE = 50;
		public const int DIGITDELAY = 51;
		public const int DIGITPATTERN = 52;
		public const int LOOPCURRENT = 53;
		public const int INTERDIGDELAY = 54;
		public const int USERSTOP = 55;
		public const int EOD = 56;
		public const int TONE = 57;
		public const int DEVICEERROR = 58;
		public const int MAXDATA = 59;
		public const int CODERTYPEREMOTE = 60;
		public const int FRAMESIZEREMOTE = 61;
		public const int VADENABLEREMOTE = 62;
		public const int DATAFLOWDIRECTION = 63;
		public const int CODERTYPELOCAL = 64;
		public const int FRAMESIZELOCAL = 65;
		public const int VADENABLELOCAL = 66;
		public const int SOUND_TONE = 67;
		public const int NO_TONE = 68;
		public const int SOUND_TONE_WHEN_RECEIVE_ONLY = 69;
		public const int NO_TONE_WHEN_RECEIVE_ONLY = 70;
		public const int MONITOR = 71;
		public const int RECEIVE_ONLY = 72;
		public const int TARIFF_TONE = 73;
		public const int COACH = 74;
		public const int PUPIL = 75;
		public const int FORMAT = 76;
		public const int ENCODING = 77;
		public const int BITRATE = 78;
		public const int SERVER_ID = 79;
		public const int HEARTBEAT_ID = 80;
		public const int CONNECTION_ATTRIBUTE = 81;
		public const int CONFERENCE_ATTRIBUTE = 82;
		public const int CONFEREE_ATTRIBUTE = 83;
		public const int MEDIA_ELAPSEDTIME = 84;
		public const int HEARTBEAT_INTERVAL = 85;
		public const int HEARTBEAT_PAYLOAD = 86;
		public const int MACHINE_NAME = 87;
		public const int QUEUE_NAME = 88;
		public const int CALL_STATE = 89;
		public const int DIGITS = 90;
		public const int QUERY = 91;
		public const int ASSIGNMENT = 92;
		public const int COMPARISON = 93;
		public const int PLUS = 94;
		public const int MINUS = 95;
		public const int TIMES = 96;
		public const int DIV = 97;
		public const int COLON = 98;
		public const int SEMI = 99;
		public const int COMMA = 100;
		public const int LBRACKET = 101;
		public const int RBRACKET = 102;
		public const int LPAREN = 103;
		public const int RPAREN = 104;
		public const int STRING = 105;
		public const int QUOTED_STRING = 106;
		public const int WS = 107;
		
		public MMSScriptLexer(Stream ins) : this(new ByteBuffer(ins))
		{
		}
		
		public MMSScriptLexer(TextReader r) : this(new CharBuffer(r))
		{
		}
		
		public MMSScriptLexer(InputBuffer ib)		 : this(new LexerSharedInputState(ib))
		{
		}
		
		public MMSScriptLexer(LexerSharedInputState state) : base(state)
		{
			initialize();
		}
		private void initialize()
		{
			caseSensitiveLiterals = false;
			setCaseSensitive(true);
			literals = new Hashtable(100, (float) 0.4, CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
			literals.Add("filename", 41);
			literals.Add("mediaElapsedTime", 84);
			literals.Add("Connect", 29);
			literals.Add("framesizeLocal", 65);
			literals.Add("NFile", 13);
			literals.Add("pupil", 75);
			literals.Add("ProvisionalAsserts", 26);
			literals.Add("loopcurrent", 53);
			literals.Add("format", 76);
			literals.Add("TestScript", 21);
			literals.Add("dataflowDirection", 63);
			literals.Add("NAssertList", 12);
			literals.Add("coderTypeRemote", 60);
			literals.Add("maxdigits", 48);
			literals.Add("userstop", 55);
			literals.Add("machineName", 87);
			literals.Add("noToneWhenReceiveOnly", 70);
			literals.Add("NParameter", 7);
			literals.Add("Disconnect", 38);
			literals.Add("digits", 90);
			literals.Add("FinalAsserts", 37);
			literals.Add("conferenceAttribute", 82);
			literals.Add("conferenceId", 33);
			literals.Add("query", 91);
			literals.Add("heartbeatId", 80);
			literals.Add("Record", 40);
			literals.Add("heartbeatPayload", 86);
			literals.Add("Play", 39);
			literals.Add("ipAddress", 30);
			literals.Add("monitor", 71);
			literals.Add("maxdata", 59);
			literals.Add("transactionId", 35);
			literals.Add("framesizeRemote", 61);
			literals.Add("noTone", 68);
			literals.Add("resultCode", 27);
			literals.Add("digitpattern", 52);
			literals.Add("terminationCondition", 44);
			literals.Add("callState", 89);
			literals.Add("interdigdelay", 54);
			literals.Add("clientId", 36);
			literals.Add("commandTimeout", 22);
			literals.Add("coach", 74);
			literals.Add("digitlist", 47);
			literals.Add("heartbeatInterval", 85);
			literals.Add("connectionId", 32);
			literals.Add("vadEnableLocal", 66);
			literals.Add("nonsilence", 50);
			literals.Add("eod", 56);
			literals.Add("digitdelay", 51);
			literals.Add("NAssert", 10);
			literals.Add("coderTypeLocal", 64);
			literals.Add("vadEnableRemote", 62);
			literals.Add("waitForFinal", 23);
			literals.Add("TestFixture", 16);
			literals.Add("bitrate", 78);
			literals.Add("connectionAttribute", 81);
			literals.Add("port", 31);
			literals.Add("false", 25);
			literals.Add("digit", 46);
			literals.Add("soundTone", 67);
			literals.Add("sessionTimeout", 34);
			literals.Add("receiveOnly", 72);
			literals.Add("soundToneWhenReceiveOnly", 69);
			literals.Add("silence", 49);
			literals.Add("confereeAttribute", 83);
			literals.Add("tone", 57);
			literals.Add("audioFileAttribute", 43);
			literals.Add("encoding", 77);
			literals.Add("expires", 42);
			literals.Add("maxtime", 45);
			literals.Add("tariffTone", 73);
			literals.Add("true", 24);
			literals.Add("serverId", 79);
			literals.Add("deviceerror", 58);
			literals.Add("queueName", 88);
		}
		
		override public Token nextToken()			//throws TokenStreamException
		{
			Token theRetToken = null;
tryAgain:
			for (;;)
			{
				Token _token = null;
				int _ttype = Token.INVALID_TYPE;
				resetText();
				try     // for char stream error handling
				{
					try     // for lexical error handling
					{
						switch ( LA(1) )
						{
						case '+':
						{
							mPLUS(true);
							theRetToken = returnToken_;
							break;
						}
						case '-':
						{
							mMINUS(true);
							theRetToken = returnToken_;
							break;
						}
						case '*':
						{
							mTIMES(true);
							theRetToken = returnToken_;
							break;
						}
						case '/':
						{
							mDIV(true);
							theRetToken = returnToken_;
							break;
						}
						case ':':
						{
							mCOLON(true);
							theRetToken = returnToken_;
							break;
						}
						case ';':
						{
							mSEMI(true);
							theRetToken = returnToken_;
							break;
						}
						case ',':
						{
							mCOMMA(true);
							theRetToken = returnToken_;
							break;
						}
						case '!':  case '<':  case '>':
						{
							mCOMPARE_OPERATOR(true);
							theRetToken = returnToken_;
							break;
						}
						case '=':
						{
							mASSIGN(true);
							theRetToken = returnToken_;
							break;
						}
						case '[':
						{
							mLBRACKET(true);
							theRetToken = returnToken_;
							break;
						}
						case ']':
						{
							mRBRACKET(true);
							theRetToken = returnToken_;
							break;
						}
						case '(':
						{
							mLPAREN(true);
							theRetToken = returnToken_;
							break;
						}
						case ')':
						{
							mRPAREN(true);
							theRetToken = returnToken_;
							break;
						}
						case '{':
						{
							mFWDCURLY(true);
							theRetToken = returnToken_;
							break;
						}
						case '}':
						{
							mBACKCURLY(true);
							theRetToken = returnToken_;
							break;
						}
						case '"':  case '$':  case '.':  case '0':
						case '1':  case '2':  case '3':  case '4':
						case '5':  case '6':  case '7':  case '8':
						case '9':  case 'A':  case 'B':  case 'C':
						case 'D':  case 'E':  case 'F':  case 'G':
						case 'H':  case 'I':  case 'J':  case 'K':
						case 'L':  case 'M':  case 'N':  case 'O':
						case 'P':  case 'Q':  case 'R':  case 'S':
						case 'T':  case 'U':  case 'V':  case 'W':
						case 'X':  case 'Y':  case 'Z':  case '_':
						case 'a':  case 'b':  case 'c':  case 'd':
						case 'e':  case 'f':  case 'g':  case 'h':
						case 'i':  case 'j':  case 'k':  case 'l':
						case 'm':  case 'n':  case 'o':  case 'p':
						case 'q':  case 'r':  case 's':  case 't':
						case 'u':  case 'v':  case 'w':  case 'x':
						case 'y':  case 'z':
						{
							mID(true);
							theRetToken = returnToken_;
							break;
						}
						case '\t':  case '\n':  case '\r':  case ' ':
						{
							mWS(true);
							theRetToken = returnToken_;
							break;
						}
						default:
						{
							if (LA(1)==EOF_CHAR) { uponEOF(); returnToken_ = makeToken(Token.EOF_TYPE); }
				else {throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());}
						}
						break; }
						if ( null==returnToken_ ) goto tryAgain; // found SKIP token
						_ttype = returnToken_.Type;
						_ttype = testLiteralsTable(_ttype);
						returnToken_.Type = _ttype;
						return returnToken_;
					}
					catch (RecognitionException e) {
							throw new TokenStreamRecognitionException(e);
					}
				}
				catch (CharStreamException cse) {
					if ( cse is CharStreamIOException ) {
						throw new TokenStreamIOException(((CharStreamIOException)cse).io);
					}
					else {
						throw new TokenStreamException(cse.Message);
					}
				}
			}
		}
		
	public void mPLUS(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = PLUS;
		
		match('+');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mMINUS(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = MINUS;
		
		match('-');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mTIMES(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = TIMES;
		
		match('*');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mDIV(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = DIV;
		
		match('/');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mCOLON(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = COLON;
		
		match(':');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mSEMI(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = SEMI;
		
		match(';');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mCOMMA(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = COMMA;
		
		match(',');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mCOMPARE_OPERATOR(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = COMPARE_OPERATOR;
		
		{
			if ((LA(1)=='>') && (LA(2)=='='))
			{
				match(">=");
			}
			else if ((LA(1)=='<') && (LA(2)=='=')) {
				match("<=");
			}
			else if ((LA(1)=='!')) {
				match("!=");
			}
			else if ((LA(1)=='<') && (true)) {
				match('<');
			}
			else if ((LA(1)=='>') && (true)) {
				match('>');
			}
			else
			{
				throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
			}
			
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mASSIGN(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = ASSIGN;
		
		match('=');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mLBRACKET(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = LBRACKET;
		
		match('[');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mRBRACKET(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = RBRACKET;
		
		match(']');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mLPAREN(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = LPAREN;
		
		match('(');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mRPAREN(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = RPAREN;
		
		match(')');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mFWDCURLY(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = FWDCURLY;
		
		match('{');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mBACKCURLY(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = BACKCURLY;
		
		match('}');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mID(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = ID;
		
		{
			switch ( LA(1) )
			{
			case '$':  case '.':  case '0':  case '1':
			case '2':  case '3':  case '4':  case '5':
			case '6':  case '7':  case '8':  case '9':
			case 'A':  case 'B':  case 'C':  case 'D':
			case 'E':  case 'F':  case 'G':  case 'H':
			case 'I':  case 'J':  case 'K':  case 'L':
			case 'M':  case 'N':  case 'O':  case 'P':
			case 'Q':  case 'R':  case 'S':  case 'T':
			case 'U':  case 'V':  case 'W':  case 'X':
			case 'Y':  case 'Z':  case '_':  case 'a':
			case 'b':  case 'c':  case 'd':  case 'e':
			case 'f':  case 'g':  case 'h':  case 'i':
			case 'j':  case 'k':  case 'l':  case 'm':
			case 'n':  case 'o':  case 'p':  case 'q':
			case 'r':  case 's':  case 't':  case 'u':
			case 'v':  case 'w':  case 'x':  case 'y':
			case 'z':
			{
				mSTRING(false);
				break;
			}
			case '"':
			{
				mQUOTED_STRING(false);
				break;
			}
			default:
			{
				throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
			}
			 }
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mSTRING(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = STRING;
		
		{
			{ // ( ... )+
			int _cnt157=0;
			for (;;)
			{
				switch ( LA(1) )
				{
				case 'a':  case 'b':  case 'c':  case 'd':
				case 'e':  case 'f':  case 'g':  case 'h':
				case 'i':  case 'j':  case 'k':  case 'l':
				case 'm':  case 'n':  case 'o':  case 'p':
				case 'q':  case 'r':  case 's':  case 't':
				case 'u':  case 'v':  case 'w':  case 'x':
				case 'y':  case 'z':
				{
					matchRange('a','z');
					break;
				}
				case 'A':  case 'B':  case 'C':  case 'D':
				case 'E':  case 'F':  case 'G':  case 'H':
				case 'I':  case 'J':  case 'K':  case 'L':
				case 'M':  case 'N':  case 'O':  case 'P':
				case 'Q':  case 'R':  case 'S':  case 'T':
				case 'U':  case 'V':  case 'W':  case 'X':
				case 'Y':  case 'Z':
				{
					matchRange('A','Z');
					break;
				}
				case '_':
				{
					match('_');
					break;
				}
				case '0':  case '1':  case '2':  case '3':
				case '4':  case '5':  case '6':  case '7':
				case '8':  case '9':
				{
					matchRange('0','9');
					break;
				}
				case '.':
				{
					match('.');
					break;
				}
				case '$':
				{
					match('$');
					break;
				}
				default:
				{
					if (_cnt157 >= 1) { goto _loop157_breakloop; } else { throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());; }
				}
				break; }
				_cnt157++;
			}
_loop157_breakloop:			;
			}    // ( ... )+
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mQUOTED_STRING(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = QUOTED_STRING;
		
		int _saveIndex = 0;
		_saveIndex = text.Length;
		match('"');
		text.Length = _saveIndex;
		{    // ( ... )*
			for (;;)
			{
				switch ( LA(1) )
				{
				case 'a':  case 'b':  case 'c':  case 'd':
				case 'e':  case 'f':  case 'g':  case 'h':
				case 'i':  case 'j':  case 'k':  case 'l':
				case 'm':  case 'n':  case 'o':  case 'p':
				case 'q':  case 'r':  case 's':  case 't':
				case 'u':  case 'v':  case 'w':  case 'x':
				case 'y':  case 'z':
				{
					matchRange('a','z');
					break;
				}
				case 'A':  case 'B':  case 'C':  case 'D':
				case 'E':  case 'F':  case 'G':  case 'H':
				case 'I':  case 'J':  case 'K':  case 'L':
				case 'M':  case 'N':  case 'O':  case 'P':
				case 'Q':  case 'R':  case 'S':  case 'T':
				case 'U':  case 'V':  case 'W':  case 'X':
				case 'Y':  case 'Z':
				{
					matchRange('A','Z');
					break;
				}
				case '_':
				{
					match('_');
					break;
				}
				case '0':  case '1':  case '2':  case '3':
				case '4':  case '5':  case '6':  case '7':
				case '8':  case '9':
				{
					matchRange('0','9');
					break;
				}
				case ' ':
				{
					match(' ');
					break;
				}
				case '\t':
				{
					match('\t');
					break;
				}
				case '.':
				{
					match('.');
					break;
				}
				case '$':
				{
					match('$');
					break;
				}
				default:
				{
					goto _loop160_breakloop;
				}
				 }
			}
_loop160_breakloop:			;
		}    // ( ... )*
		_saveIndex = text.Length;
		match('"');
		text.Length = _saveIndex;
		_ttype = testLiteralsTable(text.ToString(_begin, text.Length-_begin), _ttype);
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mWS(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = WS;
		
		{
			switch ( LA(1) )
			{
			case ' ':
			{
				match(' ');
				break;
			}
			case '\t':
			{
				match('\t');
				break;
			}
			default:
				if ((LA(1)=='\r') && (LA(2)=='\n'))
				{
					match("\r\n");
					newline();
				}
				else if ((LA(1)=='\n'||LA(1)=='\r') && (true)) {
					{
						switch ( LA(1) )
						{
						case '\n':
						{
							match('\n');
							break;
						}
						case '\r':
						{
							match('\r');
							break;
						}
						default:
						{
							throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
						}
						 }
					}
					newline();
				}
			else
			{
				throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
			}
			break; }
		}
		_ttype = Token.SKIP;
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	
	
}
}
