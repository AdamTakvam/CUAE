using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using Northwoods.Go;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Core.Package;
using Metreos.Max.Framework;
using Metreos.Max.Framework.Satellite.Property;



namespace Metreos.Max.Drawing
{
    /// <summary>App tree serialization adjunct class</summary>
    public class MaxAppTreeSerializer
    {
        private MaxAppTree appTree;
        public  MaxAppTreeSerializer(MaxAppTree parent)  { appTree = parent; }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Serialization
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public void Serialize(XmlTextWriter writer)
        {
            MaxPreTreeRecursor recursor = new MaxPreTreeRecursor();
            MaxPreTreeRecursor.NodeEvent callback;

            writer.WriteStartElement(Const.xmlEltGlobal);    // <global>
            writer.WriteAttributeString(Const.xmlAttrName,   appTree.CanvasName);   

            writer.WriteStartElement(Const.xmlEltOutline);   // <outline>
            callback = new MaxPreTreeRecursor.NodeEvent(OnSerializingFunctionNode);
            recursor.RaiseNodeEvent  += callback;
            recursor.RecurseTree(appTree.Tree.EventsAndFunctionsRoot, 0, writer);
            recursor.RaiseNodeEvent  -= callback; 
            writer.WriteEndElement();                        // </outline>

            writer.WriteStartElement(Const.xmlEltVariables); // <variables>
            recursor.RaiseNodeEvent  += new MaxPreTreeRecursor.NodeEvent(OnSerializingVariable);
            recursor.RecurseTree(appTree.Tree.VariablesRoot, 0, writer);
            writer.WriteEndElement();                        // </variables>
  
            writer.WriteEndElement();                        // </global>
        }


        /// <summary>Actions on v0.8+ app tree node during serialization</summary>
        private void OnSerializingFunctionNode(TreeNode node, int level, object param)
        {                                  
            MaxAppTreeNode tnode = node as MaxAppTreeNode;
            if  (tnode == null || tnode.NodeType != MaxAppTreeNode.NodeTypes.Function) return;

            XmlTextWriter writer = param as XmlTextWriter;
            writer.WriteStartElement(Const.xmlEltTreeNode); // <treenode>
            MaxAppTreeNodeFunc treeNode = tnode as MaxAppTreeNodeFunc;

            writer.WriteAttributeString(Const.xmlAttrType,  // type="s"
                treeNode.Subtype == MaxAppTreeNode.Subtypes.EventAndHandler? 
                Const.xmlValNodeEvh: Const.xmlValNodeFun);  // id="n"
            writer.WriteAttributeString(Const.xmlAttrID, XmlConvert.ToString(treeNode.NodeID));
                                                            // level="n"
            writer.WriteAttributeString(Const.xmlAttrLevel, level.ToString()); 
                                                            // text="s" 
            writer.WriteAttributeString(Const.xmlAttrText,  treeNode.Text); 

            if (tnode.Subtype == MaxAppTreeNode.Subtypes.EventAndHandler) 
            {                                               // actid="n"
                MaxAsyncActionNode anode = (tnode as MaxAppTreeNodeEVxEH).CanvasNodeAction;
                if (anode != null)   
                    writer.WriteAttributeString(Const.xmlAttrActionID, anode.NodeID.ToString());
            }    

            writer.WriteStartElement(Const.xmlEltNode);     // <node type="function">
            writer.WriteAttributeString(Const.xmlAttrType, Const.xmlValNodeTypeFunction);
            MaxFunctionNode fnode = treeNode.CanvasNodeFunction;
                                                            // name="s"
            writer.WriteAttributeString(Const.xmlAttrName, fnode.NodeName);   
            writer.WriteAttributeString(Const.xmlAttrID,    // id="n"
                XmlConvert.ToString(fnode.NodeID));         // "path="s"                                                      
            writer.WriteAttributeString(Const.xmlAttrPath, Const.defaultStockToolGroup); 

            writer.WriteEndElement();                       // </node>

            MaxAppTreeNodeEVxEH treeNodeEvh = treeNode as MaxAppTreeNodeEVxEH;

            this.SerializeCallReferences(writer, treeNode);        

            if (treeNodeEvh != null)
                this.SerializeEventAndHandler(writer, treeNodeEvh);

            writer.WriteEndElement();                       // </treenode>
        }


