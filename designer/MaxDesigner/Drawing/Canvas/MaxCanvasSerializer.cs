//
// MaxCanvasSerializer.cs
//
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Northwoods.Go;
using Metreos.Max.Framework;
using Metreos.Max.Manager;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Core.Package;
using Metreos.Max.Drawing;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.GlobalEvents;
using Crownwood.Magic.Docking;


 
namespace Metreos.Max.Drawing
{
    public class MaxCanvasSerializer
    {
        /// <summary>Save canvas layout to xml</summary> 
        public static void Serialize(XmlTextWriter writer, MaxCanvas canvas)
        {
            writer.WriteStartElement(Const.xmlEltCanvas);
            writer.WriteAttributeString(Const.xmlAttrType, canvas.CanvasType.ToString());  
            writer.WriteAttributeString(Const.xmlAttrName, canvas.CanvasName);

            Crownwood.Magic.Controls.TabPage tabPage  
                = MaxManager.Instance.TabPages[canvas.CanvasName]; 

            bool isHidden = (null == tabPage);
            if  (isHidden)
                 writer.WriteAttributeString(Const.xmlAttrShow, Const.xmlValBoolFalse);
            else
            if  (tabPage == MaxManager.Instance.SelectedTab) 
                 writer.WriteAttributeString(Const.xmlAttrActiveTab, Const.xmlValBoolTrue);


            MaxFunctionCanvas fcanvas = canvas as MaxFunctionCanvas;

            if (fcanvas != null)                     
            {   
                if (fcanvas.StartNode      != null)
                    writer.WriteAttributeString(Const.xmlAttrStartNode, 
                        XmlConvert.ToString(fcanvas.StartNode.NodeID));
                if (fcanvas.AppTreeNode    != null)
                    writer.WriteAttributeString(Const.xmlAttrTreeNode,  
                        XmlConvert.ToString(fcanvas.AppTreeNode.NodeID));
                if (fcanvas.AppCanvasNode  != null)
                    writer.WriteAttributeString(Const.xmlAttrAppNode,   
                        XmlConvert.ToString(fcanvas.AppCanvasNode.NodeID));
                if (fcanvas.HandlerForNode != null)
                    writer.WriteAttributeString(Const.xmlAttrHandlerFor,
                        XmlConvert.ToString(fcanvas.HandlerForNode.NodeID));
            }
                                            // Serialize graph nodes
            foreach(GoObject obj in canvas.View.Document)                                                      
            {      
                IMaxNode maxnode = obj as IMaxNode;    
                if (maxnode != null)
                    maxnode.MaxSerialize(writer);
            }
                                            // Serialize tray variables
            IMaxNode[] trayVariables = canvas.GetFunctionVariables(true);
            foreach(IMaxNode trayvar in trayVariables)
            {
                if (trayvar != null)
                    trayvar.MaxSerialize(writer);
            }
     
            writer.WriteEndElement();       // </canvas>
        } 


        /// <summary>Save current canvas selection to xml</summary>
        public static void SerializeSelection(XmlTextWriter writer, MaxCanvas canvas)
        {      
            writer.WriteStartElement(Const.xmlEltCanvas);
            writer.WriteAttributeString(Const.xmlAttrName, canvas.CanvasName);
            writer.WriteAttributeString(Const.xmlAttrID, XmlConvert.ToString(canvas.CanvasID));

            // Calculate and serialize leftmost/topmost node coordinates
            float selMinX = 0xffff, selMinY = 0xffff;

            foreach(GoObject x in canvas.View.Selection)
            {
                if (x == null || !x.Deletable) continue;
                if (x.Left < selMinX)  selMinX = x.Left; 
                if (x.Top  < selMinY)  selMinY = x.Top;      
            }  

            writer.WriteAttributeString(Const.xmlAttrLeft, XmlConvert.ToString(selMinX));  
            writer.WriteAttributeString(Const.xmlAttrTop,  XmlConvert.ToString(selMinY));           

            // Serialize each node in selection 
            foreach(GoObject x in canvas.View.Selection)                                                      
            {      
                IMaxNode maxnode = x as IMaxNode;    
                if (maxnode != null && x.Deletable)
                    maxnode.MaxSerialize(writer);   
            }

            writer.WriteEndElement(); // </canvas>
        } 


