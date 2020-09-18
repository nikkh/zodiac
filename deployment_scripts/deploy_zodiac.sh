#!/bin/bash
echo "<h2>Zodiac Infrastructure</h2>" >> deployment-log.html
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" 
echo         Deploying Zodiac Infrastructure 
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "ZODIAC_ALIAS: $ZODIAC_ALIAS"
echo "SIRMIONE_ALIAS: $SIRMIONE_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo

# set local variables
# Derive as many variables as possible
applicationName="${ZODIAC_ALIAS}"
resourceGroupName="${applicationName}-rg"
storageAccountName=${applicationName}$RANDOM
functionAppName="${applicationName}-gen-func"
acrName="${applicationName}acr"
planName="${applicationName}-plan"

echo ---Derived Variables
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "Storage Account Name: $storageAccountName"
echo "Function App Name: $functionAppName"
echo "ACR Name: $acrName"

echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=zodiac MicrososerviceName=zodiac MicroserviceID=$applicationName PendingDelete=true -o none
echo "<p>Resource Group: $resourceGroupName</p>" >> deployment-log.html 

echo "Creating storage account $storageAccountName in $resourceGroupName"
az storage account create \
 --name $storageAccountName \
 --location $DEFAULT_LOCATION \
 --resource-group $resourceGroupName \
 --sku Standard_LRS -o none
echo "<p>Storage Account: $storageAccountName</p>" >> deployment-log.html 
connectionString=$(az storage account show-connection-string -n $storageAccountName -g $resourceGroupName --query connectionString -o tsv)
export AZURE_STORAGE_CONNECTION_STRING="$connectionString"
echo "<p>Storage Connection String: $connectionString</p>" >> deployment-log.html

echo "Creating azure container registry $acrName in $resourceGroupName"
az acr create -l $DEFAULT_LOCATION --sku basic -n $acrName --admin-enabled -g $resourceGroupName -o none
acrUser=$(az acr credential show -n $acrName --query username -o tsv)
acrPassword=$(az acr credential show -n $acrName --query passwords[0].value -o tsv)
echo "<p>ACR: $acrName</p>" >> deployment-log.html 
echo "<p>ACR User Name: $acrUser</p>" >> deployment-log.html 
echo '<p style="color:Tomato;">It is very important that you set the ACR Password into your GitHub secret called ACR_PASSWORD</p>' >> deployment-log.html
echo "<p>ACR Password: $acrPassword</p>" >> deployment-log.html 

echo "Creating serverless function app $functionAppName in $resourceGroupName"
az functionapp plan create --resource-group $resourceGroupName --name $planName --location $DEFAULT_LOCATION --number-of-workers 1 --sku EP1 --is-linux
echo "<p>App Service Plan: $planName</p>" >> deployment-log.html
az functionapp create \
 --name $functionAppName \
  --storage-account $storageAccountName \
  --plan $planName \
  --resource-group $resourceGroupName \
  --functions-version 3 \
  --docker-registry-server-user $acrUser \
  --docker-registry-server-password $acrPassword \
  --runtime dotnet -o none
echo "<p>Function App: $functionAppName</p>" >> deployment-log.html

# We'll use this storage account to hold external configuration for users and sessions in user simulation processing
az storage container create -n "zodiac-generator-config" --public-access off -o none
sampleGeneratorParameters='{"Users": [{"Id": "user1@tenant.onmicrosoft.com", "Password": "password"},{"Id": "user2@tenant.onmicrosoft.com","Password": "password"}],"Sessions": [{"Steps": ["capricorn-go-red", "cap021", "cap023", "cap024" ] }, { "Steps": [ "capricorn-go-rainbow", "cap013", "cap019", "cap006" ] },{ "Steps": [ "capricorn-go-blue", "cap003" ] }]}'
echo "$sampleGeneratorParameters" > GeneratorParameters.json
az storage blob upload -c "zodiac-generator-config" -f GeneratorParameters.json -n GeneratorParameters.json -o none
echo "<p>GeneratorParameters.json was written to $storageAccountName, container=zodiac-generator-config, blob=GeneratorParameters.json <b>!! You will need to edit GeneratorParameters.json</b>" >> deployment-log.html

sirmioneBaseUrl="https://$SIRMIONE_ALIAS-web.azurewebsites.net/home/"
settings="ZodiacContext__MinimumThinkTimeInMilliseconds=1000 ZodiacContext__UserSimulationEnabled=false ZodiacContext__UserTestingParametersStorageConnectionString=$connectionString ZodiacContext__BaseUrl=$sirmioneBaseUrl" 

echo "Updating App Settings for $functionAppName"
echo "<p>Function App Settings:" >> deployment-log.html
az webapp config appsettings set -g $resourceGroupName -n $functionAppName --settings $settings >> deployment-log.html 
echo "</p>" >> deployment-log.html

