/* $Id: Program.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 27, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

/**
 * Description of Program
 */
abstract public class Program
{
	/**
	 * @param program
	 * @param args
	 * @throws Exception
	 */
	protected static void main( Program program, String[] args ) throws Exception
	{
		CommandParser cp = new CommandParser( program );
		
		cp.defineNullOption( program.getHelpTokens(), "doHelp",
			"print a description of the command options and parameters and then exit.",
			Option.NONE );
		
		program.defineOptionsAndParameters( cp );
		
		if (!cp.parse( args ))
			System.exit( 1 );
		
		program.run();
	}

	/**
	 * @return the list of tokens the user might specify to get help.
	 * The default list of tokens is "--help|-help|-h|?".
	 */
	protected String getHelpTokens()
	{
		return "--help|-help|-h|?";
	}

	/**
	 * Prints the help message for the program.
	 * @param cp
	 * @param option
	 * @param token
	 * @return false indicating the program should exit.
	 */
	public boolean doHelp( CommandParser cp, Option option, String token )
	{
		cp.showHelp( false );
		return false;
	}

	@Override
	public String toString()
	{
		return getClass().getName().toString();
	}
	
	/**
	 * Gives the program a chance to define its options and parameters.
	 * @param cp
	 * @throws Exception
	 */
	abstract protected void defineOptionsAndParameters( CommandParser cp )
		throws Exception;
	
	/**
	 * Runs the program after successful command line parsing.
	 * @throws Exception
	 */
	abstract protected void run() throws Exception;
}