        /// <summary>Paste clipboard collection of Max objects to canvas</summary>
        public static void DeserializeSelection(MaxCanvas canvas, XmlNode canvasxml, Hashtable ht)
        {
            #region xml example 1
            //  <MaxClipboardA>
            //  <canvas name="foo" varsy="596">
            //    <node type="Loop" id="632182304753750168" 
            //          text="loop 1x" cx="317" cy="415" entry="1" exit="3" 
            //          class="MaxLoopContainerNode" group="Application Components" 
            //          path="Metreos.StockTools" x="253" y="105" mx="412" my="312">
            //      <linkto id="632183145221250158" port="1" fromport="1" type="Basic" 
            //            style="Bezier" ortho="true" />
            //      <linkto id="632182304753750170" fromport="3" type="Basic" 
            //            style="Bezier" ortho="true" />
            //      <Properties type="int">1</Properties>
            //    </node>
            //    <node type="Action" id="632183443516250158" name="Sleep" container="632182304753750168" 
            //         class="MaxActionNode" group="" path="Metreos.Native.ApplicationControl" x="399" y="313">
            //     <linkto id="632183145221250158" port="4" type="Labeled" style="Bezier" 
            //         ortho="true" label="default" />
            //     <Properties final="false" type="native"></Properties>
            //    </node>
            //  </canvas>
            // </MaxClipboardA>
            #endregion

            MaxCanvasSerializer.nodeIdTranslateTable = ht;

            canvas.View.Selection.Clear(); // Since selection becomes all pasted nodes 

            MaxCanvasSerializer.Deserialize(canvas, canvasxml, true);  

            MaxCanvasSerializer.nodeIdTranslateTable = null;
        } 

    
        /// <summary>Restore a canvas layout (or partial layout) from xml</summary>   
        public static void Deserialize(MaxCanvas canvas, XmlNode canvasxml, bool partial)
        {    
            #region xml example 2
            // <canvas type="Function" name="OnIncomingCall" version="0.1" varsy="350" 
            //         startnode="632089263771875003" treenode="632089263771875004" 
            //         appnode="632089263771875002" handlerfor="632089263771875001">
            //   <node type="Start" id="632089263771875003" name="Start" 
            //         group="Metreos.StockTools" x="32" y="32">
            //     <linkto id="632089263771875013" type="Basic" style="Bezier" ortho="true" />
            //   </node>
            //   <node type="Action" id="632089263771875014" name="CallFunction" 
            //         group="" x="397" y="102">
            //     <linkto id="632089263771875018" type="Labeled" style="Bezier" 
            //         ortho="true" label="success" />
            //   </node>
            //   <node type="Action" id="632089263771875018" name="ExitFunction" 
            //         group="" x="444" y="278" />
            //   <node type="Variable" id="632089263771875023" name="variable3" 
            //         group="Application Components" x="17" y="368" />
            //   <node type="Variable" id="632089263771875024" name="variable4" 
            //         group="Application Components" x="215" y="289" />
            // </canvas>
            #endregion
      
            GoDocument document = canvas.View.Document;       
            if (!partial) document.SkipsUndoManager = true;

            try 
            {

            ArrayList pastedNodes = new ArrayList();

            CanvasInfo canvasInfo = new CanvasInfo();
            canvasInfo.canvas  = canvas;
            canvasInfo.doc     = document;
            canvasInfo.pasting = partial;
            canvasInfo.bounds.Size = document.Size.ToSize();
  
            canvasInfo.startnodeID = Utl.XmlAttrLong(canvasxml, Const.xmlAttrStartNode, Const.ErrValNodeID);  
            canvasInfo.treenodeID  = Utl.XmlAttrLong(canvasxml, Const.xmlAttrTreeNode,  Const.ErrValNodeID);
            canvasInfo.appnodeID   = Utl.XmlAttrLong(canvasxml, Const.xmlAttrAppNode,   Const.ErrValNodeID);
            canvasInfo.handler4ID  = Utl.XmlAttrLong(canvasxml, Const.xmlAttrHandlerFor,Const.ErrValNodeID);
            canvasInfo.variablesLocationY = Utl.XmlAttrFloat(canvasxml, Const.xmlAttrVarsY, Const.ErrValXY);

            foreach (XmlNode nodexml in canvasxml) // Parse each <node> tag in the xml 
            {
                if (nodexml.NodeType != XmlNodeType.Element) continue;
                if (nodexml.Name     != Const.xmlEltNode)    continue;

                #region xml example 3
                //   <node type="Start" id="632089263771875003" name="Start" 
                //         group="Metreos.StockTools" x="32" y="32">
                //     <linkto id="632089263771875013" type="Basic" style="Bezier" ortho="true" />
                //   </node>
                #endregion
        
                NodeInfo info   = new NodeInfo();
                info.canvasInfo = canvasInfo;
        
                if  (partial)                      // Determine offset from original x,y
                {
                    canvasInfo.canvasID = Utl.XmlAttrLong (canvasxml, Const.xmlAttrID,  0);
                    canvasInfo.selleft  = Utl.XmlAttrFloat(canvasxml, Const.xmlAttrLeft,0F);
                    canvasInfo.seltop   = Utl.XmlAttrFloat(canvasxml, Const.xmlAttrTop, 0F);
                    info.offset = ComputePasteOffset(canvasInfo);         
                }
                else info.offset = Const.point00;

                // Extract all pertinent node properties 

                if  (!ParseNodeAttributes(nodexml, info)) continue;

                if (partial && IsNodeIgnoredDuringCopyPaste(info.nodeType)) continue;

                // Get node's annotation if any
                GetNodeAnnotationInfo(nodexml, info);
          
                // Parse through the outgoing links for this node. We save link info
                // in lieu of parsing the XML twice, since we cannot restore links 
                // until all nodes have been placed on the canvas.
  
                GetNodeOutboundLinks(nodexml, info);   

                // Finally we place the node on the canvas, resetting its node ID
                // to its old (serialized) ID, since that ID is the link map key,
                // and deserialize the node's properties collection 

                IMaxNode maxnode = PlaceNode(info);
                
                if (maxnode == null) 
                    MaxManager.Instance.SignalFrameworkTextMessage
                        (Const.CouldNotInsertNodeMsg(canvasInfo.canvas.Name, info.nodeName), true, false);
                {
                    ParseNodeProperties(maxnode, nodexml);

                    // Select pasted object
                    if (partial) pastedNodes.Add(maxnode as GoObject);  
                }  

            } // foreach (XmlNode in canvasxml)  
  
            // 'jog' the prop grid to update behavior specific to new properties 
            MaxPropertyWindow.Instance.Grid.ForcePropertyValueChange();

            // Now that all this canvas' nodes have been placed on the canvas,
            // we can place the links for each node which has outbound links 
  
            PlaceNodeLinks(canvasInfo);

            // Identify which container, if any, each eligible node is a member of
            canvas.ContainerMonitor.SetInitialContainerMembership();

            document.Size = GetEffectiveDocumentSize(document.Size, canvasInfo.bounds);  

            GoSelection selection = canvas.View.Selection;
            selection.Clear();

            if (partial)
            {    
                foreach(GoObject pastedNode in pastedNodes) selection.Add(pastedNode);
            }

            }   // try

            catch(Exception x) { throw x; } 

            finally
            {
                document.SkipsUndoManager = false;
            }

        } // Deserialize()



