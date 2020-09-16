#!/bin/bash
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo         Deploying sirmione-web
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "SIRMIONE_ALIAS: $SIRMIONE_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo "DB_ADMIN_USER: $DB_ADMIN_USER"
echo "DB_ADMIN_PASSWORD: $DB_ADMIN_PASSWORD" 
echo "LIMONE_ALIAS: $LIMONE_ALIAS"
echo "SCORPIO_ALIAS: $SCORPIO_ALIAS"
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
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=zodiac MicrososerviceName=sirmione MicroserviceID=$applicationName PendingDelete=true

echo "Creating app service $webAppName in group $resourceGroupName"
 az group deployment create -g $resourceGroupName \
    --template-file sirmione-web/ArmTemplates/windows-webapp-sql-template.json  \
    --parameters webAppName=$webAppName hostingPlanName=$hostingPlanName appInsightsLocation=$DEFAULT_LOCATION \
        databaseServerName=$dbServerName databaseUsername=$DB_ADMIN_USER databasePassword=$DB_ADMIN_PASSWORD databaseLocation=$DEFAULT_LOCATION \
        databaseName=$dbName \
        sku="${appservice_webapp_sku}" databaseEdition=$database_edition

echo "Updating App Settings for $webAppName"
 az webapp config appsettings set -g $resourceGroupName -n $webAppName --settings LimoneBaseUrl=$limoneBaseUrl ScorpioBaseUrl=$scorpioBaseUrl AzureAD__Domain=$AAD_DOMAIN AzureAD__TenantId=$AAD_TENANTID AAD_CLIENTID=dummy-value
