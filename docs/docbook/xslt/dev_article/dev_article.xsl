<?xml version='1.0'?>
<xsl:stylesheet  
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

<xsl:import href="file:///x:/tools/docbook-xsl-1.71.1/xhtml/docbook.xsl"/>
<xsl:param name="html.stylesheet" select="'api.css'"/>

<xsl:template match="void" mode="java">
  <span class="{name(.)}">
    <xsl:text>void&#160;</xsl:text>
  </span>
</xsl:template>

<xsl:template match="article">
  <xsl:call-template name="id.warning"/>

  <!-- <div class="devarticle"></div> -->
  
  <div class="{name(.)}">
    <xsl:call-template name="dir">
      <xsl:with-param name="inherit" select="1"/>
    </xsl:call-template>
    <xsl:call-template name="language.attribute"/>
    <xsl:if test="$generate.id.attributes != 0">
      <xsl:attribute name="id">
        <xsl:call-template name="object.id"/>
      </xsl:attribute>
    </xsl:if>

    <xsl:call-template name="article.titlepage"/>

    <xsl:variable name="toc.params">
      <xsl:call-template name="find.path.params">
        <xsl:with-param name="table" select="normalize-space($generate.toc)"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:call-template name="make.lots">
      <xsl:with-param name="toc.params" select="$toc.params"/>
      <xsl:with-param name="toc">
        <xsl:call-template name="component.toc">
          <xsl:with-param name="toc.title.p" select="contains($toc.params, 'title')"/>
        </xsl:call-template>
      </xsl:with-param>
    </xsl:call-template>

    <xsl:apply-templates/>
    <xsl:call-template name="process.footnotes"/>
  </div>
</xsl:template>

<xsl:template name="header.navigation">
  <xsl:param name="prev" select="/foo"/>
  <xsl:param name="next" select="/foo"/>
  <xsl:param name="nav.context"/>

  <xsl:variable name="home" select="/*[1]"/>
  <xsl:variable name="up" select="parent::*"/>

  <xsl:variable name="row1" select="$navig.showtitles != 0"/>
  <xsl:variable name="row2" select="count($prev) &gt; 0                                     or (count($up) &gt; 0                                          and generate-id($up) != generate-id($home)                                         and $navig.showtitles != 0)                                     or count($next) &gt; 0"/>

  <xsl:if test="$suppress.navigation = '0' and $suppress.header.navigation = '0'">
    <div class="navheader">
      <xsl:if test="$row1 or $row2">
        <table width="100%" summary="Navigation header">
          <xsl:if test="$row1">
            <tr>
              <th colspan="3" align="center">
                <xsl:apply-templates select="." mode="object.title.markup"/>
              </th>
            </tr>
          </xsl:if>

          <xsl:if test="$row2">
            <tr>
              <td width="20%" align="left">
                <xsl:if test="count($prev)&gt;0">
                  <a accesskey="p">
                    <xsl:attribute name="href">
                      <xsl:call-template name="href.target">
                        <xsl:with-param name="object" select="$prev"/>
                      </xsl:call-template>
                    </xsl:attribute>
                    <xsl:call-template name="navig.content">
                      <xsl:with-param name="direction" select="'prev'"/>
                    </xsl:call-template>
                  </a>
                </xsl:if>
                <xsl:text>&#160;</xsl:text>
              </td>
              <th width="60%" align="center">
                <xsl:choose>
                  <xsl:when test="count($up) &gt; 0                                   and generate-id($up) != generate-id($home)                                   and $navig.showtitles != 0">
                    <xsl:apply-templates select="$up" mode="object.title.markup"/>
                  </xsl:when>
                  <xsl:otherwise>&#160;</xsl:otherwise>
                </xsl:choose>
              </th>
              <td width="20%" align="right">
                <xsl:text>&#160;</xsl:text>
                <xsl:if test="count($next)&gt;0">
                  <a accesskey="n">
                    <xsl:attribute name="href">
                      <xsl:call-template name="href.target">
                        <xsl:with-param name="object" select="$next"/>
                      </xsl:call-template>
                    </xsl:attribute>
                    <xsl:call-template name="navig.content">
                      <xsl:with-param name="direction" select="'next'"/>
                    </xsl:call-template>
                  </a>
                </xsl:if>
              </td>
            </tr>
          </xsl:if>
        </table>
      </xsl:if>
      <xsl:if test="$header.rule != 0">
        <hr/> <hr/>
      </xsl:if>
    </div>
  </xsl:if>
</xsl:template>

<!-- This is only in here due to the fact that the header.navigation template calls it... 
as best as I can tell this must be in this file for XALAN to work -->
<xsl:template name="navig.content">
    <xsl:param name="direction" select="next"/>
    <xsl:variable name="navtext">
        <xsl:choose>
            <xsl:when test="$direction = 'prev'">
                <xsl:call-template name="gentext.nav.prev"/>
            </xsl:when>
            <xsl:when test="$direction = 'next'">
                <xsl:call-template name="gentext.nav.next"/>
            </xsl:when>
            <xsl:when test="$direction = 'up'">
                <xsl:call-template name="gentext.nav.up"/>
            </xsl:when>
            <xsl:when test="$direction = 'home'">
                <xsl:call-template name="gentext.nav.home"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:text>xxx</xsl:text>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:variable>

    <xsl:choose>
        <xsl:when test="$navig.graphics != 0">
            <img>
                <xsl:attribute name="src">
                    <xsl:value-of select="$navig.graphics.path"/>
                    <xsl:value-of select="$direction"/>
                    <xsl:value-of select="$navig.graphics.extension"/>
                </xsl:attribute>
                <xsl:attribute name="alt">
                    <xsl:value-of select="$navtext"/>
                </xsl:attribute>
            </img>
        </xsl:when>
        <xsl:otherwise>
            <xsl:value-of select="$navtext"/>
        </xsl:otherwise>
    </xsl:choose>
</xsl:template>

  
</xsl:stylesheet>