        /// <summary>Place a node on canvas and configure</summary>
        private static IMaxNode PlaceNode(NodeInfo info)
        {
            MaxCanvas canvas = info.canvasInfo.canvas;

            MaxView.NodeArgs args = SetNodeArgs(info);
                                            // Create node and insert to canvas
            IMaxNode node = canvas.InsertNode(args);
        
            if  (node == null) return null; 

            info.UpdateBounds((node as GoObject).Bounds);
            node.NodeID = info.nodeID;      // Replace ID with serialized ID

            switch(info.nodeType)           // Set attibutes specific to type
            {
               case NodeTypes.Label:
                    ((MaxLabelNode)node).Text = info.nodeText;
                    break;

               case NodeTypes.Loop:
                    if  (info.cx != Const.ErrValXY) (node as GoGroup).Width  = info.cx;
                    if  (info.cy != Const.ErrValXY) (node as GoGroup).Height = info.cy;
                    ((MaxLoopContainerNode)node).DeserializedEntryPort = info.entryPort;
                    ((MaxLoopContainerNode)node).Text = info.nodeText;
                    break;

               case NodeTypes.Comment:
                    ((MaxCommentNode)node).Text = info.nodeText;            
                    break;

               case NodeTypes.Variable:             

                    MaxFunctionCanvas fcanvas = canvas as MaxFunctionCanvas;
                    if  (fcanvas == null || fcanvas.VtrayManager == null) break;

                    if (info.isTrayVariable)                
                    {
                        fcanvas.Tray.AddItem(node as MaxRecumbentVariableNode);

                        canvas.View.Document.Remove(node as GoObject);
                    }

                    break;

               case NodeTypes.Start:
                    if  (canvas is MaxFunctionCanvas )
                        (canvas as MaxFunctionCanvas).StartNode = node as MaxStartNode;           
                    break;
            }


            if (info.annotID > 0)
            {    
                 // Deserialize node's annotation
                MaxView.NodeArgs annotArgs = new MaxView.NodeArgs
                    (new PointF(info.annotX,info.annotY), 
                        MaxStockTools.Instance.AnnotationTool, info.annotID);

                annotArgs.nodeText = info.annotText;
                annotArgs.parent = node;

                IMaxNode annotnode = canvas.InsertNode(annotArgs);
                if (annotnode != null) (annotnode as GoObject).Visible = false;
            }

            return node;
        }


