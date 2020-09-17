#!/bin/bash
az extension add --name application-insights
blobName="deployment-log.html"
echo '<!DOCTYPE html><html><head></head><body>' >> $blobName
echo '<h1>Deployment Log</h1>' >> deployment-log.html
echo '<p style="color:Tomato;">It is very important that you set the ACR Password into your GitHub secret called ACR_PASSWORD</p>' >> deployment-log.txt
source deployment_scripts/set_environment.sh
deployment_scripts/deploy_sirmione_web.sh
deployment_scripts/deploy_scorpio_api.sh
deployment_scripts/deploy_limone_api.sh
deployment_scripts/deploy_virgo.sh
deployment_scripts/deploy_libra.sh
deployment_scripts/deploy_zodiac.sh
echo '</body></html>' >> $blobName

# Upload the deployment log to the zodiac storage account 
resourceGroupName="$ZODIAC_ALIAS-rg"
storageAccountName=$(az storage account list -g $resourceGroupName --query [0].name -o tsv)
storageConnectionString=$(az storage account show-connection-string -n $storageAccountName -g $resourceGroupName --query connectionString -o tsv)
export AZURE_STORAGE_CONNECTION_STRING="$storageConnectionString"
az storage container create -n "results" --public-access off
az storage blob upload -c "results" -f $blobName -n $blobName
echo "Uploaded $blobName to storage account $storageAccountName"

# Generate a SAS Token for direct access to the deployment log
today=$(date +%F)T
tomorrow=$(date --date="1 day" +%F)T
startTime=$(date --date="-2 hour" +%T)Z
expiryTime=$(date --date="2 hour" +%T)Z
start="$today$startTime"
expiry="$tomorrow$expiryTime"
url=$(az storage blob url -c "results" -n $blobName -o tsv)
sas=$(az storage blob generate-sas -c "results" -n $blobName --permissions r -o tsv --expiry $expiry --https-only --start $start)
echo "link to deployment-log is $url?$sas"


