#!/bin/bash
sirmione_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.sirmione_alias')
limone_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.limone_alias')
limone_servicebus_namespace=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.limone_servicebus_namespace')
limone_storageaccount_name=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.limone_storageaccount_name')
scorpio_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.scorpio_alias')
scorpio_servicebus_namespace=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.scorpio_servicebus_namespace')
scorpio_storageaccount_name=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.scorpio_storageaccount_name')
virgo_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.virgo_alias')
libra_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.libra_alias')
echo "sirmione_alias: $sirmione_alias"
echo "limone_alias: $limone_alias"
echo "limone_servicebus_namespace: $limone_servicebus_namespace"
echo "limone_storageaccount_name: $limone_storageaccount_name"
echo "scorpio_alias: $scorpio_alias"
echo "scorpio_servicebus_namespace: $scorpio_servicebus_namespace"
echo "scorpio_storageaccount_name: $scorpio_storageaccount_name"
echo "virgo_alias: $virgo_alias"
echo "libra_alias: $libra_alias"

limone_rg="${limone_alias}-rg"
echo "limone_rg: $limone_rg"
az servicebus namespace create -g $limone-rg -n $limone_servicebus_namespace
az storage account create -g $limone-rg -n $limone_storageaccount_name

scorpio_rg="${scorpio_alias}-rg"
echo "scorpio_rg: $scorpio_rg"
az servicebus namespace create -g $scorpio-rg -n $scorpio_servicebus_namespace
az storage account create -g $scorpio-rg -n $scorpio_storageaccount_name
