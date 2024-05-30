#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app

ENV TZ=Europe/Kiev
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt-get update && apt-get install -qq -y  svxlink-server svxreflector procps network-manager && apt-get clean 

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN curl -sL https://deb.nodesource.com/setup_20.x |  bash -
RUN apt-get install -y nodejs



FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

RUN curl -sL https://deb.nodesource.com/setup_20.x |  bash -
RUN apt-get install -y nodejs

WORKDIR /src
COPY ["src/SvxlinkManager/SvxlinkManager.csproj", "src/SvxlinkManager/"]
COPY ["src/SvxlinkManager.Application/SvxlinkManager.Application.csproj", "src/SvxlinkManager.Application/"]
COPY ["src/SvxlinkManager.Domain/SvxlinkManager.Domain.csproj", "src/SvxlinkManager.Domain/"]
COPY ["src/SvxlinkManager.Infrastructure/SvxlinkManager.Infrastructure.csproj", "src/SvxlinkManager.Infrastructure/"]
RUN dotnet restore "./src/SvxlinkManager/SvxlinkManager.csproj"
COPY . .
WORKDIR "/src/src/SvxlinkManager"
RUN npm install
RUN dotnet build "./SvxlinkManager.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SvxlinkManager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SvxlinkManager.dll"]