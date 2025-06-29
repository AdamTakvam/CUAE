using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.DocGen
{
    /// <summary>
    ///     Just an attempt to make it somewhat logical
    /// </summary>
    public abstract class Consts
    {
        public abstract class Id
        {
            public const string ChapterTitleId = "packagedisplayname";
            public const string ChapterSubtitleId = "packagename";
        }

        public abstract class Action
        {
            public abstract class Role
            {
                public const string Actions = "actions";
                public const string Action = "action";
                public const string ActionTitle = "actiondisplayname";
                public const string ActionSubtitle = "actionname";
                public const string ActionDescription = "description";
                public const string Branches = "branch";
                public const string Remarks = "remarks";
            }

            public abstract class Content
            {
                public const string Actions = "Actions";
                public const string ActionDesc = "Description";
                public const string BranchConditions = "Branch Conditions";
                public const string Remarks = "Remarks";
            }

            public abstract class ActionParamTable
            {
                public const string RowHeader = "rowheader";
                public const string RowHeaderValue = "firstcol";
                public const int NumCols = 4;
                public const string DisplayNameColumn = "Parameter Display Name";
                public const string DataTypeColumn = "Data Type";
                public const string DefaultColumn = "Default";
                public const string DescriptionColumn = "Description";

                public abstract class Content
                {
                    public const string Title = "Action Parameters";
                    public const string DisplayNameColumn = "Parameter Display Name";
                    public const string DataTypeColumn = ".NET Type";
                    public const string DefaultColumn = "Default";
                    public const string DescriptionColumn = "Description";
                }

                public abstract class Role
                {
                    public const string ActionParameters = "actionparams";
                    public const string FirstRowEntry = "rowhead";
                    public const string RequiredParameter = "req";
                }
            }

            public abstract class ResultDataTable
            {
                public const string RowHeader = "rowheader";
                public const string RowHeaderValue = "firstcol";
                public const int NumCols = 3;
                public const string DisplayNameColumn = "Parameter Display Name";
                public const string DataTypeColumn = "Data Type";
                public const string DescriptionColumn = "Description";

                public abstract class Content
                {
                    public const string Title = "Result Data";
                    public const string DisplayNameColumn = "Parameter Display Name";
                    public const string DataTypeColumn = ".NET Type";
                    public const string DescriptionColumn = "Description";
                }

                public abstract class Role
                {
                    public const string ResultDataParameters = "resultdata";
                }
            }

            public abstract class Properties
            {
                public abstract class Content
                {
                    public const string Properties = "Properties";
                    public const string ActionClass = "Action Class";
                    public const string ActionType = "Action Type";
                    public const string AsyncCallbacks = "Asynchronous Callback";
                    public const string Final = "Final";
                }

                public abstract class Role
                {
                    public const string Properties = "properties";
                    public const string ActionClass = "actionclass";
                    public const string ActionType = "actiontype";
                    public const string AsyncCallbacks = "asynccallbacks";
                    public const string AllowCustomParams = "allowcustomparams";
                    public const string Final = "final";
                }
            }

            public abstract class Dependencies
            {
                public abstract class Content
                {
                    public const string Dependencies = "Dependencies";
                    public const string FrameworkVersion = "CUAE Framework Version";
                    public const string ProviderVersion = "Providers";
                }

                public abstract class Role
                {
                    public const string Dependencies = "dependencies";
                    public const string FrameworkVersion = "frameworkver";
                    public const string ProviderVersion = "providerver";
                }
            }
        }

        public abstract class Event
        {
            public abstract class Role
            {
                public const string Events = "events";
                public const string Event = "event";
                public const string EventTitle = "eventdisplayname";
                public const string EventSubtitle = "eventname";
                public const string EventDescription = "description";
                public const string EventParameters = "eventparams";
                public const string Remarks = "remarks";
            }

            public abstract class Content
            {
                public const string Events = "Events";
                public const string EventDesc = "Description";
                public const string Remarks = "Remarks";
            }

            public abstract class EventParamTable
            {
                public const string RowHeader = "rowheader";
                public const string RowHeaderValue = "firstcol";
                public const int NumCols = 3;
                public const string DisplayNameColumn = "Parameter Display Name";
                public const string DataTypeColumn = "Data Type";
                public const string DescriptionColumn = "Description";

                public abstract class Content
                {
                    public const string Title = "Event Parameters";
                    public const string DisplayNameColumn = "Parameter Display Name";
                    public const string DataTypeColumn = ".NET Type";
                    public const string DescriptionColumn = "Description";
                }

                public abstract class Role
                {
                    public const string EventParameters = "eventparams";
                    public const string FirstRowEntry = "rowhead";
                    public const string RequiredParameter = "guaranteed";
                }
            }

            public abstract class Properties
            {
                public abstract class Content
                {
                    public const string Properties = "Properties";
                    public const string EventType = "Event Type";
                    //public const string Expects = "Expects";
                }

                public abstract class Role
                {
                    public const string Properties = "properties";
                    public const string EventType = "eventtype";
                    //public const string Expects = "expects";
                    public const string AllowCustomParams = "allowcustomparams";
                    public const string Final = "final";
                }
            }

            public abstract class Dependencies
            {
                public abstract class Content
                {
                    public const string Dependencies = "Dependencies";
                    public const string FrameworkVersion = "CUAE Framework Version";
                    public const string ProviderVersion = "Providers";
                }

                public abstract class Role
                {
                    public const string Dependencies = "dependencies";
                    public const string FrameworkVersion = "frameworkver";
                    public const string ProviderVersion = "providerver";
                }
            }
        }

        public abstract class Types
        {
            public abstract class Role
            {
                public const string Types = "types";
                public const string Type = "type";
                public const string TypeTitle = "typedisplayname";
                public const string TypeSubtitle = "typename";
                public const string TypeDescription = "description";
                public const string Remarks = "remarks";
            }

            public abstract class Content
            {
                public const string Events = "Events";
                public const string EventDesc = "Description";
                public const string Remarks = "Remarks";
            }

            public abstract class ParseableInputsTable
            {
                public const string RowHeader = "rowheader";
                public const string RowHeaderValue = "firstcol";
                public const int NumCols = 2;
                public const string DataTypeColumn = "Data Type";
                public const string DescriptionColumn = "Description";

                public abstract class Content
                {
                    public const string Title = "Parseable Input Types";
                    public const string DataTypeColumn = ".NET Type";
                    public const string DescriptionColumn = "Description";
                }

                public abstract class Role
                {
                    public const string ParseableInputs = "parseableinputs";
                    public const string FirstRowEntry = "rowhead";
                }
            }

            public abstract class PublicMethodsTable
            {
                public const string RowHeader = "rowheader";
                public const string RowHeaderValue = "firstcol";
                public const int NumCols = 2;
                public const string MethodColumn = "Method Name";
                public const string DescriptionColumn = "Description";

                public abstract class Content
                {
                    public const string Title = "Accessible Public Methods";
                    public const string MethodColumn = "Method Name";
                    public const string DescriptionColumn = "Description";
                }

                public abstract class Role
                {
                    public const string PublicMethod = "publicmethods";
                    public const string FirstRowEntry = "rowhead";
                }
            }

            public abstract class PublicPropertiesTable
            {
                public const string RowHeader = "rowheader";
                public const string RowHeaderValue = "firstcol";
                public const int NumCols = 2;
                public const string PropertyNameColumn = "Method Name";
                public const string DescriptionColumn = "Description";

                public abstract class Content
                {
                    public const string Title = "Accessible Public Properties";
                    public const string MethodColumn = "Method Name";
                    public const string DescriptionColumn = "Description";
                }

                public abstract class Role
                {
                    public const string PublicMethod = "publicproperties";
                    public const string FirstRowEntry = "rowhead";
                }
            }

            public abstract class Properties
            {
                public abstract class Content
                {
                    public const string Properties = "Properties";
                    public const string Serializable = "Serializable";
                }

                public abstract class Role
                {
                    public const string Properties = "properties";
                    public const string Serializable = "serializable";
                }
            }

            public abstract class Dependencies
            {
                public abstract class Content
                {
                    public const string Dependencies = "Dependencies";
                    public const string FrameworkVersion = "CUAE Framework Version";
                    public const string ProviderVersion = "Providers";
                }

                public abstract class Role
                {
                    public const string Dependencies = "dependencies";
                    public const string FrameworkVersion = "frameworkver";
                    public const string ProviderVersion = "providerver";
                }
            }
        }
    }

}
