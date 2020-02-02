FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["limone-api.csproj", ""]
RUN dotnet restore "limone-api.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "limone-api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "limone-api.csproj" -c Release -o /app

FROM base AS final
# Create an argument to allow docker builder to passing in application insights key.
# For example: docker build . --build-arg APPINSIGHTS_KEY=YOUR_APPLICATIONINSIGHTS_INSTRUMENTATION_KEY
#ARG APPINSIGHTS_KEY
# Making sure the argument is set. Fail the build of the container otherwise.
#RUN test -n "$APPINSIGHTS_KEY"

# Light up Application Insights and Service Profiler
#ENV APPINSIGHTS_INSTRUMENTATIONKEY $APPINSIGHTS_KEY
#ENV ASPNETCORE_HOSTINGSTARTUPASSEMBLIES Microsoft.ApplicationInsights.Profiler.AspNetCore
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "limone-api.dll"]