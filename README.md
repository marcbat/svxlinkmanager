# SvxlinkManager
[![Build Status](https://dev.azure.com/marcbat79/SvxlinkManager/_apis/build/status/SvxlinkManager-core-CI?repoName=marcbat%2Fsvxlinkmanager&branchName=develop)](https://dev.azure.com/marcbat79/SvxlinkManager/_build/latest?definitionId=8&repoName=marcbat%2Fsvxlinkmanager&branchName=develop)

Le but de cette application est de pouvoir piloter via une interface web les fonctionnalités du logiciel [SvxLink](https://github.com/sm0svx/svxlink).

## Installation
Téléchargez la dernière release de l'application, puis décompressez la dans le dossier de votre choix. (ex: /etc/svxlinkmanager)
Modifiez les droits du fichier SvxlinkManager pour le rendre executable.
```bash
cd /etc/swxlinkmanager/
chmod 755 SvxlinkManager
```

## Lancement de l'application

```bash
cd /etc/swxlinkmanager/ .SvxlinkManager --urls=http://0.0.0.0:8080
```

Un fois l'application lancée, vous pouvez vous y connecter à l'adresse http://{server-adresse}:8080
* user: admin@svxlinkmanager.com
* password: Pa$$w0rd

## Fonctionnalités
* Gestion de configurations svxlink multiples.
* Gestion de la connection via une configuration et affichage en temps réel des nodes connectés.