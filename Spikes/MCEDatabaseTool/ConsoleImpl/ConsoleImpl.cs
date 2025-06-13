using System;
using Metreos.Utilities;

namespace Metreos.Database
{
	class ConsoleImpl
	{
        public const string usage = @"
Parameters
-a:   action   makeusers (-1:[num_users])| deleteusers 
-1:   additional info
";
        public const string Action = "a";
        public const string AddInfo1 = "1";

        public const string MakeUsersAction = "makeusers";
        public const string DeleteUsersAction = "deleteusers";

		[STAThread]
		static void Main(string[] args)
		{
            CommandLineArguments parser = new CommandLineArguments(args);

            string action = parser.GetSingleParam(Action);

            if(action == null || action == String.Empty) 
            {
                Console.WriteLine("You must specify an action.\n{0}", usage);
                return;
            }
              
            MceDatabaseTool dbTool = new MceDatabaseTool();
            dbTool.Error += new Log(LogError);
            dbTool.Output += new Log(LogOutput);
            switch(action)
            {
                case MakeUsersAction:
                    
                    string numUsersString = parser.GetSingleParam(AddInfo1);
                    int numUsers;
                    if(numUsersString == null || numUsersString == String.Empty)
                    {
                        Console.WriteLine("You must specify the number of users.\n{0}", usage);
                        return;
                    }

                    try
                    {
                        numUsers = int.Parse(numUsersString);
                        
                        if(!dbTool.CreateRandomUsers(numUsers))
                        {
                            Console.WriteLine("Unable to 'makeusers'");
                        }
                        else
                        {
                            Console.WriteLine("Created users");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Number of users was specified, but was not a number.");
                    }

                    break;

                case DeleteUsersAction:

                    if(!dbTool.DeleteAllUsers())
                    {
                        Console.WriteLine("Unable to 'deleteusers'");
                    }
                    else
                    {
                        Console.WriteLine("Deleted users");
                    }

                    break;
            }
			
        
            
            }

        private static void LogError(string message)
        {
            Console.WriteLine("Error: {0}", message);
        }

        private static void LogOutput(string message)
        {
            Console.WriteLine("Info:  {0}", message);
        }
    }
}
