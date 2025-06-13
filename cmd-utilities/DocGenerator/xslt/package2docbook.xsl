<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:pack="http://metreos.com/ActionEventPackage.xsd" xmlns:nt="http://metreos.com/NativeTypePackage.xsd" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
        <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="pack:package">
    <chapter>
      <xsl:attribute name="id"><xsl:value-of select="@name" /></xsl:attribute>
      <title id="packagedisplayname">
        <xsl:value-of select="@name" />
      </title>
      <subtitle id="packagename">
        <xsl:value-of select="@name" />
      </subtitle>
      <para>
          <xsl:value-of disable-output-escaping="yes" select="@description" />
      </para>
      <xsl:apply-templates>
        <xsl:with-param name="packagename" select="@name" />
      </xsl:apply-templates>
    </chapter>
  </xsl:template>

  <xsl:template match="pack:actionList">
    <xsl:param name="packagename" />
    <section role="actions">
      <xsl:attribute name="id" xml:space="default"><xsl:value-of select="$packagename" />.Actions</xsl:attribute>
      <title>Actions</title>
      <xsl:apply-templates select="pack:action">
        <xsl:with-param name="packagename" select="$packagename" />
      </xsl:apply-templates>
    </section>
  </xsl:template>

  <xsl:template match="pack:action">
    <xsl:param name="packagename" />
    <section role="action">
      <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="@name" /></xsl:attribute>
      <title role="actiondisplayname">
        <xsl:value-of select="@displayName" />
      </title>

      <subtitle role="actionname">
        <xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />
      </subtitle>

      <para role="actionproperties">
		<itemizedlist>
			<listitem><para>
			<emphasis role="actionclass">
			<xsl:choose>
				<xsl:when test="@type='provider'">
				<xsl:value-of select="'Provider'"/>
				</xsl:when>
				<xsl:when test="@type='native'">
				<xsl:value-of select="'Native'"/>
				</xsl:when>
				<xsl:when test="@type='appControl'">
				<xsl:value-of select="'Application Control'"/>
				</xsl:when>
			</xsl:choose>
			</emphasis>
			</para>
			</listitem>

			<listitem>
			<para>
			<emphasis role="actiontype">
			<xsl:choose>
				<xsl:when test="count(pack:asyncCallback)=0">
				<xsl:value-of select="'Synchronous'"/>
				</xsl:when>
				<xsl:when test="count(pack:asyncCallback)>0">
				<xsl:value-of select="'Asynchronous'"/>
				</xsl:when>
			</xsl:choose>
			</emphasis>
			</para>
			</listitem>

			<listitem>
			<para>
			<emphasis role="allowcustomparams">
			<xsl:choose>
				<xsl:when test="@allowCustomParams='true'">
				<xsl:value-of select="'Custom Parameters Accepted'"/>
				</xsl:when>
				<xsl:when test="@allowCustomParams='false'">
				<xsl:value-of select="'No Custom Parameters'"/>
				</xsl:when>
			</xsl:choose>
			</emphasis>
			</para>
			</listitem>

            <xsl:if test="@final='true'"> <!-- decision was made to only show 'final' if it was true, because it is so rare and not really usable by others -->
                <listitem>
                    <para>
                        <emphasis role="final">
                            <xsl:choose>
                                <xsl:when test="@final='true'">
                                <xsl:value-of select="'Final'"/>
                                </xsl:when>
                                <xsl:when test="@final='false'">
                                <xsl:value-of select="'Not Final'"/>
                                </xsl:when>
                            </xsl:choose>
                        </emphasis>
                    </para>
                </listitem>
            </xsl:if>
		</itemizedlist>
      </para>
      
       <xsl:if test="count(pack:asyncCallback)>0">
       <emphasis role="asynccallbacks">Asynchronous Callbacks</emphasis>
		<itemizedlist>
        <xsl:for-each select="pack:asyncCallback">
            <listitem>
				<para>
					<link><xsl:attribute name="linkend"><xsl:value-of select="text()"/></xsl:attribute><xsl:value-of select="text()"/></link>
				</para>
            </listitem>
        </xsl:for-each>
        </itemizedlist>
		</xsl:if>
      
      <!-- used to line break from float in properties list -->
      <para role="break"></para>

      <xsl:choose>
        <xsl:when test="starts-with(@description,'&lt;')">
            <xsl:value-of disable-output-escaping="yes" select="@description"/>
        </xsl:when>
        <xsl:when test="not(starts-with(@description,'&lt;'))">
          <formalpara role="description">
            <title>Description</title>
            <para>
              <xsl:value-of disable-output-escaping="yes" select="@description"/>
            </para>
          </formalpara>
        </xsl:when>
      </xsl:choose>

      <xsl:if test="count(pack:actionParam)>0">
        <table class="actionparams">
            <xsl:attribute name="id">
                <xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />.ActionParams
            </xsl:attribute>

            <caption>Action Parameters</caption>

            <tr>
                <th>Parameter Name</th>

                <th>.NET Type</th>

                <th>Default</th>

                <th>Description</th>
            </tr>

            <xsl:apply-templates select="pack:actionParam">
                <xsl:with-param name="packagename" select="$packagename" />
                <xsl:with-param name="actionname" select="@name" />
            </xsl:apply-templates>

        </table>
      </xsl:if>
      <xsl:if test="count(pack:actionParam)=0">
          <para role="noactionparams">No Action Parameters</para>
      </xsl:if>

      <xsl:if test="count(pack:resultData)>0">
      <table class="resultdata">
        <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />.ResultData</xsl:attribute>
        <caption>Result Data</caption>

        <tr>
            <th>Parameter Name</th>

            <th>.NET Type</th>

            <th>Description</th>
        </tr>

        <xsl:apply-templates select="pack:resultData">
            <xsl:with-param name="packagename" select="$packagename" />
            <xsl:with-param name="actionname" select="@name" />
        </xsl:apply-templates>

      </table>
      </xsl:if>
      <xsl:if test="count(pack:resultData)=0">
          <para role="noresultdata">No Result Data</para>
      </xsl:if>

      <xsl:if test="count(pack:returnValue/pack:EnumItem)>0">
      <formalpara role="branch">
        <title>Branch Conditions</title>
        <para>
          <variablelist>
            <xsl:apply-templates select="pack:returnValue"/>
          </variablelist>
        </para>
      </formalpara>
      </xsl:if>
      <xsl:if test="count(pack:returnValue/pack:EnumItem)=0">
          <para role="nobranches">No Defined Branch Conditions</para>
      </xsl:if>
  
    </section>
  </xsl:template>

  <xsl:template name="actionParam" match="pack:actionParam">
    <xsl:param name="packagename" />
    <xsl:param name="actionname" />
    <tr>
      <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="$actionname" />.ActionParams.<xsl:value-of select="@name" /></xsl:attribute>

      <xsl:if test="@use='required'"><xsl:attribute name="role"><xsl:value-of select="'req'" /></xsl:attribute></xsl:if>
      
      <td class="actionparamdisplayname">
        <xsl:value-of select="@displayName" disable-output-escaping="yes"/><xsl:if test="@use='required'"> *</xsl:if>
      </td>

      <td class="actionparamtype">
        <ulink>
          <xsl:attribute name="url">http://msdn2.microsoft.com/en-us/library/<xsl:value-of select="@type" /></xsl:attribute>
          <code>
            <xsl:value-of select="@type" disable-output-escaping="yes"/>
          </code>
        </ulink>
      </td>

      <td class="actionparamdefault">
        <xsl:choose>
          <xsl:when test="@defaultValue">
          <xsl:value-of select="@defaultValue" disable-output-escaping="yes"/>
          </xsl:when>
          <xsl:otherwise></xsl:otherwise>
        </xsl:choose>
      </td>

      <td class="actionparamdescription">
        <xsl:value-of select="@description" disable-output-escaping="yes"/>
      </td>
    </tr>
  </xsl:template>
  
  <xsl:template name="resultData" match="pack:resultData">
    <xsl:param name="packagename" />
    <xsl:param name="actionname" />
    <tr>
      <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="$actionname" />.ResultData.<xsl:value-of select="." /></xsl:attribute>
      <td class="resultdatadisplayname">
        <xsl:value-of select="@displayName" disable-output-escaping="yes"/>
      </td>

      <td class="resultdatatype">
        <ulink>
        <xsl:attribute name="url">http://msdn2.microsoft.com/en-us/library/<xsl:value-of select="@type" /></xsl:attribute>
        <code>
          <xsl:value-of select="@type" disable-output-escaping="yes"/>
        </code>
        </ulink>
      </td>

      <td class="resultdatadescription">
        <xsl:value-of select="@description" disable-output-escaping="yes"/>
      </td>
    </tr>
  </xsl:template>

  <xsl:template name="branches" match="pack:returnValue">
    <xsl:for-each select="pack:EnumItem">
      <varlistentry>
        <term>
          <xsl:value-of select="." disable-output-escaping="yes"/>
        </term>
        <listitem>
          <para>No description.</para>
        </listitem>
      </varlistentry>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="pack:eventList">
    <xsl:param name="packagename" />
    <section role="events">
      <xsl:attribute name="id"><xsl:value-of select="$packagename" />.Events</xsl:attribute>
      <title>Events</title>
      <xsl:apply-templates select="pack:event">
        <xsl:with-param name="packagename" select="$packagename" />
      </xsl:apply-templates>
    </section>
  </xsl:template>
  
  <xsl:template match="pack:event">
    <xsl:param name="packagename" />
    <section role="event">
      <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="@name" /></xsl:attribute>
      <title role="eventdisplayname">
        <xsl:value-of select="@displayName" disable-output-escaping="yes"/>
      </title>

      <subtitle role="eventname">
        <xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />
      </subtitle>

      <para role="eventproperties">
        <itemizedlist>
          <listitem>
            <para>
              <emphasis role="eventtype">
                <xsl:choose>
                  <xsl:when test="@type='triggering'">
                    <xsl:value-of select="'Triggering'"/>
                  </xsl:when>
                  <xsl:when test="@type='nontriggering'">
                    <xsl:value-of select="'Non-Triggering'"/>
                  </xsl:when>
                  <xsl:when test="@type='asyncCallback'">
                    <xsl:value-of select="'Asynchronous Callback'"/>
                  </xsl:when>
                </xsl:choose>
              </emphasis>
            </para>
          </listitem>

        </itemizedlist>
      </para>

      <!-- used to line break from float in properties list -->
      <para role="break"></para>

      <xsl:choose>
        <xsl:when test="starts-with(@description,'&lt;')">
          <xsl:value-of disable-output-escaping="yes" select="@description"/>
        </xsl:when>
        <xsl:when test="not(starts-with(@description,'&lt;'))">
          <formalpara role="description">
            <title>Description</title>
            <para>
              <xsl:value-of disable-output-escaping="yes" select="@description"/>
            </para>
          </formalpara>
        </xsl:when>
      </xsl:choose>

      <xsl:if test="count(pack:eventParam)>0">
          <table class="eventparams">
            <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />.EventParams</xsl:attribute>
            <caption>Event Parameters</caption>
                <tr>
                  <th>Parameter Name</th>

                  <th>.NET Type</th>

                  <th>Description</th>
                </tr>

                <xsl:apply-templates select="pack:eventParam">
                  <xsl:with-param name="packagename" select="$packagename" />
                  <xsl:with-param name="eventname" select="@name" />
                </xsl:apply-templates>

          </table>
      </xsl:if>
      <xsl:if test="count(pack:eventParam)=0">
          <para role="noeventparams">No Event Parameters</para>
      </xsl:if>
      
    </section>
  </xsl:template>

  <xsl:template name="eventParam" match="pack:eventParam">
    <xsl:param name="packagename" />
    <xsl:param name="eventname" />
    <tr>
      <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="$eventname" />.EventParams.<xsl:value-of select="@name" /></xsl:attribute>
      <xsl:if test="@guaranteed='true'">
        <xsl:attribute name="role"><xsl:value-of select="'guaranteed'" /></xsl:attribute>
      </xsl:if>

      <td class="eventparamdisplayname">
        <xsl:value-of select="@displayName" disable-output-escaping="yes"/>
      </td>

      <td class="eventparamtype">
        <ulink>
          <xsl:attribute name="url">http://msdn2.microsoft.com/en-us/library/<xsl:value-of select="@type" /></xsl:attribute>
          <code>
            <xsl:value-of select="@type" disable-output-escaping="yes"/>
          </code>
        </ulink>
      </td>

      <td class="eventparamdescription">
        <xsl:value-of select="@description" disable-output-escaping="yes"/>
      </td>
    </tr>
  </xsl:template>


  <xsl:template match="nt:nativeTypePackage">
    <chapter>
      <xsl:attribute name="id"><xsl:value-of select="@name" /></xsl:attribute>
      <title id="packagedisplayname">
        <xsl:value-of select="@name" />
      </title>
      <subtitle id="packagename">
        <xsl:value-of select="@name" />
      </subtitle>
      <section role="types">
        <xsl:attribute name="id"><xsl:value-of select="@name" />.Types</xsl:attribute>
        <title>Types</title>
        <xsl:apply-templates select="nt:type">
          <xsl:with-param name="packagename" select="@name" />
        </xsl:apply-templates>
      </section>
    </chapter>
  </xsl:template>

  <xsl:template match="nt:type">
    <xsl:param name="packagename" />

    <section role="type">
      <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="@name" /></xsl:attribute>
      <title role="typedisplayname">
        <xsl:value-of select="@displayName" disable-output-escaping="yes"/>
      </title>

      <subtitle role="typename">
        <xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />
      </subtitle>

	<para role="typeproperties">
		<itemizedlist>
			<listitem><para>
			<emphasis role="serializable">
				<xsl:choose>
					<xsl:when test="@serializable='true'">
						<xsl:value-of select="'Serializable'"/>
					</xsl:when>
					<xsl:when test="@serializable='false'">
						<xsl:value-of select="'Not Serializable'"/>
					</xsl:when>
				</xsl:choose>
			</emphasis>
			</para>
			</listitem>

		</itemizedlist>
      </para>

      
      <!-- used to line break from float in properties list -->
      <para role="break"></para>


      <xsl:choose>
        <xsl:when test="starts-with(@description,'&lt;')">
          <xsl:value-of disable-output-escaping="yes" select="@description"/>
        </xsl:when>
        <xsl:when test="not(starts-with(@description,'&lt;'))">
          <formalpara role="description">
            <title>Description</title>
            <para>
              <xsl:value-of disable-output-escaping="yes" select="@description"/>
            </para>
          </formalpara>
        </xsl:when>
      </xsl:choose>

       <xsl:if test="count(nt:inputType)>0">
          <table class="parseableinputs">
            <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />.ParseableInputs</xsl:attribute>
              <caption>Parseable Inputs</caption>

                  <tr>
                    <th>.NET Type</th>

                    <th>Description</th>
                  </tr>

                  <xsl:apply-templates select="nt:inputType">
                    <xsl:with-param name="packagename" select="$packagename" />
                    <xsl:with-param name="typename" select="@name" />
                  </xsl:apply-templates>
            </table>
        </xsl:if>
        <xsl:if test="count(nt:inputType)=0">
            <para role="noinputs">No Parseable Inputs</para>
        </xsl:if>

        
        <xsl:if test="count(nt:customMethod)>0">
            <table class="publicmethods">
              <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />.PublicMethods</xsl:attribute>
              <caption>Accessible Public Methods</caption>

                  <tr>
                    <th>Method Name</th>

                    <th>Description</th>
                  </tr>

                <xsl:apply-templates select="nt:customMethod">
                    <xsl:with-param name="packagename" select="$packagename" />
                    <xsl:with-param name="typename" select="@name" />
                  </xsl:apply-templates>
            </table>
        </xsl:if>
        <xsl:if test="count(nt:customMethod)=0">
            <para role="nopublicmethods">No Public Methods</para>
        </xsl:if>

        <xsl:if test="count(nt:customProperty)>0">
            <table class="publicproperties">
              <xsl:attribute name="id"><xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />.PublicProperties</xsl:attribute>
              <caption>Accessible Public Properties</caption>

                  <tr>
                    <th>Property Name</th>
                    
                    <th>.NET Type</th>

                    <th>Description</th>
                  </tr>

                  <xsl:apply-templates select="nt:customProperty">
                    <xsl:with-param name="packagename" select="$packagename" />
                    <xsl:with-param name="typename" select="@name" />
                  </xsl:apply-templates>
            </table>
        </xsl:if>
        <xsl:if test="count(nt:customProperty)=0">
            <para role="nopublicproperties">No Public Properties</para>
        </xsl:if>

        <xsl:if test="count(nt:indexer)>0">
            <table class="publicindexers">
                <xsl:attribute name="id">
                    <xsl:value-of select="$packagename" />.<xsl:value-of select="@name" />.PublicIndexers
                </xsl:attribute>
                <caption>Accessible Public Indexer Properties</caption>

                <tr>
                    <th>.NET Indexer Type</th>

                    <th>.NET Return Type</th>

                    <th>Description</th>
                </tr>

                <xsl:apply-templates select="nt:indexer">
                    <xsl:with-param name="packagename" select="$packagename" />
                    <xsl:with-param name="typename" select="@name" />
                </xsl:apply-templates>
            </table>
        </xsl:if>


    </section>
  </xsl:template>

  <xsl:template name="inputs" match="nt:inputType">
    <xsl:param name="packagename" />
    <xsl:param name="typename" />
    
        <tr>
          <td class="inputtype">
              <code><xsl:value-of select="text()" disable-output-escaping="yes"/></code>
          </td>

          <td class="inputdescription">
              <xsl:value-of select="@description" disable-output-escaping="yes"/>
          </td>
        </tr>
      
  </xsl:template>

  <xsl:template name="publicmethods" match="nt:customMethod">
    <xsl:param name="packagename" />
    <xsl:param name="typename" />
    
    <tr>  
      <td class="publicmethodname">
        <xsl:value-of select="@name"/>
      </td>

      <td class="publicmethoddescription">
        <xsl:value-of select="@description" disable-output-escaping="yes"/>
      </td>
    </tr>
      
  </xsl:template>

  <xsl:template name="publicproperties" match="nt:customProperty">
    <xsl:param name="packagename" />
    <xsl:param name="typename" />
    
    <tr>
      <td class="publicpropertyname">
        <xsl:value-of select="text()" disable-output-escaping="yes"/>
      </td>
      
      <td class="publicpropertytype">
        <ulink>
          <xsl:attribute name="url">http://msdn2.microsoft.com/en-us/library/<xsl:value-of select="@returnType" /></xsl:attribute>
          <code>
            <xsl:value-of select="@returnType" disable-output-escaping="yes"/>
          </code>
        </ulink>
      </td>

      <td class="publicpropertydescription">
        <xsl:value-of select="@description" disable-output-escaping="yes"/>
      </td>
    </tr>
  </xsl:template>

  <xsl:template name="publicindexers" match="nt:indexer">
    <xsl:param name="packagename" />
    <xsl:param name="typename" />

    <tr>
        <td class="publicindexertype">
            <ulink>
                <xsl:attribute name="url">
                    http://msdn2.microsoft.com/en-us/library/<xsl:value-of select="@indexType" />
                </xsl:attribute>
                <code>
                    <xsl:value-of select="@indexType" disable-output-escaping="yes"/>
                </code>
            </ulink>
        </td>

        <td class="publicindexerreturntype">
            <ulink>
                <xsl:attribute name="url">
                    http://msdn2.microsoft.com/en-us/library/<xsl:value-of select="@returnType" />
                </xsl:attribute>
                <code>
                    <xsl:value-of select="@returnType" disable-output-escaping="yes"/>
                </code>
            </ulink>
        </td>

        <td class="publicindexerdescription">
            <xsl:value-of select="@description" disable-output-escaping="yes"/>
        </td>
    </tr>
</xsl:template>

 </xsl:stylesheet>