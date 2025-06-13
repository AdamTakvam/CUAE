using System;
using System.Xml;
using System.Text.RegularExpressions;

namespace StressTesting
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	public class Settings
	{
        // Global settings
        public string appServerIp;
        public string databaseName;
        public bool chooseSim;
        public bool chooseCall;
        public bool errorChecking;
        public string callManagerIp;
        public string callGenIp;
        public string createPoll;
        public string createTimeout;
        public string joinPoll;
        public string joinTimeout;
        public string kickPoll;
        public string kickTimeout;

        // SimClient settings

        public string[] lowerBounds;
        public string[] upperBounds;
        public int numberOfRanges;
        public string[] rangesSelected;
        public bool allowRoute;
        public string routingNumber;

        // Test settings

        public int maximumConferences;
        public int maximumConnections;
        public int testTime;
        public int minimumTimeBetweenCalls;
        public int averageCallInterval;
        public int averageSpike;
        public int averageLittleSpike;
        public int initialIntensity;

        Regex boundsList = new Regex(@"(\w+:\w+;)*", RegexOptions.Compiled);
        Regex singleGrabber = new Regex(@"(?<1>\w+):(?<2>\w+);");

        Regex rangesToUseList = new Regex(@"(\w+;)*", RegexOptions.Compiled);
        Regex singleRangeGrabber = new Regex(@"(?<1>\w+);");

        public Settings()
        {
            UpdateAll();
        }

        public void Refresh()
        {
            UpdateAll();
        }

        public void UpdateAll()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlTextReader file = new XmlTextReader("Globals.config");
                doc.Load(file);

                XmlNodeList topNodes = doc.ChildNodes;
                XmlNode configurationTag = topNodes[1];

                XmlNodeList configNode = configurationTag.ChildNodes;
                XmlNode appSettingTag = configNode[0];

                XmlNodeList settingNodes = appSettingTag.ChildNodes;
                    
                for(int i = 0; i <  settingNodes.Count ;i++)
                {
                    XmlNode individualSetting = settingNodes[i];

                    string key;
                    string valueForKey;

                    GetKeyValueForSetting( ref individualSetting, out key, out valueForKey);

                    if(key != null)
                    {
                        
                        switch(key)
                        {

                            case "appServerIp":

                                if(valueForKey != null)
                                {
                                    this.appServerIp = valueForKey;
                                }
                                else
                                {
                                    this.appServerIp = "127.0.0.1";
                                }
                                break;

                            case "callManagerIp":

                                if(valueForKey != null)
                                {
                                    this.callManagerIp = valueForKey;
                                }
                                else
                                {
                                    this.callManagerIp = "192.168.1.250";
                                }
                                break;

                            case "databaseName":

                                if(valueForKey != null)
                                {
                                    this.databaseName = valueForKey;
                                }
                                else
                                {
                                    this.databaseName = "mecTestDatabase";
                                }
                                break;

                            case "radioSelectSimCall":

                                if(valueForKey != null)
                                {
                                    if(valueForKey == "0")
                                    {
                                        chooseSim = true;
                                        chooseCall = false;
                                    }
                                    else if(valueForKey == "1")
                                    {
                                        chooseSim = false;
                                        chooseCall = true;
                                    }
                                    else
                                    {
                                        // default
                                        chooseSim = true;
                                        chooseCall = false;
                                    }       
                                }
                                else
                                {
                                    // default
                                    chooseSim = true;
                                    chooseCall = false;
                                }

                                break;

                            case "errorChecking":

                                if(valueForKey != null)
                                {
                                    if(valueForKey.ToLower() == "true")
                                    {
                                        errorChecking = true;
                                    }
                                    else if(valueForKey.ToLower() == "false")
                                    {
                                        errorChecking = false;
                                    }
                                    else
                                    {
                                        errorChecking = true;
                                    }
                                }
                                break;

                            case "createPoll":

                                if(valueForKey != null)
                                {
                                    createPoll = valueForKey;
                                }
                                else
                                {
                                    createPoll = "1000";
                                }
                                break;        
    
                            case "createTimeout":

                                if(valueForKey != null)
                                {
                                    createTimeout = valueForKey;
                                }
                                else
                                {
                                    createTimeout = "5000";
                                }
                                break;    

                            case "joinPoll":

                                if(valueForKey != null)
                                {
                                    joinPoll = valueForKey;
                                }
                                else
                                {
                                    joinPoll = "2000";
                                }
                                break;    

                            case "joinTimeout":

                                if(valueForKey != null)
                                {
                                    joinTimeout = valueForKey;
                                }
                                else
                                {
                                    joinTimeout = "10000";
                                }
                                break;    

                            case "kickPoll":

                                if(valueForKey != null)
                                {
                                    kickPoll = valueForKey;
                                }
                                else
                                {
                                    kickPoll = "5000";
                                }
                                break;    

                            case "kickTimeout":

                                if(valueForKey != null)
                                {
                                    kickTimeout = valueForKey;
                                }
                                else
                                {
                                    kickTimeout = "15000";
                                }
                                break;    
                        }
                    }
                }
                file.Close();
            }
            catch
            {
                // defaults 

                this.appServerIp = "10.1.12.50";
                this.callManagerIp = "10.1.14.26";
                this.databaseName = "mecTestDatabase";
                this.chooseSim = true;
                this.chooseCall = false;
                this.errorChecking = true;
                this.createPoll = "1000";
                this.createTimeout = "5000";
                this.joinPoll = "2000";
                this.joinTimeout = "10000";
                this.kickPoll = "5000";
                this.kickTimeout = "15000";
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                XmlTextReader file = new XmlTextReader("CallerSettings.config");
                doc.Load(file);

                XmlNodeList topNodes = doc.ChildNodes;
                XmlNode configurationTag = topNodes[1];

                XmlNodeList configNode = configurationTag.ChildNodes;
                XmlNode appSettingTag = configNode[0];

                XmlNodeList settingNodes = appSettingTag.ChildNodes;
                    
                for(int i = 0; i <  settingNodes.Count ;i++)
                {
                    XmlNode individualSetting = settingNodes[i];

                    string key;
                    string valueForKey;

                    GetKeyValueForSetting( ref individualSetting, out key, out valueForKey);

                    if(key != null)
                    {
                        
                        switch(key)
                        {

                            case "rangesList":

                                if(valueForKey != null)
                                {
                                    if(boundsList.IsMatch(valueForKey))
                                    {
                                        MatchCollection matches = singleGrabber.Matches(valueForKey);

                                        lowerBounds = new string[matches.Count];
                                        upperBounds = new string[matches.Count];

                                        numberOfRanges = matches.Count;

                                        for(int j = 0; j < matches.Count; j++)
                                        {
                                            Match match = matches[j];
                                           
                                            lowerBounds[j] = match.Groups[1].ToString();
                                            upperBounds[j] = match.Groups[2].ToString();
                                        }
                                    }      
                                    else
                                    {
                                        lowerBounds = new string[0];
                                        upperBounds = new string[0];
                                    }
                                }
                                else
                                {
                                    lowerBounds = new string[0];
                                    upperBounds = new string[0];
                                }
                                break;

                            case "rangesToUse":
                                
                                if(valueForKey != null)
                                {     
                                    if(rangesToUseList.IsMatch(valueForKey))
                                    {
                                        MatchCollection matches = singleRangeGrabber.Matches(valueForKey);
                                    
                                        rangesSelected = new string[matches.Count];

                                        for(int j = 0; j < matches.Count; j++)
                                        {
                                            Match match = matches[j];

                                            rangesSelected[j] = match.Groups[1].ToString();
                                        }
                                    }
                                    else
                                    {
                                        rangesSelected = new string[0];
                                    }
                                }
                                else
                                {
                                    rangesSelected = new string[0];
                                }

                                break;

                            case "callGenIp":

                                if(valueForKey != null)
                                {
                                    callGenIp = valueForKey;
                                }

                                break;

                            case "allowRoute":

                                if(valueForKey != null)
                                {
                                    if(valueForKey == "0")
                                    {
                                        allowRoute = false;
                                    }
                                    if(valueForKey == "1")
                                    {
                                        allowRoute = true;
                                    }
                                    else
                                    {
                                        allowRoute = false;
                                    }
                                }
                                break;

                            case "routingNumber":

                                if(valueForKey != null)
                                {
                                    routingNumber = valueForKey;
                                }
                                else
                                {
                                    routingNumber = "";
                                }
                                break;
                        }
                    }
                }
                file.Close();
            }
            catch
            {
                // defaults 

                rangesSelected = new string[0];
                lowerBounds = new string[0];
                upperBounds = new string[0];
                this.callGenIp = "127.0.0.1";
                this.routingNumber = "";
                this.allowRoute = false;
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                XmlTextReader file = new XmlTextReader("TestSettings.config");
                doc.Load(file);

                XmlNodeList topNodes = doc.ChildNodes;
                XmlNode configurationTag = topNodes[1];

                XmlNodeList configNode = configurationTag.ChildNodes;
                XmlNode appSettingTag = configNode[0];

                XmlNodeList settingNodes = appSettingTag.ChildNodes;
                    
                for(int i = 0; i <  settingNodes.Count ;i++)
                {
                    XmlNode individualSetting = settingNodes[i];

                    string key;
                    string valueForKey;

                    GetKeyValueForSetting( ref individualSetting, out key, out valueForKey);

                    if(key != null)
                    {
                        
                        switch(key)
                        {
                                
                            case "maximumConferences":

                                if(valueForKey != null)
                                {
                                    this.maximumConferences = Int32.Parse(valueForKey);
                                }
                                else
                                {
                                    this.maximumConferences = 16;
                                }
                                break;

                            case "maximumConnections":

                                if(valueForKey != null)
                                {
                                    this.maximumConnections = Int32.Parse(valueForKey);
                                }
                                else
                                {
                                    this.maximumConnections = 32;
                                }
                                break;

                            case "testTime":

                                if(valueForKey != null)
                                {
                                    this.testTime = Int32.Parse(valueForKey);
                                }
                                else
                                {
                                    this.testTime = 10;
                                }
                                break;

                            case "minimumTimeBetweenCalls":

                                if(valueForKey != null)
                                {
                                    this.minimumTimeBetweenCalls = Int32.Parse(valueForKey);
                                }
                                else
                                {
                                    this.minimumTimeBetweenCalls = 5;
                                }
                                break;

                            case "averageCallInterval":

                                if(valueForKey != null)
                                {
                                    this.averageCallInterval = Int32.Parse(valueForKey);
                                }
                                else
                                {
                                    this.averageCallInterval = 10;
                                }
                                break;

                            case "averageSpike":

                                if(valueForKey != null)
                                {
                                    this.averageSpike = Int32.Parse(valueForKey);
                                }
                                else
                                {
                                    this.averageSpike = 60;
                                }
                                break;

                            case "averageLittleSpike":

                                if(valueForKey != null)
                                {
                                    this.averageLittleSpike = Int32.Parse(valueForKey);
                                }
                                else
                                {
                                    this.averageLittleSpike = 60;
                                }
                                break;

                            case "initialIntensity":

                                if(valueForKey != null)
                                {
                                    this.initialIntensity = Int32.Parse(valueForKey);
                                }
                                else
                                {
                                    this.initialIntensity = 21;
                                }
                                break;       
                               
                        }
                    }
                }
                file.Close();
            }
            catch
            {
                // defaults 

                this.maximumConferences = 16;
                this.maximumConnections = 32;
                this.testTime = 10;
                this.minimumTimeBetweenCalls = 5;
                this.averageCallInterval = 10;
                this.averageSpike = 60;
                this.averageLittleSpike = 60;
                this.initialIntensity = 21;
            }
        }

        public void GetKeyValueForSetting(ref XmlNode individualSetting, out string key, out string valueForKey)
        {
            key = null;
            valueForKey = null;

            XmlAttributeCollection attributes = individualSetting.Attributes;
            XmlAttribute keyAttribute = attributes["key"];
            key = keyAttribute.Value;

            XmlAttribute valueAttribute = attributes["value"];
            valueForKey = valueAttribute.Value;
        }


	}
}
