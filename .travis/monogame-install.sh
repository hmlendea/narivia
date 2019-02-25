#!/bin/bash
# MonoGame 3.6 SDK Installation Script for Travis-CI Virtual Machines

MONOGAME_VERSION="3.6"
INSTALLER_EXE="monogame-sdk.run"
DOWNLOAD_URL="http://www.monogame.net/releases/v$MONOGAME_VERSION/$INSTALLER_EXE"
MONOGAME_DIR="./monogame"
POSTINSTALL_SCRIPT="$MONOGAME_DIR/postinstall.sh"

echo " >>> Installing gtk-sharp3"
sudo apt-get install gtk-sharp3

echo " >>> Downloading the MonoGame SDK v$MONOGAME_VERSION Installer"
wget $DOWNLOAD_URL

chmod +x monogame-sdk.run
sudo "./$INSTALLER_EXE" --noexec --keep --target "$MONOGAME_DIR"
sudo chmod 777 ./monogame/postinstall.sh

echo " >>> Removing the user input prompt from the post-installation script"
sudo sed -i '62,66d' "$POSTINSTALL_SCRIPT"

sudo chmod +x "$POSTINSTALL_SCRIPT"
sudo "$POSTINSTALL_SCRIPT"
