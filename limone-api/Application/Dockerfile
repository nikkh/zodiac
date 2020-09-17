#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["limone-api/Application/limone-api.csproj", "limone-api/Application/"]
RUN dotnet restore "limone-api/Application/limone-api.csproj"
COPY . .
WORKDIR "/src/limone-api/Application"
RUN dotnet build "limone-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "limone-api.csproj" -c Release -o /app/publish

FROM base AS final
ARG APPINSIGHTS_KEY
RUN ECHO ${$APPINSIGHTS_KEY}
RUN test -n "$APPINSIGHTS_KEY"
ENV APPINSIGHTS_INSTRUMENTATIONKEY=${$APPINSIGHTS_KEY}
ENV ASPNETCORE_HOSTINGSTARTUPASSEMBLIES Microsoft.ApplicationInsights.Profiler.AspNetCore
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "limone-api.dll"]