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

# unzip application 
unzip /tmp/svxlinkmanager/svxlinkmanager-__Build.BuildNumber__.zip -d /tmp/svxlinkmanager/__Build.BuildNumber__/

# remove current version
rm -rf  /etc/SvxlinkManager/

# copy new version
cp -R /tmp/svxlinkmanager/__Build.BuildNumber__/* /etc/SvxlinkManager/

# restore the old user db
cp /etc/SvxlinkManager.bak/SvxlinkManager.db /etc/SvxlinkManager/

# change permission on getIP
chmod 755 /etc/SvxlinkManager/SvxlinkConfig/getIP

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