        /// <summary>Serialize async handler tree node</summary>
        private void SerializeEventAndHandler(XmlTextWriter writer, MaxAppTreeNodeEVxEH tnode)
        {
            writer.WriteStartElement(Const.xmlEltNode);     // <node type="event">
            MaxEventNode enode = tnode.CanvasNodeEvent;
                                                            // name="s"
            writer.WriteAttributeString(Const.xmlAttrType, Const.xmlValNodeTypeEvent);
            writer.WriteAttributeString(Const.xmlAttrName, enode.NodeName);   
            writer.WriteAttributeString(Const.xmlAttrID,    // id="n"
                XmlConvert.ToString (enode.NodeID));  
                                                            // "path="s"
            MaxEventTool etool = enode.Tool as MaxEventTool;        
            writer.WriteAttributeString(Const.xmlAttrPath, 
                Utl.GetQualifiedName(etool.Package.Name, etool.Name));   

            if (enode.IsProjectTrigger)                     // trigger="true"
                writer.WriteAttributeString(Const.xmlAttrTrigger, Const.xmlValBoolTrue);

            writer.WriteEndElement();                       // </node>

            if (!enode.IsProjectTrigger)                  
                this.SerializeHandlerReferences(writer, tnode);
      
            // Write event properties: app tree nodes are not MaxSelectableObjects,
            // but properties may be set on the underlying canvas node, which in 
            // this case is the event node.   
            PmProxy.PropertiesManager.SerializeProperty(tnode.CanvasNodeEvent.PmObjectType,
                tnode.CanvasNodeEvent.MaxProperties, writer);
        }


        /// <summary>Serialize async actions referencing this handler</summary>
        private void SerializeHandlerReferences(XmlTextWriter writer, MaxAppTreeNodeEVxEH tnode)
        {
            writer.WriteStartElement(Const.xmlEltReferences);

            foreach(MaxAppTreeEvhRef refx in tnode.References)
            {
                if (refx.action == null) continue;            // <ref id="n" actid="n">
                writer.WriteStartElement(Const.xmlEltRef); 
                writer.WriteAttributeString(Const.xmlAttrID, 
                    XmlConvert.ToString(refx.NodeID));
                writer.WriteAttributeString(Const.xmlAttrActionID, 
                    XmlConvert.ToString(refx.action.NodeID));          
                writer.WriteEndElement();                     // </ref>
            }     

            writer.WriteEndElement();                         // </references> 
        }


        /// <summary>Serialize call actions referencing this function</summary>
        private void SerializeCallReferences(XmlTextWriter writer, MaxAppTreeNodeFunc tnode)
        {
            if (tnode.CanvasNodeCallActions.Count == 0) return;

            writer.WriteStartElement(Const.xmlEltCalls);    // <calls>

            foreach(object x in tnode.CanvasNodeCallActions)
            {
                MaxCallNode callnode = x as MaxCallNode;
                if (callnode == null) continue;
                writer.WriteStartElement(Const.xmlEltRef);  // <ref id="n" actid="n">
                writer.WriteAttributeString(Const.xmlAttrActionID, 
                    XmlConvert.ToString(callnode.NodeID));          
                writer.WriteEndElement();                   // </ref>
            } 

            writer.WriteEndElement();                       // </calls> 
        }