        /// <summary>Replace all of a canvas' links</summary>
        private static void PlaceNodeLinks(CanvasInfo canvasinfo)
        {
            try                                    
            {
              foreach(object x in canvasinfo.doc)                                           
              {
                  IMaxNode fromNode = x as IMaxNode; if (fromNode == null) continue;          
       
                  ArrayList nodeLinks = canvasinfo.nodeLinkMap[fromNode.NodeID] as ArrayList;
                  if (nodeLinks == null) continue;        // No links from this node
                                                           
                  foreach(LinkInfo linkinfo in nodeLinks) // Node has outbound links
                  {
                      RestoreNodeLink(fromNode, canvasinfo, linkinfo);
                  }             
              }    
            } 
            catch(InvalidCastException) { }
            // GoDiagram throws InvalidCastException as we add a MaxLink to a
            // GoDocument. Northwoods had no suggestions on why that might be. 
            // It is not clear at this writing if this is still occurring.
        }


        /// <summary>Rebuild an outbound link for a node</summary>
        private static bool RestoreNodeLink
        ( IMaxNode fromNode, CanvasInfo canvasinfo, LinkInfo linkinfo)
        {
            IMaxNode toNode = null;
            IGoPort  toPort = null, fromPort = null;

            foreach(object x in canvasinfo.doc)   // Find link's other node                                          
            {
                IMaxNode node = x as IMaxNode;  if (node == null) continue;  
                                            
                if (node.NodeID == linkinfo.toID) { toNode = node; break; }  
            } 
  
            if  (toNode == null) return false;
  
            bool isLabeledLink = linkinfo.label != null;

            // Instantiate the link. If a labeled link, this operation temporarily
            // sets the link label to its deserialized text value. When the link
            // created event is later raised, this permits the OnLinkCreated handler 
            // to reset the label text to this value, after it replaces this text
            // label with a choices combobox.
            
            // Preserving saved link styles
            // Code redundancy in below if block is due to the fact that MaxActionLink's and 
            // MaxBasicLink's implementations of the Style do not reside in a shared parent 
            // class, and thus they do not share a common constructor that takes a LinkStyle argument.
            IGoLink link = null;
            if (isLabeledLink)
            {
                MaxActionLink actionLink = new MaxActionLink(canvasinfo.canvas, fromNode, linkinfo.label, linkinfo.style);
                link = actionLink as IGoLink;
            }
            else
            {
                MaxBasicLink basicLink = new MaxBasicLink(canvasinfo.canvas, fromNode, linkinfo.style);
                link = basicLink as IGoLink;
            }

            if  (link == null) return false;

            if  (isLabeledLink)
                (link as MaxLabeledLink).Orthogonal = linkinfo.ortho;
            else(link as MaxBasicLink).Orthogonal   = linkinfo.ortho;

            if  (toNode is MaxLoopContainerNode)
            {
                 // Identify specific port for multiport nodes
                 int portIndex = linkinfo.toPort < 1 || linkinfo.toPort > 4? 0: 
                     linkinfo.toPort - 1;

                 toPort = (toNode as MaxLoopContainerNode).ports(portIndex);
            }
            else toPort = toNode.NodePort;

            if  (fromNode is MaxLoopContainerNode)      
                 fromPort =(fromNode as MaxLoopContainerNode).ports(linkinfo.fromPort - 1);
            else fromPort = fromNode.NodePort;

            if  (fromPort == null || toPort == null) return false;
    
            // Set the two nodes which the link connects

            link.FromPort = fromPort;   
            link.ToPort   = toPort; 

            // Now that the link's from and to nodes have been defined, we can 
            // complete the link definition, if necessary loading the link's 
            // choices dropdown and resetting the link label text  

            canvasinfo.canvas.View.RaiseLinkCreated((GoObject)link); 
  
            // This doc.Add can trigger an InvalidCastException somewhere in the Go
            // code, no matter what kind of link we add. We trap and ignore the 
            // exception up the line. Unsure if this is still occurring.
  
            canvasinfo.doc.Add((GoObject)link); 
      
            return true; 
        }


