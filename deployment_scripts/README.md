# Deployment Scripts

## manual activities
After infrastructure and before any apps update the secret for acr password!!

Will also need to add reply url to aad application

update the zodiac-generator-config blob to have the user ids and passwords from AAD included

set zodiac appsettings ZodiacContext__UserSimulationEnabled=true

## Observations

 ASPNETCORE_ENVIRONMENT=Development in Sirmione, Scorpio, Limone, Virgo and Libra
 
 Base Urls in sirmione not set correctly

 Database connection string in scorpoi sets db name as scorpio zscorpio-db-server.database.windows.net not zsirmione-db-server.database.windows.net
 
 need to create availability test in setup


