FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 8081
EXPOSE 44397

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ERP.SPA/ERP.SPA.csproj ERP.SPA/
RUN dotnet restore ERP.SPA/ERP.SPA.csproj
COPY . .
WORKDIR /src/ERP.SPA
RUN dotnet build ERP.SPA.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ERP.SPA.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ERP.SPA.dll"]