#!/bin/bash

export DEFAULT_LOCATION=uksouth
export SIRMIONE_ALIAS=sirmioneg
export LIMONE_ALIAS=limoneg
export SCORPIO_ALIAS=scorpiog
export VIRGO_ALIAS=virgog
export LIBRA_ALIAS=librag
export ZODIAC_GENERATOR_ALIAS=zodgeng
export DB_ADMIN_USER=nick
echo "Creating Infrastructure using the following environment variables" >> deployment-log.txt
echo "DEFAULT_LOCATION:$DEFAULT_LOCATION" >> deployment-log.txt
echo "SIRMIONE_ALIAS:$SIRMIONE_ALIAS" >> deployment-log.txt
echo "LIMONE_ALIAS:$LIMONE_ALIAS" >> deployment-log.txt
echo "SCORPIO_ALIAS:$SCORPIO_ALIAS" >> deployment-log.txt
echo "VIRGO_ALIAS:$VIRGO_ALIAS" >> deployment-log.txt
echo "LIBRA_ALIAS:$LIBRA_ALIAS" >> deployment-log.txt
echo "ZODIAC_GENERATOR_ALIAS:$ZODIAC_GENERATOR_ALIAS" >> deployment-log.txt
echo "DB_ADMIN_USER:$DB_ADMIN_USER" >> deployment-log.txt
