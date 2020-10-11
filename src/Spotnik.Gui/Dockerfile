#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

# création de variable d'environnement utilisées durant l'installation
ENV TZ=Europe/Kiev
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt-get update && apt-get install -qq -y svxlink-server && apt-get clean 

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Spotnik.Gui/Spotnik.Gui.csproj", "src/Spotnik.Gui/"]
RUN dotnet restore "src/Spotnik.Gui/Spotnik.Gui.csproj"
COPY . .
WORKDIR "/src/src/Spotnik.Gui"
RUN dotnet build "Spotnik.Gui.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spotnik.Gui.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spotnik.Gui.dll"]