# Deployment Scripts

## manual activities
After infrastructure and before any apps update the secret for acr password!!

Will also need to add reply url to aad application

update the zodiac-generator-config blob to have the user ids and passwords from AAD included

set zodiac appsettings ZodiacContext__UserSimulationEnabled=true

## Observations

baseurl in zodiac is wrong - needs to be sirmione-web.... etc

need to add trackexception to zodiac (exceptions are being traced)
 
need to create availability test in setup

** need to enaure application insights are set up correctly... for each component of the solution.