        #if(false)
        /// <summary>Actions on v0.7 app tree node during serialization</summary>
        private void OnOldSerializingFunctionNode(TreeNode node, int level, object param)
        {                                  
        MaxAppTreeNode tnode = node as MaxAppTreeNode;
        if  (tnode == null || tnode.NodeType != MaxAppTreeNode.NodeTypes.Function) return;

        XmlTextWriter writer = param as XmlTextWriter;
        writer.WriteStartElement(Const.xmlEltTreeNode); // <treenode>
        MaxAppTreeNodeFunc treeNode = tnode as MaxAppTreeNodeFunc;

        writer.WriteAttributeString(Const.xmlAttrType,  // type="s"
            treeNode.Subtype == MaxAppTreeNode.Subtypes.EventAndHandler? 
            Const.xmlValNodeEvh: Const.xmlValNodeFun);    // id="n"
        writer.WriteAttributeString(Const.xmlAttrID, XmlConvert.ToString(treeNode.NodeID));
                                                        // level="n"
        writer.WriteAttributeString(Const.xmlAttrLevel, level.ToString()); 
                                                        // text="s" 
        writer.WriteAttributeString(Const.xmlAttrText,  treeNode.Text); 

        if (tnode.Subtype == MaxAppTreeNode.Subtypes.EventAndHandler) 
        {                                               // actid="n"
            MaxAsyncActionNode anode = (tnode as MaxAppTreeNodeEVxEH).CanvasNodeAction;
            if (anode != null)   
                writer.WriteAttributeString(Const.xmlAttrActionID, anode.NodeID.ToString());
        }    

        writer.WriteStartElement(Const.xmlEltNode);     // <node type="function">
        writer.WriteAttributeString(Const.xmlAttrType, Const.xmlValNodeTypeFunction);
        MaxFunctionNode fnode = treeNode.CanvasNodeFunction;
                                                        // name="s"
        writer.WriteAttributeString(Const.xmlAttrName, fnode.NodeName);   
        writer.WriteAttributeString(Const.xmlAttrID,    // id="n"
            XmlConvert.ToString(fnode.NodeID));           // "path="s"                                                      
        writer.WriteAttributeString(Const.xmlAttrPath, Const.defaultStockToolGroup); 

        writer.WriteEndElement();   // </node>

        if (tnode.Subtype == MaxAppTreeNode.Subtypes.EventAndHandler)
        {
            MaxAppTreeNodeEVxEH treeNodeEvh = treeNode as MaxAppTreeNodeEVxEH;
            writer.WriteStartElement(Const.xmlEltNode);   // <node type="event">
            MaxEventNode enode = treeNodeEvh.CanvasNodeEvent;
                                                        // name="s"
            writer.WriteAttributeString(Const.xmlAttrType, Const.xmlValNodeTypeEvent);
            writer.WriteAttributeString(Const.xmlAttrName, enode.NodeName);   
            writer.WriteAttributeString(Const.xmlAttrID,  // id="n"
                XmlConvert.ToString (enode.NodeID));  
                                                        // "path="s"
            MaxEventTool etool = enode.Tool as MaxEventTool;        
            writer.WriteAttributeString(Const.xmlAttrPath, 
                Utl.GetQualifiedName(etool.Package.Name, etool.Name));   

            if   (enode.IsProjectTrigger)               // trigger="true"
                writer.WriteAttributeString(Const.xmlAttrTrigger, Const.xmlValBoolTrue);

            writer.WriteEndElement(); // </node>

            // Write event properties: app tree nodes are not MaxSelectableObjects,
            // but properties may be set on the underlying canvas node, which in 
            // this case is the event node.   
            PmProxy.PropertiesManager.SerializeProperty(treeNodeEvh.CanvasNodeEvent.PmObjectType,
                    treeNodeEvh.CanvasNodeEvent.MaxProperties, writer);
        }       

        writer.WriteEndElement();   // </treenode>
        }
        #endif


