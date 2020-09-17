#!/bin/bash
echo "Starting Libra Deploy..." >> deployment-log.txt
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo         Deploying Libra
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "LIBRA_ALIAS: $LIBRA_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo
# set local variables
# Derive as many variables as possible
applicationName="${LIBRA_ALIAS}"
resourceGroupName="${applicationName}-rg"
storageAccountName=${applicationName}$RANDOM
functionAppName="${applicationName}-func"

echo "LIMONE_ALIAS: $LIMONE_ALIAS"
limoneApplicationName="${LIMONE_ALIAS}"
limoneResourceGroupName="${limoneApplicationName}-rg"
limoneServiceBusNamespace="${limoneApplicationName}sb"
limoneServiceBusConnectionString=$(az servicebus namespace authorization-rule keys list -g $limoneResourceGroupName --namespace-name $limoneServiceBusNamespace -n RootManageSharedAccessKey --query 'primaryConnectionString' -o tsv)

# limone application insights info
limoneWebAppName=$limoneApplicationName-api
limoneAIKey=$(az monitor app-insights component show --app $limoneWebAppName -g $limoneResourceGroupName --query instrumentationKey -o tsv)


echo ---Derived Variables
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Storage Account Name: $storageAccountName"
echo "Function App Name: $functionAppName"
echo

echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION" >> deployment-log.txt
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=zodiac MicrososerviceName=libra MicroserviceID=$applicationName PendingDelete=true 

echo "Creating storage account $storageAccountName in $resourceGroupName"
echo "Creating storage account $storageAccountName in $resourceGroupName" >> deployment-log.txt
az storage account create \
--name $storageAccountName \
--location $DEFAULT_LOCATION \
--resource-group $resourceGroupName \
--sku Standard_LRS

echo "Creating function app $functionAppName in $resourceGroupName"
echo "Creating function app $functionAppName in $resourceGroupName" >> deployment-log.txt
az functionapp create \
 --name $functionAppName \
 --storage-account $storageAccountName \
 --consumption-plan-location $DEFAULT_LOCATION \
 --resource-group $resourceGroupName \
 --functions-version 3 \
 --app-insights $limoneWebAppName \
 --app-insights-key $limoneAIKey

echo "Updating App Settings for $functionAppName"
settings="ServiceBusConnection=$limoneServiceBusConnectionString"
az webapp config appsettings set -g $resourceGroupName -n $functionAppName --settings "ServiceBusConnection=$limoneServiceBusConnectionString" -o none
echo "Update settings for function app $functionAppName: $settings" >> deployment-log.txt
echo "Libra Deploy has completed." >> deployment-log.txt
