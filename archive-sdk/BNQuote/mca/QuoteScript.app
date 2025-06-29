<Application name="QuoteScript" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="QuoteScript">
    <outline>
      <treenode type="evh" id="632551377325575012" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632551377325575009" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632551377325575008" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632551377325575021" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632551377325575018" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632551377325575017" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632721438923564450" actid="632551377325575027" />
          <ref id="632721438923564474" actid="632551377325575085" />
          <ref id="632721438923564478" actid="632551377325575092" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632551377325575026" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632551377325575023" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632551377325575022" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632721438923564451" actid="632551377325575027" />
          <ref id="632721438923564475" actid="632551377325575085" />
          <ref id="632721438923564479" actid="632551377325575092" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632551377325575066" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632551377325575063" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632551377325575062" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632721438923564457" actid="632551377325575072" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632551377325575071" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632551377325575068" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632551377325575067" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632721438923564458" actid="632551377325575072" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632551377325575101" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632551377325575098" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632551377325575097" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_connectionId" id="632721438923564442" vid="632551377325575015">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_requestISBNPrompt" id="632721438923564444" vid="632551377325575030">
        <Properties type="String" initWith="RequestISBNPrompt">g_requestISBNPrompt</Properties>
      </treenode>
      <treenode text="g_callId" id="632721438923564446" vid="632551432273405259">
        <Properties type="String">g_callId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632551377325575011" treenode="632551377325575012" appnode="632551377325575009" handlerfor="632551377325575008">
    <node type="Start" id="632551377325575011" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="271">
      <linkto id="632551377325575014" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632551377325575014" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="156" y="271">
      <linkto id="632551377325575027" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632551432273405261" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">BN Quote</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632551377325575027" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="239" y="255" mx="292" my="271">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632551377325575021" />
        <item text="OnPlay_Failed" treenode="632551377325575026" />
      </items>
      <linkto id="632551377325575059" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="variable">g_requestISBNPrompt</ap>
        <ap name="UserData" type="literal">isbn</ap>
      </Properties>
    </node>
    <node type="Action" id="632551377325575059" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="420" y="270">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632551432273405261" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="157" y="385">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: failed to answer incoming call, exiting script</log>
      </Properties>
    </node>
    <node type="Comment" id="632721438923564493" text="We first attempt to&#xD;&#xA;answer the incoming call.&#xD;&#xA; If this operation fails,&#xD;&#xA; we exit the script.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="29" y="135" />
    <node type="Comment" id="632721438923564494" text="If the call was answered&#xD;&#xA;succesfully, we play the &#xD;&#xA;TTS string configured by way&#xD;&#xA;of mceadmin to the connection&#xD;&#xA;returned by the Answer action.&#xD;&#xA;We specify that the announcement&#xD;&#xA;should stop playing after the caller&#xD;&#xA;presses a digit on the keypad." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="223" y="112" />
    <node type="Comment" id="632721438923564495" text="Since the OnPlay_Complete/Failed event handlers&#xD;&#xA;will be invoked in different contexts (ie, the initial&#xD;&#xA;announcement finishes playing, or the announcement&#xD;&#xA;that informs the user of the price finishes playing, etc),&#xD;&#xA;we pass in a UserData of 'isbn' along with the Play action&#xD;&#xA;to indicate the context of the action's instance.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="359" y="365" />
    <node type="Variable" id="632551377325575013" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632551377325575020" treenode="632551377325575021" appnode="632551377325575018" handlerfor="632551377325575017">
    <node type="Start" id="632551377325575020" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="233">
      <linkto id="632551377325575089" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632551377325575072" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="233" y="216" mx="307" my="232">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632551377325575066" />
        <item text="OnGatherDigits_Failed" treenode="632551377325575071" />
      </items>
      <linkto id="632551377325575077" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632551377325575077" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="437" y="232">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632551377325575089" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="147" y="232">
      <linkto id="632551377325575090" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632551377325575072" type="Labeled" style="Bezier" ortho="true" label="isbn" />
      <linkto id="632551432273405263" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632551377325575090" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="147" y="345">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632551432273405262" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="409" y="438">
      <linkto id="632551432273405265" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Label" id="632551432273405263" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="147" y="131" />
    <node type="Label" id="632551432273405264" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="306" y="437">
      <linkto id="632551432273405262" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632551432273405265" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="529" y="438">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632721438923564496" text="The first step is to figure out the&#xD;&#xA;context of the event that we are&#xD;&#xA;are handling. We do this by branching&#xD;&#xA;on the UserData that was&#xD;&#xA;associated with the instance of&#xD;&#xA;the Play action to which this event&#xD;&#xA;corresponds." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="187" y="25" />
    <node type="Comment" id="632721438923564497" text="Here we tell the MMS that we&#xD;&#xA;want it to listen for digits, and to&#xD;&#xA;return us the completed string once&#xD;&#xA;a '#' is observed." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="394" y="271" />
    <node type="Comment" id="632721438923564498" text="This code will terminate the call once an announcement completes." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="272" y="490" />
    <node type="Variable" id="632551377325575088" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632551377325575025" treenode="632551377325575026" appnode="632551377325575023" handlerfor="632551377325575022">
    <node type="Start" id="632551377325575025" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="295">
      <linkto id="632551377325575076" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632551377325575076" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="118" y="297">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnPlay_Failed: Play command failed to execute, exiting script</log>
      </Properties>
    </node>
    <node type="Comment" id="632721438923564499" text="In the case that the announcement failed to play,&#xD;&#xA;we could put error handling&#xD;&#xA;code in this handler. " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="45" y="187" />
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632551377325575065" treenode="632551377325575066" appnode="632551377325575063" handlerfor="632551377325575062">
    <node type="Start" id="632551377325575065" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="349">
      <linkto id="632551377325575080" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632551377325575080" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="131" y="349">
      <linkto id="632551377325575081" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632551377325575082" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632551377325575081" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="456">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632551377325575082" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="245" y="348">
      <linkto id="632551377325575264" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632551377325575092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string digits)
{
	digits = digits.Replace("#", string.Empty);
	if (digits == string.Empty)
		return IApp.VALUE_FAILURE;

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632551377325575085" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="649" y="329" mx="702" my="345">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632551377325575021" />
        <item text="OnPlay_Failed" treenode="632551377325575026" />
      </items>
      <linkto id="632551377325575091" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="csharp">"The list price of the item in question is " + dollars + " dollars and " + cents + " cents."</ap>
        <ap name="Prompt2" type="literal">[vt_pause=1000] Goodbye.</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632551377325575091" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="822" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632551377325575092" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="311" y="444" mx="364" my="460">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632551377325575021" />
        <item text="OnPlay_Failed" treenode="632551377325575026" />
      </items>
      <linkto id="632551377325575095" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="Prompt1" type="literal">The price for the requested item could not be found.</ap>
        <ap name="Prompt2" type="variable">g_requestISBNPrompt</ap>
        <ap name="UserData" type="literal">isbn</ap>
      </Properties>
    </node>
    <node type="Action" id="632551377325575095" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="360" y="594">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632551377325575171" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="486" y="347">
      <linkto id="632551377325575092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632551377325575265" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">price.ToUpper()</ap>
        <ap name="Value2" type="literal">PRICE NOT AVAILABLE</ap>
      </Properties>
    </node>
    <node type="Action" id="632551377325575264" name="GetBNQuote" class="MaxActionNode" group="" path="WebServices.NativeActions.BNPrice" x="359" y="347">
      <linkto id="632551377325575171" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632551377325575092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SISBN" type="variable">digits</ap>
        <rd field="Result">price</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnGatherDigits_Complete: looking up price for item with ISBN: " + digits</log>
      </Properties>
    </node>
    <node type="Action" id="632551377325575265" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="590" y="346">
      <linkto id="632551377325575085" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string price, ref string dollars, ref string cents)
{
	string[] priceArray = price.Split('.');
	dollars = priceArray[0];
	cents = priceArray[1];
	if (cents.StartsWith("0"))
		cents = Convert.ToString(cents[1]);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632721438923564500" text="When the GatherDigits action was invoked in the OnPlay_Complete event handler,&#xD;&#xA;we specified that we want the action to terminate when the digit '#' is observed.&#xD;&#xA;However, the action could terminate due to other causes, such as a hangup, or &#xD;&#xA;because of a static MMS TimeOut configuration setting. Here we ensure that the &#xD;&#xA;GatherDigits operation terminated due to the '#' digit being pressed. We do this by&#xD;&#xA;looking at the value of the termination condition event parameter." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="21" y="58" />
    <node type="Comment" id="632721438923564501" text="In the case of an error, we will play the TTS string that&#xD;&#xA;announces to the caller that an error occured, and specify UserData &#xD;&#xA;such that the event handler will hangup the caller." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="435" y="499" />
    <node type="Comment" id="632721438923564502" text="We use a piece of custom code&#xD;&#xA;to remove the occurance of '#'&#xD;&#xA;from the received digit string" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="156" y="259" />
    <node type="Comment" id="632721438923564503" text="Next, we invoke the GetBNQuote action&#xD;&#xA;that was generated from the WSDL file.&#xD;&#xA;If the action succeeds, we compare the &#xD;&#xA;value of the 'price' variable to that variable's&#xD;&#xA;default value. If the two are unequal, then&#xD;&#xA;a value was obtained. This serves as a check&#xD;&#xA;to insure that even though the GetBNQuote &#xD;&#xA;action succeeded, the price returned was not&#xD;&#xA;an empty string." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="338" y="190" />
    <node type="Comment" id="632721438923564504" text="Next, we build the TTS string and invoke &#xD;&#xA;the Play action to play the TTS string to the caller.&#xD;&#xA;We pass in a UserData of 'exit' to specify the context&#xD;&#xA;of this instance of the Play action - whenever the action&#xD;&#xA;completes, we want the call to end." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="604" y="208" />
    <node type="Variable" id="632551377325575078" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632551377325575079" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="#" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
    <node type="Variable" id="632551377325575084" name="price" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="0" refType="reference">price</Properties>
    </node>
    <node type="Variable" id="632551377325575266" name="dollars" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="0" refType="reference">dollars</Properties>
    </node>
    <node type="Variable" id="632551377325575267" name="cents" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="0" refType="reference">cents</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632551377325575070" treenode="632551377325575071" appnode="632551377325575068" handlerfor="632551377325575067">
    <node type="Start" id="632551377325575070" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="304">
      <linkto id="632551377325575096" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632551377325575096" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="140" y="304">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" activetab="true" startnode="632551377325575100" treenode="632551377325575101" appnode="632551377325575098" handlerfor="632551377325575097">
    <node type="Start" id="632551377325575100" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="91" y="230">
      <linkto id="632551377325575102" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632551377325575102" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="219" y="229">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632721438923564505" text="Should the user hang up, the script needs to terminate." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="120" y="154" />
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>