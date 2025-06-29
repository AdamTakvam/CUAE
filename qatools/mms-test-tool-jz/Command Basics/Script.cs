using System;
using System.Diagnostics;

using AST                      = antlr.collections.AST;
using CommonAST				   = antlr.CommonAST;
using ASTPair                  = antlr.ASTPair;
using ASTFactory               = antlr.ASTFactory;
using ANTLRException           = antlr.ANTLRException;

using Metreos.MMSTestTool.Commands;


namespace Metreos.MMSTestTool.Commands
{
	/// <summary>
	/// Abstraction for the Script block
	/// </summary>
    public class Script : CommandBase
    {
        public Command[] Commands
        {
            get
            {
                return commands;
            }
        }

        public int CommandTimeoutMsecs
        {
            get { return commandTimeoutMsecs; }
            set { commandTimeoutMsecs = value; }
        }
        
        private Command[] commands;
        private int currentCommandIndex;
        private int commandTimeoutMsecs;
        
        public Script(string name)
        {
            this.name = name == null ? string.Empty : name;
        }

        public Script(string name, int commandTimeoutMsecs) : this(name)
        {
            this.commandTimeoutMsecs = commandTimeoutMsecs;
        }

        public Script(Script template)
        {
            this.name = template.name;
            this.commands = new Command[template.commands.Length];
            for (int i = 0; i < template.commands.Length; i++)
                this.commands[i] = new Command(template.commands[i]);
			
            this.currentCommandIndex = template.currentCommandIndex;
            this.commandTimeoutMsecs = template.commandTimeoutMsecs;
        }


        public Command GetNextCommand()
        {
            if (currentCommandIndex < commands.Length)
            {
                Command command = commands[currentCommandIndex];
                currentCommandIndex++;
                return command;
            }
            else 
                return null;
        }

        public void CopyFromArray(object[] objectArray)
        {
            commands = new Command[objectArray.Length];
            for (int i = 0; i < objectArray.Length; i++)
                commands[i] = (Command)objectArray[i];
        }

        public override string ToString()
        {
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            sBuilder.AppendFormat("{0} {1}{2}", Constants.SCRIPT_STRING, this.Name, "\n{\n");
            string tabulator = "\t";
            foreach (Command command in this.Commands)
                sBuilder.AppendFormat("{0}{1} {2}{3}", tabulator, command.CommandType, command.Name, "\n");
            sBuilder.Append("}\n");

            return sBuilder.ToString();
        }
    }
}
