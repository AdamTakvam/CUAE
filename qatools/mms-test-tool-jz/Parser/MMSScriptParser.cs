// $ANTLR 2.7.4: "MMSScript.g" -> "MMSScriptParser.cs"$

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
	// Generate the header common to all output files.
	using System;
	
	using TokenBuffer              = antlr.TokenBuffer;
	using TokenStreamException     = antlr.TokenStreamException;
	using TokenStreamIOException   = antlr.TokenStreamIOException;
	using ANTLRException           = antlr.ANTLRException;
	using LLkParser = antlr.LLkParser;
	using Token                    = antlr.Token;
	using TokenStream              = antlr.TokenStream;
	using RecognitionException     = antlr.RecognitionException;
	using NoViableAltException     = antlr.NoViableAltException;
	using MismatchedTokenException = antlr.MismatchedTokenException;
	using SemanticException        = antlr.SemanticException;
	using ParserSharedInputState   = antlr.ParserSharedInputState;
	using BitSet                   = antlr.collections.impl.BitSet;
	using AST                      = antlr.collections.AST;
	using ASTPair                  = antlr.ASTPair;
	using ASTFactory               = antlr.ASTFactory;
	using ASTArray                 = antlr.collections.impl.ASTArray;
	
	public 	class MMSScriptParser : antlr.LLkParser
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
		
		
		
	public ArrayList GetFixtures()
	{
		return fixtureList;
	}
	
	public ArrayList GetScriptList()
	{
		return scriptList;
	}
	
	private ArrayList scriptList = new ArrayList();
	
	private ArrayList commandList = new ArrayList();
	
	private ArrayList fixtureList = new ArrayList();
	
	private int commandTimeout = 0;
	
	private void CreateFixtureInstance(AST fixtureRootNode)
	{
		string name = ((CommonAST)fixtureRootNode).getFirstChild().getText();
		TestFixture newFixture = new TestFixture(scriptList);
		fixtureList.Add(newFixture);
		scriptList = new ArrayList();
	}
	
	private void CreateScriptInstance(AST scriptRootNode)
	{
		string name = ((CommonAST)scriptRootNode).getFirstChild().getText();
		Script newScript = new Script(name, commandTimeout);
		newScript.CopyFromArray(commandList.ToArray());
		scriptList.Add(newScript);
		commandList = new ArrayList();
	}
	
	private void SetScriptOption(string optionName, string optionValue)
	{
		switch (optionName)
		{
			case Metreos.MMSTestTool.Constants.SLO_TIMEOUT : commandTimeout = int.Parse(optionValue); break;
			default : break;
		}
	}
	
	private void CreateCommand(AST root)
	{
			if (root.Type == MMSScriptParser.NScriptOption)
				return;
				
			bool async;
			CommandDescription commandDescriptionReference;
			string name = string.Empty;
			string commandType = root.getText();
			CommonAST commandRoot = (CommonAST)root.getFirstChild();
			
			//We check to see if this command has a name.
			if ((name = commandRoot.getText()) != string.Empty)
			{
					commandRoot = (CommonAST)commandRoot.getNextSibling();
			}

			commandDescriptionReference = CommandDescriptionContainerHandler.FindCommand(commandType);
            async = commandDescriptionReference.async;
			
			System.Diagnostics.Debug.Assert(commandDescriptionReference != null, "Command type " + commandType + " not found!");

			ArrayList parameterNodes = new ArrayList();
			ArrayList provisionalNodes = new ArrayList();
			ArrayList assertNodes = new ArrayList();
            ArrayList optionNodes = new ArrayList();						
			
			//separate the various nodes and populate the parameter containers & friends
			SeparateNodes(commandRoot, parameterNodes, provisionalNodes, assertNodes, optionNodes);
			
			//Now that the different types of nodes are in their separate ArrayLists,
			//process the nodes and extract textual information out of them. 
			ArrayList separatedParameters = ProcessNodes(parameterNodes);
			ArrayList separatedProvisionalAsserts = ProcessNodes(provisionalNodes);
			ArrayList separatedAsserts = ProcessNodes(assertNodes);
            ArrayList separatedOptions = ProcessNodes(optionNodes);
			
			try
			{
				Command cmd = new Command(async, commandDescriptionReference, commandType, name, separatedParameters, separatedProvisionalAsserts, separatedAsserts, separatedOptions);
				commandList.Add(cmd);
			}
			catch
			{}
		}
		
		private void SeparateNodes(CommonAST subNode, ArrayList parameterNodes, ArrayList provisionalNodes, ArrayList assertNodes, ArrayList optionNodes)
		{
			while (subNode != null)
			{
				switch (subNode.Type) 
				{
					case MMSScriptParser.NParameter			: parameterNodes.Add(subNode); break;
					case MMSScriptParser.NAssertList		: SeparateNodes((CommonAST)subNode.getFirstChild(), null, null, assertNodes, null); break;
					case MMSScriptParser.NAssert			: assertNodes.Add(subNode); break;
					case MMSScriptParser.NProvisionalAssert	: provisionalNodes.Add(subNode); break;
                    case MMSScriptParser.NCommandOption     : optionNodes.Add(subNode); break;
					default						: break;
				}

				subNode = (CommonAST)subNode.getNextSibling();
			}
		}

		private ArrayList ProcessNodes(ArrayList nodes)
		{
			System.Collections.IEnumerator iterator = nodes.GetEnumerator();
			CommonAST parentNode, childNode = null;
			
			int parentCount;
			ArrayList separatedParameters = new ArrayList();

			while (iterator.MoveNext() != false)
			{
				//parent node is the head of the parameter/provisional/assert subtree
				//we get the text of each child as theu contain the command, operator, and value
				//in string format.
				parentNode = iterator.Current as CommonAST;
				parentCount = parentNode.getNumberOfChildren();
				childNode = (CommonAST)parentNode.getFirstChild();
								
				string[] parameters = new string[parentCount];

				//we use a for loop here as opposed to a while just because it makes it easier to deal with filling the string
				//array slots.
				for (int i = 0; i < parentCount; i++)
				{
					parameters[i] = childNode.getText();
					childNode = (CommonAST)childNode.getNextSibling();
				}

				separatedParameters.Add(parameters);
			}

			return separatedParameters;
		}	
		
		protected void initialize()
		{
			tokenNames = tokenNames_;
			initializeFactory();
		}
		
		
		protected MMSScriptParser(TokenBuffer tokenBuf, int k) : base(tokenBuf, k)
		{
			initialize();
		}
		
		public MMSScriptParser(TokenBuffer tokenBuf) : this(tokenBuf,2)
		{
		}
		
		protected MMSScriptParser(TokenStream lexer, int k) : base(lexer,k)
		{
			initialize();
		}
		
		public MMSScriptParser(TokenStream lexer) : this(lexer,2)
		{
		}
		
		public MMSScriptParser(ParserSharedInputState state) : base(state,2)
		{
			initialize();
		}
		
	public void fixture() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST fixture_AST = null;
		Token  f = null;
		AST f_AST = null;
		
		try {      // for error handling
			{
				f = LT(1);
				f_AST = astFactory.create(f);
				astFactory.makeASTRoot(currentAST, f_AST);
				match(TEST_FIXTURE_LITERAL);
				match(ASSIGN);
				AST tmp2_AST = null;
				tmp2_AST = astFactory.create(LT(1));
				astFactory.addASTChild(currentAST, tmp2_AST);
				match(ID);
				match(FWDCURLY);
				{ // ( ... )+
				int _cnt4=0;
				for (;;)
				{
					if ((LA(1)==TESTSCRIPT_LITERAL))
					{
						script();
						astFactory.addASTChild(currentAST, returnAST);
					}
					else
					{
						if (_cnt4 >= 1) { goto _loop4_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
					}
					
					_cnt4++;
				}
_loop4_breakloop:				;
				}    // ( ... )+
				match(BACKCURLY);
				fixture_AST = (AST)currentAST.root;
				f_AST.setType(NFixture); CreateFixtureInstance(fixture_AST);
			}
			fixture_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_0_);
		}
		returnAST = fixture_AST;
	}
	
	public void script() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST script_AST = null;
		Token  t = null;
		AST t_AST = null;
		
		try {      // for error handling
			{
				t = LT(1);
				t_AST = astFactory.create(t);
				astFactory.makeASTRoot(currentAST, t_AST);
				match(TESTSCRIPT_LITERAL);
				match(ASSIGN);
				AST tmp6_AST = null;
				tmp6_AST = astFactory.create(LT(1));
				astFactory.addASTChild(currentAST, tmp6_AST);
				match(ID);
				match(FWDCURLY);
				{ // ( ... )+
				int _cnt8=0;
				for (;;)
				{
					if ((tokenSet_1_.member(LA(1))))
					{
						cmd_block();
						astFactory.addASTChild(currentAST, returnAST);
					}
					else
					{
						if (_cnt8 >= 1) { goto _loop8_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
					}
					
					_cnt8++;
				}
_loop8_breakloop:				;
				}    // ( ... )+
				match(BACKCURLY);
				script_AST = (AST)currentAST.root;
				t_AST.setType(NScript); CreateScriptInstance(script_AST);
			}
			script_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_2_);
		}
		returnAST = script_AST;
	}
	
	public void cmd_block() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST cmd_block_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case CONNECT_LITERAL:
				{
					connect_block();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case DISCONNECT_LITERAL:
				{
					disconnect_block();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case PLAY_LITERAL:
				{
					play_block();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case RECORD_LITERAL:
				{
					record_block();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case COMMAND_TIMEOUT:
				{
					script_level_options();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			cmd_block_AST = (AST)currentAST.root;
			CreateCommand(cmd_block_AST);
			cmd_block_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_3_);
		}
		returnAST = cmd_block_AST;
	}
	
	public void connect_block() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST connect_block_AST = null;
		Token  c = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				{
					c = LT(1);
					c_AST = astFactory.create(c);
					astFactory.makeASTRoot(currentAST, c_AST);
					match(CONNECT_LITERAL);
					{
						switch ( LA(1) )
						{
						case ASSIGN:
						{
							match(ASSIGN);
							i = LT(1);
							i_AST = astFactory.create(i);
							astFactory.addASTChild(currentAST, i_AST);
							match(ID);
							break;
						}
						case FWDCURLY:
						{
							break;
						}
						default:
						{
							throw new NoViableAltException(LT(1), getFilename());
						}
						 }
					}
				}
				match(FWDCURLY);
				{    // ( ... )*
					for (;;)
					{
						switch ( LA(1) )
						{
						case COMMAND_TIMEOUT:
						case IP_ADDRESS:
						case PORT:
						case CONNECTION_ID:
						case CONFERENCE_ID:
						case SESSION_TIMEOUT:
						case TRANSACTION_ID:
						case CLIENT_ID:
						case CODERTYPEREMOTE:
						case FRAMESIZEREMOTE:
						case VADENABLEREMOTE:
						case DATAFLOWDIRECTION:
						case CODERTYPELOCAL:
						case FRAMESIZELOCAL:
						case VADENABLELOCAL:
						case SOUND_TONE:
						case NO_TONE:
						case SOUND_TONE_WHEN_RECEIVE_ONLY:
						case NO_TONE_WHEN_RECEIVE_ONLY:
						case MONITOR:
						case RECEIVE_ONLY:
						case TARIFF_TONE:
						case COACH:
						case PUPIL:
						{
							connect_params();
							astFactory.addASTChild(currentAST, returnAST);
							break;
						}
						case WAIT_FOR_FINAL:
						{
							command_option();
							astFactory.addASTChild(currentAST, returnAST);
							break;
						}
						default:
						{
							goto _loop26_breakloop;
						}
						 }
					}
_loop26_breakloop:					;
				}    // ( ... )*
				{
					switch ( LA(1) )
					{
					case FINALASSERTS_LITERAL:
					{
						connect_assert();
						astFactory.addASTChild(currentAST, returnAST);
						break;
					}
					case BACKCURLY:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
				match(BACKCURLY);
				c_AST.setType(NCommand);
			}
			connect_block_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_3_);
		}
		returnAST = connect_block_AST;
	}
	
	public void disconnect_block() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST disconnect_block_AST = null;
		Token  c = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		AST r_AST = null;
		AST a_AST = null;
		
		try {      // for error handling
			{
				c = LT(1);
				c_AST = astFactory.create(c);
				astFactory.makeASTRoot(currentAST, c_AST);
				match(DISCONNECT_LITERAL);
				{
					switch ( LA(1) )
					{
					case ASSIGN:
					{
						match(ASSIGN);
						i = LT(1);
						i_AST = astFactory.create(i);
						astFactory.addASTChild(currentAST, i_AST);
						match(ID);
						break;
					}
					case FWDCURLY:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
				match(FWDCURLY);
				{ // ( ... )+
				int _cnt44=0;
				for (;;)
				{
					if ((LA(1)==CONNECTION_ID||LA(1)==CONFERENCE_ID))
					{
						disconnect_reqparams();
						r_AST = (AST)returnAST;
						astFactory.addASTChild(currentAST, returnAST);
					}
					else
					{
						if (_cnt44 >= 1) { goto _loop44_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
					}
					
					_cnt44++;
				}
_loop44_breakloop:				;
				}    // ( ... )+
				{    // ( ... )*
					for (;;)
					{
						switch ( LA(1) )
						{
						case COMMAND_TIMEOUT:
						case TRANSACTION_ID:
						case CLIENT_ID:
						{
							disconnect_params();
							astFactory.addASTChild(currentAST, returnAST);
							break;
						}
						case WAIT_FOR_FINAL:
						{
							command_option();
							astFactory.addASTChild(currentAST, returnAST);
							break;
						}
						default:
						{
							goto _loop46_breakloop;
						}
						 }
					}
_loop46_breakloop:					;
				}    // ( ... )*
				{
					switch ( LA(1) )
					{
					case FINALASSERTS_LITERAL:
					{
						disconnect_assert();
						a_AST = (AST)returnAST;
						astFactory.addASTChild(currentAST, returnAST);
						break;
					}
					case BACKCURLY:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
				match(BACKCURLY);
				c_AST.setType(NCommand);
			}
			disconnect_block_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_3_);
		}
		returnAST = disconnect_block_AST;
	}
	
	public void play_block() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST play_block_AST = null;
		Token  c = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		AST f_AST = null;
		AST a_AST = null;
		
		try {      // for error handling
			{
				c = LT(1);
				c_AST = astFactory.create(c);
				astFactory.makeASTRoot(currentAST, c_AST);
				match(PLAY_LITERAL);
				{
					switch ( LA(1) )
					{
					case ASSIGN:
					{
						match(ASSIGN);
						i = LT(1);
						i_AST = astFactory.create(i);
						astFactory.addASTChild(currentAST, i_AST);
						match(ID);
						break;
					}
					case FWDCURLY:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
				match(FWDCURLY);
				play_reqparams();
				astFactory.addASTChild(currentAST, returnAST);
				{ // ( ... )+
				int _cnt70=0;
				for (;;)
				{
					if ((LA(1)==FILENAME))
					{
						file();
						f_AST = (AST)returnAST;
						astFactory.addASTChild(currentAST, returnAST);
					}
					else
					{
						if (_cnt70 >= 1) { goto _loop70_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
					}
					
					_cnt70++;
				}
_loop70_breakloop:				;
				}    // ( ... )+
				{    // ( ... )*
					for (;;)
					{
						switch ( LA(1) )
						{
						case COMMAND_TIMEOUT:
						case TRANSACTION_ID:
						case CLIENT_ID:
						case MAXTIME:
						case DIGIT:
						case DIGITLIST:
						case MAXDIGITS:
						case SILENCE:
						case NONSILENCE:
						case DIGITDELAY:
						case DIGITPATTERN:
						case FORMAT:
						case ENCODING:
						case BITRATE:
						{
							play_params();
							astFactory.addASTChild(currentAST, returnAST);
							break;
						}
						case WAIT_FOR_FINAL:
						{
							command_option();
							astFactory.addASTChild(currentAST, returnAST);
							break;
						}
						default:
						{
							goto _loop72_breakloop;
						}
						 }
					}
_loop72_breakloop:					;
				}    // ( ... )*
				{
					switch ( LA(1) )
					{
					case PROVASSERT_LITERAL:
					{
						provisional_assert();
						astFactory.addASTChild(currentAST, returnAST);
						break;
					}
					case BACKCURLY:
					case FINALASSERTS_LITERAL:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
				{
					switch ( LA(1) )
					{
					case FINALASSERTS_LITERAL:
					{
						play_assert();
						a_AST = (AST)returnAST;
						astFactory.addASTChild(currentAST, returnAST);
						break;
					}
					case BACKCURLY:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
				match(BACKCURLY);
				c_AST.setType(NCommand);
			}
			play_block_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_3_);
		}
		returnAST = play_block_AST;
	}
	
	public void record_block() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST record_block_AST = null;
		Token  c = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		AST r_AST = null;
		AST a_AST = null;
		
		try {      // for error handling
			{
				c = LT(1);
				c_AST = astFactory.create(c);
				astFactory.makeASTRoot(currentAST, c_AST);
				match(RECORD_LITERAL);
				{
					switch ( LA(1) )
					{
					case ASSIGN:
					{
						match(ASSIGN);
						i = LT(1);
						i_AST = astFactory.create(i);
						astFactory.addASTChild(currentAST, i_AST);
						match(ID);
						break;
					}
					case FWDCURLY:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
				match(FWDCURLY);
				record_reqparams();
				r_AST = (AST)returnAST;
				astFactory.addASTChild(currentAST, returnAST);
				{    // ( ... )*
					for (;;)
					{
						switch ( LA(1) )
						{
						case COMMAND_TIMEOUT:
						case CLIENT_ID:
						case FILENAME:
						case EXPIRES:
						case AUDIO_FILE_ATTRIBUTE:
						case TERMINATION_CONDITION:
						{
							record_params();
							astFactory.addASTChild(currentAST, returnAST);
							break;
						}
						case WAIT_FOR_FINAL:
						{
							command_option();
							astFactory.addASTChild(currentAST, returnAST);
							break;
						}
						default:
						{
							goto _loop98_breakloop;
						}
						 }
					}
_loop98_breakloop:					;
				}    // ( ... )*
				{
					switch ( LA(1) )
					{
					case PROVASSERT_LITERAL:
					{
						provisional_assert();
						astFactory.addASTChild(currentAST, returnAST);
						break;
					}
					case BACKCURLY:
					case FINALASSERTS_LITERAL:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
				{
					switch ( LA(1) )
					{
					case FINALASSERTS_LITERAL:
					{
						record_assert();
						a_AST = (AST)returnAST;
						astFactory.addASTChild(currentAST, returnAST);
						break;
					}
					case BACKCURLY:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
				match(BACKCURLY);
				c_AST.setType(NCommand);
			}
			record_block_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_3_);
		}
		returnAST = record_block_AST;
	}
	
	public void script_level_options() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST script_level_options_AST = null;
		Token  a = null;
		AST a_AST = null;
		Token  b = null;
		AST b_AST = null;
		
		try {      // for error handling
			{
				a = LT(1);
				a_AST = astFactory.create(a);
				astFactory.addASTChild(currentAST, a_AST);
				match(COMMAND_TIMEOUT);
				match(ASSIGN);
				b = LT(1);
				b_AST = astFactory.create(b);
				astFactory.addASTChild(currentAST, b_AST);
				match(ID);
				script_level_options_AST = (AST)currentAST.root;
				script_level_options_AST.setType(NScriptOption); SetScriptOption(a.getText(), b.getText());
			}
			script_level_options_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_3_);
		}
		returnAST = script_level_options_AST;
	}
	
	public void command_option() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST command_option_AST = null;
		AST t_AST = null;
		AST v_AST = null;
		
		try {      // for error handling
			{
				command_option_token_true_false();
				t_AST = (AST)returnAST;
				match(ASSIGN);
				command_option_values_true_false();
				v_AST = (AST)returnAST;
				command_option_AST = (AST)currentAST.root;
				command_option_AST=(AST)astFactory.make( (new ASTArray(3)).add(astFactory.create(NCommandOption)).add(t_AST).add(v_AST));
				currentAST.root = command_option_AST;
				if ( (null != command_option_AST) && (null != command_option_AST.getFirstChild()) )
					currentAST.child = command_option_AST.getFirstChild();
				else
					currentAST.child = command_option_AST;
				currentAST.advanceChildToEnd();
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_4_);
		}
		returnAST = command_option_AST;
	}
	
	public void command_option_token_true_false() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST command_option_token_true_false_AST = null;
		
		try {      // for error handling
			{
				AST tmp23_AST = null;
				tmp23_AST = astFactory.create(LT(1));
				astFactory.addASTChild(currentAST, tmp23_AST);
				match(WAIT_FOR_FINAL);
			}
			command_option_token_true_false_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = command_option_token_true_false_AST;
	}
	
	public void command_option_values_true_false() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST command_option_values_true_false_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case TRUE:
				{
					AST tmp24_AST = null;
					tmp24_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp24_AST);
					match(TRUE);
					break;
				}
				case FALSE:
				{
					AST tmp25_AST = null;
					tmp25_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp25_AST);
					match(FALSE);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			command_option_values_true_false_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_4_);
		}
		returnAST = command_option_values_true_false_AST;
	}
	
	public void provisional_assert() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST provisional_assert_AST = null;
		Token  c = null;
		AST c_AST = null;
		
		try {      // for error handling
			{
				c = LT(1);
				c_AST = astFactory.create(c);
				astFactory.makeASTRoot(currentAST, c_AST);
				match(PROVASSERT_LITERAL);
				match(FWDCURLY);
				AST tmp27_AST = null;
				tmp27_AST = astFactory.create(LT(1));
				astFactory.addASTChild(currentAST, tmp27_AST);
				match(RESULT_CODE);
				AST tmp28_AST = null;
				tmp28_AST = astFactory.create(LT(1));
				astFactory.addASTChild(currentAST, tmp28_AST);
				match(COMPARE_OPERATOR);
				AST tmp29_AST = null;
				tmp29_AST = astFactory.create(LT(1));
				astFactory.addASTChild(currentAST, tmp29_AST);
				match(ID);
				match(BACKCURLY);
				c_AST.setType(NProvisionalAssert);
			}
			provisional_assert_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_6_);
		}
		returnAST = provisional_assert_AST;
	}
	
	public void connect_params() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST connect_params_AST = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				connect_params_list();
				c_AST = (AST)returnAST;
				match(ASSIGN);
				i = LT(1);
				i_AST = astFactory.create(i);
				match(ID);
				connect_params_AST = (AST)currentAST.root;
				connect_params_AST = (AST)astFactory.make( (new ASTArray(3)).add(astFactory.create(NParameter)).add(c_AST).add(i_AST));
				currentAST.root = connect_params_AST;
				if ( (null != connect_params_AST) && (null != connect_params_AST.getFirstChild()) )
					currentAST.child = connect_params_AST.getFirstChild();
				else
					currentAST.child = connect_params_AST;
				currentAST.advanceChildToEnd();
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_7_);
		}
		returnAST = connect_params_AST;
	}
	
	public void connect_assert() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST connect_assert_AST = null;
		AST c_AST = null;
		
		try {      // for error handling
			{
				match(FINALASSERTS_LITERAL);
				match(FWDCURLY);
				{    // ( ... )*
					for (;;)
					{
						if ((tokenSet_8_.member(LA(1))))
						{
							connect_assert_line();
							c_AST = (AST)returnAST;
							astFactory.addASTChild(currentAST, returnAST);
						}
						else
						{
							goto _loop35_breakloop;
						}
						
					}
_loop35_breakloop:					;
				}    // ( ... )*
				match(BACKCURLY);
			}
			connect_assert_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_9_);
		}
		returnAST = connect_assert_AST;
	}
	
	public void connect_params_list() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST connect_params_list_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case IP_ADDRESS:
				{
					AST tmp35_AST = null;
					tmp35_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp35_AST);
					match(IP_ADDRESS);
					break;
				}
				case PORT:
				{
					AST tmp36_AST = null;
					tmp36_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp36_AST);
					match(PORT);
					break;
				}
				case CONNECTION_ID:
				{
					AST tmp37_AST = null;
					tmp37_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp37_AST);
					match(CONNECTION_ID);
					break;
				}
				case CONFERENCE_ID:
				{
					AST tmp38_AST = null;
					tmp38_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp38_AST);
					match(CONFERENCE_ID);
					break;
				}
				case SESSION_TIMEOUT:
				{
					AST tmp39_AST = null;
					tmp39_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp39_AST);
					match(SESSION_TIMEOUT);
					break;
				}
				case COMMAND_TIMEOUT:
				{
					AST tmp40_AST = null;
					tmp40_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp40_AST);
					match(COMMAND_TIMEOUT);
					break;
				}
				case SOUND_TONE:
				case NO_TONE:
				case SOUND_TONE_WHEN_RECEIVE_ONLY:
				case NO_TONE_WHEN_RECEIVE_ONLY:
				{
					conference_attribute_names();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case MONITOR:
				case RECEIVE_ONLY:
				case TARIFF_TONE:
				case COACH:
				case PUPIL:
				{
					conferee_attribute_names();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case TRANSACTION_ID:
				{
					AST tmp41_AST = null;
					tmp41_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp41_AST);
					match(TRANSACTION_ID);
					break;
				}
				case CLIENT_ID:
				{
					AST tmp42_AST = null;
					tmp42_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp42_AST);
					match(CLIENT_ID);
					break;
				}
				case CODERTYPEREMOTE:
				case FRAMESIZEREMOTE:
				case VADENABLEREMOTE:
				case DATAFLOWDIRECTION:
				case CODERTYPELOCAL:
				case FRAMESIZELOCAL:
				case VADENABLELOCAL:
				{
					connection_attribute_names();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			connect_params_list_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = connect_params_list_AST;
	}
	
	public void conference_attribute_names() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST conference_attribute_names_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case SOUND_TONE:
				{
					AST tmp43_AST = null;
					tmp43_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp43_AST);
					match(SOUND_TONE);
					break;
				}
				case NO_TONE:
				{
					AST tmp44_AST = null;
					tmp44_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp44_AST);
					match(NO_TONE);
					break;
				}
				case SOUND_TONE_WHEN_RECEIVE_ONLY:
				{
					AST tmp45_AST = null;
					tmp45_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp45_AST);
					match(SOUND_TONE_WHEN_RECEIVE_ONLY);
					break;
				}
				case NO_TONE_WHEN_RECEIVE_ONLY:
				{
					AST tmp46_AST = null;
					tmp46_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp46_AST);
					match(NO_TONE_WHEN_RECEIVE_ONLY);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			conference_attribute_names_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = conference_attribute_names_AST;
	}
	
	public void conferee_attribute_names() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST conferee_attribute_names_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case MONITOR:
				{
					AST tmp47_AST = null;
					tmp47_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp47_AST);
					match(MONITOR);
					break;
				}
				case RECEIVE_ONLY:
				{
					AST tmp48_AST = null;
					tmp48_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp48_AST);
					match(RECEIVE_ONLY);
					break;
				}
				case TARIFF_TONE:
				{
					AST tmp49_AST = null;
					tmp49_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp49_AST);
					match(TARIFF_TONE);
					break;
				}
				case COACH:
				{
					AST tmp50_AST = null;
					tmp50_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp50_AST);
					match(COACH);
					break;
				}
				case PUPIL:
				{
					AST tmp51_AST = null;
					tmp51_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp51_AST);
					match(PUPIL);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			conferee_attribute_names_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = conferee_attribute_names_AST;
	}
	
	public void connection_attribute_names() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST connection_attribute_names_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case CODERTYPEREMOTE:
				{
					AST tmp52_AST = null;
					tmp52_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp52_AST);
					match(CODERTYPEREMOTE);
					break;
				}
				case FRAMESIZEREMOTE:
				{
					AST tmp53_AST = null;
					tmp53_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp53_AST);
					match(FRAMESIZEREMOTE);
					break;
				}
				case VADENABLEREMOTE:
				{
					AST tmp54_AST = null;
					tmp54_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp54_AST);
					match(VADENABLEREMOTE);
					break;
				}
				case DATAFLOWDIRECTION:
				{
					AST tmp55_AST = null;
					tmp55_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp55_AST);
					match(DATAFLOWDIRECTION);
					break;
				}
				case CODERTYPELOCAL:
				{
					AST tmp56_AST = null;
					tmp56_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp56_AST);
					match(CODERTYPELOCAL);
					break;
				}
				case FRAMESIZELOCAL:
				{
					AST tmp57_AST = null;
					tmp57_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp57_AST);
					match(FRAMESIZELOCAL);
					break;
				}
				case VADENABLELOCAL:
				{
					AST tmp58_AST = null;
					tmp58_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp58_AST);
					match(VADENABLELOCAL);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			connection_attribute_names_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = connection_attribute_names_AST;
	}
	
	public void connect_assert_line() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST connect_assert_line_AST = null;
		AST c_AST = null;
		Token  op = null;
		AST op_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				connect_assert_tokens();
				c_AST = (AST)returnAST;
				op = LT(1);
				op_AST = astFactory.create(op);
				match(COMPARE_OPERATOR);
				i = LT(1);
				i_AST = astFactory.create(i);
				match(ID);
				connect_assert_line_AST = (AST)currentAST.root;
				connect_assert_line_AST = (AST)astFactory.make( (new ASTArray(4)).add(astFactory.create(NAssert)).add(c_AST).add(op_AST).add(i_AST));
				currentAST.root = connect_assert_line_AST;
				if ( (null != connect_assert_line_AST) && (null != connect_assert_line_AST.getFirstChild()) )
					currentAST.child = connect_assert_line_AST.getFirstChild();
				else
					currentAST.child = connect_assert_line_AST;
				currentAST.advanceChildToEnd();
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_10_);
		}
		returnAST = connect_assert_line_AST;
	}
	
	public void connect_assert_tokens() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST connect_assert_tokens_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case RESULT_CODE:
				{
					AST tmp59_AST = null;
					tmp59_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp59_AST);
					match(RESULT_CODE);
					break;
				}
				case CONNECTION_ID:
				{
					AST tmp60_AST = null;
					tmp60_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp60_AST);
					match(CONNECTION_ID);
					break;
				}
				case CONFERENCE_ID:
				{
					AST tmp61_AST = null;
					tmp61_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp61_AST);
					match(CONFERENCE_ID);
					break;
				}
				case PORT:
				{
					AST tmp62_AST = null;
					tmp62_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp62_AST);
					match(PORT);
					break;
				}
				case IP_ADDRESS:
				{
					AST tmp63_AST = null;
					tmp63_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp63_AST);
					match(IP_ADDRESS);
					break;
				}
				case TRANSACTION_ID:
				{
					AST tmp64_AST = null;
					tmp64_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp64_AST);
					match(TRANSACTION_ID);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			connect_assert_tokens_AST = (AST)currentAST.root;
			connect_assert_tokens_AST.setType(NAssert);
			connect_assert_tokens_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_11_);
		}
		returnAST = connect_assert_tokens_AST;
	}
	
	public void disconnect_reqparams() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST disconnect_reqparams_AST = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				disconnect_reqparams_list();
				c_AST = (AST)returnAST;
				match(ASSIGN);
				i = LT(1);
				i_AST = astFactory.create(i);
				match(ID);
				disconnect_reqparams_AST = (AST)currentAST.root;
				disconnect_reqparams_AST = (AST)astFactory.make( (new ASTArray(3)).add(astFactory.create(NParameter)).add(c_AST).add(i_AST));
				currentAST.root = disconnect_reqparams_AST;
				if ( (null != disconnect_reqparams_AST) && (null != disconnect_reqparams_AST.getFirstChild()) )
					currentAST.child = disconnect_reqparams_AST.getFirstChild();
				else
					currentAST.child = disconnect_reqparams_AST;
				currentAST.advanceChildToEnd();
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_12_);
		}
		returnAST = disconnect_reqparams_AST;
	}
	
	public void disconnect_params() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST disconnect_params_AST = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				disconnect_params_list();
				c_AST = (AST)returnAST;
				match(ASSIGN);
				i = LT(1);
				i_AST = astFactory.create(i);
				match(ID);
				disconnect_params_AST = (AST)currentAST.root;
				disconnect_params_AST = (AST)astFactory.make( (new ASTArray(3)).add(astFactory.create(NParameter)).add(c_AST).add(i_AST));
				currentAST.root = disconnect_params_AST;
				if ( (null != disconnect_params_AST) && (null != disconnect_params_AST.getFirstChild()) )
					currentAST.child = disconnect_params_AST.getFirstChild();
				else
					currentAST.child = disconnect_params_AST;
				currentAST.advanceChildToEnd();
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_13_);
		}
		returnAST = disconnect_params_AST;
	}
	
	public void disconnect_assert() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST disconnect_assert_AST = null;
		AST c_AST = null;
		
		try {      // for error handling
			{
				match(FINALASSERTS_LITERAL);
				match(FWDCURLY);
				{    // ( ... )*
					for (;;)
					{
						if ((LA(1)==RESULT_CODE||LA(1)==TRANSACTION_ID))
						{
							disconnect_assert_line();
							c_AST = (AST)returnAST;
							astFactory.addASTChild(currentAST, returnAST);
						}
						else
						{
							goto _loop61_breakloop;
						}
						
					}
_loop61_breakloop:					;
				}    // ( ... )*
				match(BACKCURLY);
			}
			disconnect_assert_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_9_);
		}
		returnAST = disconnect_assert_AST;
	}
	
	public void disconnect_reqparams_list() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST disconnect_reqparams_list_AST = null;
		
		try {      // for error handling
			{
				{
					switch ( LA(1) )
					{
					case CONNECTION_ID:
					{
						AST tmp70_AST = null;
						tmp70_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp70_AST);
						match(CONNECTION_ID);
						break;
					}
					case CONFERENCE_ID:
					{
						AST tmp71_AST = null;
						tmp71_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp71_AST);
						match(CONFERENCE_ID);
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
			}
			disconnect_reqparams_list_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = disconnect_reqparams_list_AST;
	}
	
	public void disconnect_params_list() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST disconnect_params_list_AST = null;
		
		try {      // for error handling
			{
				{
					switch ( LA(1) )
					{
					case COMMAND_TIMEOUT:
					{
						AST tmp72_AST = null;
						tmp72_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp72_AST);
						match(COMMAND_TIMEOUT);
						break;
					}
					case TRANSACTION_ID:
					{
						AST tmp73_AST = null;
						tmp73_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp73_AST);
						match(TRANSACTION_ID);
						break;
					}
					case CLIENT_ID:
					{
						AST tmp74_AST = null;
						tmp74_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp74_AST);
						match(CLIENT_ID);
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
			}
			disconnect_params_list_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = disconnect_params_list_AST;
	}
	
	public void disconnect_assert_line() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST disconnect_assert_line_AST = null;
		AST c_AST = null;
		Token  op = null;
		AST op_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				disconnect_assert_tokens();
				c_AST = (AST)returnAST;
				op = LT(1);
				op_AST = astFactory.create(op);
				match(COMPARE_OPERATOR);
				i = LT(1);
				i_AST = astFactory.create(i);
				match(ID);
			}
			disconnect_assert_line_AST = (AST)currentAST.root;
			disconnect_assert_line_AST = (AST)astFactory.make( (new ASTArray(4)).add(astFactory.create(NAssert)).add(c_AST).add(op_AST).add(i_AST));
			currentAST.root = disconnect_assert_line_AST;
			if ( (null != disconnect_assert_line_AST) && (null != disconnect_assert_line_AST.getFirstChild()) )
				currentAST.child = disconnect_assert_line_AST.getFirstChild();
			else
				currentAST.child = disconnect_assert_line_AST;
			currentAST.advanceChildToEnd();
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_14_);
		}
		returnAST = disconnect_assert_line_AST;
	}
	
	public void disconnect_assert_tokens() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST disconnect_assert_tokens_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case RESULT_CODE:
				{
					AST tmp75_AST = null;
					tmp75_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp75_AST);
					match(RESULT_CODE);
					break;
				}
				case TRANSACTION_ID:
				{
					AST tmp76_AST = null;
					tmp76_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp76_AST);
					match(TRANSACTION_ID);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			disconnect_assert_tokens_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_11_);
		}
		returnAST = disconnect_assert_tokens_AST;
	}
	
	public void play_reqparams() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST play_reqparams_AST = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				{
					play_reqparams_list();
					c_AST = (AST)returnAST;
					match(ASSIGN);
					i = LT(1);
					i_AST = astFactory.create(i);
					match(ID);
				}
				play_reqparams_AST = (AST)currentAST.root;
				play_reqparams_AST = (AST)astFactory.make( (new ASTArray(3)).add(astFactory.create(NParameter)).add(c_AST).add(i_AST));
				currentAST.root = play_reqparams_AST;
				if ( (null != play_reqparams_AST) && (null != play_reqparams_AST.getFirstChild()) )
					currentAST.child = play_reqparams_AST.getFirstChild();
				else
					currentAST.child = play_reqparams_AST;
				currentAST.advanceChildToEnd();
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_15_);
		}
		returnAST = play_reqparams_AST;
	}
	
	public void file() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST file_AST = null;
		Token  p = null;
		AST p_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				p = LT(1);
				p_AST = astFactory.create(p);
				astFactory.makeASTRoot(currentAST, p_AST);
				match(FILENAME);
				match(ASSIGN);
				i = LT(1);
				i_AST = astFactory.create(i);
				astFactory.addASTChild(currentAST, i_AST);
				match(ID);
				p_AST.setType(NParameter);
			}
			file_AST = (AST)currentAST.root;
			file_AST = (AST)astFactory.make( (new ASTArray(3)).add(astFactory.create(NParameter)).add(p_AST).add(i_AST));
			currentAST.root = file_AST;
			if ( (null != file_AST) && (null != file_AST.getFirstChild()) )
				currentAST.child = file_AST.getFirstChild();
			else
				currentAST.child = file_AST;
			currentAST.advanceChildToEnd();
			file_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_16_);
		}
		returnAST = file_AST;
	}
	
	public void play_params() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST play_params_AST = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				play_params_list();
				c_AST = (AST)returnAST;
				match(ASSIGN);
				i = LT(1);
				i_AST = astFactory.create(i);
				match(ID);
				play_params_AST = (AST)currentAST.root;
				play_params_AST = (AST)astFactory.make( (new ASTArray(3)).add(astFactory.create(NParameter)).add(c_AST).add(i_AST));
				currentAST.root = play_params_AST;
				if ( (null != play_params_AST) && (null != play_params_AST.getFirstChild()) )
					currentAST.child = play_params_AST.getFirstChild();
				else
					currentAST.child = play_params_AST;
				currentAST.advanceChildToEnd();
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_17_);
		}
		returnAST = play_params_AST;
	}
	
	public void play_assert() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST play_assert_AST = null;
		AST c_AST = null;
		
		try {      // for error handling
			{
				match(FINALASSERTS_LITERAL);
				match(FWDCURLY);
				{    // ( ... )*
					for (;;)
					{
						if ((tokenSet_18_.member(LA(1))))
						{
							play_assert_line();
							c_AST = (AST)returnAST;
							astFactory.addASTChild(currentAST, returnAST);
						}
						else
						{
							goto _loop89_breakloop;
						}
						
					}
_loop89_breakloop:					;
				}    // ( ... )*
				match(BACKCURLY);
			}
			play_assert_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_9_);
		}
		returnAST = play_assert_AST;
	}
	
	public void play_reqparams_list() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST play_reqparams_list_AST = null;
		
		try {      // for error handling
			{
				{
					switch ( LA(1) )
					{
					case CONNECTION_ID:
					{
						AST tmp83_AST = null;
						tmp83_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp83_AST);
						match(CONNECTION_ID);
						break;
					}
					case CONFERENCE_ID:
					{
						AST tmp84_AST = null;
						tmp84_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp84_AST);
						match(CONFERENCE_ID);
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
			}
			play_reqparams_list_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = play_reqparams_list_AST;
	}
	
	public void play_params_list() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST play_params_list_AST = null;
		
		try {      // for error handling
			{
				{
					switch ( LA(1) )
					{
					case COMMAND_TIMEOUT:
					{
						AST tmp85_AST = null;
						tmp85_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp85_AST);
						match(COMMAND_TIMEOUT);
						break;
					}
					case TRANSACTION_ID:
					{
						AST tmp86_AST = null;
						tmp86_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp86_AST);
						match(TRANSACTION_ID);
						break;
					}
					case CLIENT_ID:
					{
						AST tmp87_AST = null;
						tmp87_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp87_AST);
						match(CLIENT_ID);
						break;
					}
					case MAXTIME:
					case DIGIT:
					case DIGITLIST:
					case MAXDIGITS:
					case SILENCE:
					case NONSILENCE:
					case DIGITDELAY:
					case DIGITPATTERN:
					{
						inbound_termination_condition_names();
						astFactory.addASTChild(currentAST, returnAST);
						break;
					}
					case FORMAT:
					case ENCODING:
					case BITRATE:
					{
						audio_attribute_names();
						astFactory.addASTChild(currentAST, returnAST);
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
			}
			play_params_list_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = play_params_list_AST;
	}
	
	public void inbound_termination_condition_names() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST inbound_termination_condition_names_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case MAXTIME:
				{
					AST tmp88_AST = null;
					tmp88_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp88_AST);
					match(MAXTIME);
					break;
				}
				case DIGIT:
				{
					AST tmp89_AST = null;
					tmp89_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp89_AST);
					match(DIGIT);
					break;
				}
				case DIGITLIST:
				{
					AST tmp90_AST = null;
					tmp90_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp90_AST);
					match(DIGITLIST);
					break;
				}
				case MAXDIGITS:
				{
					AST tmp91_AST = null;
					tmp91_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp91_AST);
					match(MAXDIGITS);
					break;
				}
				case SILENCE:
				{
					AST tmp92_AST = null;
					tmp92_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp92_AST);
					match(SILENCE);
					break;
				}
				case NONSILENCE:
				{
					AST tmp93_AST = null;
					tmp93_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp93_AST);
					match(NONSILENCE);
					break;
				}
				case DIGITDELAY:
				{
					AST tmp94_AST = null;
					tmp94_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp94_AST);
					match(DIGITDELAY);
					break;
				}
				case DIGITPATTERN:
				{
					AST tmp95_AST = null;
					tmp95_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp95_AST);
					match(DIGITPATTERN);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			inbound_termination_condition_names_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_19_);
		}
		returnAST = inbound_termination_condition_names_AST;
	}
	
	public void audio_attribute_names() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST audio_attribute_names_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case FORMAT:
				{
					AST tmp96_AST = null;
					tmp96_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp96_AST);
					match(FORMAT);
					break;
				}
				case ENCODING:
				{
					AST tmp97_AST = null;
					tmp97_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp97_AST);
					match(ENCODING);
					break;
				}
				case BITRATE:
				{
					AST tmp98_AST = null;
					tmp98_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp98_AST);
					match(BITRATE);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			audio_attribute_names_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_19_);
		}
		returnAST = audio_attribute_names_AST;
	}
	
	public void play_assert_line() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST play_assert_line_AST = null;
		AST c_AST = null;
		Token  op = null;
		AST op_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				play_assert_tokens();
				c_AST = (AST)returnAST;
				op = LT(1);
				op_AST = astFactory.create(op);
				match(COMPARE_OPERATOR);
				i = LT(1);
				i_AST = astFactory.create(i);
				match(ID);
			}
			play_assert_line_AST = (AST)currentAST.root;
			play_assert_line_AST = (AST)astFactory.make( (new ASTArray(4)).add(astFactory.create(NAssert)).add(c_AST).add(op_AST).add(i_AST));
			currentAST.root = play_assert_line_AST;
			if ( (null != play_assert_line_AST) && (null != play_assert_line_AST.getFirstChild()) )
				currentAST.child = play_assert_line_AST.getFirstChild();
			else
				currentAST.child = play_assert_line_AST;
			currentAST.advanceChildToEnd();
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_20_);
		}
		returnAST = play_assert_line_AST;
	}
	
	public void play_assert_tokens() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST play_assert_tokens_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case RESULT_CODE:
				{
					AST tmp99_AST = null;
					tmp99_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp99_AST);
					match(RESULT_CODE);
					break;
				}
				case TRANSACTION_ID:
				{
					AST tmp100_AST = null;
					tmp100_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp100_AST);
					match(TRANSACTION_ID);
					break;
				}
				case MAXTIME:
				case DIGIT:
				case MAXDIGITS:
				case SILENCE:
				case NONSILENCE:
				case LOOPCURRENT:
				case INTERDIGDELAY:
				case USERSTOP:
				case EOD:
				case TONE:
				case DEVICEERROR:
				case MAXDATA:
				{
					outbound_termination_condition_names();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			play_assert_tokens_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_11_);
		}
		returnAST = play_assert_tokens_AST;
	}
	
	public void outbound_termination_condition_names() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST outbound_termination_condition_names_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case MAXTIME:
				{
					AST tmp101_AST = null;
					tmp101_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp101_AST);
					match(MAXTIME);
					break;
				}
				case MAXDIGITS:
				{
					AST tmp102_AST = null;
					tmp102_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp102_AST);
					match(MAXDIGITS);
					break;
				}
				case SILENCE:
				{
					AST tmp103_AST = null;
					tmp103_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp103_AST);
					match(SILENCE);
					break;
				}
				case NONSILENCE:
				{
					AST tmp104_AST = null;
					tmp104_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp104_AST);
					match(NONSILENCE);
					break;
				}
				case DIGIT:
				{
					AST tmp105_AST = null;
					tmp105_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp105_AST);
					match(DIGIT);
					break;
				}
				case LOOPCURRENT:
				{
					AST tmp106_AST = null;
					tmp106_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp106_AST);
					match(LOOPCURRENT);
					break;
				}
				case INTERDIGDELAY:
				{
					AST tmp107_AST = null;
					tmp107_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp107_AST);
					match(INTERDIGDELAY);
					break;
				}
				case USERSTOP:
				{
					AST tmp108_AST = null;
					tmp108_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp108_AST);
					match(USERSTOP);
					break;
				}
				case EOD:
				{
					AST tmp109_AST = null;
					tmp109_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp109_AST);
					match(EOD);
					break;
				}
				case TONE:
				{
					AST tmp110_AST = null;
					tmp110_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp110_AST);
					match(TONE);
					break;
				}
				case DEVICEERROR:
				{
					AST tmp111_AST = null;
					tmp111_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp111_AST);
					match(DEVICEERROR);
					break;
				}
				case MAXDATA:
				{
					AST tmp112_AST = null;
					tmp112_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp112_AST);
					match(MAXDATA);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			outbound_termination_condition_names_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_11_);
		}
		returnAST = outbound_termination_condition_names_AST;
	}
	
	public void record_reqparams() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST record_reqparams_AST = null;
		AST c_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				{
					record_reqparams_list();
					c_AST = (AST)returnAST;
					match(ASSIGN);
					i = LT(1);
					i_AST = astFactory.create(i);
					match(ID);
				}
				record_reqparams_AST = (AST)currentAST.root;
				record_reqparams_AST = (AST)astFactory.make( (new ASTArray(3)).add(astFactory.create(NParameter)).add(c_AST).add(i_AST));
				currentAST.root = record_reqparams_AST;
				if ( (null != record_reqparams_AST) && (null != record_reqparams_AST.getFirstChild()) )
					currentAST.child = record_reqparams_AST.getFirstChild();
				else
					currentAST.child = record_reqparams_AST;
				currentAST.advanceChildToEnd();
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_21_);
		}
		returnAST = record_reqparams_AST;
	}
	
	public void record_params() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST record_params_AST = null;
		AST c_AST = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				record_params_list();
				c_AST = (AST)returnAST;
				match(ASSIGN);
				record_params_choices();
				i_AST = (AST)returnAST;
				record_params_AST = (AST)currentAST.root;
				record_params_AST = (AST)astFactory.make( (new ASTArray(3)).add(astFactory.create(NParameter)).add(c_AST).add(i_AST));
				currentAST.root = record_params_AST;
				if ( (null != record_params_AST) && (null != record_params_AST.getFirstChild()) )
					currentAST.child = record_params_AST.getFirstChild();
				else
					currentAST.child = record_params_AST;
				currentAST.advanceChildToEnd();
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_21_);
		}
		returnAST = record_params_AST;
	}
	
	public void record_assert() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST record_assert_AST = null;
		AST c_AST = null;
		
		try {      // for error handling
			{
				match(FINALASSERTS_LITERAL);
				match(FWDCURLY);
				{    // ( ... )*
					for (;;)
					{
						if ((tokenSet_22_.member(LA(1))))
						{
							record_assert_line();
							c_AST = (AST)returnAST;
							astFactory.addASTChild(currentAST, returnAST);
						}
						else
						{
							goto _loop117_breakloop;
						}
						
					}
_loop117_breakloop:					;
				}    // ( ... )*
				match(BACKCURLY);
			}
			record_assert_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_9_);
		}
		returnAST = record_assert_AST;
	}
	
	public void record_reqparams_list() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST record_reqparams_list_AST = null;
		
		try {      // for error handling
			{
				{
					switch ( LA(1) )
					{
					case CONNECTION_ID:
					{
						AST tmp118_AST = null;
						tmp118_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp118_AST);
						match(CONNECTION_ID);
						break;
					}
					case CONFERENCE_ID:
					{
						AST tmp119_AST = null;
						tmp119_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp119_AST);
						match(CONFERENCE_ID);
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
			}
			record_reqparams_list_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = record_reqparams_list_AST;
	}
	
	public void record_params_list() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST record_params_list_AST = null;
		
		try {      // for error handling
			{
				{
					switch ( LA(1) )
					{
					case COMMAND_TIMEOUT:
					{
						AST tmp120_AST = null;
						tmp120_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp120_AST);
						match(COMMAND_TIMEOUT);
						break;
					}
					case CLIENT_ID:
					{
						AST tmp121_AST = null;
						tmp121_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp121_AST);
						match(CLIENT_ID);
						break;
					}
					case FILENAME:
					{
						AST tmp122_AST = null;
						tmp122_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp122_AST);
						match(FILENAME);
						break;
					}
					case EXPIRES:
					{
						AST tmp123_AST = null;
						tmp123_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp123_AST);
						match(EXPIRES);
						break;
					}
					case AUDIO_FILE_ATTRIBUTE:
					{
						AST tmp124_AST = null;
						tmp124_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp124_AST);
						match(AUDIO_FILE_ATTRIBUTE);
						break;
					}
					case TERMINATION_CONDITION:
					{
						AST tmp125_AST = null;
						tmp125_AST = astFactory.create(LT(1));
						astFactory.addASTChild(currentAST, tmp125_AST);
						match(TERMINATION_CONDITION);
						break;
					}
					default:
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					 }
				}
			}
			record_params_list_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_5_);
		}
		returnAST = record_params_list_AST;
	}
	
	public void record_params_choices() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST record_params_choices_AST = null;
		Token  i = null;
		AST i_AST = null;
		AST t_AST = null;
		Token  u = null;
		AST u_AST = null;
		AST a_AST = null;
		Token  b = null;
		AST b_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case ID:
				{
					i = LT(1);
					i_AST = astFactory.create(i);
					astFactory.addASTChild(currentAST, i_AST);
					match(ID);
					record_params_choices_AST = (AST)currentAST.root;
					record_params_choices_AST = i_AST;
					currentAST.root = record_params_choices_AST;
					if ( (null != record_params_choices_AST) && (null != record_params_choices_AST.getFirstChild()) )
						currentAST.child = record_params_choices_AST.getFirstChild();
					else
						currentAST.child = record_params_choices_AST;
					currentAST.advanceChildToEnd();
					break;
				}
				case FORMAT:
				case ENCODING:
				case BITRATE:
				{
					audio_attribute_names();
					t_AST = (AST)returnAST;
					astFactory.addASTChild(currentAST, returnAST);
					u = LT(1);
					u_AST = astFactory.create(u);
					astFactory.addASTChild(currentAST, u_AST);
					match(ID);
					record_params_choices_AST = (AST)currentAST.root;
					string attr = t_AST + " " + u_AST; record_params_choices_AST.setText(attr);
					break;
				}
				case MAXTIME:
				case DIGIT:
				case DIGITLIST:
				case MAXDIGITS:
				case SILENCE:
				case NONSILENCE:
				case DIGITDELAY:
				case DIGITPATTERN:
				{
					inbound_termination_condition_names();
					a_AST = (AST)returnAST;
					astFactory.addASTChild(currentAST, returnAST);
					b = LT(1);
					b_AST = astFactory.create(b);
					astFactory.addASTChild(currentAST, b_AST);
					match(ID);
					record_params_choices_AST = (AST)currentAST.root;
					string term = a_AST + " " + b_AST; record_params_choices_AST.setText(term);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			record_params_choices_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_21_);
		}
		returnAST = record_params_choices_AST;
	}
	
	public void record_assert_line() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST record_assert_line_AST = null;
		AST c_AST = null;
		Token  op = null;
		AST op_AST = null;
		Token  i = null;
		AST i_AST = null;
		
		try {      // for error handling
			{
				record_assert_tokens();
				c_AST = (AST)returnAST;
				op = LT(1);
				op_AST = astFactory.create(op);
				match(COMPARE_OPERATOR);
				i = LT(1);
				i_AST = astFactory.create(i);
				match(ID);
			}
			record_assert_line_AST = (AST)currentAST.root;
			record_assert_line_AST = (AST)astFactory.make( (new ASTArray(4)).add(astFactory.create(NAssert)).add(c_AST).add(op_AST).add(i_AST));
			currentAST.root = record_assert_line_AST;
			if ( (null != record_assert_line_AST) && (null != record_assert_line_AST.getFirstChild()) )
				currentAST.child = record_assert_line_AST.getFirstChild();
			else
				currentAST.child = record_assert_line_AST;
			currentAST.advanceChildToEnd();
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_23_);
		}
		returnAST = record_assert_line_AST;
	}
	
	public void record_assert_tokens() //throws RecognitionException, TokenStreamException
{
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST record_assert_tokens_AST = null;
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case RESULT_CODE:
				{
					AST tmp126_AST = null;
					tmp126_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp126_AST);
					match(RESULT_CODE);
					break;
				}
				case MAXTIME:
				case DIGIT:
				case MAXDIGITS:
				case SILENCE:
				case NONSILENCE:
				case LOOPCURRENT:
				case INTERDIGDELAY:
				case USERSTOP:
				case EOD:
				case TONE:
				case DEVICEERROR:
				case MAXDATA:
				{
					outbound_termination_condition_names();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case FILENAME:
				{
					AST tmp127_AST = null;
					tmp127_AST = astFactory.create(LT(1));
					astFactory.addASTChild(currentAST, tmp127_AST);
					match(FILENAME);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			record_assert_tokens_AST = currentAST.root;
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			consume();
			consumeUntil(tokenSet_11_);
		}
		returnAST = record_assert_tokens_AST;
	}
	
	private void initializeFactory()
	{
		if (astFactory == null)
		{
			astFactory = new ASTFactory();
		}
		initializeASTFactory( astFactory );
	}
	static public void initializeASTFactory( ASTFactory factory )
	{
		factory.setMaxNodeType(107);
	}
	
	public static readonly string[] tokenNames_ = new string[] {
		@"""<0>""",
		@"""EOF""",
		@"""<2>""",
		@"""NULL_TREE_LOOKAHEAD""",
		@"""NFixture""",
		@"""NScript""",
		@"""NCommand""",
		@"""NParameter""",
		@"""NReqParameter""",
		@"""NParameterList""",
		@"""NAssert""",
		@"""NProvisionalAssert""",
		@"""NAssertList""",
		@"""NFile""",
		@"""NCommandOption""",
		@"""NScriptOption""",
		@"""TestFixture""",
		@"""ASSIGN""",
		@"""an identifier""",
		@"""FWDCURLY""",
		@"""BACKCURLY""",
		@"""TestScript""",
		@"""commandTimeout""",
		@"""waitForFinal""",
		@"""true""",
		@"""false""",
		@"""ProvisionalAsserts""",
		@"""resultCode""",
		@"""COMPARE_OPERATOR""",
		@"""Connect""",
		@"""ipAddress""",
		@"""port""",
		@"""connectionId""",
		@"""conferenceId""",
		@"""sessionTimeout""",
		@"""transactionId""",
		@"""clientId""",
		@"""FinalAsserts""",
		@"""Disconnect""",
		@"""Play""",
		@"""Record""",
		@"""filename""",
		@"""expires""",
		@"""audioFileAttribute""",
		@"""terminationCondition""",
		@"""maxtime""",
		@"""digit""",
		@"""digitlist""",
		@"""maxdigits""",
		@"""silence""",
		@"""nonsilence""",
		@"""digitdelay""",
		@"""digitpattern""",
		@"""loopcurrent""",
		@"""interdigdelay""",
		@"""userstop""",
		@"""eod""",
		@"""tone""",
		@"""deviceerror""",
		@"""maxdata""",
		@"""coderTypeRemote""",
		@"""framesizeRemote""",
		@"""vadEnableRemote""",
		@"""dataflowDirection""",
		@"""coderTypeLocal""",
		@"""framesizeLocal""",
		@"""vadEnableLocal""",
		@"""soundTone""",
		@"""noTone""",
		@"""soundToneWhenReceiveOnly""",
		@"""noToneWhenReceiveOnly""",
		@"""monitor""",
		@"""receiveOnly""",
		@"""tariffTone""",
		@"""coach""",
		@"""pupil""",
		@"""format""",
		@"""encoding""",
		@"""bitrate""",
		@"""serverId""",
		@"""heartbeatId""",
		@"""connectionAttribute""",
		@"""conferenceAttribute""",
		@"""confereeAttribute""",
		@"""mediaElapsedTime""",
		@"""heartbeatInterval""",
		@"""heartbeatPayload""",
		@"""machineName""",
		@"""queueName""",
		@"""callState""",
		@"""digits""",
		@"""query""",
		@"""ASSIGNMENT""",
		@"""COMPARISON""",
		@"""PLUS""",
		@"""MINUS""",
		@"""TIMES""",
		@"""DIV""",
		@"""COLON""",
		@"""SEMI""",
		@"""COMMA""",
		@"""LBRACKET""",
		@"""RBRACKET""",
		@"""LPAREN""",
		@"""RPAREN""",
		@"""an identifier""",
		@"""a quoted string""",
		@"""WS"""
	};
	
	private static long[] mk_tokenSet_0_()
	{
		long[] data = { 2L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_0_ = new BitSet(mk_tokenSet_0_());
	private static long[] mk_tokenSet_1_()
	{
		long[] data = { 1924686413824L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_1_ = new BitSet(mk_tokenSet_1_());
	private static long[] mk_tokenSet_2_()
	{
		long[] data = { 3145728L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_2_ = new BitSet(mk_tokenSet_2_());
	private static long[] mk_tokenSet_3_()
	{
		long[] data = { 1924687462400L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_3_ = new BitSet(mk_tokenSet_3_());
	private static long[] mk_tokenSet_4_()
	{
		long[] data = { -1143916230490456064L, 32767L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_4_ = new BitSet(mk_tokenSet_4_());
	private static long[] mk_tokenSet_5_()
	{
		long[] data = { 131072L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_5_ = new BitSet(mk_tokenSet_5_());
	private static long[] mk_tokenSet_6_()
	{
		long[] data = { 137440002048L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_6_ = new BitSet(mk_tokenSet_6_());
	private static long[] mk_tokenSet_7_()
	{
		long[] data = { -1152921230789050368L, 4095L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_7_ = new BitSet(mk_tokenSet_7_());
	private static long[] mk_tokenSet_8_()
	{
		long[] data = { 50600083456L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_8_ = new BitSet(mk_tokenSet_8_());
	private static long[] mk_tokenSet_9_()
	{
		long[] data = { 1048576L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_9_ = new BitSet(mk_tokenSet_9_());
	private static long[] mk_tokenSet_10_()
	{
		long[] data = { 50601132032L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_10_ = new BitSet(mk_tokenSet_10_());
	private static long[] mk_tokenSet_11_()
	{
		long[] data = { 268435456L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_11_ = new BitSet(mk_tokenSet_11_());
	private static long[] mk_tokenSet_12_()
	{
		long[] data = { 253416701952L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_12_ = new BitSet(mk_tokenSet_12_());
	private static long[] mk_tokenSet_13_()
	{
		long[] data = { 240531800064L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_13_ = new BitSet(mk_tokenSet_13_());
	private static long[] mk_tokenSet_14_()
	{
		long[] data = { 34495004672L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_14_ = new BitSet(mk_tokenSet_14_());
	private static long[] mk_tokenSet_15_()
	{
		long[] data = { 2199023255552L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_15_ = new BitSet(mk_tokenSet_15_());
	private static long[] mk_tokenSet_16_()
	{
		long[] data = { 8974454504816640L, 28672L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_16_ = new BitSet(mk_tokenSet_16_());
	private static long[] mk_tokenSet_17_()
	{
		long[] data = { 8972255481561088L, 28672L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_17_ = new BitSet(mk_tokenSet_17_());
	private static long[] mk_tokenSet_18_()
	{
		long[] data = { 1145990217799303168L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_18_ = new BitSet(mk_tokenSet_18_());
	private static long[] mk_tokenSet_19_()
	{
		long[] data = { 393216L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_19_ = new BitSet(mk_tokenSet_19_());
	private static long[] mk_tokenSet_20_()
	{
		long[] data = { 1145990217800351744L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_20_ = new BitSet(mk_tokenSet_20_());
	private static long[] mk_tokenSet_21_()
	{
		long[] data = { 33191588003840L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_21_ = new BitSet(mk_tokenSet_21_());
	private static long[] mk_tokenSet_22_()
	{
		long[] data = { 1145992382462820352L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_22_ = new BitSet(mk_tokenSet_22_());
	private static long[] mk_tokenSet_23_()
	{
		long[] data = { 1145992382463868928L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_23_ = new BitSet(mk_tokenSet_23_());
	
}
}
