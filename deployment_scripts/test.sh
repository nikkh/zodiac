#!/bin/bash
echo "GITHUB_ROOT_NAME: $GITHUB_ROOT_NAME"
echo "LIMONE_BASE_URL: $GITHUB_LIMONE_BASE_URL"
applicationName="${GITHUB_ROOT_NAME}"
webAppName="${GITHUB_ROOT_NAME}-web"
hostingPlanName="${webAppName}-plan"
echo "Web App Name: $webAppName"
echo "Hosting Plan: $hostingPlanName"
az group create --location $location --name $resouceGroupName  --tags  Application=$applicationName
#az group deployment create --resource-group <resource-group-name> --template-file <path-to-template>
