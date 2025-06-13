using System;
using System.Collections.Generic;
using System.Text;

using Metreos.Utilities;

namespace EmailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                EMailer.Send("smtp.gmail.com", "abrasax93", "xlr8tion", "adchaney@cisco.com", "abrasax93@gmail.com", "Test", "Test Body");
                Console.WriteLine("Message sent successfully");
            }
            catch(Exception e)
            {
                Console.WriteLine(Exceptions.FormatException(e));
            }
            
            Console.ReadKey();
        }
    }
}
