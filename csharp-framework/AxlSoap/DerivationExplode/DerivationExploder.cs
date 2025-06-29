using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Collections;
using System.Xml.Serialization;
using System.Xml.XPath;


namespace DerivationExplode
{
	public class DerivationExploder
	{
        private XmlDocument doc;
        private XmlNode rootNode;
        private Hashtable treeNodes;
		private Hashtable skipTreeNodes;
		private string[] specificClassesToReplace;
		private bool specificMode;

		public DerivationExploder()
        {
            treeNodes = new Hashtable();
			skipTreeNodes = new Hashtable();
            doc = new XmlDocument();
			specificMode = false;
        }

		/// <summary>
		///		Takes an xml file, builds up all the derivation chain, 
		///		and 'explodes' the derived classes so that they are still 
		///		the same XML 'on-the-wire', but no derivation is used.
		/// </summary>
		/// <param name="input">input file path</param>
		/// <param name="output">output file path</param>
		/// <param name="specificClassesToReplace">if not null and count > 0, only the specified type names will be replaced</param>
        public void ExplodeAndSave(string input, string output, string[] specificClassesToReplace)
        {
			this.specificClassesToReplace = specificClassesToReplace;
			if(specificClassesToReplace != null && specificClassesToReplace.Length > 0)
			{
				specificMode = true;
			}

            if(!File.Exists(input))
            {
                Console.WriteLine("Unable to find input file");
				return;
            }
            
            doc.Load(input);
            rootNode = doc.ChildNodes[3];

            // Figure out who derives from what, and what derivations there are for a given class
            BuildDerivationChain();

			if(specificMode)
			{
				ExplodeSpecific();
			}
			else
			{
				// To get the content needed for replacement for each derived class, 
				// we need the contents of the parent.  So you want to start at the bottom
				// of the inherentience chain, and move your way up, replacing content as you go
				ExplodeAll();
			}

			RemoveAllAnnotations();

            // Save!
            doc.Save(output);
            
        }

        private void BuildDerivationChain()
        {
            // child::extension[attribute::base = '*']

			XPathNavigator navi = doc.CreateNavigator();
			XPathNodeIterator iterator = navi.Select("//*[@base]");

            while(iterator.MoveNext())
            {
				XmlNode node = (iterator.Current as IHasXmlNode).GetNode();

				if(!node.Attributes["base"].Value.StartsWith("axl")) continue; // Should be a part of xpath
				if(node.LocalName != "extension") continue; // should be part of xpath

                // Find encompassing node
                XmlNode currentNode = node.ParentNode.ParentNode;

				string currentName = null;
				if(currentNode.Attributes["name"] != null) // not all complexType nodes have a name... not part of derivation chain
				{
					currentName = currentNode.Attributes["name"].Value;
				}

				if(currentName == "XPhone")
				{
					Console.Write("");
				}

				string parentName = node.Attributes["base"].Value.Split(new char[] {':'})[1];

                NodeData currentNodeData = null;

				if(currentName != null && treeNodes.Contains(currentName))
				{
					currentNodeData = treeNodes[currentName] as NodeData;
					currentNodeData.Contents = currentNode;
					currentNodeData.ParentName = parentName;
				}
				else
				{
					currentNodeData             = new NodeData();
					currentNodeData.Contents    = currentNode;
					currentNodeData.Name        = currentName;   
					currentNodeData.ParentName  = parentName;
					if(currentName != null)
					{
						treeNodes[currentName]      = currentNodeData;
					}
				}

                if(treeNodes.Contains(parentName))
                {
                    NodeData parentNode = treeNodes[parentName] as NodeData;
                    currentNodeData.Parent = parentNode;
                    parentNode.Children.Add(currentNodeData);
                }
                else
                {
                    NodeData parentNode         = new NodeData();
                    parentNode.Name             = parentName;
                    parentNode.Children.Add(currentNodeData);
                    currentNodeData.Parent      = parentNode;
					currentNodeData.ParentName  = parentNode.Name;
                    treeNodes[parentNode.Name]  = parentNode;
                }
            }

            // Due to our xpath search, We would not have grabbed any 
            // nodes that do not extend anything (the ultimate base class)
            // So let's iterate through all nodes with parent == null, and grab
            // this last run of nodes to complete our chain
			ArrayList toRemove = new ArrayList();
            foreach(NodeData node in treeNodes.Values)
            {
				if(node.Parent == null)
				{
					// If we haven't found their parent yet, they are base class

					// build query for finding base class
					// child::complexType[attribute::name = 'parentName']

					XPathNavigator navi2 = doc.CreateNavigator();
					XPathNodeIterator iterator2 = navi.Select("//*[@name = '" + node.Name + "']");

					Debug.Assert(iterator2.Count < 2, "Multiple matches for name attribute of value " + node.Name);
					//Debug.Assert(iterator2.Count > 0, "No matches for name attribute of value " + node.Name);

					
					if(iterator2.Count == 0)
					{
						// Clean up this node from derivation chain
						skipTreeNodes[node.Name] = node;
						continue;
					}

					iterator2.MoveNext();
					XmlNode contentNode = (iterator2.Current as IHasXmlNode).GetNode();
					node.Contents = contentNode;
				}
            }

			foreach(NodeData node in skipTreeNodes.Values)
			{
				RemoveNode(node);
			}
        }

