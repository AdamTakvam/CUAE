using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using Metreos.AxlSoap413;

namespace Metreos.Common.Ccem
{
	/// <summary>  
	///     An abstraction layer between AXL response structs and data pertinent to
	///     the CCEM process
	/// </summary>
	[Serializable()]
	public class PhoneInfo
	{
        // User Info acts an adapter from the cumbersome responses from AXL SOAP
        // into straightforward primitives
        public PhoneInfo()
		{
            Lines = new LineInfo[0];
            csses = new SortedList();
            partitions = new SortedList();
        }

        // Used to store css name-id maps, stored as keys
        [XmlIgnore()]
        public SortedList csses;
        [XmlIgnore()]
        public SortedList partitions;

        public string DeviceCSSName;
        public string DeviceId;
       
        public LineInfo[] Lines;

        /// <summary>
        ///     Adapter for 413 AXL Phone
        /// </summary>
        public void SetDevice(Metreos.AxlSoap413.getPhoneResponse phone)
        {
            if(phone != null && phone.@return != null && phone.@return.device != null)
            {
                DeviceCSSName = phone.@return.device.Item2 as string;
                DeviceId = phone.@return.device.uuid;
            }

            // Add line info bound to device
            if(phone.@return.device.lines != null && phone.@return.device.lines.Items != null)
            {
                ArrayList linesList = new ArrayList();

                foreach(XLine line in phone.@return.device.lines.Items)
                {
                    LineInfo lineInfo = new LineInfo(true);
                    lineInfo.DisplayName = line.display;
                    lineInfo.E164Mask = line.e164Mask;
                    lineInfo.Label = line.label;
                    lineInfo.Index = line.index;
                    lineInfo.LineId = line.Item.uuid;
                    linesList.Add(lineInfo);
                }

                Lines = new LineInfo[linesList.Count];
                linesList.CopyTo(Lines);
            }
        }

        public void AddLineInfo(Metreos.AxlSoap413.getLineResponse line)
        {
            // Hunt through existing lines to see if this one is going to augment an
            // already existing line we know about due to an added device

            if(Lines != null && line != null && line.@return != null && line.@return.directoryNumber != null)
            {
                LineInfo foundKnown = null;
                foreach(LineInfo knownLine in Lines)
                {
                    if(knownLine.LineId == line.@return.directoryNumber.uuid)
                    {
                        foundKnown = knownLine;
                        break;
                    }
                }

                if(foundKnown != null)
                {
                    // Set CallingSearchSpace
                    if(line.@return.directoryNumber.Item3 is XCallingSearchSpace)
                    {
                        foundKnown.css = AddCssId((line.@return.directoryNumber.Item3 as XCallingSearchSpace).uuid);
                    }
                    else if(line.@return.directoryNumber.Item3 is String)
                    {
                        foundKnown.css = AddCssId(line.@return.directoryNumber.Item3 as string);
                    }
                    else
                    {
                        foundKnown.css = AddCssId(null);
                    }

                    // Set Partition
                    if(line.@return.directoryNumber.Item is XRoutePartition)
                    {
                        XRoutePartition partition = line.@return.directoryNumber.Item as XRoutePartition;
                        foundKnown.Partition = AddPartitionId(partition.uuid);
                    }
                    else if(line.@return.directoryNumber.Item is String)
                    {
                        foundKnown.Partition = AddPartitionId(line.@return.directoryNumber.Item as string);
                    }
                    else
                    {
                        foundKnown.Partition = AddPartitionId(null);
                    }

                    // Build CFAs
                    foundKnown.All = BuildCfaInfo(line.@return.directoryNumber.callForwardAll);
                    foundKnown.BusyExt = BuildCfaInfo(line.@return.directoryNumber.callForwardBusy);
                    foundKnown.BusyInt = BuildCfaInfo(line.@return.directoryNumber.callForwardBusyInt);
                    foundKnown.NoAnswerExt = BuildCfaInfo(line.@return.directoryNumber.callForwardNoAnswer);
                    foundKnown.NoAnswerInt = BuildCfaInfo(line.@return.directoryNumber.callForwardNoAnswerInt);
                    foundKnown.NoCoverageExt = BuildCfaInfo(line.@return.directoryNumber.callForwardNoCoverage);
                    foundKnown.NoCoverageInt = BuildCfaInfo(line.@return.directoryNumber.callForwardNoCoverageInt);
                    foundKnown.Pattern = line.@return.directoryNumber.pattern;
                }
                else
                {
                    // Append line to end
                }
            }
        }

