# Deployment Scripts

## manual activities
After infrastructure and before any apps update the secret for acr password!!

Will also need to add reply url to aad application

update the zodiac-generator-config blob to have the user ids and passwords from AAD included

set zodiac appsettings ZodiacContext__UserSimulationEnabled=true

## Observations



need to add trackexception to zodiac (exceptions are being traced)
 
need to create availability test in setup

### need to enaure application insights are set up correctly... for each component of the solution.

HEre's some preliminary notes: flesh these out

#### Libra

Does have an appinsights key, but no other settings for ai.
no data is flowing thorugh to libra-ai. it should be using limone
config key is fb477479-3052-4891-915c-aef256b9f794 it should be 18761362-129e-4466-b89a-a56c1162d9b6
stop this libra ne from being created (or delete it).
no ai packages are present in the csproj.

manually added appinsights (limone) via the portal. APPLICATIONINSIGHTS_CONNECTION_STRING

> app insights lights up then

#### virgo

same as above but key in config is 94433157-a399-4f60-b761-3bf4c0d43922.  this is the virgo one 94433157-a399-4f60-b761-3bf4c0d43922. again no data is flowing through
no ai packages are present in the csproj.

*What happens when I add ai in the portal?* APPLICATIONINSIGHTS_CONNECTION_STRING added

check what dlls get deployed via kudu:

> app insights lights up then

#### limone

no ai config present

in the csproj are: ms.ai.AspNetCore, profiler and snapshot collector

you cant configure for a linux web application containers via portal.

the exising limine settings are:- AppInsights_InstrumentationKey

added the limone key as per existing settings:  

original limone dockerfile had 

# Light up Application Insights and Service Profiler
#ENV APPINSIGHTS_INSTRUMENTATIONKEY $APPINSIGHTS_KEY
#ENV ASPNETCORE_HOSTINGSTARTUPASSEMBLIES Microsoft.ApplicationInsights.Profiler.AspNetCore

probably needs t go back in?

# Create an argument to allow docker builder to passing in application insights key.
# For example: docker build . --build-arg APPINSIGHTS_KEY=YOUR_APPLICATIONINSIGHTS_INSTRUMENTATION_KEY
#ARG APPINSIGHTS_KEY
# Making sure the argument is set. Fail the build of the container otherwise.
#RUN test -n "$APPINSIGHTS_KEY"

# Light up Application Insights and Service Profiler
#ENV APPINSIGHTS_INSTRUMENTATIONKEY $APPINSIGHTS_KEY
#ENV ASPNETCORE_HOSTINGSTARTUPASSEMBLIES Microsoft.ApplicationInsights.Profiler.AspNetCore













