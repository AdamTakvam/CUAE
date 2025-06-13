using System;
using System.Xml;
using System.Collections;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.Max.Core;
using Metreos.Interfaces;
using PropertyGrid.Core;

namespace Metreos.Max.Framework
{
  /// <summary> Validates the map and constructs metadata</summary>
  public class ScriptParser : Stepper
  {
    public delegate void ScriptDelegate(XmlNode xmlDoc);
    public ErrorInformation[] Errors { get { return errorInformation.Errors; } }
    public ErrorInformation[] Warnings { get { return warningInformation.Warnings; } }
    public string Name { get { return scriptName; } }
    public bool Error { get { return error; } }
    public bool Warning { get { return warning; } }
    public ScriptMap Map { get { return metadata; } }
    private XmlNode scriptXml;
    private string scriptName;
    private ScriptMap metadata;

    public ScriptParser(string scriptPath) : base(null)
    {
      this.scriptName   = MaxMainUtil.peekedAppName;
      this.scriptXml    = RetrieveTopNode(scriptPath);
    }


    public static XmlNode RetrieveTopNode(string scriptPath)
    {
      XmlDocument scriptDoc = new XmlDocument();
      scriptDoc.Load(scriptPath);
      return scriptDoc.ChildNodes[0];
    }


    protected override void Reset()
    {
      error = false;
      warning = false;

      base.Reset();
    }


    protected override void ConstructSteps()
    {
      executeStep = Delegate.Combine(new ScriptDelegate[]
        {
          new ScriptDelegate(InitializeScriptMap),
          new ScriptDelegate(EstablishPhysicalFunctions)
        });
    }


    public ScriptMap Parse()
    {
      this.Execute(new object[] { scriptXml } );

      return error? null: metadata;
    }


    private void InitializeScriptMap(XmlNode scriptXml)
    {
      globalVariablesType globalVars = ProjectXmlUtility.GetGlobalVariables(scriptXml);

      string description;

      ProjectXmlUtility.GetScriptAttributes(scriptXml, out description);
      
      metadata = new ScriptMap(scriptName, globalVars, description);
    }


    private void EstablishPhysicalFunctions(XmlNode scriptXml)
    { 
      string[] functionNames;
      eventType[] events;
      eventParamType[][] allEventParams;

      // Search through XmlTextReader to find this functions info.
      bool eventHandlerUndefined = ProjectXmlUtility.GetFunctionNamesAndHandlers(
        scriptXml, out functionNames, out events, out allEventParams);

      if(eventHandlerUndefined)
      {
        errorInformation.AddError(IErrors.undefinedEventHandler);
        return;
      }

      for(int i = 0; i < (functionNames != null ? functionNames.Length : 0); i++)
      {
        string functionName                     = functionNames[i];
        eventType @event                        = events[i];
        eventParamType[] eventParams            = allEventParams[i];

        FunctionParser functionParser = new FunctionParser(functionName, @event, eventParams, this);

        XmlNode canvasNode  = ProjectXmlUtility.MoveToFunction(functionName, scriptXml);        

        functionParser.Execute(new object[] { canvasNode , metadata });
      }
    }
  }
}
