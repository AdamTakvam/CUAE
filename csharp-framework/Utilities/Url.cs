using System;

namespace Metreos.Utilities
{
    /// <summary>
    /// Utility class for generic URL manipulations. These seem to be mysteriously missing
    /// from the .NET Framework.
    /// </summary>
    public abstract class Url
    {
        /// <summary>
        /// Special characters that must be escaped in a URL.
        /// </summary>
        private readonly static char[] specialChars = 
        {
            '<', '>', '/', ' ', '~', '&', '?', '=', ';', '\r', '\n', '"'
        };

        /// <summary>
        /// Will encode a given string according to the URL escaping rules. If a special
        /// character is found in the input text, it will be converted to its hexadecimal
        /// representation.
        /// </summary>
        /// <param name="text">The string input to encode.</param>
        /// <returns>The encoded string</returns>
        public static string Encode(string text)
        {
            System.Text.StringBuilder encodedText = new System.Text.StringBuilder();

            CharEnumerator e = text.GetEnumerator();

            bool special;

            // Iterate through the string.
            while(e.MoveNext())
            {
                special = false;

                // Iterate through the special characters.
                for(int i = 0; i < specialChars.Length; i++)
                {
                    // Is the current character special?
                    if(e.Current == specialChars[i])
                    {
                        // Yes, convert it to hex.
                        encodedText.Append(Uri.HexEscape(e.Current));
                        special = true;
                        break;
                    }
                }

                // The character was not special, so just copy it over.
                if(special == false)
                {
                    encodedText.Append(e.Current);
                }
            }

            return encodedText.ToString();
        }

        public static string FormatUri(string uri)
        {
            if(uri != null)
            {
                if(!uri.StartsWith("http://"))
                {
                    uri = "http://" + uri;
                }
                return uri;
            }
            return null;
        }

        public static string GetFilename(string uri)
        {
            int p = uri.LastIndexOf("/");
            if(p == -1) { return ""; }

            p++;

            int q = uri.IndexOf("?");
            if(q == -1)
            {
                q = uri.Length;
            }

            return uri.Substring(p, (q-p));
        }
    }
}