        /// <summary>Extract node's attributes to the supplied info structure</summary>
        private static bool ParseNodeAttributes(XmlNode nodexml, NodeInfo info)
        {
            string nodetype  = Utl.XmlAttr(nodexml, Const.xmlAttrType);
            info.nodeType    = nodetype == null? NodeTypes.None: 
                (NodeTypes)System.Enum.Parse(typeof(NodeTypes), nodetype, true); 

            if (info.nodeType == NodeTypes.None) return false; 
        
            info.nodeName      = Utl.XmlAttr(nodexml, Const.xmlAttrName);
            info.nodeGroup     = Utl.XmlAttr(nodexml, Const.xmlAttrGroup);
            info.nodePackage   = Utl.XmlAttr(nodexml, Const.xmlAttrPath);
            info.nodeClass     = Utl.XmlAttr(nodexml, Const.xmlAttrClass);
            info.nodeID        = Utl.XmlAttrLong(nodexml, Const.xmlAttrID, Const.ErrValNodeID); 
            info.nodeContainer = Utl.XmlAttrLong(nodexml, Const.xmlAttrContainer, 0);

            if (TranslatingNodeIDs)               // If paste operation in progress,
            {                                     // get new node IDs for this node
                info.nodeID        = TranslateNodeID(info.nodeID);
                info.nodeContainer = TranslateNodeID(info.nodeContainer);
            }
  
            info.x = Utl.XmlAttrFloat(nodexml, Const.xmlAttrX, Const.ErrValXY); 
            info.y = Utl.XmlAttrFloat(nodexml, Const.xmlAttrY, Const.ErrValXY); 

            if (info.nodeType == NodeTypes.Variable)
            {   // We force a variable to variables tray if the app file was
                // created prior to the new tray style (serializer version 0.7). 
                if (Const.IsPriorPt7(MaxProjectSerializer.serializedVersionF))          
                    info.x = info.y = 0F;
            
                // Serialized location (0,0) indicates tray variable
                if ((Utl.ftoi(info.x) == 0) && (Utl.ftoi(info.y) == 0))
                    info.isTrayVariable = true;      
            }
                           
            if (Config.ConstrainingCoordinates && !info.isTrayVariable)
            {
                bool isMultiTextNode              // Constrain nodes to positive xy 
                    = info.nodeClass.Equals(Const.NameOfMaxAsyncActionNode) ||
                      info.nodeClass.Equals(Const.NameOfMaxCallNode);

                if (info.x < Config.minNodeXf) 
                    info.x = isMultiTextNode? Config.minNodeXf - Const.MultiTextOffsetX: 
                        Config.minNodeXf;

                if (info.y < Config.minNodeYf) 
                    info.y = isMultiTextNode? Config.minNodeYf - Const.MultiTextOffsetY:
                        Config.minNodeYf; 
            }
   
            if (info.nodeGroup == null   || info.nodeGroup.Length == 0)
                info.nodeGroup = Const.defaultStockToolGroup;

            if (info.nodePackage == null || info.nodePackage.Length == 0) 
                info.nodePackage = Const.stockPackageName;

            info.nodePath = info.nodePackage; 

            switch(info.nodeType)
            {
                case NodeTypes.Loop:
                     info.nodeText  = Utl.XmlAttr(nodexml, Const.xmlAttrText);
                     info.entryPort = Utl.XmlAttrInt(nodexml, Const.xmlAttrEntry, 0);
                     if (info.entryPort > 0) info.entryPort--; // Serialized as one-based
                     info.cx = Utl.XmlAttrFloat (nodexml, Const.xmlAttrWidth, Const.ErrValXY); 
                     info.cy = Utl.XmlAttrFloat (nodexml, Const.xmlAttrHeight,Const.ErrValXY); 
                     break;

                case NodeTypes.Label:
                case NodeTypes.Comment:
                     info.nodeText = Utl.XmlAttr(nodexml, Const.xmlAttrText);
                     break;

                case NodeTypes.Action:
                        
                     if  (info.nodeClass == Const.NameOfMaxAsyncActionNode)
                          info.subtype    = NodeInfo.SubType.AsyncAction;
                     else 
                     if  (info.nodeClass == Const.NameOfMaxCallNode)
                          info.subtype    = NodeInfo.SubType.CallFunction;
                     else // Legacy project not saved with call nodes                                           
                     if  (info.nodeName  == Const.NameOfCallFunction)
                          info.subtype    = NodeInfo.SubType.CallFunction;
                     break;
            }

            MaxStockTools stockTools = MaxStockTools.Instance;

            // Get the appropriate MaxTool for the node. Tools for action and event
            // nodes are specific to a package, so we ask the package for them. 
                        
            switch(info.nodeType)
            {
                case NodeTypes.Start:    info.tool = stockTools.StartTool;    break;
                case NodeTypes.Function: info.tool = stockTools.FunctionTool; break;      
                case NodeTypes.Variable: info.tool = stockTools.VariableTool; break;
                case NodeTypes.Loop:     info.tool = stockTools.LoopTool;     break;
                case NodeTypes.Label:    info.tool = stockTools.LabelTool;    break;
                case NodeTypes.Comment:  info.tool = stockTools.CommentTool;  break;

                default: 

                    string fullyQualifiedName = info.nodePath + Const.dot + info.nodeName;

                    // First check if node is a customCodeTool, since the NodeType of a 
                    // CustomCode action is Action, yet it is represented by a stock tool

                    if (MaxManager.Instance.Packages.IsCustomCodeTool(fullyQualifiedName))
                        info.tool = stockTools.CodeTool; 
                    else
                    {
                        info.tool = MaxManager.Instance.Packages.FindByToolName(fullyQualifiedName,
                             info.nodeType == NodeTypes.Action? 
                                Framework.Satellite.Property.DataTypes.Type.Action:
                                Framework.Satellite.Property.DataTypes.Type.Event);

                        // When a tool is not found, it will be part of a package which is not
                        // present. In order to display the node, we assign a stub action or
                        // event tool, and populate the stub with minimal package info  

                        if (info.tool == null) 
                            info.tool = GetStubTool(info.nodeType, info.nodeName, info.nodePath);
                    }
                    break;
            }

            // Extract any <item> names for the node. Most nodes will not have an
            // <items> section. MaxAsyncActionNode has one, containing the current
            // names of its event handlers. MaxCallNode also has one, containing
            // the name of the called function
                                   
            foreach(XmlNode subnode in nodexml)                                                       
                if (subnode.Name == Const.xmlEltItems)        // <items>
                    foreach(XmlNode itemxml in subnode)        
                        if (itemxml.Name == Const.xmlEltItem) // <item text="xxx" treenode="xxx"> 
                        {   info.nodeItems.Add(Utl.XmlAttr(itemxml, Const.xmlAttrText));
                            info.itemIDs.Add  (Utl.XmlAttr(itemxml, Const.xmlAttrTreeNode));
                        }

            return info.tool != null;
        }


