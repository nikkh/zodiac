#!/bin/bash
echo "<h2>Virgo Infrastructure</h2>" >> deployment-log.html
echo "Starting Virgo Deploy..." >> deployment-log.txt
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo         Deploying Virgo
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "VIRGO_ALIAS: $VIRGO_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo
# set local variables
# Derive as many variables as possible
applicationName="${VIRGO_ALIAS}"
resourceGroupName="${applicationName}-rg"
storageAccountName=${applicationName}$RANDOM
functionAppName="${applicationName}-func"

echo "LIMONE_ALIAS: $LIMONE_ALIAS"
limoneApplicationName="${LIMONE_ALIAS}"
limoneResourceGroupName="${limoneApplicationName}-rg"
limoneServiceBusNamespace="${limoneApplicationName}sb"
limoneServiceBusConnectionString=$(az servicebus namespace authorization-rule keys list -g $limoneResourceGroupName --namespace-name $limoneServiceBusNamespace -n RootManageSharedAccessKey --query 'primaryConnectionString' -o tsv)

echo ---Derived Variables
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Storage Account Name: $storageAccountName"
echo "Function App Name: $functionAppName"
echo

echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  ZodiacInstance=$ZODIAC_INSTANCE Application=zodiac MicrososerviceName=virgo MicroserviceID=$applicationName PendingDelete=true >> deployment-log.html
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
 --functions-version 3 -o none
echo "<p>Function App: $functionAppName</p>" >> deployment-log.html

# virgo application insights info
# virgoAIKey=$(az monitor app-insights component show --app $functionAppName -g $resourceGroupName --query instrumentationKey -o tsv)
az functionapp config appsettings delete --name $functionAppName --resource-group $resourceGroupName --setting-names APPINSIGHTS_INSTRUMENTATIONKEY APPLICATIONINSIGHTS_CONNECTION_STRING -o none
az monitor app-insights component delete --app $functionAppName -g $resourceGroupName ->> deployment-log.html
#

echo "Updating App Settings for $functionAppName"
settings="ServiceBusConnection=$limoneServiceBusConnectionString"
echo "<p>Function App Settings:" >> deployment-log.html

az webapp config appsettings set -g $resourceGroupName -n $functionAppName --settings "$settings"  >> deployment-log.html
echo "</p>" >> deployment-log.html
