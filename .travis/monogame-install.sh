#!/bin/bash
# MonoGame SDK Installation Script for Travis-CI Virtual Machines

MONOGAME_VERSION="3.6"
INSTALLER_EXE="monogame-sdk.run"
DOWNLOAD_URL="http://www.monogame.net/releases/v$MONOGAME_VERSION/$INSTALLER_EXE"
MONOGAME_DIR=$(pwd)"/monogame"
POSTINSTALL_SCRIPT="postinstall.sh"
ORIGINAL_DIR=$(pwd)

echo " >>> Installing gtk-sharp3"
sudo apt-get install gtk-sharp3

echo " >>> Installing libfreeimage3"
sudo apt-get install libfreeimage3

echo " >>> Downloading the MonoGame SDK v$MONOGAME_VERSION Installer"
wget -c "$DOWNLOAD_URL"

echo " >>> Running the MonoGame installer"
chmod +x monogame-sdk.run
sudo "./$INSTALLER_EXE" --noexec --keep --target "$MONOGAME_DIR"

echo " >>> Entering the '$MONOGAME_DIR' directory"
cd "$MONOGAME_DIR"

echo " >>> Removing the user input prompt from the post-installation script"
sudo chmod 777 "$POSTINSTALL_SCRIPT"
sudo sed -i '62,66d' "$POSTINSTALL_SCRIPT"

sudo chmod +x "$POSTINSTALL_SCRIPT"
sudo "./$POSTINSTALL_SCRIPT"

echo " >>> Going back into the '$ORIGINAL_DIR' directory"
cd "$ORIGINAL_DIR"