        /// <summary>Get a stub tool and package for use by node whose package is not available</summary>
        public static MaxTool GetStubTool(NodeTypes type, string nodeName, string toolPath)   
        {
            MaxTool tool = null;
            MaxStubPackage stubPackage 
                = new MaxStubPackage(MaxManager.Instance.Packages, toolPath);

            switch(type)
            {
                case NodeTypes.Action:   
                     tool = new MaxActionTool(nodeName, stubPackage);
                     break;

                case NodeTypes.Event: 
                     tool = new MaxEventTool(nodeName, stubPackage);
                     break;             
            }

            return tool;
        }   


        /// <summary>Accumulate node's link properties in our link map</summary>
        private static void GetNodeOutboundLinks(XmlNode nodexml, NodeInfo info)
        {
            foreach(XmlNode linkxml in nodexml)
            {
                if (linkxml.NodeType != XmlNodeType.Element) continue;
                if (linkxml.Name     != Const.xmlEltLinkTo)  continue;
                LinkInfo linkinfo = new LinkInfo();

                linkinfo.toID = Utl.XmlAttrLong(linkxml, Const.xmlAttrID, Const.ErrValNodeID); 
                if  (linkinfo.toID == Const.ErrValNodeID) continue;
                                                // If paste operation in progress,
                if (TranslatingNodeIDs)         // get the new ID for linked node
                    linkinfo.toID = TranslateNodeID(linkinfo.toID);
         
                // Multiport nodes may have "port=n" or "fromport=n"
                linkinfo.fromPort = Utl.XmlAttrInt (linkxml, Const.xmlAttrFromPort, 0);    
                linkinfo.toPort   = Utl.XmlAttrInt (linkxml, Const.xmlAttrPort, 0);        
                linkinfo.label    = Utl.XmlAttr    (linkxml, Const.xmlAttrLabel);
                linkinfo.ortho    = Utl.XmlAttrBool(linkxml, Const.xmlAttrOrthogonal, false);
                string style      = Utl.XmlAttr    (linkxml, Const.xmlAttrStyle);
                try
                {
                    linkinfo.style = (LinkStyles)Enum.Parse(typeof(LinkStyles), style, true);
                }
                catch
                {
                    linkinfo.style = LinkStyles.None;
                }


                info.nodeLinks.Add(linkinfo);
            }
                                        
            if (info.nodeLinks.Count > 0) 
                info.canvasInfo.nodeLinkMap.Add(info.nodeID, info.nodeLinks); 
        }



