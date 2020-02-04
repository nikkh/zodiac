#!/bin/bash
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo         Deploying limone-api
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "LIMONE_ALIAS: $LIMONE_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo

# set local variables
appservice_webapp_sku="S1 Standard"
acrSku="Standard"
imageName='limoneapi:$(Build.BuildId)'
echo ---Local Variables
echo "App service Sku: $appservice_webapp_sku"
echo "Azure Container Registry Sku: $acrSku"
echo "Image name: $imageName"
echo 

# Derive as many variables as possible
applicationName="${LIMONE_ALIAS}"
webAppName="${applicationName}-api"
hostingPlanName="${applicationName}-plan"
resourceGroupName="${applicationName}-rg"
acrRegistryName="${applicationName}acr"
serviceBusNamespace="${applicationName}sb"
storageAccountName=${applicationName}$RANDOM

echo ---Derived Variables
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Web App Name: $webAppName"
echo "Hosting Plan: $hostingPlanName"
echo "ACR Name: $acrRegistryName"
echo "Service Bus Namespace: $serviceBusNamespace"
echo "Storage account name: $storageAccountName"
echo

echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags Application=zodiac Micrososervice=$applicationName PendingDelete=true

echo "Creating service bus namespace $serviceBusNamespace in group $resourceGroupName"
az servicebus namespace create -g $resourceGroupName -n $serviceBusNamespace

echo "Creating storage account $storageAccountName in $resourceGroupName"
 az storage account create \
  --name $storageAccountName \
  --location $DEFAULT_LOCATION \
  --resource-group $resourceGroupName \
  --sku Standard_LRS

echo "Creating azure container registry $acrRegistryName in group $resourceGroupName"
 az group deployment create -g $resourceGroupName \
    --template-file limone-api/ArmTemplates/containerRegistry-template.json  \
    --parameters registryName=$acrRegistryName registryLocation=$DEFAULT_LOCATION registrySku=$acrSku

echo "Creating app service $webAppName in group $resourceGroupName"
 az group deployment create -g $resourceGroupName \
    --template-file limone-api/ArmTemplates/container-webapp-template.json  \
    --parameters webAppName=$webAppName hostingPlanName=$hostingPlanName appInsightsLocation=$DEFAULT_LOCATION \
        sku="${appservice_webapp_sku}" registryName=$acrRegistryName imageName="$imageName" registryLocation="$DEFAULT_LOCATION" registrySku="$acrSku"

echo "Updating App Settings for $webAppName"
storageConnectionString="dummy-value"
serviceBusConnectionString="dummy-value"
 az webapp config appsettings set -g $resourceGroupName -n $webAppName --settings AZURE_STORAGE_CONNECTIONSTRING=$storageConnectionString \ 
  AZURE_SERVICEBUS_CONNECTIONSTRING=$serviceBusConnectionString 
