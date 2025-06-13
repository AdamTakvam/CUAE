using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Microsoft.CSharp;

namespace MethodInfoSpike
{
    class MainClass
    {
        [MTAThread]
        static void Main(string[] args)
        {
            string[] srcCodes = new string[] { newAssemblyText };
            string[] srcNames = new string[] { "nothing" };
            string target = "NewAssembly.dll";

            System.Collections.Specialized.ListDictionary compileOpts = new System.Collections.Specialized.ListDictionary();
            compileOpts.Add("target", "library");

            CompilerError[] cerr = Compiler.Compile(srcCodes, srcNames, target, null, compileOpts);

            if(cerr.Length > 0)
            {
                for(int i = 0; i < cerr.Length; i++)
                {
                    Console.WriteLine("Compiler error: {0}", cerr[i].ToString());
                }

                return;
            }

            Assembly a = Assembly.LoadFrom(target);

            Hashtable hash = new Hashtable();
            hash["test"] = "wheeeeeeee";

            foreach(Type t in a.GetTypes())
            {
                if(t.IsClass)
                {
                    foreach(MethodInfo mi in t.GetMethods())
                    {
                        if(mi.Name.StartsWith("TestMethod"))
                        {
                            string retValue;
                            retValue = (string)mi.Invoke(null, new object[] { hash });
                            Console.WriteLine("Returned: {0}", retValue);
                        }
                    }
                }
            }
        }

        private static string newAssemblyText =
            @"
                using System;
                using System.Collections;

                namespace NewAssembly
                {
                    class TestClass
                    {
                        public static string TestMethodA(Hashtable hash)
                        {
                            Console.WriteLine(""{0}"", (string)hash[""test""]);
                            return (string)hash[""test""];
                        }

                        public static string TestMethodB(Hashtable hash)
                        {
                            Console.WriteLine(""{0}"", (string)hash[""test""]);
                            return (string)hash[""test""];
                        }
                    }
                }
            ";
    }
}
