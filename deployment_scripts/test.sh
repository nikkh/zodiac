#!/bin/bash
echo Environment Variables...
echo "ROOT_NAME: $GITHUB_ROOT_NAME"
echo "LOCATION: $GITHUB_LOCATION"
echo "LIMONE_BASE_URL: $GITHUB_LIMONE_BASE_URL"
echo "SKU: $GITHUB_SKU"
echo "DB USER: $GITHUB_DB_USER"
echo "DB PASSWORD: $GITHUB_DB_PASSWORD"

# Derive as many variables as possible
applicationName="${GITHUB_ROOT_NAME}"
webAppName="${GITHUB_ROOT_NAME}-web"
hostingPlanName="${webAppName}-plan"
dbServerName="${GITHUB_ROOT_NAME}-db-server"
dbName="${GITHUB_ROOT_NAME}-web-db"
resourceGroupName="${GITHUB_ROOT_NAME}-rg"


echo Derived Variables...
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Web App Name: $webAppName"
echo "Hosting Plan: $hostingPlanName"
echo "DB Server Name: $dbServerName"
echo "DB Name: $dbName"

az group create -l "$GITHUB_LOCATION" --n "$resourceGroupName" --tags  Application=$applicationName
az group deployment create -g $resourceGroupName --template-file sirmione-web/ArmTemplates/windows-webapp-sql-template.json --parameters '{
         "webAppName": {
            "value": "$webAppName"
        },
        "hostingPlanName": {
            "value": "$hostingPlanName"
        },
        "appInsightsLocation": {
            "value": "$GITHUB_LOCATION"
        },
        "databaseServerName": {
            "value": "$dbServerName"
        },
        "databaseUsername": {
            "value": "$GITHUB_DB_USER"
        },
        "databasePassword": {
            "value": "$GITHUB_DB_PASSWORD"
        },
        "databaseLocation": {
            "value": "$GITHUB_LOCATION"
        },
        "databaseName": {
            "value": "$dbName"
        }
       
    }'
