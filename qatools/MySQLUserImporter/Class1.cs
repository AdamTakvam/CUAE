using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using Metreos.Utilities;

namespace Metreos.Import
{
    public class MySqlUserImporter
    {
        public const string Usage = @"
bit.exe [Input CSV Filepath]

[Input CSV Filepath] -- A relative or full path to the CSV file containing
                        formatted user information.

CSV Format:
First Name, Last Name, Email, User Name, Password, GMT Offset, Device Number 
";
        public const int firstNamePos = 0;
        public const int lastNamePos = 1;
        public const int emailPos = 2;
        public const int usernamePos = 3;
        public const int passwordPos = 4;
        public const int gmtOffsetPos = 5;
        public const int deviceNumberPos = 6;
        public static Regex checkAlphanumeric = new Regex("[\\w@\\.]+",RegexOptions.Compiled | RegexOptions.ECMAScript);
        public static Regex checkNumeric = new Regex("[0-9]+", RegexOptions.Compiled | RegexOptions.ECMAScript);
        public static Regex checkE164 = new Regex(@"1\[2-9]\d\d[2-9]\d\d\d\d\d\d", RegexOptions.Compiled | RegexOptions.ECMAScript);
        public MySqlUserImporter(){ users = new ArrayList(); }
        protected ArrayList users;
 
        protected bool Import(string inputCsv)
        {
            users.Clear();

            bool success = false;

            try
            {
                bool formatOk = true;
                if(File.Exists(inputCsv))
                {
                    using(FileStream stream = File.OpenRead(inputCsv))
                    {
                        using(StreamReader reader = new StreamReader(stream))
                        {
                            int rowCount = 1;
                            while(reader.Peek() > -1)
                            {
                                string line = reader.ReadLine();
                                
                                string[] elements = line.Split(new char[] { ','}); 

                                if(elements.Length != 7)
                                {
                                    formatOk = false;
                                    Console.WriteLine("Row {0} does not have the correct number of columns.  {1} colums found.", rowCount, elements.Length);
                                }
                                else
                                {
                                    // Trim off extra whitespace
                                    for(int i = 0; i < elements.Length; i++)
                                    {
                                        elements[i] = elements[i].Trim();
                                    }

                                    // Check each field
                                
                                    string firstName = elements[firstNamePos];
                                    string lastName = elements[lastNamePos];
                                    string email = elements[emailPos];
                                    string username = elements[usernamePos];
                                    string password = elements[passwordPos];
                                    string gmtOffset = elements[gmtOffsetPos];
                                    string deviceNumber = elements[deviceNumberPos];

                                    // Nothing to check with first name.  Just opaque string
                                    // Nothing to check with last name.  Just opaque string
                                    
                                    // Validate email
                                    if(email == String.Empty)
                                    {
                                        formatOk = false;
                                        Console.WriteLine("Email address is empty at row {0}", rowCount);
                                    }
                                    // Might as well check for the @ sign, and trailing domain name
                                    else if(email.IndexOf('@') < 0)
                                    {
                                        formatOk = false;
                                        Console.WriteLine("The email address '{0}' at row {1} doesn't contain an '@'", email, rowCount);
                                    }
                                    else
                                    {
                                        email = email.Substring(email.IndexOf('@'));
                                        string[] domainBits = email.Split( new char[] {'.'} );
                                        foreach(string bit in domainBits)
                                        {
                                            if(bit.Length == 0)
                                            {
                                                // Found an empty domain bit.  
                                                formatOk = false;
                                                Console.WriteLine("The email address '{0}' at row {1} appears invalid", elements[emailPos], rowCount);
                                                break;
                                            }
                                        }
                                    }

                                    // Username should be alphanumeric
                                    if(username == String.Empty)
                                    {
                                        formatOk = false;
                                        Console.WriteLine("User Name is empty at row {0}", rowCount);
                                    }
                                    else if(!checkAlphanumeric.IsMatch(username))
                                    {
                                        formatOk = false;
                                        Console.WriteLine("Username '{0}' is not comprised of a-z, 0-9, _, @, ., at row {1}", username, rowCount); 
                                    }

                                    // Password should be numeric, because pin must derive from it
                                    if(password == String.Empty)
                                    {
                                        formatOk = false;
                                        Console.WriteLine("Password is empty at row {0}", rowCount);
                                    }
                                    else if(!checkNumeric.IsMatch(password))
                                    {
                                        formatOk = false;
                                        Console.WriteLine("Password '{0}' is not comprised of 0-9 characters at row {1}", password, rowCount);
                                    }

                                    // GMT Offset must be -15 to 15

                                    if(gmtOffset == String.Empty)
                                    {
                                        formatOk = false;
                                        Console.WriteLine("GMT Offset is empty at row {0}", rowCount);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int gmtOffsetInt = int.Parse(gmtOffset);

                                            if(gmtOffsetInt > -16 && gmtOffsetInt < 16)
                                            {
                                            }
                                            else
                                            {
                                                Console.WriteLine("GMT Offset '{0}' at row '{1}' must be between -15 and 15", gmtOffsetInt, rowCount);
                                                formatOk = false;
                                            }
                                        }
                                        catch
                                        {
                                            Console.WriteLine("GMT Offset '{0}' at row '{1}' is not a number", gmtOffset, rowCount);
                                            formatOk = false;
                                        }
                                   } 

                                    // Device number should be E.164, because pin must derive from it
                                    if(deviceNumber == String.Empty)
                                    {
                                        formatOk = false;
                                        Console.WriteLine("Device Number is empty at row {0}", rowCount);
                                    }
                                    else if(!checkE164.IsMatch(deviceNumber))
                                    {
                                        formatOk = false;
                                        Console.WriteLine("Device Number '{0}' is an E.164-formatted number at row {1}", deviceNumber, rowCount);
                                    }
                                }

                                users.Add(elements);
                                rowCount++;
                            }
                        }
                    }

                    if(!formatOk)
                    {
                        Console.WriteLine("CSV was found to not be formatted correctly");
                    }
                    else
                    {
                        Console.WriteLine("CSV was found to be formatted correctly.  Importing users...");

                        success = ImportUsers(users);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("A unanticipated error occured! \n\n {0}", Exceptions.FormatException(e));
            }

            return success;
        }

        protected bool ImportUsers(ArrayList users)
        {
            bool success = true;

            return success;
        }
        
        
        [STAThread]
        static void Main(string[] args)
        {
            if(args == null || args.Length != 1)
            {
                Console.WriteLine(Usage);
            }
            else
            {
                MySqlUserImporter importer = new MySqlUserImporter();
                bool success = importer.Import(args[0]);

                if(success)
                {
                    Console.WriteLine("\nImport of users succeeded");
                }
                else
                {
                    Console.WriteLine("\nImport of users failed");
                }
            }
        }
    }

}
