#!/bin/bash

export DEFAULT_LOCATION=uksouth
export SIRMIONE_ALIAS=sirmionec
export LIMONE_ALIAS=limonec
export SCORPIO_ALIAS=scorpioc
export VIRGO_ALIAS=virgoc
export LIBRA_ALIAS=librac
export ZODIAC_GENERATOR_ALIAS=zodgenc
export DB_ADMIN_USER=nick

echo "DEFAULT_LOCATION:$DEFAULT_LOCATION" >> deployment-log.txt
echo "SIRMIONE_ALIAS:$SIRMIONE_ALIAS" >> deployment-log.txt
echo "LIMONE_ALIAS:$LIMONE_ALIAS" >> deployment-log.txt
echo "SCORPIO_ALIAS:$SCORPIO_ALIAS" >> deployment-log.txt
echo "VIRGO_ALIAS:$VIRGO_ALIAS" >> deployment-log.txt
echo "LIBRA_ALIAS:$LIBRA_ALIAS" >> deployment-log.txt
echo "ZODIAC_GENERATOR_ALIAS:$ZODIAC_GENERATOR_ALIAS" >> deployment-log.txt
echo "DB_ADMIN_USER:$DB_ADMIN_USER" >> deployment-log.txt
