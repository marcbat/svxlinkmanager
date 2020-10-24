# etc
spotnik configuration files

the main files are in /etc/spotnik

This part contains .sh and configurations files for the spotnik HAM radio/internet relay.

for user documentation please refer to the [spotnik documentation]

## 1ST très important et dépendant de l'architecture 

il faut avoir une connection wifi fonctionnel dans /etc/NetworkManager/system-connections
peut importe son nom, il faut la renommer en SPOTNIK en majuscule pour que la configuration faite par l'interface graphique
gui fonctionne correctement .

## /etc/rc.local 
```
# Orange Pi0
echo "7" > /sys/class/gpio/export
sleep 1
echo out > /sys/class/gpio/gpio7/direction
echo "10" > /sys/class/gpio/export 
sleep 1
echo in > /sys/class/gpio/gpio10/direction
echo "6" > /sys/class/gpio/export 
sleep 1
echo out > /sys/class/gpio/gpio6/direction
echo "2" > /sys/class/gpio/export 
sleep 1
echo in > /sys/class/gpio/gpio2/direction
sleep 1

# Raspberry Pi 
#echo "17" >sys/class/gpio/export
#sleep 1
#echo out > /sys/class/gpio/gpio7/direction
#echo "18" > /sys/class/gpio/export 
#sleep 1
#echo in > /sys/class/gpio/gpio18/direction

sleep 1
/etc/spotnik/restart

sleep 2
cd /opt/spotnik/gui
make start 

exit 0

```

## Production

### Install

```
ssh spotnik
cd /etc
git clone --single-branch --branch Version_4 https://github.com/spotnik-ham/etc.git spotnik

```

### Update

```
ssh spotnik
cd /etc/spotnik
git pull

```

