# SvxlinkManager
[![Build Status](https://dev.azure.com/marcbat79/SvxlinkManager/_apis/build/status/SvxlinkManager-core-CI?repoName=marcbat%2Fsvxlinkmanager&branchName=develop)](https://dev.azure.com/marcbat79/SvxlinkManager/_build/latest?definitionId=8&repoName=marcbat%2Fsvxlinkmanager&branchName=develop)

Le but de cette application est de pouvoir piloter via une interface web les fonctionnalités du logiciel [SvxLink](https://github.com/sm0svx/svxlink).

## Installation
Téléchargez la dernière release de l'application, puis décompressez la dans le dossier de votre choix. (ex: /etc)
```bash
wget https://github.com/marcbat/svxlinkmanager/releases/download/x.x.x/svxlinkmanager-x.x.x.zip

unzip -o svxlinkmanager-x.x.x.zip
rm svxlinkmanager-x.x.x.zip
```
Modifiez les droits du fichier SvxlinkManager pour le rendre executable.
```bash
cd SvxlinkManager
chmod 755 SvxlinkManager
```

## Activer la détection DTMF (facultatif)
Pour activer la détection DTMF, vous devez ajouter quelques lignes de script dans /usr/share/svxlink/event.d/local/logic.tcl
Repérez la méthode
```tcl
proc dtmf_cmd_received {cmd}
```
et ajoutez directement dessous
```tcl
if {$cmd != "10"} {
  # Write DTMF in monitored file
  puts "Write DTMF monitored file"
  set dtmf_file "/etc/SvxlinkConfig/SvxlinkConfig/dtmf.conf"
  set dtmf_file_id [open $dtmf_file "w"]
  puts $dtmf_file_id $cmd 
  close $dtmf_file_id
}
```

## Lancement de l'application

```bash
cd /etc/svxlinkmanager/ ./SvxlinkManager --urls=http://0.0.0.0:8080
```

Un fois l'application lancée, vous pouvez vous y connecter à l'adresse http://{server-adresse}:8080
* user: admin@svxlinkmanager.com
* password: Pa$$w0rd

## Fonctionnalités
* Gestion de configurations svxlink multiples.
* Gestion de la connection svxlink via une configuration et affichage en temps réel des nodes connectés.
* Gestion des profils radios multiples, si le module SA818 est présent.
* Pilotage du changement de salon par DTMF (si activé)
