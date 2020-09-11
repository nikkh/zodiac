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

resourceGroupName="${ZODIAC_GENERATOR_ALIAS}-rg"
echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=zodiac Micrososervice=$applicationName PendingDelete=true -o none

storageAccountName="${ZODIAC_GENERATOR_ALIAS}$RANDOM"
echo "Creating storage account $storageAccountName in $resourceGroupName"
az storage account create \
 --name $storageAccountName \
 --location $DEFAULT_LOCATION \
 --resource-group $resourceGroupName \
 --sku Standard_LRS -o none

connectionString=$(az storage account show-connection-string -n $storageAccountName -g $resourceGroupName --query connectionString -o tsv)
export AZURE_STORAGE_CONNECTION_STRING="$connectionString"
echo "Generator storage account: $storageAccountName" >> deployment-log.txt
echo "Generator storage account connection string: $connectionString" >> deployment-log.txt

# We'll use this storage account to hold external configuration for users and sessions in user simulation processing
az storage container create -n "zodiac-generator-config" --public-access off
sampleGeneratorParameters="{"Users": [{"Id": "user1@tenant.onmicrosoft.com", "Password": "password"},{"Id": "user2@tenant.onmicrosoft.com","Password": "password"}],"Sessions": [{"Steps": ["capricorn-go-red", "cap021", "cap023", "cap024" ] }, { "Steps": [ "capricorn-go-rainbow", "cap013", "cap019", "cap006" ] },{ "Steps": [ "capricorn-go-blue", "cap003" ] }]}"
echo "$sampleGeneratorParameters" > GeneratorParameters.json
az storage blob upload -c "zodiac-generator-config" -f GeneratorParameters.json -n GeneratorParameters.json
echo "GeneratorParameters.json was written to $storageAccountName, container=zodiac-generator-config, blob=GeneratorParameters.json" >> deployment-log.txt
echo "!! You will need to edit GeneratorParameters.json" >> deployment-log.txt
# We'll use this storage account to hold the log and secrets generated during infrastructure creation.

echo finished >> deployment-log.txt

az storage container create -n "results" --public-access off
az storage blob upload -c "results" -f $blobName -n $blobName
today=$(date +%F)T
longStartTime=$(date --date="-1 hour" +%T)
longExpiryTime=$(date --date="2 hour" +%T)
startTime=${longStartTime:0:5}Z
expiryTime=${longExpiryTime:0:5}Z
start="$today$startTime"
expiry="$today$expiryTime"
echo $start
expiry=$(date --date="1 day" +%F)
echo $expiry
url=$(az storage blob url -c "results" -n $blobName -o tsv)
sas=$(az storage blob generate-sas -c "private" -n $blobName --permissions r -o tsv --expiry $expiry --https-only --start $start)
echo "Click here to access a concise list of parameter that may be useful: $url?$sas".
