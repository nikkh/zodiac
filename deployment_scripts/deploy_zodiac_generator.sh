#!/bin/bash
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" 
echo         Deploying Zodiac Generator Infrastructure 
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "ZODIAC_GENERATOR_ALIAS: $ZODIAC_GENERATOR_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo
echo "starting deploy_zodiac_generator.sh" >> deployment-log.txt
# set local variables
# Derive as many variables as possible
applicationName="${ZODIAC_GENERATOR_ALIAS}"
resourceGroupName="${applicationName}-rg"
storageAccountName=${applicationName}$RANDOM
functionAppName="${applicationName}-gen-func"
acrName="${applicationName}acr"
planName="${applicationName}-plan"

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

echo "Creating azure container registry $acrName in $resourceGroupName"
az acr create -l $DEFAULT_LOCATION --sku basic -n $acrName --admin-enabled -g $resourceGroupName
acrUser=$(az acr credential show -n $acrName --query username -o tsv)
acrPassword=$(az acr credential show -n $acrName --query passwords[0].value -o tsv)
echo "ACR User Name: $acrUser" >> deployment-log.txt
echo "ACR Password: $acrPassword" >> deployment-log.txt

echo "Creating serverless function app $functionAppName in $resourceGroupName"
az functionapp plan create --resource-group $resourceGroupName --name $planName --location $DEFAULT_LOCATION --number-of-workers 1 --sku EP1 --is-linux
az functionapp create \
 --name $functionAppName \
  --storage-account $storageAccountName \
  --plan $planName \
  --resource-group $resourceGroupName \
  --functions-version 3 \
  --docker-registry-server-user $acrUser \
  --docker-registry-server-password $acrPassword \
  --runtime dotnet

echo "Updating App Settings for $functionAppName"
settings="ZodiacContext__MinimumThinkTimeInMilliseconds=1000 ZodiacContext__UserSimulationEnabled=false" 
#storageConnectionString="dummy-value"
az webapp config appsettings set -g $resourceGroupName -n $functionAppName --settings $settings
echo "TODO: change above command so that it doesnt output results or save to a variable and write to secure output"
echo "TODO: set ZodiacContext__UserTestingParametersStorageConnectionString in appsettings

