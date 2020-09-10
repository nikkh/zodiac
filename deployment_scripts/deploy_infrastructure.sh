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

blobName="deployment-log.txt"
cat $blobName

resourceGroupName="${ZODIAC_GENERATOR_ALIAS}-rg"
echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=zodiac Micrososervice=$applicationName PendingDelete=true

storageAccountName="${ZODIAC_GENERATOR_ALIAS}$RANDOM"
echo "Creating storage account $storageAccountName in $resourceGroupName"
az storage account create \
 --name $storageAccountName \
 --location $DEFAULT_LOCATION \
 --resource-group $resourceGroupName \
 --sku Standard_LRS

# We'll use this storage account to hold the log and secrets generated during infrastructure creation.
connectionString=$(az storage account show-connection-string -n $storageAccountName -g $resourceGroupName --query connectionString -o tsv)
export AZURE_STORAGE_CONNECTION_STRING=$connectionString

az storage container create -n "results" --public-access off
az storage blob upload -c "results" -f $blobName -n $blobName
expiry=$(date --date="1 day" +%F)
$url=$(az storage blob url -c "results" -n $blobName -o tsv)
$sas = az storage blob generate-sas -c "private" -n $blobName --permissions r -o tsv -expiry $expiry
echo "Click here to access a concise list of parameter that may be useful: $($url)?$($sas)".