        /// <summary>Determine if we do not deserialize nodes found on the clipboard</summary>      
        private static bool IsNodeIgnoredDuringCopyPaste(NodeTypes nodeType)
        {
            switch(nodeType)
            {
               case NodeTypes.Annotation: // Annotation is shown temporarily on request 
               case NodeTypes.Start:      // Start node is fixed; not removed during cut or delete
                    return true;
            }

            return false;
        }



        /// <summary>Recreate node's annotation node if any</summary>
        private static void GetNodeAnnotationInfo(XmlNode nodexml, NodeInfo info)
        {
            foreach(XmlNode annotxml in nodexml)
            {
                if (annotxml.NodeType != XmlNodeType.Element) continue;
                if (annotxml.Name     != Const.xmlEltAnnotation) continue;

                info.annotID = Utl.XmlAttrLong(annotxml, Const.xmlAttrID, Const.ErrValNodeID);
                info.annotX  = Utl.XmlAttrFloat(annotxml, Const.xmlAttrX, Const.ErrValXY); 
                info.annotY  = Utl.XmlAttrFloat(annotxml, Const.xmlAttrY, Const.ErrValXY); 
                info.annotText = Utl.XmlAttr(annotxml, Const.xmlAttrText);
                break;
            }
        }



        /// <summary>Deserialize properties of a max node</summary>
        public static bool ParseNodeProperties(IMaxNode maxnode, XmlNode xmlnode)
        {
            if  (maxnode == null) return false;
            bool result = false;
            try
            {
                foreach(XmlNode propsnode in xmlnode)
                {
                    if (propsnode.Name != Const.xmlEltProperties) continue;

                    // The RichTextBox control uses \n for newlines. This is inconsistent 
                    // with the value of System.Environment.NewLine under Windows. 
                    if (propsnode.InnerXml != null && propsnode.InnerXml != String.Empty)
                    {
                        propsnode.InnerXml = propsnode.InnerXml.Replace("\r", string.Empty);
                        propsnode.InnerXml = propsnode.InnerXml.Replace("\n", System.Environment.NewLine);
                    }
                                                
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


        /// <summary>Configure parameters for PlaceNode</summary>
        private static MaxView.NodeArgs SetNodeArgs(NodeInfo info)
        {
            float x = info.x + info.offset.X, y = info.y + info.offset.Y;

            MaxView.NodeArgs args = new MaxView.NodeArgs
                (new PointF(x,y), info.tool, info.nodeID, info.nodeContainer, 
                 info.nodeName, info.nodeItems, info.itemIDs);

            args.entryPort = info.entryPort;

            args.complexType 
                = info.subtype == NodeInfo.SubType.AsyncAction?
                    MaxView.NodeArgs.ComplexType.AsyncAction:
                    info.subtype == NodeInfo.SubType.CallFunction?
                        MaxView.NodeArgs.ComplexType.CallFunction:
                        MaxView.NodeArgs.ComplexType.None;

            return args;
        }


        /// <summary>Determine node paste offset from original xy</summary>
        protected static PointF ComputePasteOffset(CanvasInfo info)
        {
            // If pasting into same canvas as copied from, use standard 32x32 offset
            if (info.canvasID == info.canvas.CanvasID || info.canvasID == 0)       
                return Const.EditPasteNodeOffset;
       
            // If pasting into different canvas, insert at current left/top, plus margin
            float currentLeft = info.canvas.View.DocPosition.X;
            float currentTop  = info.canvas.View.DocPosition.Y;
            float offsetLeft  = currentLeft - info.selleft + Const.EditPasteNodeOffset.X;
            float offsetTop   = currentTop  - info.seltop  + Const.EditPasteNodeOffset.Y;
            return new PointF(offsetLeft, offsetTop);
        }


        /// <summary>Set size of document when observed size is larger than default size</summary>
        private static SizeF GetEffectiveDocumentSize(SizeF defaultSize, RectangleF observedBounds)
        {
            // When deserializing a canvas, GoDiagram does not appear to set the document
            // size based upon the nodes added to the document. We set the size here based
            // upon the observed bounds of the document. We add some margin as well.
            SizeF effectiveSize = defaultSize;
            if (observedBounds.Size.Width   > defaultSize.Width)
                effectiveSize.Width = observedBounds.Size.Width  + 16;
            if (observedBounds.Size.Height  > defaultSize.Height)
                effectiveSize.Height= observedBounds.Size.Height + 12;
            return effectiveSize;
        }


        /// <summary>Return a node ID translation table for an xml clipboard selection</summary>
        public static Hashtable BuildClipboardTranslateTable(XmlNode canvasxml)
        {
            // Each time we paste a selection onto a canvas, we must not only assign
            // new unique IDs to the selected nodes, but we also must change each 
            // reference to those IDs, such as in linkto and container IDs. As the
            // selection is deserialized prior to pasting, the IDs will be replaced
            // using the table we supply here.

            Hashtable xTable = new Hashtable();

            foreach (XmlNode nodexml in canvasxml)               // <canvas>
            {
                if  (nodexml.Name != Const.xmlEltNode) continue; // <node id="nnnnn" ...
                long nodeID = Utl.XmlAttrLong(nodexml, Const.xmlAttrID, Const.ErrValNodeID);
                if  (nodeID == 0) continue;
                long newID  = Const.Instance.NextNodeID; 
                xTable.Add(nodeID, newID);
            }

            return xTable;
        }


        /// <summary>Return the current Edit/Paste translation for this node ID</summary>
        public static long TranslateNodeID(long nodeID)
        {      
            long newID = 0;  

            if (nodeIdTranslateTable != null && nodeIdTranslateTable.Contains(nodeID))        
                newID = (long)nodeIdTranslateTable[nodeID];

            return newID == 0? nodeID: newID;
        }
    

        /// <summary>Canvas deserialization properties</summary>
        public class CanvasInfo
        {
            public MaxCanvas  canvas;
            public GoDocument doc;
            public bool  pasting;
            public long  canvasID;
            public long  startnodeID;  
            public long  treenodeID;
            public long  appnodeID;
            public long  handler4ID;
            public float selleft;
            public float seltop;
            public float variablesLocationY; 
            public RectangleF bounds;

            public Hashtable nodeLinkMap;

            public CanvasInfo()
            {
                nodeLinkMap  = new Hashtable();
            }
        }
  
        /// <summary>Node deserialization properties</summary> 
        public class NodeInfo
        {
            public CanvasInfo canvasInfo;
            public NodeTypes  nodeType;
            public MaxTool tool;
            public long    nodeID;
            public SubType subtype;

            public string nodeName;
            public string nodeGroup;
            public string nodePackage;
            public string nodeClass;
            public string nodePath;
            public string nodeText;
            public string toolName;
            public string annotText;
            public long   annotID;
            public int    entryPort;       
            public float  annotX, annotY;
            public long   nodeContainer;
            public bool   isTrayVariable;

            public float  x, y, cx, cy;
            public PointF offset;
    
            public ArrayList nodeLinks;
            public ArrayList nodeItems;
            public ArrayList itemIDs;

            public enum SubType { None, AsyncAction, CallFunction }

            public void UpdateBounds(RectangleF node)
            {
                RectangleF r = canvasInfo.bounds;
                if (node.X < r.X) r.X = node.X;
                if (node.Y < r.Y) r.Y = node.Y;
                if (node.Right  > r.Right)  r.Width  = r.X + node.Right;
                if (node.Bottom > r.Bottom) r.Height = r.Y + node.Bottom;
                canvasInfo.bounds = r;
            }

            public NodeInfo()
            {
                nodeLinks = new ArrayList();
                nodeItems = new ArrayList();
                itemIDs   = new ArrayList();  
                toolName  = null;
                tool = null;
                annotID = 0;
            }
        }

        /// <summary>Link deserialization properties</summary> 
        public class LinkInfo 
        { 
            public long   toID; 
            public int    fromPort;  // 1-based, containers only
            public int    toPort;    // ditto
            public bool   ortho;
            public string label;
            public LinkStyles style; // style of the link
        }

        public  static bool TranslatingNodeIDs  { get { return nodeIdTranslateTable != null; } }
        private static Hashtable nodeIdTranslateTable;
        public  static Hashtable NodeIdTranslateTable { get { return nodeIdTranslateTable; } }
 
    } // class MaxCanvasSerializer
}     // namespace
