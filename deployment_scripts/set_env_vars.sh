#!/bin/bash
default_location=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.default_location')
sirmione_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.sirmione_alias')
limone_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.limone_alias')
limone_servicebus_namespace=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.limone_servicebus_namespace')
limone_storageaccount_name=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.limone_storageaccount_name')
scorpio_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.scorpio_alias')
scorpio_servicebus_namespace=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.scorpio_servicebus_namespace')
scorpio_storageaccount_name=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.scorpio_storageaccount_name')
virgo_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.virgo_alias')
libra_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.libra_alias')
limone_rg="${limone_alias}-rg"
scorpio_rg="${scorpio_alias}-rg"

export DEFAULT_LOCATION=$default_location
export SIRMIONE_ALIAS=$sirmione_alias
export LIMONE_ALIAS=$limone_alias
export LIMONE_SERVICEBUS_NAMESPACE=$limone_servicebus_namespace
export LIMONE_STORAGEACCOUNT_NAME=$limone_storageaccount_name
export SCORPIO_ALIAS=$scorpio_alias
export SCORPIO_SERVICEBUS_NAMESPACE=$scorpio_servicebus_namespace
export SCORPIO_STORAGEACCOUNT_NAME=$scorpio_storageaccount_name
export VIRGO_ALIAS=$$virgo_alias
export LIBRA_ALIAS=$libra_alias
export SIRMIONE_ALIAS=$sirmione_alias
export SCORPIO_RG=$scorpio_rg
