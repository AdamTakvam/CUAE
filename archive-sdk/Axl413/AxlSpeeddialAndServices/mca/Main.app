<Application name="Main" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Main">
    <outline>
      <treenode type="evh" id="632423474098293106" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632423474098293103" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632423474098293102" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/speeddialservice</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_devicename" id="632746892548451563" vid="632586614329867941">
        <Properties type="String" initWith="devicename">g_devicename</Properties>
      </treenode>
      <treenode text="g_ccmIp" id="632746892548451565" vid="632586614329867943">
        <Properties type="String" initWith="callManagerIP">g_ccmIp</Properties>
      </treenode>
      <treenode text="g_ccmUsername" id="632746892548451567" vid="632586614329867945">
        <Properties type="String" initWith="ccmUsername">g_ccmUsername</Properties>
      </treenode>
      <treenode text="g_ccmPassword" id="632746892548451569" vid="632586614329867947">
        <Properties type="String" initWith="ccmPassword">g_ccmPassword</Properties>
      </treenode>
      <treenode text="g_name1" id="632746892548451571" vid="632746799038485520">
        <Properties type="String" initWith="Name1">g_name1</Properties>
      </treenode>
      <treenode text="g_url1" id="632746892548451573" vid="632746799038485522">
        <Properties type="String" initWith="Url1">g_url1</Properties>
      </treenode>
      <treenode text="g_urlLabel1" id="632746892548451575" vid="632746799038485524">
        <Properties type="String" initWith="UrlLabel1">g_urlLabel1</Properties>
      </treenode>
      <treenode text="g_index1" id="632746892548451577" vid="632746799038485526">
        <Properties type="String" initWith="Index1">g_index1</Properties>
      </treenode>
      <treenode text="g_teleName1" id="632746892548451579" vid="632746799038485528">
        <Properties type="String" initWith="TelecasterName1">g_teleName1</Properties>
      </treenode>
      <treenode text="g_url2" id="632746892548451581" vid="632746799038485530">
        <Properties type="String" initWith="Url2">g_url2</Properties>
      </treenode>
      <treenode text="g_urlLabel2" id="632746892548451583" vid="632746799038485532">
        <Properties type="String" initWith="UrlLabel2">g_urlLabel2</Properties>
      </treenode>
      <treenode text="g_index2" id="632746892548451585" vid="632746799038485534">
        <Properties type="String" initWith="Index2">g_index2</Properties>
      </treenode>
      <treenode text="g_name2" id="632746892548451587" vid="632746799038485536">
        <Properties type="String" initWith="Name2">g_name2</Properties>
      </treenode>
      <treenode text="g_teleName2" id="632746892548451589" vid="632746799038485538">
        <Properties type="String" initWith="TelecasterName2">g_teleName2</Properties>
      </treenode>
      <treenode text="g_teleId1" id="632746892548451591" vid="632746892548451438">
        <Properties type="String" initWith="TeleId1">g_teleId1</Properties>
      </treenode>
      <treenode text="g_teleId2" id="632746892548451593" vid="632746892548451440">
        <Properties type="String" initWith="TeleId2">g_teleId2</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632423474098293105" treenode="632423474098293106" appnode="632423474098293103" handlerfor="632423474098293102">
    <node type="Start" id="632423474098293105" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632746799038485590" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632423474098293107" name="AddSpeeddialItem" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="153" y="370">
      <linkto id="632423474098293112" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="literal">1000</ap>
        <ap name="Label" type="literal">Speedy</ap>
        <ap name="Index" type="literal">1</ap>
        <rd field="Speeddial">speeddials</rd>
      </Properties>
    </node>
    <node type="Action" id="632423474098293112" name="AddSpeeddialItem" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="264" y="368">
      <linkto id="632423474098293113" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="literal">1001</ap>
        <ap name="Label" type="literal">Speedier</ap>
        <ap name="Index" type="literal">2</ap>
        <rd field="Speeddial">speeddials</rd>
      </Properties>
    </node>
    <node type="Action" id="632423474098293113" name="AddServiceItem" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="400" y="368">
      <linkto id="632746799038485540" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">g_name1</ap>
        <ap name="Url" type="variable">g_url1</ap>
        <ap name="UrlButtonIndex" type="variable">g_index1</ap>
        <ap name="UrlLabel" type="variable">g_urlLabel1</ap>
        <ap name="Uuid" type="variable">g_teleId1</ap>
        <ap name="TelecasterServiceName" type="variable">g_teleName1</ap>
        <rd field="Service">services</rd>
        <log condition="entry" on="true" level="Info" type="csharp"> host + "/testscript"</log>
      </Properties>
    </node>
    <node type="Action" id="632423474098293114" name="UpdatePhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="688" y="368">
      <linkto id="632423474098293116" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_devicename</ap>
        <ap name="Speeddials" type="variable">speeddials</ap>
        <ap name="Services" type="variable">services</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmUsername</ap>
        <ap name="AdminPassword" type="variable">g_ccmPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632423474098293116" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="800" y="368">
      <linkto id="632423474098293117" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"Attempted to update device " + g_devicename</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632423474098293117" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="908" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632746799038485540" name="AddServiceItem" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="540" y="368">
      <linkto id="632423474098293114" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">g_name2</ap>
        <ap name="Url" type="variable">g_url2</ap>
        <ap name="UrlButtonIndex" type="variable">g_index2</ap>
        <ap name="UrlLabel" type="variable">g_urlLabel2</ap>
        <ap name="Uuid" type="variable">g_teleId2</ap>
        <ap name="TelecasterServiceName" type="variable">g_teleName2</ap>
        <rd field="Service">services</rd>
        <log condition="entry" on="true" level="Info" type="csharp"> host + "/testscript"</log>
      </Properties>
    </node>
    <node type="Action" id="632746799038485590" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="188" y="539">
      <linkto id="632423474098293107" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string g_name1, ref string g_url1, ref string g_teleName1, ref string g_urlLabel1, ref string g_index1, ref string g_name2, ref string g_url2, ref string g_teleName2, ref string g_urlLabel2, ref string g_index2, ref string g_teleId1, ref string g_teleId2)
{
	if(g_name1 == "NONE")
	{
		g_name1 = null;
	}
	if(g_url1 == "NONE")
	{
		g_url1 = null;
	}
	if(g_teleName1 == "NONE")
	{
		g_teleName1 = null;
	}
	if(g_urlLabel1 == "NONE")
	{
		g_urlLabel1 = null;
	}
	if(g_index1 == "NONE")
	{
		g_index1 = null;
	}
	if(g_teleId1 == "NONE")
	{
		g_teleId1 = null;
	}
	if(g_teleId2 == "NONE")
	{
		g_teleId2 = null;
	}



	if(g_name2 == "NONE")
	{
		g_name2 = null;
	}
	if(g_url2 == "NONE")
	{
		g_url2 = null;
	}
	if(g_urlLabel2 == "NONE")
	{
		g_urlLabel2 = null;
	}
	if(g_index2 == "NONE")
	{
		g_index2 = null;
	}
	if(g_teleName2 == "NONE")
	{
		g_teleName2 = null;
	}

	if(g_name1 == "EMPTY")
	{
		g_name1 = String.Empty;
	}
	if(g_url1 == "EMPTY")
	{
		g_url1 = String.Empty;
	}
	if(g_teleName1 == "EMPTY")
	{
		g_teleName1 = String.Empty;
	}
	if(g_urlLabel1 == "EMPTY")
	{
		g_urlLabel1 = String.Empty;
	}
	if(g_index1 == "EMPTY")
	{
		g_index1 = String.Empty;
	}
	if(g_name2 == "EMPTY")
	{
		g_name2 = String.Empty;
	}
	if(g_url2 == "EMPTY")
	{
		g_url2 = String.Empty;
	}
	if(g_urlLabel2 == "EMPTY")
	{
		g_urlLabel2 = String.Empty;
	}
	if(g_index2 == "EMPTY")
	{
		g_index2 = String.Empty;
	}
	if(g_teleName2 == "EMPTY")
	{
		g_teleName2 = String.Empty;
	}
	if(g_teleId1 == "EMPTY")
	{
		g_teleId1 = String.Empty;
	}
	if(g_teleId2 == "EMPTY")
	{
		g_teleId2 = String.Empty;
	}

	return "";
}
</Properties>
    </node>
    <node type="Variable" id="632423474098293109" name="speeddials" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.Speeddials" refType="reference">speeddials</Properties>
    </node>
    <node type="Variable" id="632423474098293110" name="services" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.Services" refType="reference">services</Properties>
    </node>
    <node type="Variable" id="632423474098293111" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632423474098293118" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632423474098293149" name="getPhoneResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetPhoneResponse" refType="reference">getPhoneResponse</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>