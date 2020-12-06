#!/bin/bash

#################################################################################################
# updater.sh is a simple script to update SvxlinkManager application                            #
# Writen by Nathanael Mermoud (HB9GVE)                                                          #
# 05.12.2020                                                                                    #
#                                                                                               #
# Usage :                                                                                       #
# UpdateSvxlinkManager.sh checkversion  - get latest application version                        #
# UpdateSvxlinkManager.sh download      - download the latest version of the application        #
# UpdateSvxlinkManager.sh update        - download and update to latest application version     #
# UpdateSvxlinkManager.sh clean         - clean the download temp dir of this script            #
#################################################################################################

#########################################################
# fonction to get the file with the release information #
# and useful stuf                                       #
#########################################################
get_release_info () {
        wget -q -N  https://api.github.com/repos/marcbat/svxlinkmanager/releases/latest -P /tmp/svxlinkmanager/
        LATEST_VERSION=$(grep tag_name /tmp/svxlinkmanager/latest | cut -d'"' -f4)
        LATEST_URL=$(grep browser_download_url /tmp/svxlinkmanager/latest | cut -d'"' -f4)
        UPDATE_FILENAME=$(grep -E "name.*zip" /tmp/svxlinkmanager/latest | cut -d'"' -f4)
}

#############################
# get latest version number #
#############################
if [ $1 = "checkversion" ]
then
get_release_info

# display the version number
echo $LATEST_VERSION
fi

###########################
# download latest version #
###########################
if [ $1 = "download" ]
then
get_release_info

# download the update
wget -q -N  $LATEST_URL -P /tmp/svxlinkmanager/
fi

########################################
# update application to latest version #
########################################
if [ $1 = "update" ]
then
get_release_info

# download the update
wget -q -N  $LATEST_URL -P /tmp/svxlinkmanager/

# stop the application before update it
sudo systemctl stop svxlinkmanager

# make a backup of the application before update
rsync -ar /etc/SvxlinkManager/ /etc/SvxlinkManager.bak/
rm -rf  /etc/SvxlinkManager/

# update the application
unzip -qq /tmp/svxlinkmanager/$UPDATE_FILENAME -d /etc/

# restore the old user db
cp /etc/SvxlinkManager.bak/Spotnik.db /etc/SvxlinkManager/

# start the application
sudo systemctl start svxlinkmanager
fi

##############################################
# clean temp files downloaded by this script #
##############################################
if [ $1 = "clean" ]
then
rm -f /tmp/svxlinkmanager/*
fi
