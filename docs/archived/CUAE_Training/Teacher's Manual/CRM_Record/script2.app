<Application name="script2" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632935022220433364" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632935022220433361" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632935022220433360" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/recordRequest</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_phoneUser" id="632935022220433670" vid="632935022220433376">
        <Properties type="String" initWith="phoneUser">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="632935022220433672" vid="632935022220433378">
        <Properties type="String" initWith="phonePass">g_phonePass</Properties>
      </treenode>
      <treenode text="g_dialIn" id="632935022220433674" vid="632935022220433388">
        <Properties type="String" initWith="dialin">g_dialIn</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632935022220433363" treenode="632935022220433364" appnode="632935022220433361" handlerfor="632935022220433360">
    <node type="Start" id="632935022220433363" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="432">
      <linkto id="632935022220433369" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632935022220433367" text="When the IP phone makes a request using a service or service URL,&#xD;&#xA;the phone expects a response before receiving an execute command." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="96" y="296" />
    <node type="Action" id="632935022220433368" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="568" y="560">
      <linkto id="632935022220433375" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Key:Soft4</ap>
        <ap name="URL2" type="literal">Key:Soft2</ap>
        <rd field="ResultData">executeXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632935022220433369" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="192" y="432">
      <linkto id="632935022220433372" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Record Requested</ap>
        <ap name="Text" type="literal">Recording will be initiated shortly...</ap>
        <rd field="ResultData">textXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632935022220433371" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="432" y="432">
      <linkto id="632935022220433604" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632935022220433372" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="328" y="432">
      <linkto id="632935022220433371" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">textXml.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Comment" id="632935022220433373" text="Give phone a second, &#xD;&#xA;Hit more key,&#xD;&#xA;then hit conference softkey" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="776" y="512" />
    <node type="Action" id="632935022220433375" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="712" y="560">
      <linkto id="632935022220433383" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">executeXml.ToString()</ap>
        <ap name="URL" type="variable">remoteIPAddress</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Comment" id="632935022220433380" text="Give the phone a second,&#xD;&#xA; then push out a dial command to the IncomingCall application" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="776" y="656" />
    <node type="Action" id="632935022220433382" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="568" y="696">
      <linkto id="632935022220433384" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"Dial:" + g_dialIn</ap>
        <rd field="ResultData">executeXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632935022220433383" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="432" y="696">
      <linkto id="632935022220433382" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632935022220433384" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="712" y="696">
      <linkto id="632935022220433424" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">executeXml.ToString()</ap>
        <ap name="URL" type="variable">remoteIPAddress</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632935022220433423" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="568" y="848">
      <linkto id="632935022220433425" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Key:Soft3</ap>
        <rd field="ResultData">executeXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632935022220433424" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="432" y="848">
      <linkto id="632935022220433423" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">2000</ap>
      </Properties>
    </node>
    <node type="Action" id="632935022220433425" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="712" y="848">
      <linkto id="632935022220433431" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">executeXml.ToString()</ap>
        <ap name="URL" type="variable">remoteIPAddress</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Comment" id="632935022220433429" text="Give the phone a second to connect the calll,&#xD;&#xA;then hit the conference softkey one last time for the user." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="776" y="792" />
    <node type="Action" id="632935022220433431" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="568" y="928">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632935022220433604" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="568" y="432">
      <linkto id="632935022220433608" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Init:Services</ap>
        <rd field="ResultData">executeXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632935022220433606" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="432" y="560">
      <linkto id="632935022220433368" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632935022220433608" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="720" y="432">
      <linkto id="632935022220433606" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">executeXml.ToString()</ap>
        <ap name="URL" type="variable">remoteIPAddress</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Comment" id="632935022220433699" text="Clear services if showing." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="760" y="376" />
    <node type="Variable" id="632935022220433365" name="remoteIPAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteIPAddress</Properties>
    </node>
    <node type="Variable" id="632935022220433366" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632935022220433370" name="textXml" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">textXml</Properties>
    </node>
    <node type="Variable" id="632935022220433374" name="executeXml" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">executeXml</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>