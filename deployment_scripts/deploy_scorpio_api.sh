#!/bin/bash
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo         Deploying scorpio-api
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "SCORPIO_ALIAS: $SCORPIO_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo "DB_ADMIN_USER: $DB_ADMIN_USER"
echo "DB_ADMIN_PASSWORD: $DB_ADMIN_PASSWORD"
echo

# set local variables
appservice_webapp_sku="S1 Standard"

echo ---Local Variables
echo "App service Sku: $appservice_webapp_sku"
echo 

# Derive as many variables as possible
applicationName="${SCORPIO_ALIAS}"
webAppName="${applicationName}-api"
hostingPlanName="${applicationName}-plan"
resourceGroupName="${applicationName}-rg"
databaseConnectionString="Server=tcp:$dbServerName.database.windows.net;Database=$dbName;User ID=$DB_ADMIN_USER;Password=$DB_ADMIN_PASSWORD;Encrypt=True;Connection Timeout=30;"
dbServerName="${applicationName}-db-server"
dbName="${applicationName}-web-db"
storageAccountName=${applicationName}$RANDOM

echo ---Derived Variables
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Web App Name: $webAppName"
echo "Hosting Plan: $hostingPlanName"
echo "DB Server Name: $dbServerName"
echo "DB Name: $dbName"
echo "Database connection string: $databaseConnectionString"
echo "Storage account name: $storageAccountName"
echo

echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
# az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=zodiac Micrososervice=$applicationName PendingDelete=true

echo "Creating storage account $storageAccountName in group $resourceGroupName"
 az storage account create \
  --name $storageAccountName \
  --location $DEFAULT_LOCATION \
  --resource-group $resourceGroupName \
  --sku Standard_LRS

echo "Creating app service $webAppName in group $resourceGroupName "
 az group deployment create -g $resourceGroupName \
    --template-file scorpio-api/ArmTemplates/windows-webapp-template.json  \
    --parameters webAppName=$webAppName hostingPlanName=$hostingPlanName appInsightsLocation=$DEFAULT_LOCATION \
        sku="${appservice_webapp_sku}" databaseConnectionString="{$databaseConnectionString}"

echo "Updating App Settings for $webAppName"
storageConnectionString="dummy-value"
serviceBusConnectionString="dummy-value"
databaseConnectionString="dummy-value"
 az webapp config appsettings set -g $resourceGroupName -n $webAppName \
 --settings AZURE_STORAGE_CONNECTIONSTRING=$storageConnectionString AZURE_SERVICEBUS_CONNECTIONSTRING=$serviceBusConnectionString AZURE_A3SSDEVDB_CONNECTIONSTRING=$databaseConnectionString