        private CallForwardInfo BuildCfaInfo(Metreos.AxlSoap413.XCallForwardInfo cf)
        {
            CallForwardInfo info = new CallForwardInfo();
            if(cf != null)
            {
                info.Destination = cf.destination;
                info.css = AddCssId(cf.Item != null ? (cf.Item as XCallingSearchSpace).uuid : null);
                info.Duration = cf.duration;
                info.ForwardToVoiceMail = cf.forwardToVoiceMail;
            }

            return info;
        }

        public CSS AddCssName(string name)
        {
            CSS css;
            if(name == null)
            {
                css = CSS.NoneCss;
            }
            else if(csses.ContainsKey(name))
            {
                css = csses[name] as CSS;
            }
            else
            {
                css = new CSS();
                css.Name = name;
                csses[name] = css; 
            }
            return css;
        }

        public CSS AddCssId(string id)
        {
            CSS css;
            if(id == null)
            {
                css = CSS.NoneCss;
            }
            else if(csses.ContainsKey(id))
            {
                css = csses[id] as CSS;
            }
            else
            {
                css = new CSS();
                css.ID = id;
                csses[id] = css; 
            }
            return css;
        }
        
        public Partition AddPartitionName(string name)
        {
            Partition partition;
            if(name == null)
            {
                partition = Partition.NonePartition;
            }
            else  if(partitions.ContainsKey(name))
            {
                partition = partitions[name] as Partition;
            }
            else
            {
                partition = new Partition();
                partition.Name = name;
                partitions[name] = partition; 
            }
            return partition;
        }

        public Partition AddPartitionId(string id)
        {
            Partition partition;
            if(id == null)
            {
                partition = Partition.NonePartition;
            }
            else if(partitions.ContainsKey(id))
            {
                partition = partitions[id] as Partition;
            }
            else
            {
                partition = new Partition();
                partition.ID = id;
                partitions[id] = partition; 
            }
            return partition;
        }

        public void UpdateCSS(getCSSResponse addCss)
        {
            string cssName = addCss.@return.callingSearchSpace.name;
            string cssId = addCss.@return.callingSearchSpace.uuid;

            CSS css = new CSS();
            css.ID = cssId;
            css.Name = cssName;

            if(csses.ContainsKey(cssName))
            {
                css = csses[cssName] as CSS; // on-purpose overwrite of css var
                css.ID = cssId;
            }
            else
            {
                csses[cssName] = css;
            }

            if(csses.ContainsKey(cssId))
            {
                css = csses[cssId] as CSS;
                css.Name = cssName;
            }
            else
            {
                csses[cssId] = css;
            }
        }

        public void UpdatePartition(getRoutePartitionResponse addPartition)
        {
            string partitionName = addPartition.@return.routePartition.name;
            string partitionId = addPartition.@return.routePartition.uuid;

            Partition partition = new Partition();
            partition.ID = partitionId;
            partition.Name = partitionName;

            if(partitions.ContainsKey(partitionName))
            {
                partition = csses[partitionName] as Partition; // on-purpose overwrite of partition var
                partition.ID = partitionId;
            }
            else
            {
                partitions[partitionId] = partition;
            }

            if(partitions.ContainsKey(partitionId))
            {
                partition = partitions[partitionId] as Partition;
                partition.Name = partitionName;
            }
            else
            {
                partitions[partitionId] = partition;
            }
        }    
	}

    [Serializable()]
    public class LineInfo
    {
        public LineInfo() { }
        public LineInfo(bool fromDevice){ this.FromDevice = fromDevice; }

        [XmlIgnore()]
        public bool FromDevice;
        public string LineId;
        public string Pattern;
        public string Label;
        public string DisplayName;
        public string E164Mask;
        public CSS css;
        public Partition Partition;
        public string Index;

        public CallForwardInfo All;
        public CallForwardInfo BusyInt;
        public CallForwardInfo BusyExt;
        public CallForwardInfo NoAnswerInt;
        public CallForwardInfo NoAnswerExt;
        public CallForwardInfo NoCoverageInt;
        public CallForwardInfo NoCoverageExt;
        
    }

    [Serializable()]
    public class CallForwardInfo
    {
        public bool   ForwardToVoiceMail;
        public CSS css;
        public string Destination;
        public string Duration;
    }
    
    [Serializable()]
    public class CSS
    {
        public CSS() { }
        public string Name;
        public string ID;

        public static CSS NoneCss = new CSS();
    }

    [Serializable()]
    public class Partition
    {
        public Partition() { }
        public string Name;
        public string ID;

        public static Partition NonePartition = new Partition();
    }
}
