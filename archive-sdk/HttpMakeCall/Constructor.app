<Application name="Constructor" trigger="Metreos.ApplicationControl.StaticConstruction" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Constructor">
    <outline>
      <treenode type="evh" id="633027341840000481" level="1" text="Metreos.ApplicationControl.StaticConstruction (trigger): OnStaticConstruction">
        <node type="function" name="OnStaticConstruction" id="633027341840000478" path="Metreos.StockTools" />
        <node type="event" name="StaticConstruction" id="633027341840000477" path="Metreos.ApplicationControl.StaticConstruction" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633027341840000486" level="2" text="Metreos.ApplicationControl.InstanceDestruction: OnInstanceDestruction">
        <node type="function" name="OnInstanceDestruction" id="633027341840000483" path="Metreos.StockTools" />
        <node type="event" name="InstanceDestruction" id="633027341840000482" path="Metreos.ApplicationControl.InstanceDestruction" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_Prompt" id="633177738297969218" vid="633056872827656753">
        <Properties type="String" initWith="Locale.Hello">g_Prompt</Properties>
      </treenode>
      <treenode text="g_DefaultValue" id="633177738297969220" vid="633131902456406779">
        <Properties type="String" defaultInitWith="Smurfs">g_DefaultValue</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnStaticConstruction" activetab="true" startnode="633027341840000480" treenode="633027341840000481" appnode="633027341840000478" handlerfor="633027341840000477">
    <node type="Start" id="633027341840000480" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="134">
      <linkto id="633059261878906644" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633027347017500383" name="ConstructionComplete" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="567" y="134">
      <linkto id="633027347017500384" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Success" type="literal">true</ap>
        <log condition="entry" on="true" level="Warning" type="csharp">String.Format("Locale changed to: {0} (test={1})", sessionData.Culture.DisplayName, test);</log>
      </Properties>
    </node>
    <node type="Action" id="633027347017500384" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="731" y="131">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633051682264219241" name="ChangeLocale" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="290" y="134">
      <linkto id="633054209328906743" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Locale" type="literal">fr-FR</ap>
        <ap name="ResetStrings" type="literal">true</ap>
        <log condition="entry" on="true" level="Warning" type="csharp">String.Format("Locale: {0} (test={1})", sessionData.Culture.DisplayName, test);</log>
      </Properties>
    </node>
    <node type="Action" id="633054209328906743" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="426" y="134">
      <linkto id="633027347017500383" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">g_Prompt</ap>
        <rd field="ResultData">test</rd>
      </Properties>
    </node>
    <node type="Action" id="633059261878906644" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="162" y="134">
      <linkto id="633051682264219241" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">g_Prompt</ap>
        <rd field="ResultData">test</rd>
        <log condition="entry" on="true" level="Warning" type="csharp">"Default Value: " + g_DefaultValue</log>
      </Properties>
    </node>
    <node type="Variable" id="633054209328906744" name="test" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" defaultInitWith="Not Working" refType="reference">test</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnInstanceDestruction" startnode="633027341840000485" treenode="633027341840000486" appnode="633027341840000483" handlerfor="633027341840000482">
    <node type="Start" id="633027341840000485" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="141">
      <linkto id="633027349417500385" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633027349417500385" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="178" y="141">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Warning" type="csharp">String.Format("Destructor called (errorCode={0}, errorText={1})", errorCode, errorText);</log>
      </Properties>
    </node>
    <node type="Variable" id="633027349417500504" name="errorCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="ErrorCode" refType="reference" name="Metreos.ApplicationControl.InstanceDestruction">errorCode</Properties>
    </node>
    <node type="Variable" id="633027349417500505" name="errorText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ErrorText" refType="reference" name="Metreos.ApplicationControl.InstanceDestruction">errorText</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>