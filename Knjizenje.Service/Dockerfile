FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Knjizenje.Domain/Knjizenje.Domain.csproj Knjizenje.Domain/
RUN dotnet restore Knjizenje.Domain/Knjizenje.Domain.csproj
COPY Knjizenje.Application/Knjizenje.Application.csproj Knjizenje.Application/
RUN dotnet restore Knjizenje.Application/Knjizenje.Application.csproj
COPY Knjizenje.Persistence/Knjizenje.Persistence.csproj Knjizenje.Persistence/
RUN dotnet restore Knjizenje.Persistence/Knjizenje.Persistence.csproj
COPY Integration.Contracts/Integration.Contracts.csproj Integration.Contracts/
RUN dotnet restore Integration.Contracts/Integration.Contracts.csproj
COPY Knjizenje.Service/Knjizenje.Service.csproj Knjizenje.Service/
RUN dotnet restore Knjizenje.Service/Knjizenje.Service.csproj
COPY . .
WORKDIR /src/Knjizenje.Service
RUN dotnet build Knjizenje.Service.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Knjizenje.Service.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Knjizenje.Service.dll"]
