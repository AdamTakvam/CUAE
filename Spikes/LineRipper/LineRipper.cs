using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace FileParser
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class FileParser
	{
        static ArrayList regexToMatch;
        static string inputFileName;
        static string outputFileName;

		public FileParser()
		{
            regexToMatch = new ArrayList();
            
		}

        private void Go()
        {
            FileStream fileToOpen;
            try
            {fileToOpen = File.Open(inputFileName,FileMode.Open);
            }
            catch(Exception)
            {
                Console.WriteLine("Invalid input file specified");
                return;
            }

            StreamReader ftoReader;
            ftoReader = new StreamReader(fileToOpen);

            FileStream fileToCreate;
            try
            {
                fileToCreate = File.Create(outputFileName);
            }
            catch(Exception)
            {
                Console.WriteLine("Invalid output file specified");
                return;
            }

            StreamWriter ftcWriter = new StreamWriter(fileToCreate);

            while(ftoReader.Peek() != -1)
            {
                string tempString  = ftoReader.ReadLine();
                
                for(int i = 0; i < regexToMatch.Count; i++)
                {
                    Regex tempRegex = (Regex) regexToMatch[i];

                    if(tempRegex.IsMatch(tempString))
                    {
                        ftcWriter.WriteLine(tempString);
                        break;
                    }
                }
            }
            ftcWriter.Close();
            ftoReader.Close();
        }



        [STAThread]
        static void Main(string[] args)
        {
            FileParser fileParser = new FileParser();
            if(args.Length >= 2)
            {
                inputFileName = args[0];
                outputFileName = args[1];

                if(args.Length > 2)
                {
                    for(int i = 2; i < args.Length; i++)
                    {
                        regexToMatch.Add(new Regex(args[i]));
                    }
                
                    fileParser.Go();

                    fileParser = null;
                }
                else
                {
                    Console.WriteLine("Please enter regular expressions to match.");
                    Console.WriteLine("CommandLine format is as follows:");
                    Console.WriteLine("inputFileName outputFileName [regular expression to match]...");
                    Console.WriteLine("\n");
                    NetRegularExpressionsOutput();
                }
            }
            else
            {
                Console.WriteLine("CommandLine format is as follows:");
                Console.WriteLine("inputFileName outputFileName [regular expression to match]...");
                Console.WriteLine("\n");
                NetRegularExpressionsOutput();
            }
            return;
        }

        private static void NetRegularExpressionsOutput()
        {
            Console.WriteLine(@"

Character Escapes
-----------------
ordinary characters Characters other than . $ ^ { [ ( | ) * + ? \ match themselves. 

\a Matches a bell (alarm) \u0007. 

\b Matches a backspace \u0008 if in a [] character class; otherwise, see the note following this table. 

\t Matches a tab \u0009. 

\r Matches a carriage return \u000D. 

\v Matches a vertical tab \u000B. 

\f Matches a form feed \u000C. 

\n Matches a new line \u000A. 

\e Matches an escape \u001B. 

\040 Matches an ASCII character as octal (up to three digits); numbers with no leading zero are backreferences if they have only one digit or if they correspond to a capturing group number. (For more information, see Backreferences.) For example, the character \040 represents a space.
 
\x20 Matches an ASCII character using hexadecimal representation (exactly two digits). 

\cC Matches an ASCII control character; for example, \cC is control-C. 

\u0020 Matches a Unicode character using hexadecimal representation (exactly four digits). 

\ When followed by a character that is not recognized as an escaped character, matches that character. For example, \* is the same as \x2A. 

Character Classes
-----------------
. Matches any character except \n. If modified by the Singleline option, a period character matches any character. For more information, see Regular Expression Options. 

[aeiou] Matches any single character included in the specified set of characters. 

[^aeiou] Matches any single character not in the specified set of characters. 

[0-9a-fA-F] Use of a hyphen (–) allows specification of contiguous character ranges. 

\p{name} Matches any character in the named character class specified by {name}. Supported names are Unicode groups and block ranges. For example, Ll, Nd, Z, IsGreek, IsBoxDrawing. 

\P{name} Matches text not included in groups and block ranges specified in {name}. 

\w Matches any word character. Equivalent to the Unicode character categories

[\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Pc}]. If ECMAScript-compliant behavior is specified with the ECMAScript option, \w is equivalent to [a-zA-Z_0-9]. 

\W Matches any nonword character. Equivalent to the Unicode categories [^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Pc}]. If ECMAScript-compliant behavior is specified with the ECMAScript option, \W is equivalent to [^a-zA-Z_0-9]. 

\s Matches any white-space character. Equivalent to the Unicode character categories [\f\n\r\t\v\x85\p{Z}]. If ECMAScript-compliant behavior is specified with the ECMAScript option, \s is equivalent to [ \f\n\r\t\v]. 

\S Matches any non-white-space character. Equivalent to the Unicode character categories [^\f\n\r\t\v\x85\p{Z}]. If ECMAScript-compliant behavior is specified with the ECMAScript option, \S is equivalent to [^ \f\n\r\t\v]. 

\d Matches any decimal digit. Equivalent to \p{Nd} for Unicode and [0-9] for non-Unicode, ECMAScript behavior. 

\D Matches any nondigit. Equivalent to \P{Nd} for Unicode and [^0-9] for non-Unicode, ECMAScript behavior. 


Quantifiers
-----------
* Specifies zero or more matches; for example, \w* or (abc)*. Equivalent to {0,}. 

+ Specifies one or more matches; for example, \w+ or (abc)+. Equivalent to {1,}. 

? Specifies zero or one matches; for example, \w? or (abc)?. Equivalent to {0,1}. 

{n} Specifies exactly n matches; for example, (pizza){2}. 

{n,} Specifies at least n matches; for example, (abc){2,}. 

{n,m} Specifies at least n, but no more than m, matches. 

*? Specifies the first match that consumes as few repeats as possible (equivalent to lazy *). 

+? Specifies as few repeats as possible, but at least one (equivalent to lazy +). 

?? Specifies zero repeats if possible, or one (lazy ?).
 
{n}? Equivalent to {n} (lazy {n}). 

{n,}? Specifies as few repeats as possible, but at least n (lazy {n,}). 

{n,m}? Specifies as few repeats as possible between n and m (lazy {n,m}). 

");
        }
	}
}
