# Deployment Scripts

## manual activities
After infrastructure and before any apps update the secret for acr password!!

AAD Client Id needs to be set in sirmone settings

Will also need to add reply url to aad application

update the zodiac-generator-config blob to have the user ids and passwords from AAD included

set zodiac appsettings ZodiacContext__UserSimulationEnabled=true


## errors
AZURE_STORAGE_CONNECTIONSTRING is not set for limone

still creating limone acr

