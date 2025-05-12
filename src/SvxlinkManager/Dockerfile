#FROM svxlink:25.05 AS svxlink

##########################################################################

FROM mcr.microsoft.com/dotnet/aspnet:8.0-noble AS base
#USER app

ENV TZ=Europe/Kiev
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt-get update && apt-get install -qq -y  procps network-manager \
                                                libsigc++-2.0-dev libgsm1-dev libpopt-dev tcl8.6-dev \
                                                libgcrypt20-dev libspeex-dev libasound2-dev alsa-utils \
                                                vorbis-tools qtbase5-dev qttools5-dev \
                                                qttools5-dev-tools libopus-dev librtlsdr-dev \
                                                libjsoncpp-dev libcurl4-openssl-dev libgpiod-dev \
                                                libogg-dev ladspa-sdk libssl-dev && apt-get clean 

COPY --from=svxlink:25.05 /home/svxlink/build/svxlink-25.05-Linux.deb /home/root/svxlink-25.05-Linux.deb
WORKDIR /home/root
RUN dpkg -i *.deb

RUN mkdir -p /var/spool/svxlink/{voice_mail,qso_recorder} /var/lib/svxlink/pki
RUN chown -R $(id -u):$(id -g) /var/spool/svxlink /var/lib/svxlink

WORKDIR /app

EXPOSE 8080
EXPOSE 8081

RUN curl -sL https://deb.nodesource.com/setup_20.x |  bash -
RUN apt-get install -y nodejs

##########################################################################

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

##########################################################################

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SvxlinkManager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

##########################################################################

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SvxlinkManager.dll"]