        /// <summary>Actions on app tree variable node during serialization</summary>
        private void OnSerializingVariable(TreeNode node, int level, object param)
        {
            MaxAppTreeNodeVar vnode = node as MaxAppTreeNodeVar; if (vnode == null) return;
            XmlTextWriter writer = param as XmlTextWriter;
            writer.WriteStartElement(Const.xmlEltTreeNode); // <treenode>  
  
            writer.WriteAttributeString(Const.xmlAttrText, vnode.Text); 
            writer.WriteAttributeString(Const.xmlAttrID,  XmlConvert.ToString(vnode.NodeID)); 
            writer.WriteAttributeString(Const.xmlAttrVID, 
                XmlConvert.ToString(vnode.CanvasNodeVariable.NodeID)); 

            // Write properties for the variable
            PmProxy.PropertiesManager.SerializeProperty(vnode.CanvasNodeVariable.PmObjectType, 
                vnode.CanvasNodeVariable.MaxProperties, writer);

            writer.WriteEndElement();                       // </treenode>
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Deserialization
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  
        /// <summary>Reconstruct the tree from a project file xml "outline" node</summary>
        public bool DeserializeOutline(XmlNode xmlnode) // 1016
        {
            #region xmlexample1
            // <treenode type="evh" id="632336196392031499" level="2" 
            //         text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
            //   <node type="function" name="OnPlayAnnouncement_Failed" id="632331298536251030" 
            //         path="Metreos.StockTools" />
            //   <node type="event" name="PlayAnnouncement_Failed" id="632331298536251029" 
            //         path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
            //   <references>
            //     <ref id="632336196392031500" actid="632331298536251034" />
            //     <ref id="632336196392031501" actid="632331298536251056" />
            //   </references>
            //   <Properties type="asyncCallback">
            //   </Properties>
            // </treenode>
            // <treenode type="fun" id="632336196392031502" level="1" text="FunctionOne">
            //   <node type="function" name="FunctionOne" id="632331298536251036" path="Metreos.StockTools" />
            //   <calls>
            //     <ref actid="632331298536251035" />
            //   </calls>
            // </treenode>
            #endregion

            // We maintain an array or map with each slot corresponding to a level
            // of the tree, and containing the most recent node at that level.
            // Serialized levels must be > 0, as level zero is the folder node.

            Hashtable levelnodes = new Hashtable();
            levelnodes[0] = appTree.Tree.EventsAndFunctionsRoot;
      
            foreach(XmlNode treexml in xmlnode)    // <treenode>
            {
                // Recreate each function/event or function node
                if (treexml.NodeType != XmlNodeType.Element)  continue;
                if (treexml.Name     != Const.xmlEltTreeNode) continue;

                string nodeType  = Utl.XmlAttr    (treexml, Const.xmlAttrType); 
                long   nodeID    = Utl.XmlAttrLong(treexml, Const.xmlAttrID,    Const.ErrValNodeID);            
                int    nodeLevel = Utl.XmlAttrInt (treexml, Const.xmlAttrLevel, Const.ErrValTreeLevel);  
                string nodeText  = Utl.XmlAttr    (treexml, Const.xmlAttrText);  
                // string nodeFunc  = Utl.XmlAttr    (treexml, Const.xmlAttrFunction); 
                // long   actionID  = Utl.XmlAttrLong(treexml, Const.xmlAttrActionID, 0); 

                if (nodeType == null || nodeID == Const.ErrValNodeID || nodeLevel < 1) continue; 

                MaxAppTreeNode parentnode = levelnodes[nodeLevel-1] as MaxAppTreeNode;
                if  (parentnode == null) continue;
                MaxFunctionCanvas functionCanvas = null;
                MaxAppTreeNode treenode = null;

                switch(nodeType)   
                {                   
                    case Const.xmlValNodeEvh:         // Event + handler tree node       
                                                                               
                        if  (this.GetEventNode   (treexml) == null) continue;              
                        if  (this.GetFunctionNode(treexml) == null) continue;              
                                                                                 
                        // The app node creation above has created explorer entries.       
                        // The RegisterNewHandler call below will construct the function 
                        // canvas and tab, in advance of parsing the function canvas xml.

                        if (this.appTree.GetFirstEntryFor(maxFunctionNode.NodeName) != null)
                            continue;       // Should not occur for v08 & after
                                            // Will not duplicate canvas and tab
                        functionCanvas = appTree.RegisterNewHandler
                            (this.maxFunctionNode, this.maxEventNode);    
                                            // Create and insert app tree node
                        treenode = appTree.Tree.AddFunctionWithHandler          
                            (parentnode, this.maxFunctionNode, this.maxEventNode, String.Empty);   

                        treenode.NodeID = nodeID;    // Use serialized ID

                        levelnodes[nodeLevel] = treenode;

                        // We have set the function canvas appTreeNode here, in advance of   
                        // parsing the function canvas xml. A function serializes its node 
                        // references independently, so we could wait and look up the app 
                        // tree node during function canvas deserialization if we wished.

                        // As of v08 there should again be only one AppTreeNode per canvas
                        // so we can deprecate canvas.AddAppTreeNode and its array
                        functionCanvas.AppTreeNode = treenode as MaxAppTreeNodeFunc;
               
                        // Parse out event handler references
                        this.GetHandlerReferences(treexml, treenode);

                        // Parse out calls to this function (which can occur for handlers as well)
                        this.GetFunctionCalls(xmlnode, treenode);

                        // Properties are maintained for the hosted MaxEventNode
                        this.DeserializeProperties(this.maxEventNode, treexml);

                        break;

                    case Const.xmlValNodeFun:    // Called function tree node      
                                                                              
                        if (this.GetFunctionNode(treexml) == null) continue;
                                            
                        if (this.appTree.GetFirstEntryFor(maxFunctionNode.NodeName) != null)
                            continue;           // Should not occur for v08 & after
                                 
                        functionCanvas = appTree.RegisterNewHandler
                            (this.maxFunctionNode, this.maxEventNode);   

                        treenode = appTree.AddFunction(this.maxFunctionNode.NodeName, parentnode);  

                        treenode.NodeID = nodeID;   

                        // Parse out calls to this function
                        this.GetFunctionCalls(treexml, treenode);     

                        functionCanvas.AddAppTreeNode(treenode as MaxAppTreeNodeFunc);
             
                        break;

                } // switch(nodeType)

                if (treenode == null) continue;

                levelnodes[nodeLevel] = treenode;

                if (!Config.ExpandAppTreeOnLoad) parentnode.Collapse();

            } // foreach(XmlNode treexml in xmlnode

            return true;

        } //  DeserializeEventFunctionBranch()
    


        #region DeserializeOutlinePriorV08(XmlNode xmlnode)
        /// <summary>Ver 0.7 reconstruct tree from project file xml "outline" node</summary>
        /// <remarks>Deprecated until all script app trees upgraded to ver 0.8</remarks>
        public bool DeserializeOutlinePriorV08(XmlNode xmlnode)
        {
            #region xmlexample1
            // <treenode type="evh" id="632088315204137761" level="1" 
            //         text="IncomingCall (trigger): OnIncomingCall">
            //   <node type="function" name="OnIncomingCall" id="632088315204137758" 
            //         path="Metreos.StockTools" />
            //   <node type="event" name="IncomingCall" id="632088315204137757" 
            //         path="Metreos.CallControl.IncomingCall" trigger="true" />
            // </treenode>
            #endregion

            // We maintain an array or map with each slot corresponding to a level
            // of the tree, and containing the most recent node at that level.
            // Serialized levels must be > 0, as level zero is the folder node

            Hashtable levelnodes = new Hashtable();
            levelnodes[0] = appTree.Tree.EventsAndFunctionsRoot;
      
            foreach(XmlNode treexml in xmlnode)    // <treenode>
            {
                // Recreate each function/event or function node
                if (treexml.NodeType != XmlNodeType.Element)  continue;
                if (treexml.Name     != Const.xmlEltTreeNode) continue;

                string nodeType  = Utl.XmlAttr    (treexml, Const.xmlAttrType); 
                long   nodeID    = Utl.XmlAttrLong(treexml, Const.xmlAttrID,    Const.ErrValNodeID);            
                int    nodeLevel = Utl.XmlAttrInt (treexml, Const.xmlAttrLevel, Const.ErrValTreeLevel);  
                string nodeText  = Utl.XmlAttr    (treexml, Const.xmlAttrText);  
                string nodeFunc  = Utl.XmlAttr    (treexml, Const.xmlAttrFunction); 
                long   actionID  = Utl.XmlAttrLong(treexml, Const.xmlAttrActionID, 0); 

                if (nodeType == null || nodeID == Const.ErrValNodeID || nodeLevel < 1) continue; 

                MaxAppTreeNode parentnode = levelnodes[nodeLevel-1] as MaxAppTreeNode;
                if  (parentnode == null) continue;
                MaxFunctionCanvas functionCanvas = null;
                MaxAppTreeNode treenode = null;

                switch(nodeType)   
                {                   
                    case Const.xmlValNodeEvh:         // Event + handler tree node       
                                                                               
                        if  (this.GetEventNode   (treexml) == null) continue;              
                        if  (this.GetFunctionNode(treexml) == null) continue;              
                                                                                 
                        // The app node creation above has created explorer entries.       
                        // The RegisterNewHandler call below will construct the function 
                        // canvas and tab, in advance of parsing the function canvas xml.

                        functionCanvas               // Will not duplicate canvas and tab
                            = appTree.RegisterNewHandler(this.maxFunctionNode, this.maxEventNode);

                        treenode = appTree.Tree.AddFunctionWithHandler
                            (parentnode, this.maxFunctionNode, this.maxEventNode, String.Empty);

                        treenode.NodeID = nodeID;   
                        treenode.Tag    = actionID;  // Note for 1016 

                        levelnodes[nodeLevel] = treenode;

                        // We have set the function canvas appTreeNode here, in advance of   
                        // parsing the function canvas xml. A function serializes its node 
                        // references independently, so we could wait and look up the app 
                        // tree node during function canvas deserialization if we wished.

                        functionCanvas.AddAppTreeNode(treenode as MaxAppTreeNodeFunc);

                        // Properties are maintained for the hosted MaxEventNode
                        this.DeserializeProperties(this.maxEventNode, treexml);

                        break;

                    case Const.xmlValNodeFun:         // Called function tree node      
                                                                              
                        if  (this.GetFunctionNode(treexml) == null) continue;

                        functionCanvas               // Will not duplicate canvas and tab
                            = appTree.RegisterNewHandler(this.maxFunctionNode, this.maxEventNode);

                        treenode = appTree.AddFunction(this.maxFunctionNode.NodeName, parentnode);

                        treenode.NodeID = nodeID;        

                        functionCanvas.AddAppTreeNode(treenode as MaxAppTreeNodeFunc);
           
                        break;

                } // switch(nodeType)

                if (treenode == null) continue;

                levelnodes[nodeLevel] = treenode;

                if (!Config.ExpandAppTreeOnLoad) parentnode.Collapse();

            } // foreach(XmlNode treexml in xmlnode


            MaxManager.Upgrading = true;          // Trigger immediate upgrade to ver 0.8

            return true;

        } //  DeserializeOutline()
        #endregion



        /// <summary>Reconstruct global variables from a project file xml "variables" node</summary>
        public bool DeserializeVariables(XmlNode xmlnode)
        {
            #region xmlexample2
            // <treenode text="foo" id="632089263771875020" vid="632089263771875019">
            //   <Properties>
            //    ...
            //   </Properties>
            // </treenode>
            #endregion

            foreach(XmlNode treexml in xmlnode)    // <treenode>
            {
                if (treexml.NodeType != XmlNodeType.Element)  continue;
                if (treexml.Name     != Const.xmlEltTreeNode) continue;

                long treenodeID = Utl.XmlAttrLong(treexml, Const.xmlAttrID,  Const.ErrValNodeID);  
                long varnodeID  = Utl.XmlAttrLong(treexml, Const.xmlAttrVID, Const.ErrValNodeID);            
                string nodeText = Utl.XmlAttr    (treexml, Const.xmlAttrText);  
                if (treenodeID == Const.ErrValNodeID) continue; 

                MaxAppTreeNodeVar vnode = appTree.Tree.CreateVariableNode
                    (MaxStockTools.Instance.VariableTool, nodeText, treenodeID, varnodeID);  

                this.DeserializeProperties(vnode.CanvasNodeVariable, treexml);
            }   

            return true;
        } 


        /// <summary>Parse out the function and return app canvas function
        /// node, creating same if necessary</summary>
        public MaxFunctionNode GetFunctionNode(XmlNode treenode)
        { 
            // <node type="function" name="OnIncomingCall" id="632088315204137758" 
            //  path="Metreos.StockTools" />  
     
            this.maxFunctionNode = null;
            MaxAppCanvas canvas  = appTree.AppCanvas;
            GoDocument   doc     = canvas.View.Document;
            MaxManager   manager = MaxManager.Instance;

            foreach(XmlNode maxnode in treenode)   
            {
                if (maxnode.NodeType != XmlNodeType.Element) continue;
                if (maxnode.Name     != Const.xmlEltNode)    continue;
                                            // <node type="function">
                string nodeType = Utl.XmlAttr(maxnode, Const.xmlAttrType);
                if  (nodeType == null || nodeType != Const.xmlValNodeTypeFunction) continue;
                                            // id="n"
                long nodeID = Utl.XmlAttrLong(maxnode, Const.xmlAttrID, Const.ErrValNodeID);
                if  (nodeID == Const.ErrValNodeID) continue;
                                            // Look up function node
                this.maxFunctionNode  = canvas.FindByNodeID(nodeID) as MaxFunctionNode;
                if  (maxFunctionNode != null) break; 
                                            // Create function node
                this.maxFunctionNode  = MaxStockTools.NewMaxFunctionNode(canvas);
                if  (maxFunctionNode == null) continue;

                string functionName = Utl.XmlAttr(maxnode, Const.xmlAttrName);
                maxFunctionNode.NodeName = functionName;
                maxFunctionNode.NodeID   = nodeID;  // Replace ID with serialized ID
                         
                doc.Add(maxFunctionNode);   // Add to app, tab, and explorer
                break;
            }

            return maxFunctionNode;
        }


        /// <summary>Parse out the event and return app canvas event
        /// node, creating same if necessary</summary>
        public MaxEventNode GetEventNode(XmlNode treenode)
        {        
            // <node type="event" name="IncomingCall" id="632088315204137757" 
            //  path="Metreos.CallControl.IncomingCall" trigger="true" />

            this.maxEventNode = null;
            MaxAppCanvas canvas   = appTree.AppCanvas;
            GoDocument   doc      = canvas.View.Document;
            MaxManager   manager  = MaxManager.Instance;
            MaxPackages  packages = manager.Packages;

            foreach(XmlNode maxnode in treenode)   
            {
                if (maxnode.NodeType != XmlNodeType.Element) continue;
                if (maxnode.Name     != Const.xmlEltNode)    continue;
                                                    // <node type="event">
                string nodeType = Utl.XmlAttr(maxnode, Const.xmlAttrType);
                if  (nodeType == null || nodeType != Const.xmlValNodeTypeEvent) continue;
                                                    // id="n"
                long nodeID = Utl.XmlAttrLong(maxnode, Const.xmlAttrID, Const.ErrValNodeID);
                if  (nodeID == Const.ErrValNodeID) continue;
                                            
                this.maxEventNode  = canvas.FindByNodeID(nodeID) as MaxEventNode;
                if  (maxEventNode != null) break;   // Event node already exists

                string toolpath    = Utl.XmlAttr(maxnode, Const.xmlAttrPath);
                if  (toolpath == null) continue;

                string trigger  = Utl.XmlAttr(maxnode, Const.xmlAttrTrigger);
                bool isProjectTrigger = Utl.ConvertStringToBool(trigger, false);

                string eventName   = Utl.XmlAttr(maxnode, Const.xmlAttrName);
                                                    // Locate event in packages
                MaxEventTool tool  = packages.FindEventByToolName(toolpath) as MaxEventTool; 
                                                    // If package is not available ...
                if (tool == null)                   // get a placeholder tool and package
                    tool = MaxCanvasSerializer.GetStubTool   
                        (NodeTypes.Event, eventName, Utl.GetQualifiers(toolpath)) as MaxEventTool;
                if  (tool == null) continue;
                                                    // Create event node
                this.maxEventNode  = new MaxEventNode(canvas, tool);
                if  (maxEventNode == null) continue;
        
                maxEventNode.NodeName = eventName;
                maxEventNode.NodeID   = nodeID;     // Replace ID with serialized ID
                maxEventNode.IsProjectTrigger = isProjectTrigger;  
                                             
                doc.Add(maxEventNode);              // Add to app canvas
                break;
            }

            return maxEventNode;
        }

    

        /// <summary>Parse out handler references, temporarily adding ID references</summary>
        public void GetHandlerReferences(XmlNode xmlnode, MaxAppTreeNode tnode)
        {        
            // <references>
            //   <ref id="632336196392031492" actid="632331298536251136" />
            //   <ref id="632336196392031493" actid="632331298536251048" />
            // </references>

            // We may not need to use the serialized references, since references 
            // are accumulated as async action nodes are constructed. We'll leave
            // the code here for a while in case we do need it. 1016

            MaxAppTreeNodeEVxEH treenode = tnode as MaxAppTreeNodeEVxEH;
            if (treenode == null) return;

            foreach(XmlNode thisnode in xmlnode)   
            {
                if (thisnode.Name != Const.xmlEltReferences) continue;
        
                foreach(XmlNode refnode in thisnode)
                {                         
                    if  (refnode.Name != Const.xmlEltRef) continue;           
                    long nodeID  = Utl.XmlAttrLong(refnode, Const.xmlAttrID, Const.ErrValNodeID);
                    long actID   = Utl.XmlAttrLong(refnode, Const.xmlAttrActionID, Const.ErrValNodeID);
                    if  (nodeID == Const.ErrValNodeID || actID == Const.ErrValNodeID) continue;

                    // During app tree deserialization we store the action ID with the reference.
                    // After the script is deserialized, we use these IDs to get the action node.

                    // The reference is added instead in the async action node constructor

                    // MaxAppTreeEvhRef evhref = new MaxAppTreeEvhRef(treenode, nodeID, actID);
                    // treenode.AddReference(evhref); 
                } 
            }            
        }


        /// <summary>Parse out function calls, temporarily adding ID references</summary>
        public void GetFunctionCalls(XmlNode xmlnode, MaxAppTreeNode tnode)
        {        
            //  <calls>
            //    <ref actid="632331298536251035" />
            //  </calls>

            // Since the call node is registered with the tree node as the call node is
            // constructed, there is no need to cache the call node IDs here. However
            // this may change in the future, so we retain the code below.
            // Also see MaxAppTree.ResolveReferences

            #if(false)

            MaxAppTreeNodeFunc treenode = tnode as MaxAppTreeNodeFunc;
            if (treenode == null) return;

            foreach(XmlNode thisnode in xmlnode)   
            {
                if (thisnode.Name != Const.xmlEltCalls) continue;
                
                foreach(XmlNode refnode in thisnode)
                {   
                if  (refnode.Name != Const.xmlEltRef) continue;                                 
                long actID = Utl.XmlAttrLong(refnode, Const.xmlAttrActionID, Const.ErrValNodeID);
                if  (actID == Const.ErrValNodeID) continue;

                // During app tree deserialization we store the call node ID with the tree node.
                // After the script is deserialized, we replace these IDs with the call action node.
                treenode.CanvasNodeCallActions.Add(actID);
                } 
            }  

            #endif          
        }


        /// <summary>Deserialize properties of a hosted max node</summary>
        public bool DeserializeProperties(IMaxNode maxnode, XmlNode xmlnode)
        {
            if  (maxnode == null) return false;
            bool result = false;
            try
            {
                foreach(XmlNode propsnode in xmlnode)
                {
                    if (propsnode.NodeType != XmlNodeType.Element)    continue;
                    if (propsnode.Name     != Const.xmlEltProperties) continue;

                    bool isPackageUnavailable = maxnode.Tool.Package is MaxStubPackage;
                                                    
                    PmProxy.PropertiesManager.DeserializeProperty
                       (maxnode, maxnode.PmObjectType, maxnode.MaxProperties, 
                        propsnode, !isPackageUnavailable); 

                    result = true; 
                }
            }
            catch(Exception x) { MaxManager.Instance.Trace(x.Message); }

            return result;
        }

        MaxFunctionNode maxFunctionNode;
        MaxEventNode    maxEventNode;

    } // class MaxAppTreeSerializer

}   // namespace
