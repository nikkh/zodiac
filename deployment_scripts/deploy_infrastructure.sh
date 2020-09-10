#!/bin/bash
echo starting >> deployment-log.txt
source deployment_scripts/set_environment.sh
deployment_scripts/deploy_sirmione_web.sh
deployment_scripts/deploy_scorpio_api.sh
deployment_scripts/deploy_limone_api.sh
deployment_scripts/deploy_virgo.sh
deployment_scripts/deploy_libra.sh
deployment_scripts/deploy_zodiac_generator.sh
echo finished >> deployment-log.txt
cat deployment-log.txt
blobName="deployment-log.txt"
az storage container create -n "results" --public-access off
az storage blob upload -c "results" -f $blobName -n $blobName
expiry=$(date --date="1 day" +%F)
$url=$(az storage blob url -c "results" -n $blobName -o tsv)
$sas = az storage blob generate-sas -c "private" -n $blobName --permissions r -o tsv -expiry $expiry
echo "Click here to access a concise list of parameter that may be useful: $($url)?$($sas)".
