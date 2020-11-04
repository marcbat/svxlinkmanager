#!/bin/bash

# DTMF 102#
# stop numeric mode
/etc/spontnik/num.sh stop
pkill -f svxbridge.py

# Stop svxlink
if pgrep -x svxlink >/dev/null
then
    pkill -TERM svxlink
    pkill -f timersalon
fi

# stop vncserver
if pgrep -x Xtightvnc >/dev/null
then
    pkill -TERM vncserver:1
fi


# Save network
echo "el" > /etc/spotnik/network
sleep 1

# Clear logs
> /tmp/svxlink.log

# Launch svxlink
svxlink --daemon --logfile=/tmp/svxlink.log --pidfile=/var/run/svxlink.pid --runasuser=root --config=/etc/spotnik/svxlink.el
sleep 1

# Enable EchoLink 
echo "2#" > /tmp/dtmf_uhf
echo "10#" > /tmp/dtmf_vhf


# debut gestion timer salon:
pkill -f timersalon
sh /etc/spotnik/timersalon.sh &
