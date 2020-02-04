#!/bin/bash
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo         Deploying scorpio-api
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "SCORPIO_ALIAS: $SCORPIO_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo "DBADMIN_USER: $DBADMIN_USER"
echo "DBADMIN_USER_PASSWORD_DO_DIFFERENTLY: $DBADMIN_USER_PASSWORD_DO_DIFFERENTLY"
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
databaseConnectionString="Server=tcp:$dbServerName.database.windows.net;Database=$dbName;User ID=$DBADMIN_USER;Password=$DBADMIN_USER_PASSWORD_DO_DIFFERENTLY;Encrypt=True;Connection Timeout=30;"
dbServerName="${applicationName}-db-server"
dbName="${applicationName}-web-db"

echo ---Derived Variables
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Web App Name: $webAppName"
echo "Hosting Plan: $hostingPlanName"
echo "DB Server Name: $dbServerName"
echo "DB Name: $dbName"
echo "Database connection string: $databaseConnectionString"
echo

echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
# az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=$applicationName
   
echo "Creating app service $webAppName in $DEFAULT_LOCATION"
# az group deployment create -g $resourceGroupName \
#    --template-file scorpio-api/ArmTemplates/windows-webapp-template.json  \
#    --parameters webAppName=$webAppName hostingPlanName=$hostingPlanName appInsightsLocation=$DEFAULT_LOCATION \
#        sku="${appservice_webapp_sku}" databaseConnectionString="{$databaseConnectionString}"
