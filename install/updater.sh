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

#############################
# get latest version number #
#############################
if [ $1 = "checkversion" ]
then

# display the version number
echo "__Build.BuildNumber__"
fi

###########################
# download latest version #
###########################
if [ $1 = "download" ]
then

# download the update
wget -q -N  https://github.com/marcbat/svxlinkmanager/releases/download/__Build.BuildNumber__/svxlinkmanager-__Build.BuildNumber__.zip -P /tmp/svxlinkmanager/
fi

########################################
# update application to latest version #
########################################
if [ $1 = "update" ]
then

# make a backup of the application before update
rsync -ar /etc/SvxlinkManager/ /etc/SvxlinkManager.bak/
rm -rf  /etc/SvxlinkManager/

# update the application
unzip /tmp/svxlinkmanager/svxlinkmanager-__Build.BuildNumber__.zip -d /etc/

# restore the old user db
cp /etc/SvxlinkManager.bak/SvxlinkManager.db /etc/SvxlinkManager/

# start the application
sudo systemctl restart svxlinkmanager
fi

##############################################
# clean temp files downloaded by this script #
##############################################
if [ $1 = "clean" ]
then
rm -f /tmp/svxlinkmanager/*
fi