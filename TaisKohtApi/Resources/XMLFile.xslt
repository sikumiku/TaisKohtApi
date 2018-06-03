<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="/restaurants">
    <html>
      <head>
        <title>Täis Kõht</title>
      </head>
      <body>
        <xsl:for-each select="restaurant">
          <h1 style="color:DodgerBlue">
            <xsl:value-of select="name"/>
          </h1>
          <div>
            <xsl:value-of select="@url"/>
          </div>
          <div>
            <xsl:text>Kontakt number: </xsl:text>
            <xsl:value-of select="@contactnumber"/>
          </div>
          <div>
            <xsl:text>email: </xsl:text>
            <xsl:value-of select="@email"/>
          </div>
          <div>
            <xsl:text>Aadress: </xsl:text>
            <xsl:value-of select="address/addressfirstline"/>
            <xsl:text>, </xsl:text>
            <xsl:value-of select="address/locality"/>
            <xsl:text>, </xsl:text>
            <xsl:value-of select="address/postcode"/>
            <xsl:text>, </xsl:text>
            <xsl:value-of select="address/@country"/>
          </div>

          <xsl:choose>
            <xsl:when test="menus/menu">
              <xsl:for-each select="menus/menu">
                <xsl:sort select="name"/>
                <h3>
                  <xsl:value-of select="name"/>
                </h3>

                <xsl:if test="activefrom != '' and activeto != ''">
                  <xsl:text> </xsl:text>
                  <xsl:value-of select="substring(activefrom,1,10)"/>
                  <xsl:text> kuni </xsl:text>
                  <xsl:value-of select="substring(activeto,1,10)"/>
                </xsl:if>

                <ul>
                  <xsl:for-each select="dishes/dish">
                    <xsl:sort select="title"/>

                    <xsl:if test="availablefrom != '' and availableto != ''">
                      <xsl:value-of select="substring(availablefrom,1,10)"/>
                      <xsl:text> kuni </xsl:text>
                      <xsl:value-of select="substring(availableto,1,10)"/>
                    </xsl:if>

                    <li>
                      <span style="font-weight:bold">
                        <xsl:value-of select="title"/>
                      </span>
                      <xsl:text> - </xsl:text>
                      <xsl:value-of select="description"/>
                      <xsl:text>  </xsl:text>
                      <span style="font-weight:bold">
                        <xsl:if test="@glutenfree='true'">
                          <xsl:text>G </xsl:text>
                        </xsl:if>
                        <xsl:if test="@lactosefree='true'">
                          <xsl:text>L </xsl:text>
                        </xsl:if>
                        <xsl:if test="@vegan='true'">
                          <xsl:text>V </xsl:text>
                        </xsl:if>
                      </span>
                    </li>

                    <xsl:for-each select="ingredients/ingredient">
                      <xsl:value-of select="name"/>
                      <xsl:if test="position()!=last()">
                        <xsl:text>, </xsl:text>
                      </xsl:if>
                    </xsl:for-each>

                    <p style="font-weight:bold">
                      <xsl:text>Hind: </xsl:text>
                      <xsl:choose>
                        <xsl:when test="@price!=''">
                          <xsl:value-of select="@price"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text>-</xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>
                      <xsl:text> / Päevapakkumise hind: </xsl:text>
                      <xsl:choose>
                        <xsl:when test="@dailyprice!=''">
                          <xsl:value-of select="@dailyprice"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text>-</xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>
                    </p>

                    <div style="background-color:LightGray;">
                      <xsl:text>Hinnang praele: </xsl:text>
                      <xsl:choose>
                        <xsl:when test="rating/@ratingvalue!=''">
                          <xsl:value-of select="rating/@ratingvalue"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text>Ei ole veel hinnatud!</xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>

                      <br/>
                      <xsl:for-each select="rating/comments/comment">
                        <xsl:value-of select="username"/>
                        <xsl:text>: </xsl:text>
                        <xsl:value-of select="commenttext"/>
                        <br/>
                      </xsl:for-each>
                    </div>
                    <br/>
                  </xsl:for-each>
                </ul>
              </xsl:for-each>
            </xsl:when>
            <xsl:otherwise>
              <ul>
                <xsl:for-each select="dishes/dish">
                  <xsl:sort select="title"/>

                  <xsl:if test="availablefrom != '' and availableto != ''">
                    <xsl:value-of select="substring(availablefrom,1,10)"/>
                    <xsl:text> kuni </xsl:text>
                    <xsl:value-of select="substring(availableto,1,10)"/>
                  </xsl:if>

                  <li>
                    <span style="font-weight:bold">
                      <xsl:value-of select="title"/>
                    </span>
                    <xsl:text> - </xsl:text>
                    <xsl:value-of select="description"/>
                    <xsl:text>  </xsl:text>
                    <span style="font-weight:bold">
                      <xsl:if test="@glutenfree='true'">
                        <xsl:text>G </xsl:text>
                      </xsl:if>
                      <xsl:if test="@lactosefree='true'">
                        <xsl:text>L </xsl:text>
                      </xsl:if>
                      <xsl:if test="@vegan='true'">
                        <xsl:text>V </xsl:text>
                      </xsl:if>
                    </span>
                  </li>

                  <xsl:for-each select="ingredients/ingredient">
                    <xsl:value-of select="name"/>
                    <xsl:if test="position()!=last()">
                      <xsl:text>, </xsl:text>
                    </xsl:if>
                  </xsl:for-each>

                  <p style="font-weight:bold">
                    <xsl:text>Hind: </xsl:text>
                    <xsl:choose>
                      <xsl:when test="@price!=''">
                        <xsl:value-of select="@price"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text>-</xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                    <xsl:text> / Päevapakkumise hind: </xsl:text>
                    <xsl:choose>
                      <xsl:when test="@dailyprice!=''">
                        <xsl:value-of select="@dailyprice"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text>-</xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </p>

                  <div style="background-color:LightGray;">
                    <xsl:text>Hinnang praele: </xsl:text>
                    <xsl:choose>
                      <xsl:when test="rating/@ratingvalue!=''">
                        <xsl:value-of select="rating/@ratingvalue"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text>Ei ole veel hinnatud!</xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>

                    <br/>
                    <xsl:for-each select="rating/comments/comment">
                      <xsl:value-of select="username"/>
                      <xsl:text>: </xsl:text>
                      <xsl:value-of select="commenttext"/>
                      <br/>
                    </xsl:for-each>
                  </div>
                  <br/>
                </xsl:for-each>
              </ul>
            </xsl:otherwise>
          </xsl:choose>
          
          <div style="background-color:LightGray;">
            <xsl:text>Hinnang restoranile: </xsl:text>
            <xsl:choose>
              <xsl:when test="rating/@ratingvalue!=''">
                <xsl:value-of select="rating/@ratingvalue"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>Ei ole veel hinnatud!</xsl:text>
              </xsl:otherwise>
            </xsl:choose>
            
            
            <br/>
            <xsl:for-each select="rating/comments/comment">
              <xsl:value-of select="username"/>
              <xsl:text>: </xsl:text>
              <xsl:value-of select="commenttext"/>
              <br/>
            </xsl:for-each>
          </div>

          <p>
            <xsl:text>*G - gluteenivaba, </xsl:text>
            <xsl:text>L - laktoosivaba, </xsl:text>
            <xsl:text>V - vegan </xsl:text>
          </p>
          
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>