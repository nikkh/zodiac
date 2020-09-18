#!/bin/bash
echo "<h2>Libra Infrastructure</h2>" >> deployment-log.html
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
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=zodiac MicrososerviceName=libra MicroserviceID=$applicationName PendingDelete=true -o none
echo "<p>Resource Group: $resourceGroupName</p>" >> deployment-log.html

echo "Creating storage account $storageAccountName in $resourceGroupName"
az storage account create \
--name $storageAccountName \
--location $DEFAULT_LOCATION \
--resource-group $resourceGroupName \
--sku Standard_LRS -o none
echo "<p>Storage Account: $storageAccountName</p>" >> deployment-log.html

echo "Creating function app $functionAppName in $resourceGroupName"
az functionapp create \
 --name $functionAppName \
 --storage-account $storageAccountName \
 --consumption-plan-location $DEFAULT_LOCATION \
 --resource-group $resourceGroupName \
 --functions-version 3 \
 --app-insights $limoneWebAppName \
 --app-insights-key $limoneAIKey -o none
 
echo "<p>Function App: $functionAppName</p>" >> deployment-log.html

# libra application insights info
# Attempt to get App Insights configured without the need for the portal
APPLICATIONINSIGHTS_CONNECTION_STRING="InstrumentationKey=$limoneAIKey;"
APPINSIGHTS_INSTRUMENTATIONKEY=$limoneAIKey
ApplicationInsightsAgent_EXTENSION_VERSION='~2'

echo "Updating App Settings for $functionAppName"
settings="ServiceBusConnection=$limoneServiceBusConnectionString APPLICATIONINSIGHTS_CONNECTION_STRING=$APPLICATIONINSIGHTS_CONNECTION_STRING APPINSIGHTS_INSTRUMENTATIONKEY=$APPINSIGHTS_INSTRUMENTATIONKEY ApplicationInsightsAgent_EXTENSION_VERSION=$ApplicationInsightsAgent_EXTENSION_VERSION"
echo "<p>Function App Settings:" >> deployment-log.html
az webapp config appsettings set -g $resourceGroupName -n $functionAppName --settings "$settings"  >> deployment-log.html
echo "</p>" >> deployment-log.html
