#!/bin/bash

export DEFAULT_LOCATION=uksouth
export SIRMIONE_ALIAS=zsirmione
export LIMONE_ALIAS=zlimone
export SCORPIO_ALIAS=zscorpio
export VIRGO_ALIAS=zvirgo
export LIBRA_ALIAS=zlibra
export ZODIAC_ALIAS=zzodiac
export DB_ADMIN_USER=nick
export AAD_DOMAIN=xekina.onmicrosoft.com
export AAD_TENANTID=3bc03625-3a0a-48c5-8aa5-12f22e401fff
export AAD_CLIENTID=ccee7608-940c-42d9-ba86-a2845ef3a808
echo "<h1>Environment Variables</h1>" >> deployment-log.html
echo "DEFAULT_LOCATION:$DEFAULT_LOCATION" >> deployment-log.html
echo "SIRMIONE_ALIAS:$SIRMIONE_ALIAS" >> deployment-log.html
echo "LIMONE_ALIAS:$LIMONE_ALIAS" >> deployment-log.html
echo "SCORPIO_ALIAS:$SCORPIO_ALIAS" >> deployment-log.html
echo "VIRGO_ALIAS:$VIRGO_ALIAS" >> deployment-log.html
echo "LIBRA_ALIAS:$LIBRA_ALIAS" >> deployment-log.html
echo "ZODIAC_ALIAS:$ZODIAC_ALIAS" >> deployment-log.html
echo "DB_ADMIN_USER:$DB_ADMIN_USER" >> deployment-log.html
