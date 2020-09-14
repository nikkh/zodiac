#!/bin/bash
echo starting >> deployment-log.txt
source deployment_scripts/set_environment.sh
deployment_scripts/deploy_sirmione_web.sh
deployment_scripts/deploy_scorpio_api.sh
deployment_scripts/deploy_limone_api.sh
deployment_scripts/deploy_virgo.sh
deployment_scripts/deploy_libra.sh
deployment_scripts/deploy_zodiac_generator.sh

echo ">>>> Temporary"
blobName="deployment-log.txt"
cat $blobName
echo "<<<< Temporary"


# We'll use this storage account to hold the log and secrets generated during infrastructure creation.

echo finished >> deployment-log.txt

az storage container create -n "results" --public-access off
az storage blob upload -c "results" -f $blobName -n $blobName
today=$(date +%F)T
startTime=$(date --date="-1 hour" +%T)Z
expiryTime=$(date --date="2 hour" +%T)Z
start="$today$startTime"
expiry="$today$expiryTime"
echo $start
expiry=$(date --date="1 day" +%F)
echo $expiry
url=$(az storage blob url -c "results" -n $blobName -o tsv)
sas=$(az storage blob generate-sas -c "private" -n $blobName --permissions r -o tsv --expiry $expiry --https-only --start $start)
echo "TODO: Check the SAS url below works!"
echo "Click here to access a concise list of parameter that may be useful: $url?$sas"