		private void RemoveNode(NodeData node)
		{
			if(treeNodes.Contains(node.Name))
			{
				treeNodes.Remove(node.Name);

				foreach(NodeData child in node.Children)
				{
					if(child.Name != null)
					{
						treeNodes.Remove(child.Name);
					}
				}
			}
		}

		private void ExplodeSpecific()
		{
			
			bool foundMoreToDerive = false;

			do
			{
				foundMoreToDerive = false;

				foreach(string specificClass in specificClassesToReplace)
				{
					XPathNavigator navi = doc.CreateNavigator();
					XPathNodeIterator iterator = navi.Select("//*[@base]");

					ArrayList list = new ArrayList();
		
					while(iterator.MoveNext())
					{
						list.Add((iterator.Current as IHasXmlNode).GetNode());
					}		


					bool foundIt = false;
					foreach(XmlNode extensionNode in list)
					{
						if(!extensionNode.Attributes["base"].Value.StartsWith("axl")) continue; // Should be built into xpath query
						if(extensionNode.LocalName != "extension") continue; // should be built into xpath query


						// ---Everywhere you find an extended node matching the specified name
						// 1) Hold onto the node -- it's content we want to preserve (they extends the base class)
						// 2) Back up two nodes--should always be complexType node -- we want to then
						//    iterate through it's children until we find complexContent.   We remove that node altogether--it'll
						//    be replaced by derived content and base content
						// 3) Pass extended content from step 1, base class NodeData, and complexType node into exploder function

		
						// 1
						// done... the extensionNode is good to go...


						// 2

						XmlAttribute attr = extensionNode.Attributes["base"];
						string parentName = null;
						if(attr != null)
						{
							parentName = attr.Value.Split(new char[] {':'})[1];
						}


						string currentName = null;
						Debug.Assert(extensionNode.ParentNode.ParentNode.LocalName == "complexType", "ComplexType node not two levels up from extended node: " + extensionNode.ParentNode.ParentNode.LocalName);
						XmlNode complexType = extensionNode.ParentNode.ParentNode;
						XmlAttribute nameAttr = complexType.Attributes["name"];

						if(nameAttr != null)
						{
							currentName = nameAttr.Value;
							if(String.Compare(currentName, specificClass, true) == 0)
							{
								// Found the node that we need to explode
								foundIt = true;
								foundMoreToDerive = true;
							}
						}

						if(foundIt)
						{
							XmlNode contentNode = null;
							foreach(XmlNode childNode in complexType.ChildNodes)
							{
								if(childNode.LocalName == "complexContent")
								{
									contentNode = childNode;
								}
								else if(childNode.LocalName == "simpleContent")
								{
									continue;
								}
							}

							Debug.Assert(contentNode != null, "Unable to find complexContent or simpleContent!");
							complexType.RemoveChild(contentNode);

			
							//NodeData nodeData = treeNodes[complexType.Attributes["name"].Value] as NodeData;
							NodeData baseNodeData = treeNodes[parentName] as NodeData;

							//3
							Explode(extensionNode, baseNodeData.Contents, complexType, true);

							break;

						}
					}
				}
			}
			while(foundMoreToDerive);
		}

