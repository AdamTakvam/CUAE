using System.Collections;
using Metreos.ApplicationFramework.ScriptXml;

namespace Metreos.Max.Framework
{
    public interface IMetreosScript
    {
        string ScriptName { get; }
        string Description { get; }
        globalVariablesType GlobalVariables { get; }
    }

    public interface IMetreosFunction
    {
        string FunctionName             { get; }
        eventType @Event                { get; }
        variableType[] RegularVariables { get; }
        parameterType[] Parameters      { get; } 
        eventParamType[] EventParams    { get; }
        /// <summary> Hashtable of ActionMap's </summary>
        Hashtable Actions { get; }
    }

    public interface IMetreosAction
    {
        actionType ActionXml { get; }
    }
}
