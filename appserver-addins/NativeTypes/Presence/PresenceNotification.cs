using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;

using Metreos.ApplicationFramework;
using Metreos.LoggingFramework;
using Metreos.Types.Presence.Pidf;

using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.PresenceTypes.Types.PresenceNotification;

namespace Metreos.Types.Presence
{
    /// <summary>
    /// This class pserses the notification message for an event list subscription.
    /// To use it, create an instance and call Parse with the received message. If the 
    /// message is valid, Parse will build and return ResourceList object.
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public class PresenceNotification : IVariable
    {
        private const string Xml_Content_Opening_Marker = "<?xml";

        private LogWriter log;

        public PresenceNotification(LogWriter log)
        {
            Debug.Assert(log != null, "Cannot create PresenseNotification with null LogWriter");

            this.log = log;
        }

        /// <summary>
        /// The resulting resource list object if the notification is for an event list.
        /// Otherwise it is null.
        /// </summary>
        private ResourceList resourceList = null;

        [TypeMethod(Package.CustomProperties.ResourceList.DESCRIPTION)]
        public ResourceList ResourceList
        {
            get { return resourceList; }
        }

        /// <summary>
        /// The resulting resource object if the notification is for a single resource.
        /// Otherwise it is null.
        /// </summary>
        private Resource resource = null;

        [TypeMethod(Package.CustomProperties.Resource.DESCRIPTION)]
        public Resource Resource
        {
            get { return resource; }
        }

        /// <summary>
        /// It takes the notification message for an event list subscription and parses and builds
        /// a ResourceList object if the message is for an event list or a Resource object if it
        /// is for a single resource notification. The application should check the appropriate property
        /// for the result data.
        /// </summary>
        /// <param name="message">notification message for an event list subscription</param>
        /// <returns>true if there is no catastrophic error occures</returns>
        [TypeInput("string", Package.CustomMethods.Parse_String.DESCRIPTION)]
        public bool Parse(string message)
        {
            if(message.StartsWith(Xml_Content_Opening_Marker, StringComparison.CurrentCultureIgnoreCase)) //it is a pidf
            {
                resource = ParsePidf(message);
            }
            else
            {
                resourceList = ParseResourceList(message);
            }

            return true;
        }

        /// <summary>
        /// It parses the notification for a single resource and returns the resulting Resource
        /// object if the response is valid.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>a Resource object if the message is valid. Otherwise null.</returns>
        private Resource ParsePidf(string message)
        {
            Resource rsrc = null;
            TextReader reader = new StringReader(message);
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(reader);
                XmlNode root = doc.DocumentElement;
                if(root.Name.CompareTo("presence") == 0)
                {
                    XmlAttribute attr = root.Attributes["entity"];
                    if(attr != null)
                    {
                        rsrc =  new Resource(log);
                        rsrc.Uri = attr.Value;
                        rsrc.Pidf = message;
                    }
                    else
                    {
                        if(log!= null)
                            log.Write(TraceLevel.Error, "There is no entity attribute for presence element in the pidf xml.");
                    }

                }
                else
                {
                    if(log != null)
                        log.Write(TraceLevel.Error, "There is no presence element in the pidf xml.");
                }
            }
            catch(XmlException xe)
            {
                log.Write(TraceLevel.Error, "Error in xml document: " + xe.Message, xe);
            }
            finally
            {
            }

            return rsrc;
        }

