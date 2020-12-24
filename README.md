# SvxlinkManager
[![Build Status](https://dev.azure.com/marcbat79/SvxlinkManager/_apis/build/status/SvxlinkManager-core-CI?repoName=marcbat%2Fsvxlinkmanager&branchName=develop)](https://dev.azure.com/marcbat79/SvxlinkManager/_build/latest?definitionId=8&repoName=marcbat%2Fsvxlinkmanager&branchName=develop)

## Présentation
Le projet SvxlinkManager permet la gestion de plusieurs connexions à des Svxlink Reflector. 
Le programme gère également les connexions à Echolink. 
Il se présente sous la forme d’une application web qui pilote les redémarrages et reconfigurations du client Svxlink. 

Il est fortement inspiré du super projet RRF de l’équipe F4HWN, F4VSJ, F1EVM, F5NLG, F1TZO. SvxlinkManager est d’ailleurs compatible avec le RRF. Vous pouvez l’utiliser pour vous connecter aux salons traditionnels. Il peut s’installer sur un Spotnik et le piloter sans problème.

SvxlinkManager ne supporte pas les modes numériques type C4FM ou DMR et cette fonctionnalité n’est pas prévue dans l’avenir. 

## Fonctionnalités

Interface web protégée par authentification sécurisée. 

### Connexions svxlink reflector
*	Configuration de multiples reflector.
*	Configuration d’un reflector principal.
*	Temporisation avec retour automatique au reflector principal
*	Visualisation en temps réel de l’activité du reflector courant

### Echolink
*	Configuration de multiples profils Echolink
*	Connection à Echolink et suivi en temps réel de l’activité

### Gestion radio
*	Configuration de multiples profils radio, fréquence, tone, etc…
*	Activation en 1 click de la configuration souhaitée

### Wifi
*	Gestion des connexions WiFi avec détection automatique des SSID à portée
*	Activation, désactivation en 1 click d’une ou plusieurs connexions WiFi
Mise à jour
*	Gestion des mises à jour avec installation automatique
*	Vous pouvez choisir la version cible et tester les versions en avant-première

