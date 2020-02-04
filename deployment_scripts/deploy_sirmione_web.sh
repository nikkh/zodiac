#!/bin/bash
echo Deploying sirmione-web...
echo "I dont do anything"
echo Global Variables...
echo "SIRMIONE_ALIAS: $SiRMIONE_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo "SKU: $GITHUB_SKU"
echo "DBADMIN_USER: $DBADMIN_USER"
echo "DBADMIN_USER_PASSWORD_DO_DIFFERENTLY: $DBADMIN_USER_PASSWORD_DO_DIFFERENTLY"

# set local variables
appservice_webapp_sku="S1 Standard"
database_edition="Basic"
echo local Variables...
echo "App service Sku: $appservice_webapp_sku"
echo "Database Edition: $database_edition"

# Derive as many variables as possible
applicationName="${SiRMIONE_ALIAS}"
webAppName="${applicationName}-web"
hostingPlanName="${applicationName}-plan"
dbServerName="${applicationName}-db-server"
dbName="${applicationName}-web-db"
resourceGroupName="${applicationName}-rg"

echo Derived Variables...
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Web App Name: $webAppName"
echo "Hosting Plan: $hostingPlanName"
echo "DB Server Name: $dbServerName"
echo "DB Name: $dbName"
echo
echo "I dont do anything else"

# az group create -l "$GITHUB_LOCATION" --n "$resourceGroupName" --tags  Application=$applicationName
   
# az group deployment create -g $resourceGroupName \
#    --template-file sirmione-web/ArmTemplates/windows-webapp-sql-template.json  \
#    --parameters webAppName=$webAppName hostingPlanName=$hostingPlanName appInsightsLocation=$GITHUB_LOCATION \
#        databaseServerName=$dbServerName databaseUsername=$GITHUB_DB_USER databasePassword=$GITHUB_DB_PASSWORD databaseLocation=$GITHUB_LOCATION \
#        databaseName=$dbName \
#        sku="${GITHUB_SKU}" databaseEdition=$database_edition
