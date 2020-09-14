#!/bin/bash
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" 
echo         Creating ACR Infrastructure 
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
echo ---Global Variables
echo "ZODIAC_ALIAS: $ZODIAC_ALIAS"
echo "DEFAULT_LOCATION: $DEFAULT_LOCATION"
echo
echo "starting create_acr_infra.sh" >> deployment-log.txt
# set local variables
# Derive as many variables as possible
applicationName="${ZODIAC_ALIAS}"
resourceGroupName="${applicationName}-rg"
acrName="${applicationName}acr"

echo ---Derived Variables
echo "Application Name: $applicationName"
echo "Resource Group Name: $resourceGroupName"
echo "ACR Name: $acrName"

echo "Creating resource group $resourceGroupName in $DEFAULT_LOCATION"
az group create -l "$DEFAULT_LOCATION" --n "$resourceGroupName" --tags  Application=zodiac Micrososervice=$applicationName PendingDelete=true

echo "Creating azure container registry $acrName in $resourceGroupName"
az acr create -l $DEFAULT_LOCATION --sku basic -n $acrName --admin-enabled -g $resourceGroupName
acrUser=$(az acr credential show -n $acrName --query username -o tsv)
acrPassword=$(az acr credential show -n $acrName --query passwords[0].value -o tsv)
echo "ACR User Name: $acrUser" >> deployment-log.txt
echo "ACR Password: $acrPassword" >> deployment-log.txt
echo "finished create_acr_infra.sh" >> deployment-log.txt
