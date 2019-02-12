FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Pregledi.API/Pregledi.API.csproj Pregledi.API/
RUN dotnet restore Pregledi.API/Pregledi.API.csproj
COPY . .
WORKDIR /src/Pregledi.API
RUN dotnet build Pregledi.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Pregledi.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pregledi.API.dll"]
