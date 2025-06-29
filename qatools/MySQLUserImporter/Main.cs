using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
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
        public static Regex checkAlphanumeric = new Regex("^[\\w@\\.]+$",RegexOptions.Compiled | RegexOptions.ECMAScript);
        public static Regex checkNumeric = new Regex("^[0-9]+$", RegexOptions.Compiled | RegexOptions.ECMAScript);
        public static Regex checkE164 = new Regex(@"^1[2-9]\d\d[2-9]\d\d\d\d\d\d$", RegexOptions.Compiled | RegexOptions.ECMAScript);
        public MySqlUserImporter(){ users = new ArrayList(); }
        protected ArrayList users;
        protected static string[] testFiles = { "test1.csv", "nofirstname.csv", "nolastname.csv", "noemail.csv", 
            "nousername.csv", "nopassword.csv", "nogmtoffset.csv", "nodevicenumber.csv", "bademail.csv",
            "badusername.csv", "badpassword.csv", "badgmtoffset.csv", "baddevicenumber.csv",
            "dupemail.csv", "dupusername.csv", "dupdevicenumber.csv"};

        protected bool Import(string inputCsv)
        {
            return Import(inputCsv, true);
        }

        protected bool Import(string inputCsv, bool importToDb)
        {
            Console.WriteLine("\nProcessing {0}...", inputCsv);
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
                                        Console.WriteLine("Email is empty at row {0}", rowCount);
                                    }
                                        // Might as well check for the @ sign, and trailing domain name
                                    else if(email.IndexOf('@') < 0)
                                    {
                                        formatOk = false;
                                        Console.WriteLine("The email '{0}' at row {1} doesn't contain an '@'", email, rowCount);
                                    }
                                    else
                                    {
                                        int index = email.IndexOf('@');
                                        if(index == 0)
                                        {
                                            // Found an empty username for email.  
                                            formatOk = false;
                                            Console.WriteLine("The email '{0}' at row {1} appears invalid", elements[emailPos], rowCount);
                                        }
                                        else
                                        {
                                            email = email.Substring(index);
                                            string[] domainBits = email.Split( new char[] {'.'} );
                                            foreach(string bit in domainBits)
                                            {
                                                if(bit.Length == 0)
                                                {
                                                    // Found an empty domain bit.  
                                                    formatOk = false;
                                                    Console.WriteLine("The email '{0}' at row {1} appears invalid", elements[emailPos], rowCount);
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    // Validate username (must be an email as well)
                                    if(username == String.Empty)
                                    {
                                        formatOk = false;
                                        Console.WriteLine("Username is empty at row {0}", rowCount);
                                    }
                                        // Might as well check for the @ sign, and trailing domain name
                                    else if(username.IndexOf('@') < 0)
                                    {
                                        formatOk = false;
                                        Console.WriteLine("The username '{0}' at row {1} doesn't contain an '@'", username, rowCount);
                                    }
                                    else
                                    {
                                        int index = username.IndexOf('@');
                                        if(index == 0)
                                        {
                                            // Found an empty username for email.  
                                            formatOk = false;
                                            Console.WriteLine("The username '{0}' at row {1} appears invalid", elements[usernamePos], rowCount);
                                        }
                                        else
                                        {
                                            username = username.Substring(index);
                                            string[] domainBits = username.Split( new char[] {'.'} );
                                            foreach(string bit in domainBits)
                                            {
                                                if(bit.Length == 0)
                                                {
                                                    // Found an empty domain bit.  
                                                    formatOk = false;
                                                    Console.WriteLine("The username '{0}' at row {1} appears invalid", elements[usernamePos], rowCount);
                                                    break;
                                                }
                                            }
                                        }
                                    }

//                                    // Username should be alphanumeric
//                                    if(username == String.Empty)
//                                    {
//                                        formatOk = false;
//                                        Console.WriteLine("User Name is empty at row {0}", rowCount);
//                                    }
//                                    else if(!checkAlphanumeric.IsMatch(username))
//                                    {
//                                        formatOk = false;
//                                        Console.WriteLine("Username '{0}' is not comprised of a-z, 0-9, _, @, ., at row {1}", username, rowCount); 
//                                    }

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
                                        Console.WriteLine("Device Number '{0}' is not of the form 1[2-9]XX[2-9]XXXXXX at row {1}", deviceNumber, rowCount);
                                    }
                                }

                                users.Add(elements);
                                rowCount++;
                            }
                        }
                    }

                    // If successful so far, check through all data for duplicates etc 
            
                    if(formatOk)
                    {
                        foreach(string[] elements in users)
                        {
                            string firstName = elements[firstNamePos];
                            string lastName = elements[lastNamePos];
                            string email = elements[emailPos];
                            string username = elements[usernamePos];
                            string password = elements[passwordPos];
                            string gmtOffset = elements[gmtOffsetPos];
                            string deviceNumber = elements[deviceNumberPos];

                            foreach(string[] otherElements in users)
                            {
                                if(elements == otherElements) continue;

                                if(String.Compare(email, otherElements[emailPos], true) == 0)
                                {
                                    formatOk = false;
                                    Console.WriteLine("Duplicate email found! {0}", email);
                                }

                                if(String.Compare(username, otherElements[usernamePos], true) == 0)
                                {
                                    formatOk = false;
                                    Console.WriteLine("Duplicate username found! {0}", username);
                                }

                                if(String.Compare(deviceNumber, otherElements[deviceNumberPos], true) == 0)
                                {
                                    formatOk = false;
                                    Console.WriteLine("Duplicate device number found! {0}", deviceNumber);
                                }
                            }
                        }
                    }

                    if(!formatOk)
                    {
                        Console.WriteLine("CSV was found to not be formatted correctly");
                    }
                    else
                    {
                        Console.WriteLine("\nCSV was found to be formatted correctly!");

                        if(importToDb)
                        {
                            Console.WriteLine("Importing users...");
                            success = ImportUsers(users);
                        }
                        else
                        {
                            success = importToDb;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("File {0} not found.", inputCsv);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("A unanticipated error occured! \n\n {0}", Exceptions.FormatException(e));
            }

            

            return success;
        }

        protected bool ImportTest(string sampleDir)
        {
            bool success = true;

            for(int i = 0; i < testFiles.Length; i++)
            {
                success &= Import(Path.Combine(sampleDir, testFiles[i]), false);
                // Clear db again
                
            }
            
            return success;
        }

        protected bool ImportUsers(ArrayList users)
        {
            bool success = false;

            string dsn = Database.FormatDSN("application_suite", "localhost", 3306, "root", "metreos", true);

            IDbConnection connection = null;
            try
            {
                connection = Database.CreateConnection(Database.DbType.mysql, dsn);
                connection.Open();
                success = true;
            }
            catch
            {
                Console.WriteLine("Unable to create connection to the MySQL database.\nPlease note that this tool must be executed on the publisher MCE in order to add users.\n");
                success = false;
            }

            if(success)
            {
                for(int i = 0; i < users.Count; i++)
                {
                    string[] elements = users[i] as string[];

                    string firstName = elements[firstNamePos];
                    string lastName = elements[lastNamePos];
                    string email = elements[emailPos];
                    string username = elements[usernamePos];
                    string password = elements[passwordPos];
                    string gmtOffset = elements[gmtOffsetPos];
                    string deviceNumber = elements[deviceNumberPos];

                    SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.INSERT, "as_users");
                    builder.AddFieldValue("username", username);
                    builder.AddFieldValue("password", Security.EncryptPassword(password));
                    builder.AddFieldValue("account_code", i + 10000); // Mike Neal has asked that the accounts start numbering 10000
                    builder.AddFieldValue("pin", int.Parse(password));
                    builder.AddFieldValue("first_name", firstName);
                    builder.AddFieldValue("last_name", lastName);
                    builder.AddFieldValue("email", email);
                    builder.AddFieldValue("status", 1);
                    builder.AddFieldValue("gmt_offset", int.Parse(gmtOffset));
                    builder.AddFieldValue("created", new SqlBuilder.PreformattedValue("NOW()"));
                    builder.AddFieldValue("lockout_threshold", 3);

                    using(IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = builder.ToString();
                        command.ExecuteNonQuery();
                    }

                    int userId = GetLastAutoId(connection);

                    // Populate device for user, adding one line as well

                    SqlBuilder addDeviceSql = new SqlBuilder(SqlBuilder.Method.INSERT, "as_phone_devices");
                    addDeviceSql.AddFieldValue("as_users_id", userId);
                    addDeviceSql.AddFieldValue("is_primary_device", 1);
                    addDeviceSql.AddFieldValue("is_ip_phone", 0);
                    addDeviceSql.AddFieldValue("name", "Office Phone");

                    using(IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = addDeviceSql.ToString();
                        command.ExecuteNonQuery();
                    }

                    int deviceId = GetLastAutoId(connection);

                    // Add line to device

                    SqlBuilder addLineSql = new SqlBuilder(SqlBuilder.Method.INSERT, "as_directory_numbers");
                    addLineSql.AddFieldValue("as_phone_devices_id", deviceId);
                    addLineSql.AddFieldValue("directory_number", deviceNumber);
                    addLineSql.AddFieldValue("is_primary_number", 1);
                    using(IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = addLineSql.ToString();
                        command.ExecuteNonQuery();
                    }
                }
            }

            return success;
        }
        
        protected int GetLastAutoId(IDbConnection connection)
        {
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT LAST_INSERT_ID()";
                return Convert.ToInt32(command.ExecuteScalar());
            }
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
                try
                {
                    // check for test mode
                    if(new DirectoryInfo(args[0]).Name == "Samples")
                    {
                        MySqlUserImporter testImporter = new MySqlUserImporter();
                        bool testSuccess = testImporter.ImportTest(args[0]);

                        Console.WriteLine("Test done");

                        return;
                    }
                }
                catch { }

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
