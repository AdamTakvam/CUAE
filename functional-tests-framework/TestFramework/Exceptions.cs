using System;

namespace Metreos.Samoa.FunctionalTestFramework
{

    public class InstallException : Exception
    {
        public InstallException(string msg) : base(msg){}
    }

    public class LoadException : Exception
    {
        public LoadException(string msg) : base(msg){}
    }

    public class MalformedTestBank : Exception
    {
        public MalformedTestBank(string msg) : base(msg){}
    }
}
