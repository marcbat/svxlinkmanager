#!/bin/sh
#
# Perform return to RRF after timeout  
# F4HWN Armel
#

# Set timeout in seconds

if [ $# -eq 0 ]
then
    timeout=300
else
    timeout=$1
fi

# Init other values (don't touch)

last=`date +%s`
timer=0
talker_start=0
talker_stop=`date +%s`
log='/tmp/timersalon.log' 

# Start log

cat << EOF > $log
Start QSY at        : `date +'%d-%m-%Y %H:%M:%S' -d @$last` ($last)
--------------------
EOF

# Main loop

while [ $timer -lt $timeout ]; do
    # Standby
    sleep 10

    # Catch last Talker start (if exist)

    tmp=`grep 'ReflectorLogic: Talker start:' /tmp/svxlink.log | tail -1 | cut -c1-24`
    if [ ! -z "$tmp" ]
    then
        talker_start=`date -d "$tmp" +%s`
    fi

    # Catch last Talker stop (if exist)

    tmp=`grep 'ReflectorLogic: Talker stop:' /tmp/svxlink.log | tail -1 | cut -c1-24`
    if [ ! -z "$tmp" ]
    then
        talker_stop=`date -d "$tmp" +%s`
    fi

    # If last Talker start > last Talker stop, then somebody is speaking so
    #   last activity is now
    # Else
    #   last activity was at last Talker stop... 

    if [ $talker_start -gt $talker_stop ]
    then
        last=`date +%s`
        trace=false
    else
        last=$talker_stop
        trace=true
    fi

    now=`date +%s`

    timer=$(($now-$last))

    # Write trace for debug, only if trace is true (nobody speaking...)

    if [ "$trace" = true ]
    then

cat << EOF >> $log
Last Talker Start   : `date +'%d-%m-%Y %H:%M:%S' -d @$talker_start` ($talker_start)
Last Talker Stop    : `date +'%d-%m-%Y %H:%M:%S' -d @$talker_stop` ($talker_stop)
Last Radio Activity : `date +'%d-%m-%Y %H:%M:%S' -d @$last` ($last)
Timout              : $timeout seconds
Timer               : $timer seconds
--------------------
EOF
    else

cat << EOF > $log
Last QSO active at  : `date +'%d-%m-%Y %H:%M:%S' -d @$now` ($now)
--------------------
EOF

    fi
done

# Return to Salon Suisse Romand

cat << EOF >> $log
Return to Salon Suisse Romand at    : `date +'%d-%m-%Y %H:%M:%S' -d @$now` ($now)
EOF

/etc/spotnik/restart.reg
