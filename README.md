# SvxlinkManager

| Branche        | Status       |
| ------------- |:-------------:| 
| master      | [![Build Status](https://dev.azure.com/marcbat79/SvxlinkManager/_apis/build/status/SvxlinkManager-core-CI?repoName=marcbat%2Fsvxlinkmanager&branchName=develop)](https://dev.azure.com/marcbat79/SvxlinkManager/_build/latest?definitionId=8&repoName=marcbat%2Fsvxlinkmanager&branchName=master) | 
| develop      | [![Build Status](https://dev.azure.com/marcbat79/SvxlinkManager/_apis/build/status/SvxlinkManager-core-CI?repoName=marcbat%2Fsvxlinkmanager&branchName=develop)](https://dev.azure.com/marcbat79/SvxlinkManager/_build/latest?definitionId=8&repoName=marcbat%2Fsvxlinkmanager&branchName=develop) |

## Présentation
Le projet SvxlinkManager permet la gestion de plusieurs connexions à des [Svxlink Reflector](https://github.com/sm0svx/svxlink). 
Le programme gère également les connexions à Echolink. 
Il se présente sous la forme d’une application web qui pilote le client [Svxlink](http://www.svxlink.org). 

Il est fortement inspiré du super projet [RRF](https://f5nlg.wordpress.com) de l’équipe F4HWN, F4VSJ, F1EVM, F5NLG, F1TZO. SvxlinkManager est d’ailleurs compatible avec le RRF. Vous pouvez l’utiliser pour vous connecter aux salons traditionnels. Il peut s’installer sur un [Spotnik](https://f5nlg.wordpress.com/2016/01/06/svx-boutique) et le piloter sans problème.

SvxlinkManager n’a pas pour but d’intégrer la gestion des modes numériques.

## Fonctionnalités

Interface web protégée par authentification sécurisée. 

### Connexions svxlink reflector
*	Configuration de multiples reflector.
*	Configuration d’un reflector principal.
*	Temporisation avec retour automatique au reflector principal
*	Visualisation en temps réel de l’activité du reflector courant

### Echolink
*	Configuration de plusieurs profils Echolink
*	Connexion à Echolink et suivi en temps réel de l’activité

### Hébergement de reflecteur
* Vous pouvez configurer autant de reflecteur vous voulez
* Les reflecteurs peuvent être lancé en parallèle
* Vous géreu les froits d'accès indépendamment pour chaque reflector 

### Gestion radio
*	Configuration de multiples profils radio, fréquence, tone, etc…
*	Activation en 1 click de la configuration souhaitée

### Wifi
*	Gestion des connexions WiFi avec détection automatique des SSID à portée
*	Activation, désactivation en 1 click d’une ou plusieurs connexions WiFi

### Mise à jour
* Possibité de mettre à jour l'application directement depuis le site web
*	Gestion des mises à jour avec installation automatique
*	Vous pouvez choisir la version cible et tester les versions en avant-première

### Documentation

La documentation détaillée se trouve sur la page [Wiki](https://github.com/marcbat/svxlinkmanager/wiki).

![](https://github.com/marcbat/svxlinkmanager/wiki/images/Accueil.png)
