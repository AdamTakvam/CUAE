using System;
using System.Xml;
using System.Drawing;
using System.Collections;

namespace Metreos.Max.Framework
{    
    public class LoopParser : FunctionParser
    {    
        // private string id = null;
        // private string parentFunctionName = null;

        public LoopParser(FunctionMetadata loopMetadata,  Stepper parent) : base(parent)
        {    
            // this.id = id;
            // this.parentFunctionName = parentFunctionName;
        }

        protected override void ConstructSteps()
        {
            executeStep = Delegate.Combine(new FunctionDelegate[]
            {
                new FunctionDelegate(InitializeLoopMetadata),
                new FunctionDelegate(PopulateVariables)
            });
        } 

        private void InitializeLoopMetadata(XmlNode canvasNode, ScriptMap metadata)
        {
            // metadata.AddLoop(functionName, actions, null);
        } 

        protected override void PopulateVariables(XmlNode canvasNode, ScriptMap metadata)
        {
            // FunctionMap parentFunction = metadata[parentFunctionName];
        
            // Add loopIndex to the variables. 
            // Get UserDefined name of the index if this is not accessible
            // parentFunction.Variables
        }
    }
}
