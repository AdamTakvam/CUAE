<?xml version='1.0'?>
<xsl:stylesheet  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="file:///x:/tools/docbook-xsl-1.71.1/html/docbook.xsl"/>
	<xsl:param name="html.stylesheet" select="'api.css'"/>
	<xsl:param name="generate.id.attributes" select="1" />
	<xsl:param name="para.propagates.style" select="1" />
	<xsl:param name="emphasis.propagates.style" select="1" />
	<xsl:param name="entry.propagates.style" select="1" />
	<xsl:param name="phrase.propagates.style" select="1" />
	<xsl:param name="chunk.first.sections" select="1" />
	<xsl:param name="chunk.section.depth" select="2" />
	<xsl:param name="use.id.as.filename" select="1" />
	<!--<xsl:param name="root.filename" select="'CUAE-API-Reference-Guide'" />-->
	<!-- can't seem to make this work unless it's a param invoked on XSLT <xsl:param name="base.dir" select="c:/workspace/head/docs/cuae-developer-api-reference/obj/cuae-api-reference-guide/content/" />-->
	<xsl:param name="runinhead.default.title.end.punct" select="''" />
  <xsl:param name="generate.toc">
   appendix  nop
   article   toc,title
   book      toc,title
   chapter   toc
   part      nop
   preface   nop
   qandadiv  nop
   qandaset  nop
   reference toc,title
   section   nop
   set       toc
   </xsl:param>

<xsl:param name="local.l10n.xml" select="document('')"/>
<l:i18n xmlns:l="http://docbook.sourceforge.net/xmlns/l10n/1.0">
  <l:l10n language="en">
    <l:context name="title">
      <l:template name="table" text="%t"/>
    </l:context>    
  </l:l10n>
</l:i18n>
</xsl:stylesheet>
