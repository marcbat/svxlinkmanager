#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS base

# cr�ation de variable d'environnement utilis�es durant l'installation
ENV TZ=Europe/Kiev
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt-get update && apt-get install -qq -y  svxlink-server svxreflector procps network-manager && apt-get clean 

WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ["src/SvxlinkManager/SvxlinkManager.csproj", "src/SvxlinkManager/"]
RUN dotnet restore "src/SvxlinkManager/SvxlinkManager.csproj"
COPY . .
WORKDIR "/src/src/SvxlinkManager"
RUN npm install
RUN dotnet build "SvxlinkManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SvxlinkManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SvxlinkManager.dll"]