<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632849342826660339" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632849342826660336" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632849342826660335" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CGI/Execute</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_performAuth" id="632850945194272695" vid="632849342826660340">
        <Properties type="String" initWith="performAuth">g_performAuth</Properties>
      </treenode>
      <treenode text="authUrl" id="632850945194272697" vid="632849342826660362">
        <Properties type="String" initWith="authUrl">authUrl</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632849342826660338" treenode="632849342826660339" appnode="632849342826660336" handlerfor="632849342826660335">
    <node type="Start" id="632849342826660338" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="425">
      <linkto id="632849342826660364" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632849342826660364" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="354" y="425">
      <linkto id="632850945194272700" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string body)
{
		Metreos.Types.CiscoIpPhone.CiscoIPPhoneResponseType response = new Metreos.Types.CiscoIpPhone.CiscoIPPhoneResponseType();
            response.ResponseItem = new Metreos.Types.CiscoIpPhone.CiscoIPPhoneResponseItemType[3];
            response.ResponseItem[0] = new Metreos.Types.CiscoIpPhone.CiscoIPPhoneResponseItemType();
            response.ResponseItem[0].URL = String.Empty;
            response.ResponseItem[0].Status = 0;
            response.ResponseItem[0].Data = String.Empty;
            response.ResponseItem[1] = new Metreos.Types.CiscoIpPhone.CiscoIPPhoneResponseItemType();
            response.ResponseItem[1].URL = String.Empty;
            response.ResponseItem[1].Status = 0;
            response.ResponseItem[1].Data = String.Empty;
            response.ResponseItem[2] = new Metreos.Types.CiscoIpPhone.CiscoIPPhoneResponseItemType();
            response.ResponseItem[2].URL = String.Empty;
            response.ResponseItem[2].Status = 0;
            response.ResponseItem[2].Data = String.Empty;

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            using(StringWriter writer = new StringWriter(builder))
            {
                XmlSerializer seri = new XmlSerializer(typeof(Metreos.Types.CiscoIpPhone.CiscoIPPhoneResponseType));
                seri.Serialize(writer, response);
            }
            body = builder.ToString();
		return "";
}
</Properties>
    </node>
    <node type="Action" id="632850945194272700" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="528" y="426">
      <linkto id="632850945194272703" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632850945194272703" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="696" y="427">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632850945194272701" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="632850945194272702" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>