        private void ExplodeAll()
        {
			int procCount = 0;
			int skipCount = 0;
			bool foundExtension = false;

			bool includeExtendedContent = true;

			do
			{
				foundExtension = false;
				procCount = 0;
				skipCount = 0;

				XPathNavigator navi = doc.CreateNavigator();
				XPathNodeIterator iterator = navi.Select("//*[@base]");

				ArrayList list = new ArrayList();

				while(iterator.MoveNext())
				{
					list.Add((iterator.Current as IHasXmlNode).GetNode());
				}

				foreach(XmlNode extensionNode in list)
				{
					//XmlNode extensionNode = (iterator.Current as IHasXmlNode).GetNode();

					if(!extensionNode.Attributes["base"].Value.StartsWith("axl")) continue; // Should be built into xpath query
					if(extensionNode.LocalName != "extension") continue; // should be built into xpath query

					foundExtension = true;
					procCount++;

					// ---Everywhere you find an extended node,
					// 1) Hold onto the node -- it's content we want to preserve (they extends the base class)
					// 2) Back up two nodes--should always be complexType node -- we want to then
					//    iterate through it's children until we find complexContent.   We remove that node altogether--it'll
					//    be replaced by derived content and base content
					// 3) Pass extended content from step 1, base class NodeData, and complexType node into exploder function

			
					// 1
					// done... the extensionNode is good to go...


					// 2

					XmlAttribute attr = extensionNode.Attributes["base"];
					string parentName = null;
					if(attr != null)
					{
						parentName = attr.Value.Split(new char[] {':'})[1];
					}

					if(skipTreeNodes.Contains(parentName))
					{
						skipCount++;
						continue;
					}

					Debug.Assert(extensionNode.ParentNode.ParentNode.LocalName == "complexType", "ComplexType node not two levels up from extended node: " + extensionNode.ParentNode.ParentNode.LocalName);
					XmlNode complexType = extensionNode.ParentNode.ParentNode;
					XmlNode contentNode = null;
					bool skip = false;
					foreach(XmlNode childNode in complexType.ChildNodes)
					{
						if(childNode.LocalName == "complexContent")
						{
							contentNode = childNode;
							break;
						}
						else if(childNode.LocalName == "simpleContent")
						{
							skip = true;
							break;
						}
					}

					if(skip)
					{
						skipCount++;
						continue;
					}

					Debug.Assert(contentNode != null, "Unable to find complexContent or simpleContent!");
					complexType.RemoveChild(contentNode);

					
					//NodeData nodeData = treeNodes[complexType.Attributes["name"].Value] as NodeData;
					NodeData baseNodeData = treeNodes[parentName] as NodeData;

					//3
					Explode(extensionNode, baseNodeData.Contents, complexType, includeExtendedContent);
				}

				includeExtendedContent = false; // Only 1st time through
			}
			while(foundExtension && procCount > skipCount);
        }

