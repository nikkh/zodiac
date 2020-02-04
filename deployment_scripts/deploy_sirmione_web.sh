#!/bin/bash
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo         Deploying sirmione-web
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "SIRMIONE_ALIAS: $SIRMIONE_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo "DBADMIN_USER: $DBADMIN_USER"
echo "DBADMIN_USER_PASSWORD_DO_DIFFERENTLY: $DBADMIN_USER_PASSWORD_DO_DIFFERENTLY"
echo "LIMONE_ALIAS: $LIMONE_ALIAS"
echo "SCORPIO_ALIAS: $SCORPIO_ALIAS_ALIAS"
echo
# set local variables
appservice_webapp_sku="S1 Standard"
database_edition="Basic"
echo ---Local Variables
echo "App service Sku: $appservice_webapp_sku"
echo "Database Edition: $database_edition"
echo 
# Derive as many variables as possible
applicationName="${SIRMIONE_ALIAS}"
webAppName="${applicationName}-web"
hostingPlanName="${applicationName}-plan"
dbServerName="${applicationName}-db-server"
dbName="${applicationName}-web-db"
resourceGroupName="${applicationName}-rg"
limoneBaseUrl="${LIMONE_ALIAS}-api.azurewebsites.net"
scorpioBaseUrl="${SCORPIO_ALIAS}-api.azurewebsites.net"

echo ---Derived Variables
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Web App Name: $webAppName"
echo "Hosting Plan: $hostingPlanName"
echo "DB Server Name: $dbServerName"
echo "DB Name: $dbName"
echo "Limone base url: $limoneBaseUrl"
echo "Scorpio base url: $scorpioBaseUrl"
echo

echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
# az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=$applicationName

echo "Creating app service $webAppName in group $resourceGroupName"
# az group deployment create -g $resourceGroupName \
#    --template-file sirmione-web/ArmTemplates/windows-webapp-sql-template.json  \
#    --parameters webAppName=$webAppName hostingPlanName=$hostingPlanName appInsightsLocation=$DEFAULT_LOCATION \
#        databaseServerName=$dbServerName databaseUsername=$DBADMIN_USER databasePassword=$DBADMIN_USER_PASSWORD_DO_DIFFERENTLY databaseLocation=$DEFAULT_LOCATION \
#        databaseName=$dbName \
#        sku="${appservice_webapp_sku}" databaseEdition=$database_edition
