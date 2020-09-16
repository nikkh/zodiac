#!/bin/bash

export DEFAULT_LOCATION=uksouth
export SIRMIONE_ALIAS=zxsirmione
export LIMONE_ALIAS=zxlimone
export SCORPIO_ALIAS=zxscorpio
export VIRGO_ALIAS=zxvirgo
export LIBRA_ALIAS=zxlibra
export ZODIAC_ALIAS=zxzodiac
export DB_ADMIN_USER=nick
export AAD_DOMAIN=xekina.onmicrosoft.com
export AAD_TENANTID=3bc03625-3a0a-48c5-8aa5-12f22e401fff
echo "Creating Infrastructure using the following environment variables" >> deployment-log.txt
echo "DEFAULT_LOCATION:$DEFAULT_LOCATION" >> deployment-log.txt
echo "SIRMIONE_ALIAS:$SIRMIONE_ALIAS" >> deployment-log.txt
echo "LIMONE_ALIAS:$LIMONE_ALIAS" >> deployment-log.txt
echo "SCORPIO_ALIAS:$SCORPIO_ALIAS" >> deployment-log.txt
echo "VIRGO_ALIAS:$VIRGO_ALIAS" >> deployment-log.txt
echo "LIBRA_ALIAS:$LIBRA_ALIAS" >> deployment-log.txt
echo "ZODIAC_ALIAS:$ZODIAC_ALIAS" >> deployment-log.txt
echo "DB_ADMIN_USER:$DB_ADMIN_USER" >> deployment-log.txt