		private void Explode(XmlNode extensionNode, XmlNode baseNode, XmlNode complexType, bool includeExtendedContent)
		{
			// Stick baseNode child nodes (the real content) above the extendedContent child nodes (the derived contents)
			// As we iterate through the baseNodes, if we find an xsd:attribute, it goes at the end of the extended Content
			
			if(includeExtendedContent == includeExtendedContent)
			{
				// Set up the complexType node with the extended content first
				foreach(XmlNode individualExtensionNode in extensionNode.ChildNodes)
				{
					complexType.AppendChild(individualExtensionNode.CloneNode(true));
				}
			}

			foreach(XmlNode individualBaseNode in baseNode.ChildNodes)
			{
				if(individualBaseNode.LocalName != "attribute")
				{
					if(complexType.ChildNodes.Count == 0)
					{
						complexType.AppendChild(individualBaseNode.CloneNode(true));
					}
					else
					{
						complexType.InsertBefore(individualBaseNode.CloneNode(true), complexType.ChildNodes[0]);
					}
				}
				else
				{
					complexType.AppendChild(individualBaseNode.CloneNode(true));
				}
			}	
	
			// Remove annotations
			ArrayList annots = new ArrayList(); 
			foreach(XmlNode contentNode in complexType.ChildNodes)
			{
				if(contentNode.LocalName == "annotation")
				{
					annots.Add(contentNode);
				}
			}

			foreach(XmlNode annotNode in annots)
			{
				complexType.RemoveChild(annotNode);
			}

			// Review top level nodes for bad xsd logic
			// For instance, if two xsd:sequence tags are next to each other (siblings), 
			// they should be merged into one.

			ArrayList flowNodes = new ArrayList(); // sequence, centent
			foreach(XmlNode contentNode in complexType.ChildNodes)
			{
				if(contentNode.LocalName == "sequence" || contentNode.LocalName == "choice")
				{
					flowNodes.Add(contentNode);
				}
			}

			XmlNode firstSequenceContent = null;
			ArrayList sequenceContent = new ArrayList();
			if(flowNodes.Count > 1)
			{
				// Create new parent to contain others
				firstSequenceContent = doc.CreateNode(XmlNodeType.Element, "xsd", "sequence", "http://www.w3.org/2001/XMLSchema");

				// Grab all child nodes of sequences (past 1st one), bundle up into one list
				for(int i = 0; i < flowNodes.Count; i++)
				{
					XmlNode flowNode = flowNodes[i] as XmlNode;

					if(flowNode.LocalName == "sequence")
					{
						foreach(XmlNode contentNode in flowNode.ChildNodes)
						{
							sequenceContent.Add(contentNode.CloneNode(true));
						}
					}
					else // if flowNode.LocalName == "choice"
					{
						sequenceContent.Add(flowNode.CloneNode(true));
					}
				}

				// Remove sequences (past 1st one)
				for(int i = 0; i < flowNodes.Count; i++)
				{
					XmlNode sequenceNode = flowNodes[i] as XmlNode;
					complexType.RemoveChild(sequenceNode);
				}

				XmlNode firstAttrNode = null;
				// Find 1st attribute node
				foreach(XmlNode node in complexType.ChildNodes)
				{
					if(node.LocalName == "attribute")
					{
						firstAttrNode = node;
						break;
					}
				}

				if(firstAttrNode != null)
				{
					complexType.InsertBefore(firstSequenceContent, firstAttrNode);
				}
				else
				{
					complexType.AppendChild(firstSequenceContent);
				}

				// Add content from other sequence nodes to 1st sequence
				foreach(XmlNode contentNode in sequenceContent)
				{
					firstSequenceContent.AppendChild(contentNode);
				}

			}
	
			

			// Do a final sweep to get attributes to end of content
		}

		private void RemoveAllAnnotations()
		{
			XPathNavigator navi = doc.CreateNavigator();
			XPathNodeIterator iterator = navi.Select("//*");

			ArrayList annots = new ArrayList();
			while(iterator.MoveNext())
			{
				XmlNode node = (iterator.Current as IHasXmlNode).GetNode();

				
				if(node.LocalName == "annotation")
				{
					annots.Add(node);
				}
			}

			foreach(XmlNode annot in annots)
			{
				annot.ParentNode.RemoveChild(annot);
			}
		}
	}

    public class NodeData
    {
        public string Name;
        public string ParentName;
        public NodeData Parent;
        public ArrayList Children;
        public XmlNode Contents;

        public NodeData()
        {
            Children = new ArrayList();
        }
    }
}
