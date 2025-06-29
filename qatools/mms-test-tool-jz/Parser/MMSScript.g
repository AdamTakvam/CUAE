/*	This grammar defines the scripting language to be used in the Metreos MultiMedia Server Test Tool
	Written by John Zdunczyk
	(C) 2004 Metreos Corporation
*/
header 
{
    // gets inserted in the C# source file before any
    // generated namespace declarations
    // hence -- can only be using directives
	using System.IO;
	using System.Collections;
	using Metreos.MMSTestTool.Commands;
	using Metreos.MMSTestTool.Sessions;
	
	using AST                      = antlr.collections.AST;
	using CommonAST				   = antlr.CommonAST;
}

options {
	language  =  "CSharp";
	namespace  =  "Metreos.MMSTestTool.Parser";
}

class MMSScriptParser extends Parser;

options {
	defaultErrorHandler=true;
	buildAST=true;
	k=2;
}


tokens
{
	NFixture;
	NScript;
	NCommand;
	NParameter			=	"NParameter";
	NReqParameter;
	NParameterList;
	NAssert				=	"NAssert";
	NProvisionalAssert;
	NAssertList			=	"NAssertList";
	NFile				=	"NFile";
	NCommandOption;
	NScriptOption;
}

{
		
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
}

fixture : 
	(
		f:TEST_FIXTURE_LITERAL^ ASSIGN! ID FWDCURLY! (script)+ BACKCURLY!  {#f.setType(NFixture); CreateFixtureInstance(#fixture);}
	)
;

script :
	(
		t:TESTSCRIPT_LITERAL^ ASSIGN! ID FWDCURLY! (cmd_block)+ BACKCURLY! {#t.setType(NScript); CreateScriptInstance(#script);}
	)
;

cmd_block :
	(  connect_block | disconnect_block | play_block | record_block | script_level_options
	) {CreateCommand(#cmd_block);}
;

script_level_options :
	(
		a:COMMAND_TIMEOUT ASSIGN! b:ID {#script_level_options.setType(NScriptOption); SetScriptOption(a.getText(), b.getText());}
	)
;

command_option! : 
	( 
		t:command_option_token_true_false ASSIGN! v:command_option_values_true_false {#command_option=#([NCommandOption],t,v);}
	)	
;	

//Command options that are either true or false
command_option_token_true_false :
	(
		WAIT_FOR_FINAL
	)
;

//Values for the above command options
command_option_values_true_false :
	(
		TRUE | FALSE
	)
;

provisional_assert :
	( c:PROVASSERT_LITERAL^ FWDCURLY! RESULT_CODE COMPARE_OPERATOR ID BACKCURLY!
	{#c.setType(NProvisionalAssert);}
	)
;


// &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&


connect_block :
	(	
		(c:CONNECT_LITERAL^ (ASSIGN! i:ID)?) FWDCURLY! (connect_params | command_option)* (connect_assert)? BACKCURLY!
		{#c.setType(NCommand); }
	)
;

//we change the tree generating behavior here so that each parameter is a separate child of the connect block.
connect_params! : 
	(	
	c:connect_params_list ASSIGN! i:ID
	{#connect_params = #([NParameter],c,i);}
	)
;
connect_params_list :
	(
		IP_ADDRESS | PORT | CONNECTION_ID | CONFERENCE_ID | SESSION_TIMEOUT | COMMAND_TIMEOUT | conference_attribute_names
		| conferee_attribute_names | TRANSACTION_ID | CLIENT_ID | connection_attribute_names 
	) 
;

connect_assert :
	( FINALASSERTS_LITERAL! FWDCURLY! (c:connect_assert_line)* BACKCURLY! 
	)
;
	
connect_assert_line! :
	( c:connect_assert_tokens op:COMPARE_OPERATOR i:ID
	{#connect_assert_line = #([NAssert], c, op, i);}
	)
;

connect_assert_tokens	: 
	( RESULT_CODE | CONNECTION_ID |	CONFERENCE_ID | PORT | IP_ADDRESS | TRANSACTION_ID 
	) {#connect_assert_tokens.setType(NAssert);}
;



/* &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& */

	
disconnect_block : 
	(
			c:DISCONNECT_LITERAL^ (ASSIGN! i:ID)? FWDCURLY! (r:disconnect_reqparams)+  (disconnect_params | command_option)* (a:disconnect_assert)? BACKCURLY!
			{#c.setType(NCommand);}
	)
;
disconnect_reqparams! :
	(
	c:disconnect_reqparams_list ASSIGN! i:ID
	{#disconnect_reqparams = #(#[NParameter],c,i);}
	)	
;	
disconnect_reqparams_list :
	(
		(CONNECTION_ID | CONFERENCE_ID)
	) 
;
disconnect_params! :
	(
	c:disconnect_params_list ASSIGN! i:ID
	{#disconnect_params = #(#[NParameter],c,i);}
	)
;		
disconnect_params_list :
	(
		(COMMAND_TIMEOUT | TRANSACTION_ID | CLIENT_ID)
	) 
;

disconnect_assert :
	( FINALASSERTS_LITERAL! FWDCURLY! (c:disconnect_assert_line)* BACKCURLY! 
	)
;

disconnect_assert_line! :
	(c:disconnect_assert_tokens op:COMPARE_OPERATOR i:ID)
	{#disconnect_assert_line = #([NAssert],c,op,i);}
;

disconnect_assert_tokens	: 
	( RESULT_CODE | TRANSACTION_ID 
	)
;
	
// &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
	
play_block : 
	(
			c:PLAY_LITERAL^ (ASSIGN! i:ID)? FWDCURLY! play_reqparams  (f:file)+ (play_params | command_option)* (provisional_assert)? (a:play_assert)? BACKCURLY!
			{#c.setType(NCommand);}
	)
;
play_reqparams! :
	(
	(c:play_reqparams_list ASSIGN! i:ID)
	{#play_reqparams = #(#[NParameter],c,i);}
	)	
;	
play_reqparams_list :
	(
		(CONNECTION_ID | CONFERENCE_ID)
	) 
;

play_params! :
	(
	c:play_params_list ASSIGN! i:ID
	{#play_params = #(#[NParameter],c,i);}
	)
;		
play_params_list :
	(
		(COMMAND_TIMEOUT | TRANSACTION_ID | CLIENT_ID | inbound_termination_condition_names | audio_attribute_names)
	) 
;

play_assert :
	( FINALASSERTS_LITERAL! FWDCURLY! (c:play_assert_line)* BACKCURLY! 
	)
;

play_assert_line! :
	(c:play_assert_tokens op:COMPARE_OPERATOR i:ID)
	{#play_assert_line = #([NAssert],c,op,i);}
;

play_assert_tokens	: 
	( RESULT_CODE | TRANSACTION_ID | outbound_termination_condition_names 
	)
;
	
// &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

/*	 ***** Currently not implemented?
playTone_block : 
	(
			c:PLAYTONE_LITERAL^ FWDCURLY! r:playTone_reqparams  (p:playTone_params)* (provisional_assert)? (a:playTone_assert)? BACKCURLY!
			{#c.setType(NCommand);}
	)
;
playTone_reqparams! :
	(
	(c:playTone_reqparams_list ASSIGN! i:ID)
	{#playTone_reqparams = #(#[NParameter],c,i);}
	)	
;	
playTone_reqparams_list :
	(
		(CONNECTION_ID | CONFERENCE_ID)
	) 
;

playTone_params! :
	(
	c:playTone_params_list ASSIGN! i:ID
	{#playTone_params = #(#[NParameter],c,i);}
	)
;		
playTone_params_list :
	(
		(COMMAND_TIMEOUT | TRANSACTION_ID | CLIENT_ID | inbound_termination_condition_names)
	) 
;

playTone_assert :
	( FINALASSERTS_LITERAL! FWDCURLY! (c:playTone_assert_line)* BACKCURLY! 
	)
;

playTone_assert_line! :
	(c:playTone_assert_tokens op:COMPARE_OPERATOR i:ID)
	{#playTone_assert_line = #([NAssert],c,op,i);}
;

playTone_assert_tokens	: 
	( RESULT_CODE | TRANSACTION_ID | outbound_termination_condition_names 
	)
;
*/
// &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

record_block : 
	(
			c:RECORD_LITERAL^ (ASSIGN! i:ID)? FWDCURLY! r:record_reqparams  (record_params | command_option)* (provisional_assert)? (a:record_assert)? BACKCURLY!
			{#c.setType(NCommand);}
	)
;
record_reqparams! :
	(
	(c:record_reqparams_list ASSIGN! i:ID)
	{#record_reqparams = #(#[NParameter],c,i);}
	)	
;	
record_reqparams_list :
	(
		(CONNECTION_ID | CONFERENCE_ID)
	) 
;

/*
record_params! :
	(
	  c:record_params_list ASSIGN! i:ID {#record_params = #(#[NParameter],c,i);}
	) 
;	
*/

record_params! :
	(
	  c:record_params_list ASSIGN! i:record_params_choices {#record_params = #(#[NParameter],c,i);}
	) 
;		

record_params_choices :
	(
		i:ID {#record_params_choices = #i;}
		|  t:audio_attribute_names u:ID {string attr = #t + " " + #u; #record_params_choices.setText(attr);}
		|  a:inbound_termination_condition_names b:ID {string term = #a + " " + #b; #record_params_choices.setText(term);}
	) 
;

record_params_list :
	(
		(COMMAND_TIMEOUT | CLIENT_ID | FILENAME | EXPIRES | AUDIO_FILE_ATTRIBUTE | TERMINATION_CONDITION)
	) 
;

record_assert :
	( FINALASSERTS_LITERAL! FWDCURLY! (c:record_assert_line)* BACKCURLY! 
	)
;

record_assert_line! :
	(c:record_assert_tokens op:COMPARE_OPERATOR i:ID)
	{#record_assert_line = #([NAssert],c,op,i);}
;

record_assert_tokens	: 
	( RESULT_CODE | outbound_termination_condition_names | FILENAME 
	)
;
	
	
file	:
	(
		p:FILENAME^ ASSIGN! i:ID {#p.setType(NParameter);}
	) {#file = #(#[NParameter],p,i);}
;
	
// &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&


inbound_termination_condition_names :
	(MAXTIME
	|DIGIT
	|DIGITLIST
	|MAXDIGITS
	|SILENCE
	|NONSILENCE
	|DIGITDELAY
	|DIGITPATTERN
	)
;



outbound_termination_condition_names :
	(
	MAXTIME
	| MAXDIGITS
	| SILENCE
	| NONSILENCE
	| DIGIT
	| LOOPCURRENT
	| INTERDIGDELAY
	| USERSTOP
	| EOD
	| TONE
	| DEVICEERROR
	| MAXDATA
	)
;


connection_attribute_names :
(
 CODERTYPEREMOTE
 | FRAMESIZEREMOTE
 | VADENABLEREMOTE
 | DATAFLOWDIRECTION
 | CODERTYPELOCAL
 | FRAMESIZELOCAL
 | VADENABLELOCAL
)
;

conference_attribute_names :
(  SOUND_TONE
 | NO_TONE
 | SOUND_TONE_WHEN_RECEIVE_ONLY
 | NO_TONE_WHEN_RECEIVE_ONLY
)
;

conferee_attribute_names :
(
 MONITOR
 | RECEIVE_ONLY
 | TARIFF_TONE
 | COACH
 | PUPIL
)
;



audio_attribute_names :
	( FORMAT
	| ENCODING
	| BITRATE
	)
;

/* &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& */



class MMSScriptLexer extends Lexer;
options {
  k=2; // two characters of lookahead for things like >, >= etc etc.
  charVocabulary = '\0'..'\377';
  caseSensitiveLiterals = false;
}

tokens {
	//COMMAND tokens
	CONNECT_LITERAL			=	"Connect";
	DISCONNECT_LITERAL		=	"Disconnect";
	PLAY_LITERAL			=	"Play";
	RECORD_LITERAL			=	"Record";
	PROVASSERT_LITERAL		=	"ProvisionalAsserts";
	FINALASSERTS_LITERAL	=	"FinalAsserts";
	TEST_FIXTURE_LITERAL	=	"TestFixture";
	TESTSCRIPT_LITERAL		=	"TestScript";

	//Command Option Tokens
	WAIT_FOR_FINAL			=	"waitForFinal";
	TRUE					=	"true";
	FALSE					=	"false";
	
	RESULT_CODE				=	"resultCode";
	TRANSACTION_ID			=	"transactionId";
	CONNECTION_ID			=	"connectionId";
	CONFERENCE_ID			=	"conferenceId";
	CLIENT_ID				=	"clientId";
	SERVER_ID				=	"serverId";
	HEARTBEAT_ID			=	"heartbeatId";
	COMMAND_TIMEOUT			=	"commandTimeout";
	SESSION_TIMEOUT			=	"sessionTimeout";
	IP_ADDRESS				=	"ipAddress";
	PORT					=	"port";
	CONNECTION_ATTRIBUTE	=	"connectionAttribute";
	CONFERENCE_ATTRIBUTE	=	"conferenceAttribute";
	CONFEREE_ATTRIBUTE		=	"confereeAttribute";
	AUDIO_FILE_ATTRIBUTE	=	"audioFileAttribute";
	TERMINATION_CONDITION	=	"terminationCondition";
	MEDIA_ELAPSEDTIME		=	"mediaElapsedTime";
	FILENAME				=	"filename";
	HEARTBEAT_INTERVAL		=	"heartbeatInterval";
	HEARTBEAT_PAYLOAD		=	"heartbeatPayload";
	MACHINE_NAME			=	"machineName";
	QUEUE_NAME				=	"queueName";
	CALL_STATE				=	"callState";
	EXPIRES					=	"expires";
	DIGITS					=	"digits";
	QUERY					=	"query";
	
		
	//inbound_termination_condition_names literals:
	MAXTIME			=	"maxtime";
	DIGIT			=	"digit";
	DIGITLIST		=	"digitlist";
	MAXDIGITS		=	"maxdigits";
	SILENCE			=	"silence";
	NONSILENCE		=	"nonsilence";
	DIGITDELAY		=	"digitdelay";
	DIGITPATTERN	=	"digitpattern";

	//outbound_termination_condition_names literals:
	LOOPCURRENT		=	"loopcurrent";
	INTERDIGDELAY	=	"interdigdelay";
	USERSTOP		=	"userstop";
	EOD				=	"eod";
	TONE			=	"tone";
	DEVICEERROR		=	"deviceerror";
	MAXDATA			=	"maxdata";

	//CONNECTION_ATTRIBUTE_NAMES literals
	CODERTYPEREMOTE		=	"coderTypeRemote";
	FRAMESIZEREMOTE		=	"framesizeRemote";
	VADENABLEREMOTE		=	"vadEnableRemote";
	DATAFLOWDIRECTION	=	"dataflowDirection";
	CODERTYPELOCAL		=	"coderTypeLocal";
	FRAMESIZELOCAL		=	"framesizeLocal";
	VADENABLELOCAL		=	"vadEnableLocal";

	//conference_attribute_names :
	SOUND_TONE						=	"soundTone";
	NO_TONE							=	"noTone";
	SOUND_TONE_WHEN_RECEIVE_ONLY	=	"soundToneWhenReceiveOnly";
	NO_TONE_WHEN_RECEIVE_ONLY		=	"noToneWhenReceiveOnly";


	//audioFileAttributes
	FORMAT		=	"format";
	ENCODING	=	"encoding";
	BITRATE		=	"bitrate";
	
	//conferee_attribute_names :
	MONITOR			=	"monitor";
	RECEIVE_ONLY	=	"receiveOnly";
	TARIFF_TONE		=	"tariffTone";
	COACH			=	"coach";
	PUPIL			=	"pupil";

	ASSIGNMENT;
	COMPARISON;
}



PLUS       : '+'   ;
MINUS      : '-'   ;
TIMES      : '*'   ;
DIV        : '/'   ;
//Possible use in range checking.
//DOTDOT     : ".."  ;
COLON      : ':'   ;
SEMI       : ';'   ;
COMMA      : ','   ;


COMPARE_OPERATOR :
	("!=" | '<' | '>' | ">=" | "<=" //| "=="
	) //{$setType(COMPARISON);}
	
;

ASSIGN	:	'=' 
;

		
LBRACKET    : '['   ;
RBRACKET    : ']'   ;
LPAREN      : '('   ;
RPAREN      : ')'   ;
FWDCURLY	:	'{';
BACKCURLY	:	'}';



ID
options {
	testLiterals = false; 
	paraphrase = "an identifier";
}	
	: ( STRING | QUOTED_STRING )
	;
	

protected STRING
options {
	testLiterals = false; 
	paraphrase = "an identifier";
}
	:
	(  ( 'a'..'z' | 'A'..'Z' | '_' | '0'..'9' | '.' | '$')+
	)
      ;	
      

protected
QUOTED_STRING 
options {
	testLiterals = true;
	paraphrase = "a quoted string";
}
	: '"'! ( 'a'..'z' | 'A'..'Z' | '_' | '0'..'9' | ' ' | '\t' | '.' | '$')* '"'! 
	;

WS     :
    (' ' 
    | '\t'
    | "\r\n"			{ newline(); }
	| ('\n' | '\r')		{ newline(); } 
    ) 
    { $setType(Token.SKIP); } 
  ;