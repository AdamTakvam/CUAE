using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.ProviderPackagerCore
{
    public abstract class IPackager
    {
        public const string TempDir         = "mcp_temp";
        public const string ManifestFile    = "manifest.xml";

        public const string PackFileExt     = ".mcp";
        public const string ProvFileExt     = ".dll";
        public const string DebugFileExt    = ".pdb";
        
        public const char ServiceArgDelimiter = '&';
    }
}
