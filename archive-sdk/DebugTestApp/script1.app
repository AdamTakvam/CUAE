<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632583116996452595" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632583116996452592" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632583116996452591" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/debug</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632583116996452604" level="1" text="Respond">
        <node type="function" name="Respond" id="632583116996452601" path="Metreos.StockTools" />
        <calls>
          <ref actid="632583116996452600" />
        </calls>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632583116996452594" treenode="632583116996452595" appnode="632583116996452592" handlerfor="632583116996452591">
    <node type="Loop" id="632583116996452599" name="Loop" text="loop 5x" cx="148" cy="171" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="224" y="341" mx="298" my="426">
      <linkto id="632583116996452597" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632583116996452600" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="literal">5</Properties>
    </node>
    <node type="Start" id="632583116996452594" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="427">
      <linkto id="632583116996452596" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632583116996452596" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="112" y="427">
      <linkto id="632583116996452599" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Main Menu</ap>
        <ap name="Prompt" type="literal">Post Menu To Page</ap>
        <rd field="ResultData">Menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632583116996452597" name="AddMenuItem" container="632583116996452599" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="295" y="426">
      <linkto id="632583116996452599" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">"MenuItem" + loopIndex</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">Menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632583116996452600" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="436" y="410" mx="473" my="426">
      <items count="1">
        <item text="Respond" />
      </items>
      <linkto id="632583116996452605" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Menu" type="variable">Menu</ap>
        <ap name="remotehost" type="variable">remotehost</ap>
        <ap name="FunctionName" type="literal">Respond</ap>
      </Properties>
    </node>
    <node type="Action" id="632583116996452605" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="572" y="426">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632651551399770429" text="PURPOSE: Test the debugger&#xD;&#xA;&#xD;&#xA;Start the debugger and then trigger the application by doing the following:&#xD;&#xA;Goto http://&lt;appserver&gt;:8000/debug&#xD;&#xA;&#xD;&#xA;Step through the app or debug in any way that you like.  When you are done, &#xD;&#xA;verify that a page is displayed in XML format containing menu items 0-5" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="14" y="16" />
    <node type="Variable" id="632583116996452598" name="Menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">Menu</Properties>
    </node>
    <node type="Variable" id="632583116996452607" name="remotehost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remotehost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Respond" startnode="632583116996452603" treenode="632583116996452604" appnode="632583116996452601" handlerfor="632583116996452591">
    <node type="Start" id="632583116996452603" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="287">
      <linkto id="632583116996452606" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632583116996452606" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="192" y="288">
      <linkto id="632583116996452611" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remotehost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">Menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632583116996452611" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="352" y="288">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">0</ap>
      </Properties>
    </node>
    <node type="Variable" id="632583116996452608" name="Menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" initWith="Menu" refType="reference">Menu</Properties>
    </node>
    <node type="Variable" id="632583116996452609" name="remotehost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remotehost" refType="reference">remotehost</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>