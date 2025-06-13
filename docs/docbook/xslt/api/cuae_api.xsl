<?xml version='1.0'?>
<xsl:stylesheet  
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

<xsl:import href="file:///x:/tools/docbook-xsl-1.71.1/html/docbook.xsl"/>
<xsl:param name="html.stylesheet" select="'api.css'"/>


<xsl:template mode="java" match="methodsynopsis">
  <xsl:variable name="start-modifiers" select="modifier[following-sibling::*[name(.) != 'modifier']]"/>
  <xsl:variable name="notmod" select="*[name(.) != 'modifier']"/>
  <xsl:variable name="end-modifiers" select="modifier[preceding-sibling::*[name(.) != 'modifier']]"/>
  <xsl:variable name="decl">
    <xsl:if test="parent::classsynopsis">
      <xsl:text>&#160;&#160;</xsl:text>
    </xsl:if>
    <xsl:apply-templates select="$start-modifiers" mode="java"/>
    
    <!-- type -->
    <xsl:if test="name($notmod[1]) != 'methodname'">
      <xsl:apply-templates select="$notmod[1]" mode="java"/>
    </xsl:if>

    <xsl:apply-templates select="methodname" mode="java"/>
  </xsl:variable>

  <div class="{name(.)}">
    <xsl:copy-of select="$decl"/>
    <xsl:apply-templates select="methodparam" mode="java">
      <xsl:with-param name="indent" select="string-length($decl)"/>
    </xsl:apply-templates>
    <xsl:if test="exceptionname">
      <br/>
      <xsl:text>&#160;&#160;&#160;&#160;throws&#160;</xsl:text>
      <xsl:apply-templates select="exceptionname" mode="java"/>
    </xsl:if>
    <xsl:if test="modifier[preceding-sibling::*[name(.) != 'modifier']]">
      <xsl:text> </xsl:text>
      <xsl:apply-templates select="$end-modifiers" mode="java"/>
    </xsl:if>
  </div>
  <xsl:call-template name="synop-break"/>
</xsl:template>

<xsl:template match="type" mode="java">
  <code class="{name(.)}">
    <xsl:apply-templates mode="java"/>
    <xsl:text>&#160;</xsl:text>
  </code>
</xsl:template>
	
<xsl:template match="varname" mode="java">
 <xsl:variable name="choice" select="../../@choice" />
  <span class="{name(.)}">
    <xsl:apply-templates mode="java"/>
    <xsl:choose>
		<xsl:when test="$choice='plain'">
		</xsl:when>
		<xsl:when test="$choice='req'">
			<b>*</b>
		</xsl:when>
		<xsl:when test="$choice='opt'">
		</xsl:when>
		<xsl:otherwise>
		</xsl:otherwise>
	</xsl:choose>
  </span>
</xsl:template>

<xsl:template match="initializer" mode="java">
  <span class="{name(.)}">
    <xsl:text>=&#160;</xsl:text>
    <xsl:apply-templates mode="java"/>
  </span>
</xsl:template>

<xsl:template match="void" mode="java">
  <span class="{name(.)}">
    <xsl:text>void&#160;</xsl:text>
  </span>
</xsl:template>

<xsl:template match="methodname" mode="java">
  <div class="{name(.)}">
    <xsl:apply-templates mode="java"/>
  </div>
</xsl:template>

<xsl:template match="methodparam" mode="java">
  <xsl:param name="indent">0</xsl:param>
  <xsl:variable name="choice" select="@choice"/>
  <div class="{name(.)}">
    <xsl:apply-templates mode="java"/>
  </div>
</xsl:template>

<xsl:template match="parameter" mode="java">
  <span class="{name(.)}">
    <xsl:apply-templates mode="java"/>
  </span>
</xsl:template>

<xsl:template match="parameter/description/text()" mode="java"> <!-- put a div around that  -->
  <div class="description">
    <xsl:value-of select="self::text()" />
  </div>
</xsl:template>

<xsl:template match="modifier[@role='direction']" mode="java">
  <span class="{name(.)}">
    <xsl:if test="node() = 'in'">
		<xsl:text>&#8594;</xsl:text>
    </xsl:if>
    <xsl:if test="node() = 'out'">
		<xsl:text>&#8592;</xsl:text>
    </xsl:if>
  </span>
</xsl:template>

</xsl:stylesheet>
