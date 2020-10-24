#!/bin/bash

# This script start or stop numeric services
# F4HWN Armel
# Usage : num.sh [stop|start|state|enable|disable]
# Version 0.1

SERVICES=("analog_bridge" "ircddbgateway" "md380-emu" "mmdvm_bridge" "nxdngateway" "p25gateway" "ysfgateway")
#SERVICES=("ambeserver" "analog_bridge" "ircddbgateway" "md380-emu" "mmdvm_bridge" "nxdngateway" "p25gateway" "ysfgateway")

for SERVICE in "${SERVICES[@]}"
do
    case "$1" in
        start)
            echo "Start $SERVICE.service"
            systemctl start $SERVICE.service
            ;;
        stop)
            echo "Stop $SERVICE.service"
            systemctl stop $SERVICE.service
            ;;
        state)
            echo -n "State of $SERVICE.service : "
            systemctl is-active $SERVICE.service
            ;;
        enable)
            echo -n "Enable $SERVICE.service : "
            systemctl enable $SERVICE.service
            ;;
        disable)
            echo "Disable $SERVICE.service"
            systemctl disable $SERVICE.service
            ;;
        version)
            echo "num.sh version 0.1 - F4HWN Armel"
            exit
        esac
done

case "$1" in 
    enable)
        echo "You need to reboot !"
        ;;
    disable)
        echo "You need to reboot !"
        ;;
    esac
