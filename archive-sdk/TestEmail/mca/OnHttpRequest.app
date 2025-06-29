<Application name="OnHttpRequest" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="OnHttpRequest">
    <outline>
      <treenode type="evh" id="632934096737344185" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632934096737344182" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632934096737344181" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/testemail</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_host" id="632934096737344214" vid="632934096737344196">
        <Properties type="String" initWith="host">g_host</Properties>
      </treenode>
      <treenode text="g_to" id="632934096737344218" vid="632934096737344200">
        <Properties type="String" initWith="to">g_to</Properties>
      </treenode>
      <treenode text="g_from" id="632934096737344220" vid="632934096737344202">
        <Properties type="String" initWith="from">g_from</Properties>
      </treenode>
      <treenode text="g_body" id="632934096737344222" vid="632934096737344204">
        <Properties type="String" initWith="body">g_body</Properties>
      </treenode>
      <treenode text="g_subject" id="632934096737344226" vid="632934096737344225">
        <Properties type="String" initWith="subject">g_subject</Properties>
      </treenode>
      <treenode text="g_username" id="632934096737344228" vid="632934096737344227">
        <Properties type="String" initWith="username">g_username</Properties>
      </treenode>
      <treenode text="g_password" id="632934096737344230" vid="632934096737344229">
        <Properties type="String" initWith="password">g_password</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632934096737344184" treenode="632934096737344185" appnode="632934096737344182" handlerfor="632934096737344181">
    <node type="Start" id="632934096737344184" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="184" y="448">
      <linkto id="632934096737344186" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934096737344186" name="Send" class="MaxActionNode" group="" path="Metreos.Native.Mail" x="384" y="448">
      <linkto id="632934096737344231" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632934096737344233" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="native" log="On">
        <ap name="To" type="variable">g_to</ap>
        <ap name="From" type="variable">g_from</ap>
        <ap name="MailServer" type="variable">g_host</ap>
        <ap name="Username" type="variable">g_username</ap>
        <ap name="Password" type="variable">g_password</ap>
        <ap name="Subject" type="variable">g_subject</ap>
        <ap name="Body" type="variable">g_body</ap>
      </Properties>
    </node>
    <node type="Action" id="632934096737344231" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="608" y="448">
      <linkto id="632934096737344235" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Email sent successfully</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632934096737344233" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="384" y="656">
      <linkto id="632934096737344235" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Failure to send email</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632934096737344235" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="656">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632934096737344232" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>