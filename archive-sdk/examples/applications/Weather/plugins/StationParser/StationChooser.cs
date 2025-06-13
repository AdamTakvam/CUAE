using System;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using Metreos.Weather.Common;

namespace StationParser
{
    /// <summary> Contains helper information and lookup routines to show valid stations </summary>
    public class StationChooser
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(Wx_station_index));
        public SortedList states;

        public  StationChooser(string path) 
        {
            FileInfo stationFile = new FileInfo(path);
            FileStream stream = null;
            try
            {
                stream = stationFile.Open(FileMode.Open);
                Wx_station_index stations = serializer.Deserialize(stream) as Wx_station_index;

                if(!ParseStations(stations.station))
                {
                    Console.WriteLine("Invalid file");
                }
            }
            catch
            {
                Console.WriteLine("Invalid file");
            }
            finally
            {
                if(stream != null)
                {
                    stream.Close();
                }
            }
        }

        private bool ParseStations(Wx_station_indexStation[] stations)
        {
            states = new SortedList(System.Collections.CaseInsensitiveComparer.Default);

            if(stations == null) { return false; }

            foreach(Wx_station_indexStation station in stations)
            {
                if(!states.Contains(station.state))
                {
                    states[station.state] = new State();
                }
                
                State currentState = states[station.state] as State;

                if(!currentState.cities.Contains(station.station_name))
                {
                    currentState.cities[station.station_id] = new string[] { station.station_name, station.xml_url };
                }
            }

            return true;
        }

        public void WriteCsFile()
        {
            System.CodeDom.CodeNamespace codeGen = new CodeNamespace("Metreos.Native.Weather");

            codeGen.Imports.Add(new CodeNamespaceImport("System"));
            codeGen.Imports.Add(new CodeNamespaceImport("System.Collections"));

            CodeTypeDeclaration topLevelClass = new CodeTypeDeclaration("StationInfo");

            topLevelClass.Attributes = MemberAttributes.Abstract & MemberAttributes.Public;
            codeGen.Types.Add(topLevelClass);
            
            CodeMemberMethod getStatesMethod = new CodeMemberMethod();
            getStatesMethod.Attributes = MemberAttributes.Static & MemberAttributes.Public;
            getStatesMethod.Name = "GetStates";
            getStatesMethod.ReturnType = new CodeTypeReference(typeof(SortedList));
            

            CodeVariableDeclarationStatement statesVar = new CodeVariableDeclarationStatement(typeof(SortedList), "states", new CodeSnippetExpression("new SortedList(System.Collections.CaseInsensitiveComparer.Default)"));
            getStatesMethod.Statements.Add(statesVar);


            IDictionaryEnumerator statesEnum = states.GetEnumerator();
            while(statesEnum.MoveNext())
            {
                string stateName = statesEnum.Key as string;
                State state = statesEnum.Value as State; 
                
                CodeVariableDeclarationStatement stateNameVar = new CodeVariableDeclarationStatement(typeof(SortedList), stateName, new CodeSnippetExpression("new SortedList(System.Collections.CaseInsensitiveComparer.Default)"));
                getStatesMethod.Statements.Add(stateNameVar);

                CodeSnippetExpression addToStates = new CodeSnippetExpression("states[\"" + stateName + "\"] = " + stateName);
                getStatesMethod.Statements.Add(addToStates);

                IDictionaryEnumerator citiesEnum = state.cities.GetEnumerator();
                while(citiesEnum.MoveNext())
                {
                    string cityId = citiesEnum.Key as string;
                    string[] cityNameUrl = citiesEnum.Value as string[];

                    CodeSnippetExpression addToCities = new CodeSnippetExpression(stateName + "[\"" + cityId + "\"] = new string[] { @\"" + cityNameUrl[0] + "\", @\"" + cityNameUrl[1] + "\" }");
                    getStatesMethod.Statements.Add(addToCities);
                }
            }

            topLevelClass.Members.Add(getStatesMethod);

            WriteCode(codeGen);
        }

        public void WriteCode(System.CodeDom.CodeNamespace ns)
        {
            CSharpCodeProvider cSharpProvider = new CSharpCodeProvider();

            ICodeGenerator creator = cSharpProvider.CreateGenerator();

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            options.BlankLinesBetweenMembers = false;
            options.IndentString = "    ";

            FileInfo outputCsFile = new FileInfo("StationInfo.cs");

            if(outputCsFile.Exists)
            {
                outputCsFile.Delete();
            }

            StreamWriter outputFileStream = outputCsFile.CreateText();
            
            creator.GenerateCodeFromNamespace(ns, outputFileStream, options);
  
            outputFileStream.Close();
        }

        public class State
        {
            public SortedList cities;

            public State()
            {
                cities = new SortedList();
            }
        }

        /* 
            public class StationInfo
            {
                public static SortedList GetStates()
                {
                    SortedList states = new SortedList(System.Collections.CaseInsensitiveComparer);
                    
                    SortedList stateName = new SortedList(System.Collections.CaseInsensitiveComparer);
                    states[keyName] = stateName;
                    stateName[cityName] = cityUrl;
                    ...
                    
                    State stateName = new State();
                    ...
                    
                    return states;
                }
            }
         */
        

    }
}
