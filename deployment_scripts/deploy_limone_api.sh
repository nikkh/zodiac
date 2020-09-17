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
acrRegistryName="${ZODIAC_ALIAS}acr"
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
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags Application=zodiac MicrososerviceName=limone MicroserviceID=$applicationName PendingDelete=true 

echo "Creating service bus namespace $serviceBusNamespace in group $resourceGroupName"
az servicebus namespace create -g $resourceGroupName -n $serviceBusNamespace
az servicebus queue create -g $resourceGroupName --namespace-name $serviceBusNamespace --name libra-queue
az servicebus queue create -g $resourceGroupName --namespace-name $serviceBusNamespace --name virgo-queue
serviceBusConnectionString=$(az servicebus namespace authorization-rule keys list -g $resourceGroupName --namespace-name $serviceBusNamespace -n RootManageSharedAccessKey --query 'primaryConnectionString' -o tsv)

echo "Creating storage account $storageAccountName in $resourceGroupName"
 az storage account create \
  --name $storageAccountName \
  --location $DEFAULT_LOCATION \
  --resource-group $resourceGroupName \
  --sku Standard_LRS
  
storageConnectionString=$(az storage account show-connection-string -n $storageAccountName -g $resourceGroupName --query connectionString -o tsv)

#echo "Creating azure container registry $acrRegistryName in group $resourceGroupName"
# az group deployment create -g $resourceGroupName \
#    --template-file limone-api/ArmTemplates/containerRegistry-template.json  \
#    --parameters registryName=$acrRegistryName registryLocation=$DEFAULT_LOCATION registrySku=$acrSku

echo "Creating app service $webAppName in group $resourceGroupName"
 az group deployment create -g $resourceGroupName \
    --template-file limone-api/ArmTemplates/container-webapp-template.json  \
    --parameters webAppName=$webAppName hostingPlanName=$hostingPlanName appInsightsLocation=$DEFAULT_LOCATION \
        sku="${appservice_webapp_sku}" registryName=$acrRegistryName imageName="$imageName" registryLocation="$DEFAULT_LOCATION" registrySku="$acrSku"

limoneAIKey=$(az monitor app-insights component show --app $webAppName -g $resourceGroupName --query instrumentationKey -o tsv)

echo "Updating App Settings for $webAppName"
az webapp config appsettings set -g $resourceGroupName -n $webAppName --settings AZURE__STORAGE__CONNECTIONSTRING=$storageConnectionString AZURE__SERVICEBUS__CONNECTIONSTRING=$serviceBusConnectionString ASPNETCORE_ENVIRONMENT=Development APPINSIGHTS_KEY=$limoneAIKey 
  
  

