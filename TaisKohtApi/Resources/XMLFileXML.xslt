<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/restaurants">
    <xsl:for-each select="restaurant">
      <xsl:value-of select="name"/>
      <xsl:value-of select="@url"/>
      <!--<xsl:text>Kontakt number: </xsl:text>-->
      <xsl:value-of select="@contactnumber"/>
      <!--<xsl:text>email: </xsl:text>-->
      <xsl:value-of select="@email"/>
      <!--<xsl:text>Aadress: </xsl:text>-->
      <xsl:value-of select="address/addressfirstline"/>
      <!--<xsl:text>, </xsl:text>-->
      <xsl:value-of select="address/locality"/>
      <!--<xsl:text>, </xsl:text>-->
      <xsl:value-of select="address/postcode"/>
      <!--<xsl:text>, </xsl:text>-->
      <xsl:value-of select="address/@country"/>
    </xsl:for-each>


    
  </xsl:template>
</xsl:stylesheet>
