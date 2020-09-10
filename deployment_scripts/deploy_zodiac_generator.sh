#!/bin/bash
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo         Deploying Zodiac Generator Infrastructure
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "ZODIAC_GENERATOR_ALIAS: $ZODIAC_GENERATOR_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo
# set local variables
# Derive as many variables as possible
applicationName="${ZODIAC_GENERATOR_ALIAS}"
resourceGroupName="${applicationName}-rg"
storageAccountName=${applicationName}$RANDOM
functionAppName="${applicationName}-gen-func"
acrName="${applicationName}acr"

echo ---Derived Variables
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Storage Account Name: $storageAccountName"
echo "Function App Name: $functionAppName"
echo "ACR Name: $acrName"
echo

echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=zodiac Micrososervice=$applicationName PendingDelete=true

echo "Creating storage account $storageAccountName in $resourceGroupName"
az storage account create \
 --name $storageAccountName \
 --location $DEFAULT_LOCATION \
 --resource-group $resourceGroupName \
 --sku Standard_LRS

echo "Creating serverless function app $functionAppName in $resourceGroupName"
az functionapp create \
 --name $functionAppName \
  --storage-account $storageAccountName \
  --consumption-plan-location $DEFAULT_LOCATION \
  --resource-group $resourceGroupName

echo "Updating App Settings for $functionAppName"
#storageConnectionString="dummy-value"
#az webapp config appsettings set -g $resourceGroupName -n $functionAppName --settings AzureWebJobsStorage=$storageConnectionString

echo "Creating azure container registry $acrName in $resourceGroupName"
az acr create -l $DEFAULT_LOCATION --sku basic -n $acrName --admin-enabled -g $resourceGroupName
acrUser=$(az acr credential show -n $acrName --query username -o tsv)
acrPassword=$(az acr credential show -n $acrName --query passwords[0].value -o tsv)
echo "ACR User Name: $acrUser"
echo "ACR Password: $acrPassword"
