#!/bin/bash

#
#	Copyright (C) 2017 Michel GACEM, F1TZO - French Open Networks Project
#
#	This program is free software; you can redistribute it and/or modify
#	it under the terms of the GNU General Public License as published by
#	the Free Software Foundation; version 2 of the License.
#
#	This program is distributed in the hope that it will be useful,
#	but WITHOUT ANY WARRANTY; without even the implied warranty of
#	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#	GNU General Public License for more details.
#
#
#	Ce script permet de rechercher les Proxy Echolink PUBLIC libres
#	Il les test et selectionne celui que aura répondu le plus vite
#	la selection se fait sur un seul test afin de ne pas prendre trop de temps
#	Le resultat est donne par les variables BPROXY (comme BestProxy)
#	avec la variable purement informative BLAT (comme Best Latence)
#
#	This script allows you to search for Free Echolink PUBLIC Proxies
#	It test and select the one that will have answered the fastest
#	The selection is made on a single test so as not to take too long
#	The result is given by BPROXY variables (such as BestProxy)
#	With the purely informative BLAT variable (such as Best Latency)

LAT=9999
BLAT=9999

lynx -dump http://www.echolink.org/proxylist.jsp | grep Ready | grep 8100 | gawk -F '[[:space:]][[:space:]]+' '{ print $3 }' | grep -v ":" | grep -v "192." | grep -v "44."> /tmp/List-Free.txt
NBP=`wc ./List-Free.txt | gawk '{ print $1 }'`g

while read PROXY
do
	LAT=`ping -q -c 1 -W 1 $PROXY | grep rtt | gawk  -F "/" '{ print $6 }'`
	echo "PROXY="$PROXY" LAT="$LAT
	LAT=${LAT%.*}
	LAT="${LAT:-9999}"
	if [ $LAT -lt $BLAT ]
	then
		BLAT=$LAT
		BPROXY=$PROXY
	fi
done < /tmp/List-Free.txt

echo "Nous avons notre Proxy : " $BPROXY" Avec une Latence de : "$BLAT
echo "Choisi parmi "$NBP" Proxy"

#
# On genere le nouveau fichier ModuleEchoLink.conf
# We create the new ModuleEchoLink.conf file
#
rm /tmp/new.conf
IFS=''
while read -r LIGNE
do
        if  echo "$LIGNE" | grep -q PROXY_SERVER
        then
                LIGNE="PROXY_SERVER="$BPROXY
        fi
        if  echo "$LIGNE" | grep -q PROXY_PASSWORD
        then
                LIGNE="PROXY_PASSWORD=PUBLIC"
        fi
        echo $LIGNE >> /tmp/new.conf
done < /etc/spotnik/svxlink.d/ModuleEchoLink.conf

#
# On remplace le fichler ModuleEchoLink.conf par le nouveau 
# We replace the old ModuleEchoLink.conf by the new one
#
cp /tmp/new.conf /etc/svxlink/svxlink.d/ModuleEchoLink.conf
cp /tmp/new.conf /etc/spotnik/svxlink.d/ModuleEchoLink.conf
#
# On redemarre SVXlink dans un Screen 
# We restart SVXlink in a Screen 
# 
# METTRE ICI SA COMMANDE DE RESTART SVXLINK

