<Application name="Second" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Second">
    <outline>
      <treenode type="evh" id="632423474098293160" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632423474098293157" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632423474098293156" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/speeddialservice2</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_devicename" id="632586614329868025" vid="632586614329867972">
        <Properties type="String" initWith="devicename">g_devicename</Properties>
      </treenode>
      <treenode text="g_ccmIp" id="632586614329868027" vid="632586614329867974">
        <Properties type="String" initWith="callManagerIP">g_ccmIp</Properties>
      </treenode>
      <treenode text="g_ccmUsername" id="632586614329868029" vid="632586614329867976">
        <Properties type="String" initWith="ccmUsername">g_ccmUsername</Properties>
      </treenode>
      <treenode text="g_ccmPassword" id="632586614329868031" vid="632586614329867978">
        <Properties type="String" initWith="ccmPassword">g_ccmPassword</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632423474098293159" treenode="632423474098293160" appnode="632423474098293157" handlerfor="632423474098293156">
    <node type="Loop" id="632424417737187602" name="Loop" text="loop (expr)" cx="209" cy="148" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="345" y="62" mx="450" my="136">
      <linkto id="632424417737187605" fromport="1" type="Basic" style="Bevel" />
      <linkto id="632586605968725302" fromport="3" type="Labeled" style="Bevel" label="default" />
      <Properties iteratorType="int" type="csharp">getPhoneResponse.Response.@return.device.speeddials.Length</Properties>
    </node>
    <node type="Loop" id="632424417737187610" name="Loop" text="loop (expr)" cx="227" cy="148" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="344" y="246" mx="458" my="320">
      <linkto id="632424417737187611" fromport="1" type="Basic" style="Bevel" />
      <linkto id="632424417737187612" fromport="3" type="Labeled" style="Bevel" label="default" />
      <Properties iteratorType="int" type="csharp">getPhoneResponse.Response.@return.device.services.Length</Properties>
    </node>
    <node type="Start" id="632423474098293159" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="136">
      <linkto id="632423474098293161" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632423474098293161" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="112" y="136">
      <linkto id="632423474098293163" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_devicename</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmUsername</ap>
        <ap name="AdminPassword" type="variable">g_ccmPassword</ap>
        <rd field="GetPhoneResponse">getPhoneResponse</rd>
      </Properties>
    </node>
    <node type="Action" id="632423474098293163" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="208" y="136">
      <linkto id="632424417737187602" port="1" type="Labeled" style="Bevel" label="true" />
      <linkto id="632424417737187609" type="Labeled" style="Bevel" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">getPhoneResponse.Response.@return.device.speeddials == null ? false : (getPhoneResponse.Response.@return.device.speeddials.Length == 0 ? false : true)</ap>
      </Properties>
    </node>
    <node type="Action" id="632424417737187605" name="AddSpeeddialItem" container="632424417737187602" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="449" y="136">
      <linkto id="632424417737187602" port="3" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="csharp">getPhoneResponse.Response.@return.device.speeddials[loopIndex].dirn</ap>
        <ap name="Label" type="csharp">getPhoneResponse.Response.@return.device.speeddials[loopIndex].label + "!"</ap>
        <ap name="Index" type="csharp">getPhoneResponse.Response.@return.device.speeddials[loopIndex].index</ap>
        <rd field="Speeddial">speeddials</rd>
      </Properties>
    </node>
    <node type="Action" id="632424417737187609" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="208" y="320">
      <linkto id="632424417737187610" port="1" type="Labeled" style="Bevel" label="true" />
      <linkto id="632424433605337724" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">getPhoneResponse.Response.@return.device.services == null ? false : (getPhoneResponse.Response.@return.device.services.Length == 0 ? false : true)</ap>
      </Properties>
    </node>
    <node type="Action" id="632424417737187611" name="AddServiceItem" container="632424417737187610" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="459" y="320">
      <linkto id="632424417737187610" port="3" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Url" type="csharp">getPhoneResponse.Response.@return.device.services[loopIndex].url</ap>
        <ap name="Uuid" type="csharp">getPhoneResponse.Response.@return.device.services[loopIndex].uuid</ap>
        <rd field="Service">services</rd>
      </Properties>
    </node>
    <node type="Action" id="632424417737187612" name="UpdatePhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="649" y="320">
      <linkto id="632424417737187613" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_devicename</ap>
        <ap name="Speeddials" type="variable">speeddials</ap>
        <ap name="Services" type="variable">services</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmUsername</ap>
        <ap name="AdminPassword" type="variable">g_ccmPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632424417737187613" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="753" y="320">
      <linkto id="632424417737187614" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"Attempted to update device " + g_devicename</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632424417737187614" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="849" y="320">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632424433605337724" text="s" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="208" y="432" />
    <node type="Label" id="632424433605337725" text="s" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="649" y="240">
      <linkto id="632424417737187612" type="Basic" style="Bevel" />
    </node>
    <node type="Label" id="632586605968725301" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="128" y="320">
      <linkto id="632424417737187609" type="Basic" style="Bevel" />
    </node>
    <node type="Label" id="632586605968725302" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="679" y="136" />
    <node type="Variable" id="632423474098293162" name="getPhoneResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.GetPhoneResponse" refType="reference">getPhoneResponse</Properties>
    </node>
    <node type="Variable" id="632424417737187606" name="speeddials" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.Speeddials" refType="reference">speeddials</Properties>
    </node>
    <node type="Variable" id="632424417737187607" name="services" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.Services" refType="reference">services</Properties>
    </node>
    <node type="Variable" id="632424417737187608" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>