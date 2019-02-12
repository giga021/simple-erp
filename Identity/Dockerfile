FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 57360
EXPOSE 44317

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Identity/Identity.csproj Identity/
RUN dotnet restore Identity/Identity.csproj
COPY . .
WORKDIR /src/Identity
RUN dotnet build Identity.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Identity.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.dll"]
