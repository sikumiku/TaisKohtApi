<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/restaurants">
    <restoranid>
      <xsl:for-each select="restaurant">
        <restoran url="{@url}" telefon="{@contactnumber}" email="{@email}">
          <nimi>
            <xsl:value-of select="name"/>
          </nimi>
          <aadress>
            <postiaadress>
              <xsl:value-of select="address/addressfirstline"/>
            </postiaadress>
            <linn>
              <xsl:value-of select="address/locality"/>
            </linn>
            <indeks>
              <xsl:value-of select="address/postcode"/>
            </indeks>
            <riik>
              <xsl:value-of select="address/@country"/>
            </riik>
          </aadress>

          <parimadToidud>
            <xsl:choose>
              <xsl:when test="menus/menu/dishes/dish">
                <xsl:for-each select="menus/menu/dishes/dish">
                  <xsl:if test="rating/@ratingvalue &gt;= 8.0">
                    <heaToit hinnang="{rating/@ratingvalue}" paevaPakkumiseHind="{@dailyprice}" hind="{@price}" 
                             vegan="{@vegan}" laktoosivaba="{@lactosefree}" gluteenivaba="{@glutenfree}">
                      <nimi>
                        <xsl:value-of select="title"/>
                      </nimi>
                      <kirjeldus>
                        <xsl:value-of select="description"/>
                      </kirjeldus>
                    </heaToit>
                  </xsl:if>
                </xsl:for-each>
              </xsl:when>
              <xsl:otherwise>
                <xsl:for-each select="dishes/dish">
                  <xsl:if test="rating/@ratingvalue &gt;= 8.0">
                    <heaToit hinnang="{rating/@ratingvalue}" paevaPakkumiseHind="{@dailyprice}" hind="{@price}" 
                             vegan="{@vegan}" laktoosivaba="{@lactosefree}" gluteenivaba="{@glutenfree}">
                      <nimi>
                        <xsl:value-of select="title"/>
                      </nimi>
                      <kirjeldus>
                        <xsl:value-of select="description"/>
                      </kirjeldus>
                    </heaToit>
                  </xsl:if>
                </xsl:for-each>
              </xsl:otherwise>
            </xsl:choose>
          </parimadToidud>
        </restoran>
      </xsl:for-each>
    </restoranid>
  </xsl:template>
</xsl:stylesheet>
