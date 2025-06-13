//
// MaxNodeSerializer.cs
//
using System;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Core.Tool;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
    /// <summary>MaxNode serialization/deserialization helper</summary>
    public sealed class MaxNodeSerializer
    {
        #region singleton
        private static readonly MaxNodeSerializer instance = new MaxNodeSerializer();
        public  static MaxNodeSerializer Instance { get { return instance; }}
        private MaxNodeSerializer() {} 
        #endregion

        /// <summary>Write all common node tag attributes. Presumably no node 
        /// type-specific attributes are present</summary>
        public void WriteCommonAttibutes(IMaxNode node, XmlTextWriter writer)
        {
            WriteCommonAttibutesA(node, writer);
       
            WriteCommonAttibutesB(node, writer);
        }


        /// <summary>Write common node tag attributes appearing prior to node 
        /// type-specific attributes</summary>
        public void WriteCommonAttibutesA(IMaxNode node, XmlTextWriter writer)
        {
            writer.WriteAttributeString(Const.xmlAttrType, node.NodeType.ToString()); 

            writer.WriteAttributeString(Const.xmlAttrID, XmlConvert.ToString(node.NodeID));     
        }


        /// <summary>Write common node tag attributes appearing after the node 
        /// type-specific attributes</summary>
        public void WriteCommonAttibutesB(IMaxNode node, XmlTextWriter writer)
        {
            if (node.Container != 0)              // Node ID of container
                writer.WriteAttributeString(Const.xmlAttrContainer, XmlConvert.ToString(node.Container));

            writer.WriteAttributeString(Const.xmlAttrClass, Utl.StripQualifiers(node.GetType().ToString()));
            writer.WriteAttributeString(Const.xmlAttrGroup, node.GroupName);
    
            writer.WriteAttributeString(Const.xmlAttrPath,  node.Tool.Package.Name);

            GoGroup gonode   = node as GoGroup;
            PointF  location = gonode.Location;
            writer.WriteAttributeString(Const.xmlAttrX, XmlConvert.ToString(location.X));
            writer.WriteAttributeString(Const.xmlAttrY, XmlConvert.ToString(location.Y));

            // Go nodes have their own notion of Location, which may not be upper left.
            // In many cases the Location may in fact be the node's icon's midpoint.
            // We only serialize out the node midpoint x,y if they are different.

            RectangleF rect = Utl.GetNodeIconBounds(node);
            PointF midpoint = Utl.Midpoint(rect);
            int lx = Utl.ftoi(location.X), ly = Utl.ftoi(location.Y);
            int mx = Utl.ftoi(midpoint.X), my = Utl.ftoi(midpoint.Y);
            if (mx != 0 && my != 0 && mx != lx && my != ly) 
            {
                writer.WriteAttributeString(Const.xmlAttrMidX, mx.ToString());
                writer.WriteAttributeString(Const.xmlAttrMidY, my.ToString());
            }
        }


        /// <summary>Write node tag attributes specific to MaxIconicNode</summary>
        public void WriteIconicNodeSpecificAttributes(MaxIconicNode node, XmlTextWriter writer)
        {
            writer.WriteAttributeString(Const.xmlAttrName, node.NodeName);
        }


        /// <summary>Write node tag attributes specific to MaxCommentNode</summary> 
        public void WriteCommentNodeSpecificAttributes(MaxCommentNode node, XmlTextWriter writer)
        {
            writer.WriteAttributeString(Const.xmlAttrText, node.Label.Text);
        }


        /// <summary>Write node tag attributes specific to MaxAnnotationNode</summary> 
        public void WriteAnnotationNodeSpecificAttributes(MaxAnnotationNode node, XmlTextWriter writer)
        {
            writer.WriteAttributeString(Const.xmlAttrText, node.Label.Text);
        }


        /// <summary>Write node tag attributes specific to MaxLabelNode</summary> 
        public void WriteLabelNodeSpecificAttributes(MaxLabelNode node, XmlTextWriter writer)
        {
            writer.WriteAttributeString(Const.xmlAttrText, node.Label.Text);
        }


        /// <summary>Write node tag attributes specific to MaxLoopContainerNode</summary>
        public void WriteLoopNodeSpecificAttributes(GoObject node, XmlTextWriter writer)
        {
            MaxLoopContainerNode loop = node as MaxLoopContainerNode;
            writer.WriteAttributeString(Const.xmlAttrName, loop.NodeName);
            string text = loop != null? loop.Label.Text: Const.emptystr;
            writer.WriteAttributeString(Const.xmlAttrText, text);
            writer.WriteAttributeString(Const.xmlAttrWidth, XmlConvert.ToString(node.Bounds.Width));
            writer.WriteAttributeString(Const.xmlAttrHeight,XmlConvert.ToString(node.Bounds.Height));
            if (loop == null) return;
            writer.WriteAttributeString(Const.xmlAttrEntry, XmlConvert.ToString(loop.EntryPort+1));
            #if(false)
            writer.WriteAttributeString(Const.xmlAttrExit,  XmlConvert.ToString(loop.ExitPort+1));
            #endif
        }


        /// <summary>Write node tag attributes specific to MaxIconicMultiTextNode</summary>
        public void WriteIconicMultiTextNodeSpecificAttributes
        ( MaxIconicMultiTextNode node, XmlTextWriter writer)
        {
            writer.WriteAttributeString(Const.xmlAttrName, node.NodeName);
        }


        /// <summary>Write list items from supplied MaxIconicMultiTextNode</summary>
        public void WriteIconicMultiTextNodeItems
        ( MaxIconicMultiTextNode.ChildSubnode node, XmlTextWriter writer)
        {        
            writer.WriteStartElement(Const.xmlEltItems);    // <items>
            writer.WriteAttributeString(Const.xmlAttrCount, XmlConvert.ToString(node.Count));
      
            foreach(object x in node)
            {
                MaxIconicMultiTextNode.ChildSubnodeLabel entry = x as MaxIconicMultiTextNode.ChildSubnodeLabel;
                if (entry == null) continue;

                writer.WriteStartElement(Const.xmlEltItem);   // <item text="x" treenode="n">
                writer.WriteAttributeString(Const.xmlAttrText, entry.Text);

                // List item's Tag contains an app tree node for which we store the node ID
                MaxAppTreeNodeEVxEH treenode = entry.Tag as MaxAppTreeNodeEVxEH;
                if (treenode != null)
                    writer.WriteAttributeString(Const.xmlAttrTreeNode, XmlConvert.ToString(treenode.NodeID));

                writer.WriteEndElement(); // </item>
            }

            writer.WriteEndElement();   // </items>
        }


        /// <summary>Write each outbound link for supplied node and port</summary>
        public void WriteLinks(IMaxNode node, IGoPort port, XmlTextWriter writer)
        {                                        
            foreach(IGoLink ilink in port.Links)                                                       
                    WriteLinkToTag(node, ilink, port, writer);       
        }

        /// <summary>Write an XML linkto tag entry</summary
        private void WriteLinkToTag(IMaxNode node, IGoLink ilink, IGoPort port, XmlTextWriter writer)
        {
            IMaxLink maxlink = ilink as IMaxLink;     
            IMaxNode toNode  = ilink.ToNode as IMaxNode;
            if  (maxlink == null || toNode == null || toNode == node) return;

            writer.WriteStartElement(Const.xmlEltLinkTo); // <linkto>
            writer.WriteAttributeString(Const.xmlAttrID, XmlConvert.ToString(toNode.NodeID));

            if (toNode is MaxLoopContainerNode)
            {
                // At such time as we have more than one type of container node, we'll  
                // put GetPortNumber(port) in IMaxNode, and generalize this code section
                IGoPort toPort = ilink.GetOtherPort(port);
                int  whichport = toPort == null? 1: 
                    (toNode as MaxLoopContainerNode).GetPortNumber(toPort);
                writer.WriteAttributeString(Const.xmlAttrPort, whichport.ToString());
            }

            if (node is MaxLoopContainerNode)
            {         
                int whichport = (node as MaxLoopContainerNode).GetPortNumber(port);
                writer.WriteAttributeString(Const.xmlAttrFromPort, whichport.ToString());                 
            } 

            writer.WriteAttributeString(Const.xmlAttrType,  maxlink.LinkType.ToString());
            writer.WriteAttributeString(Const.xmlAttrStyle, maxlink.LinkStyle.ToString());
            if (maxlink.IsOrthogonal)
                writer.WriteAttributeString(Const.xmlAttrOrthogonal, Const.xmlValBoolTrue);

            string linklabel = maxlink is MaxLabeledLink? 
                ((MaxActionLinkLabel)((MaxLabeledLink)maxlink).MidLabel).Text: null;

            if (linklabel != null)
                writer.WriteAttributeString(Const.xmlAttrLabel, linklabel); 

            writer.WriteEndElement();  // </linkto>
        }


        /// <summary>Write each outbound link for a loop container node</summary
        public void WriteContainerLinks(MaxLoopContainerNode loop, XmlTextWriter writer)
        {
            for(int i=0; i < 4; i++)
            {
                MaxLoopContainerNode.CustomPort port = loop.ports(i);

                foreach(IGoLink ilink in port.DestinationLinks)
                        this.WriteLinkToTag(loop, ilink, port, writer);  
            }
        }


        /// <summary>Write the node's annotation if any</summary
        public void WriteAnnotation(IMaxNode node, XmlTextWriter writer)
        {
            if (node.Annotation == null) return;
            MaxAnnotationNode annotation = node.Annotation as MaxAnnotationNode;
            if (annotation == null || annotation.Text == null) return;
            writer.WriteStartElement(Const.xmlEltAnnotation); // <annot>
            writer.WriteAttributeString(Const.xmlAttrID, XmlConvert.ToString(annotation.NodeID));  
            PointF location = annotation.Location;
            writer.WriteAttributeString(Const.xmlAttrX, XmlConvert.ToString(location.X));
            writer.WriteAttributeString(Const.xmlAttrY, XmlConvert.ToString(location.Y));
            writer.WriteAttributeString(Const.xmlAttrText, annotation.Text); 
            writer.WriteEndElement();  // </annot>
        }


        /// <summary>Write all property records for supplied node</summary>
        public void WriteProperties(IMaxNode node, XmlTextWriter writer)
        {
            PmProxy.PropertiesManager.SerializeProperty(node.PmObjectType, node.MaxProperties, writer);
        } 
 
    }  // class MaxNodeSerializer
}    // namespace
