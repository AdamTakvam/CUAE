using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Metreos.Types.Presence
{
    class Mime
    {
        private const string BoundaryMarker = "--";
        private const string ContentEncodingHeader = "Content-Transfer-Encoding:";
        private const string ContentIdHeader = "Content-ID:";
        private const string ContentTypeHeader = "Content-Type:";
        private const string NewLineChars = "\r\n";

        private string message = null;
        public string Message 
        {
            set { message = value; }
        }

        List<Part> parts = new List<Part>();
        public List<Part> Parts
        {
            get { return parts; }
        }

        public Mime(string msg)
        {
            message = msg;
        }

        public bool Parse()
        {
            parts.Clear();
            int pos  = 0;
            string boundary = null;
            string endOfMsg = null;

            char[]  newlines = NewLineChars.ToCharArray();
            //first look for --, the biginning of the boundary
            int start = message.IndexOf(BoundaryMarker);
            if (start >= 0)
            {
                pos = message.IndexOfAny(newlines, start);
                if (pos > 0) //we should have the full boundary string
                {
                    boundary = message.Substring(start, pos-start);
                    if(boundary != null)
                        endOfMsg = boundary+BoundaryMarker;

                    start = SkipSpace(message, pos);
                }
                else 
                    start = -1;
            }

            while (start > 0 && start+endOfMsg.Length < message.Length) 
            {
                start = GetPart(message, start, boundary);
            }

            return (start > 0);
            
        }

        private int GetPart(string message, int start, string boundary)
        {
            string encoding = null;
            string id = null;
            string type = null;
            StringBuilder content = new StringBuilder();

            char[]  newlines = NewLineChars.ToCharArray();
            bool endOfPart = false;
            string line;

            int msgPos = start;
            while(start > 0 && 
                    start < message.Length && 
                    (line = NextLine(message, start)) != null && 
                    !endOfPart)
            {
                //now match up agaisnt content encoding/id/type
                if(line.StartsWith(ContentEncodingHeader))
                {
                    //found the encoding header
                    encoding = line.Substring(ContentEncodingHeader.Length).Trim();
                }
                else if(line.StartsWith(ContentIdHeader))
                {
                    id = line.Substring(ContentIdHeader.Length).Trim();
                }
                else if(line.StartsWith(ContentTypeHeader))
                {
                    type = line.Substring(ContentTypeHeader.Length).Trim();
                }
                else if(line.StartsWith(boundary)) //end of part
                {
                    endOfPart = true;
                }
                else if(encoding != null && id != null && type != null)
                {
                    content.Append(line.TrimStart(null));
                }

                start += line.Length;
            }

            if (encoding != null && id != null && type != null && content.Length>0)
            {
                Part part = new Part(encoding, id, type, content.ToString());
                parts.Add(part);
            }

            return start;
        }

        private int SkipSpace(string message, int start)
        {
            while (start < message.Length && (message[start] == '\r' || 
                                                message[start] == '\n' || 
                                                message[start] == ' ' ||
                                                message[start] == '\t'))
                ++start;

            return start;
        }

        private string NextLine(string message, int start)
        {
            string rc = null;
            int pos = message.IndexOf('\n', start);
            if(pos > 0)
                rc = message.Substring(start, pos-start+1);

            return rc;
        }
    }

    class Part
    {
        private string encoding;
        public string Encoding
        {
            get { return encoding; }
        }
        
        private string id;
        public string Id
        {
            get { return id; }
        }

        private string type;
        public string Type
        {
            get { return type; }
        }
        
        private string content;
        public string Content
        {
            get { return content; }
        }

        public Part(string enc, string id, string type, string content)
        {
            this.encoding = enc;
            this.id = id;
            int pos = type.IndexOf(';');
            if(pos > 0)
                this.type = type.Substring(0, pos);
            else
                this.type = type;
            this.content = content;
        }

    }
}