        /// <summary>
        /// It parses the notification message for an event list and returns a ResourceList 
        /// object as a result.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>a ResourceList object if the message is valid. Otherwise null.</returns>
        private ResourceList ParseResourceList(string message)
        {
            ResourceList list = null;
            Mime m = new Mime(message);
            bool rc = m.Parse();
            //ResourceList resourceList = null;

            for(int i = 0; rc && i < m.Parts.Count; i++)
            {
                switch(m.Parts[i].Type)
                {
                    case ResourceList.MimeType:
                        list = new ResourceList(log);
                        if(!list.Parse(m.Parts[i].Content))
                        {
                            if(log != null)
                                log.Write(TraceLevel.Error, "There are errors while parsing resource list content.");

                            //what else should I do with this error?
                            rc = false;
                            break;
                        }
                        break; //case ResourceList.MimeType

                    case Presence.MimeType:
                        {
                            TextReader reader = new StringReader(m.Parts[i].Content);
                            XmlDocument doc = new XmlDocument();
                            try
                            {
                                doc.Load(reader);
                                XmlNode root = doc.DocumentElement;
                                if (root.Name.CompareTo("presence") == 0)
                                {
                                    XmlAttribute attr = root.Attributes["entity"];
                                    if(attr != null)
                                    {
                                        Resource rsrc = (Resource) list.Resources[attr.Value];
                                        if(rsrc != null)
                                            rsrc.Pidf = m.Parts[i].Content;
                                        else
                                        {
                                            if(log != null)
                                                log.Write(TraceLevel.Verbose, "There is no resource with uri: {0} defined in rlmi part of notfication message.",
                                                    attr.Value);
                                        }
                                    }
                                    else
                                    {
                                        if(log!= null)
                                            log.Write(TraceLevel.Error, "There is no entity attribute for presence element in the pidf xml.");
                                        rc = false;
                                    }

                                }
                                else
                                {
                                    if(log != null)
                                        log.Write(TraceLevel.Error, "There is no presence element in the pidf xml.");
                                    rc = false;   //     <--------------
                                }

                            }
                            catch(XmlException xe)
                            {
                                //error
                                if(log != null)
                                    log.Write(TraceLevel.Error, "There are errors in pidf contents: {0}", xe.Message);
                                rc = false;
                            }
                        }
                        break; //case Presence.MimeType

                    default:
                        break;
                }//switch
            }//for

            return (rc ? list : null);
        }
    }

    /// <summary>
    /// This class represents the notification message for event list subscription. 
    /// </summary>
    public class ResourceList : IXmlElement
    {
        public const string MimeType = "application/rlmi+xml";
        public const string NodeName = "list";
        public const string NameElement = "name";

        public static class AttributeNames
        {
            public const string Uri         = "uri";
            public const string Version     = "version";
            public const string FullState   = "fullState";
            public const string ContentId   = "cid";
        }

        public ResourceList(LogWriter log)
        {
            this.log = log;
        }

        private LogWriter log = null;

        private string uri;
        /// <summary>
        /// The list uri of the notification
        /// </summary>
        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        private int version;
        /// <summary>
        /// The version of the notify. It increases by one for each
        /// subsequent notify within the same subsribe.
        /// </summary>
        public int Version
        {
            get { return version; }
        }

        private bool fullstate;
        /// <summary>
        /// Indicates whether the notify contains full state information
        /// for the list.
        /// </summary>
        public bool FullState
        {
            get { return fullstate; }
        }

        private string cid;
        /// <summary>
        /// The content id. It is optional.
        /// </summary>
        public string Cid
        {
            get { return cid; }
        }

        private Hashtable names = new Hashtable();
        /// <summary>
        /// Returns the human friendly name of the resource for the given 
        /// language.
        /// </summary>
        /// <param name="lang">The language tag defined in RFC 3066. It can't be null. But 
        /// It can be an empty string.</param>
        /// <returns></returns>
        public string Name(string lang)
        {
            Name n = (Name) names[lang];
            return (n != null ? n.Value : null);
        }

        public void AddName(Name n)
        {
            names[n.Language] = n;
        }

        public void AddInstance(Instance i)
        {
        }

        private Hashtable resources = new Hashtable();
        /// <summary>
        /// The hashtable contains all the resources defined in the notification message. It's a
        /// map from resource uri to Resource object.
        /// </summary>
        public Hashtable Resources
        {
            get { return resources; }
        }

        public bool OnText(string txt)
        {
            return false;
        }

        /// <summary>
        /// It parses rlmi portion of the multi part related message and builds the hashmap between
        /// uri and its Resource object.
        /// </summary>
        /// <param name="content">the rlmi part of the notification message</param>
        /// <returns>true if the message is valid and parsable.</returns>
        public bool Parse(string content)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(content));
            IXmlElement currentElement = null;
            Stack<IXmlElement> elementTree = new Stack<IXmlElement>();
            bool error = false;
            try
            {
                while(!error && reader.Read())
                {
                    switch(reader.NodeType)
                    {
                        case XmlNodeType.Document:
                            //start of the document
                            break;

                        case XmlNodeType.Element:   //start of element
                            currentElement = StartElement(currentElement, reader);
                            error = currentElement == null;
                            if(!error)
                                elementTree.Push(currentElement);
                            break;

                        case XmlNodeType.EndElement: //end of element
                            currentElement = elementTree.Pop();
                            break;

                        case XmlNodeType.Text: //text for the element
                            currentElement.OnText(reader.Value);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch(XmlException xe)
            {
                if(log != null)
                    log.Write(TraceLevel.Error, "Error while parsing resource list xml string: {0}", xe.Message);
                error = true;
            }

            return !error;
       }

        /// <summary>
        /// Start of element handler for parsing the xml string. It creates appropriate 
        /// object based on different element name. It then parses the attributes of the
        /// element and populates the fields of the object.
        /// </summary>
        /// <param name="parent">the parent node of the current element</param>
        /// <param name="reader">current xml reader</param>
        /// <returns>the object for the xml element if no error. otherwise null.</returns>
        private IXmlElement StartElement(IXmlElement parent, XmlReader reader) 
        {
            IXmlElement element = null;
            if(ResourceList.NodeName.Equals(reader.Name))
            {
                //we have a list element
                //now populate its attributes
                if(PopulateListAttributes(reader))
                    element = this;
            }
            else if(Resource.NodeName.Equals(reader.Name)) //a resource
            {
                //parse the resource
                Resource r= new Resource(log);
                if(r.PopulateAttributes(reader))
                {
                    element = r;
                    resources.Add(r.Uri, r);
                }
            }
            else if(NameElement.Equals(reader.Name)) //the name node
            {
                //parse the name node
                Name n = new Name();
                if(n.PopulateAttributes(reader))
                {
                    element = n;
                    parent.AddName(n);
                }
            }
            else if(Instance.NodeName.Equals(reader.Name))
            {
                Instance i = new Instance();
                if(i.PopulateAttributes(reader))
                {
                    element = i;
                    parent.AddInstance(i);
                }
            }

            return element;
        }

        /// <summary>
        /// This function parses the attributes of the elenment and populate the corresponding fields 
        /// of the object.
        /// 
        /// </summary>
        /// <param name="reader">the current xml reader</param>
        /// <returns>true if there is no error.</returns>
        private bool PopulateListAttributes(XmlReader reader)
        {
            bool rc = true;
            for(int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);

                if(AttributeNames.Uri.Equals(reader.Name))
                    uri = reader.Value;
                else if(AttributeNames.Version.Equals(reader.Name))
                {
                    try
                    {
                        version = Int32.Parse(reader.Value);
                    }
                    catch(Exception)
                    {
                        //bad verserion format
                        if(log != null)
                            log.Write(TraceLevel.Error, "Invalid List attribute value for version: {0}", reader.Value);
                        rc = false;
                        break;
                    }
                }
                else if(AttributeNames.FullState.Equals(reader.Name))
                {
                    try
                    {
                        fullstate = Boolean.Parse(reader.Value);
                    }
                    catch(Exception)
                    {
                        //need to bail out 
                        if(log != null)
                            log.Write(TraceLevel.Error, "Invalid List attribute value for fullState: {0}", reader.Value);
                        rc = false;
                        break;
                    }

                }
                else if(AttributeNames.ContentId.Equals(reader.Name))
                {
                    cid = reader.Value;
                }
            }//foreach

            return rc;
        }
    }

    /// <summary>
    /// This class represents a resource in the list notfication message for an event
    /// list subscription.
    /// </summary>
    public class Resource : IXmlElement
    {
        public const string NodeName = "resource";

        public static class AttributeNames
        {
            public const string Uri = "uri";
        }

        public Resource(LogWriter log)
        {
            this.log = log;
        }

        private LogWriter log = null;

        private string uri;
        /// <summary>
        /// The list uri of the notification
        /// </summary>
        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        private Hashtable names = new Hashtable();
        /// <summary>
        /// Returns the human friendly name of the resource for the given 
        /// language.
        /// </summary>
        /// <param name="lang">The language tag defined in RFC 3066. It can't be null. But 
        /// It can be an empty string.</param>
        /// <returns></returns>
        public string Name(string lang)
        {
            Name n = (Name) names[lang];
            return (n != null ? n.Value : null);
        }

        public void AddName(Name n)
        {
            names[n.Language] = n;
        }

        public void AddInstance(Instance i)
        {
            this.instances.Add(i);
        }

        private List<Instance> instances = new List<Instance>();
        /// <summary>
        /// The list of instances for this resource.
        /// </summary>
        public List<Instance> Instances
        {
            get { return instances; }
        }

        private string pidf = null;
        /// <summary>
        /// The PIDF xml string for this resource.
        /// </summary>
        public string Pidf
        {
            get { return pidf; }
            set { pidf = value; ParsePidf(); }
        }

        private presence presence = null;
        public presence Presence
        {
            get { return presence; }
        }

        public bool Parse(XmlTextReader reader)
        {
            return false;

        }

        public bool OnText(string txt)
        {
            //there is no text for resource
            return false;
        }

        public bool PopulateAttributes(XmlReader reader)
        {
            bool rc = false;
            for(int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if(AttributeNames.Uri.Equals(reader.Name))
                {
                    uri = reader.Value;
                    rc = true;
                }
            }


            return rc;
        }

        private void ParsePidf()
        {
            TextReader reader = new StringReader(pidf);
            XmlSerializer xs = new XmlSerializer(typeof(presence));
            try
            {
                presence = (presence) xs.Deserialize(reader);
            }
            catch(Exception )
            {
                //CUPS sometimes sends presence data with default namespace on the root element
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                XmlAttributes attrs = new XmlAttributes();
                XmlRootAttribute ra = new XmlRootAttribute();
                ra.Namespace = "urn:ietf:params:xml:ns:pidf";
                attrs.XmlRoot = ra;
                overrides.Add(typeof(presence), attrs);
                xs = new XmlSerializer(typeof(presence), overrides);
                reader.Close();
                reader = new StringReader(pidf);
                presence = (presence) xs.Deserialize(reader);
            }
        }
    }

    /// <summary>
    /// This class represents the instance element in the notification message.
    /// </summary>
    public class Instance : IXmlElement
    {
        public const string NodeName = "instance";
        public static class AttributeNames
        {
            public const string Id          = "id";
            public const string State       = "state";
            public const string Reason      = "reason";
            public const string ContentId   = "cid";
        }

        private string id;
        /// <summary>
        /// The instance id.
        /// </summary>
        public string Id
        {
            get { return id; }
        }

        private State state;
        /// <summary>
        /// The state of the subscription
        /// </summary>
        public State State
        {
            get { return state; }
        }

        private string reason;
        /// <summary>
        /// The reason if it is in terminated state.
        /// </summary>
        public string Reason
        {
            get { return reason; }
        }

        private string cid;
        /// <summary>
        /// The content id for the instance.
        /// </summary>
        public string Cid
        {
            get { return cid; }
        }

        public bool OnText(string txt)
        {
            //dont care about the text for now
            return true;
        }

        public void AddInstance(Instance i)
        {
        }

        public void AddName(Name n)
        {
        }

        public bool PopulateAttributes(XmlReader reader)
        {
            bool rc = true;
            for(int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if(AttributeNames.ContentId.Equals(reader.Name))
                {
                    cid = reader.Value;
                }
                else if(AttributeNames.Id.Equals(reader.Name))
                    id = reader.Value;
                else if(AttributeNames.State.Equals(reader.Name))
                {
                    try
                    {
                        state = (State) Enum.Parse(typeof(State), reader.Value);
                    }
                    catch(Exception)
                    {
                        rc = false;
                    }
                }
                else if(AttributeNames.Reason.Equals(reader.Name))
                    reason = reader.Value;
            }

            return rc;
        }
    }

    public enum State { active, pending, terminated };

    /// <summary>
    /// This class represents the name element of the notification message.
    /// </summary>
    public class Name : IXmlElement
    {
        public static class AttributeNames
        {
            public const string Language    = "xml:lang";
        }

        private string value = null;
        /// <summary>
        /// The value of this name element.
        /// </summary>
        public string Value
        {
            get { return value; }
        }

        private string lang = null;
        /// <summary>
        /// The language of this name element. It can be empty.
        /// </summary>
        public string Language
        {
            get { return (lang == null ? "" : lang); }
        }

        public bool PopulateAttributes(XmlReader reader)
        {
            for(int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if(AttributeNames.Language.Equals(reader.Name))
                    lang = reader.Value;
            }

            return true;
        }

        public bool OnText(string txt)
        {
            value = txt;
            return true;
        }

        public void AddName(Name n)
        {
        }

        public void AddInstance(Instance i)
        {
        }
    }

    /// <summary>
    /// The interface for all xml elements
    /// </summary>
    public interface IXmlElement
    {
        bool OnText(string txt);
        void AddName(Name n);
        void AddInstance(Instance i);
    }

    public static class Presence
    {
        public const string MimeType = "application/pidf+xml";
    }
}
