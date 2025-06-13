using System;

namespace UrlFormEncoding
{
    public class Encoder
    {
        [STAThread]
        static void Main(string[] args)
        {
            System.Text.StringBuilder text = new System.Text.StringBuilder();
            string input;

            Console.WriteLine("Enter text to encode. Enter an empty line to end.");
            
            while((input = Console.ReadLine()) != "")
            {
                text.Append(input);
                text.Append("\r\n");
            }

            Console.WriteLine();
            
            string encodedText = Encoder.Encode(text.ToString());

            Console.WriteLine("Encoded text is:");
            Console.WriteLine(encodedText);
        } 

        public static string Encode(string text)
        {
            char[] specialChars = new char[] {'/', ' ', '~', '&', '?', '=', ';', '>', '<', '\r', '\n', '"'};

            System.Text.StringBuilder encodedText = new System.Text.StringBuilder();

            CharEnumerator e = text.GetEnumerator();

            while(e.MoveNext())
            {
                bool special = false;

                for(int i = 0; i < specialChars.Length; i++)
                {
                    if(e.Current == specialChars[i])
                    {
                        encodedText.Append(Uri.HexEscape(e.Current));
                        special = true;
                        break;
                    }
                }

                if(special == false)
                {
                    encodedText.Append(e.Current);
                }
            }

            return encodedText.ToString();
        }
    }
}
