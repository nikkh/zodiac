#!/bin/bash
echo "<h2>Sirmione Infrastructure</h2>" >> deployment-log.html
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
limoneBaseUrl="https://${LIMONE_ALIAS}-api.azurewebsites.net/"
scorpioBaseUrl="https://${SCORPIO_ALIAS}-api.azurewebsites.net/"

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
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  ZodiacInstance=$ZODIAC_INSTANCE Application=zodiac MicrososerviceName=sirmione MicroserviceID=$applicationName PendingDelete=true -o none
echo "<p>Resource Group: $resourceGroupName</p>" >> deployment-log.html

echo "Creating app service $webAppName in group $resourceGroupName"
 az group deployment create -g $resourceGroupName \
    --template-file sirmione-web/ArmTemplates/windows-webapp-sql-template.json  \
    --parameters webAppName=$webAppName hostingPlanName=$hostingPlanName appInsightsLocation=$DEFAULT_LOCATION \
        databaseServerName=$dbServerName databaseUsername=$DB_ADMIN_USER databasePassword=$DB_ADMIN_PASSWORD databaseLocation=$DEFAULT_LOCATION \
        databaseName=$dbName \
        sku="${appservice_webapp_sku}" databaseEdition=$database_edition -o none
echo "<p>App Service (Web App): $webAppName</p>" >> deployment-log.html

# sirmione application insights info
sirmioneAIKey=$(az monitor app-insights component show --app $webAppName -g $resourceGroupName --query instrumentationKey -o tsv)
# Attempt to get App Insights configured without the needd for the portal
APPLICATIONINSIGHTS_CONNECTION_STRING="InstrumentationKey=$sirmioneAIKey;"
APPINSIGHTS_INSTRUMENTATIONKEY=$sirmioneAIKey
ApplicationInsightsAgent_EXTENSION_VERSION='~2'

echo "Updating App Settings for $webAppName"
echo "<p>Web App Settings:" >> deployment-log.html
 az webapp config appsettings set -g $resourceGroupName -n $webAppName --settings LimoneBaseUrl=$limoneBaseUrl ScorpioBaseUrl=$scorpioBaseUrl ASPNETCORE_ENVIRONMENT=Development AzureAD__Domain=$AAD_DOMAIN AzureAD__TenantId=$AAD_TENANTID AzureAD__ClientId=$AAD_CLIENTID APPLICATIONINSIGHTS_CONNECTION_STRING=$APPLICATIONINSIGHTS_CONNECTION_STRING APPINSIGHTS_INSTRUMENTATIONKEY=$APPINSIGHTS_INSTRUMENTATIONKEY ApplicationInsightsAgent_EXTENSION_VERSION=$ApplicationInsightsAgent_EXTENSION_VERSION >> deployment-log.txt
echo "</p>" >> deployment-log.html

