#!/bin/bash
# MonoGame SDK Installation Script for Travis-CI Virtual Machines

MONOGAME_VERSION="3.6"
INSTALLER_EXE="monogame-sdk.run"
FREEIMAGE_ZIP="FreeImage3170.zip"
FREEIMAGE_DOWNLOAD_URL="http://downloads.sourceforge.net/freeimage/$FREEIMAGE_ZIP"
MONOGAME_DOWNLOAD_URL="http://www.monogame.net/releases/v$MONOGAME_VERSION/$INSTALLER_EXE"
MONOGAME_DIR=$(pwd)"/monogame"
POSTINSTALL_SCRIPT="postinstall.sh"
ORIGINAL_DIR=$(pwd)

echo " >>> Installing GTK#3"
sudo apt-get install gtk-sharp3

echo " >>> Installing FreeImage 3.17"
wget -c "$FREEIMAGE_DOWNLOAD_URL"
unzip "$FREEIMAGE_ZIP"
cd "FreeImage"
make
sudo make install
cd "$ORIGINAL_DIR"

sudo apt-get install libfreeimage3
sudo ln -s "/usr/lib/x86_64/libfreeimage-3.17.0.so" "/usr/lib/libfreeimage-3.17.0.so"

echo " >>> Installing ttf-mscorefonts-installer"
echo ttf-mscorefonts-installer msttcorefonts/accepted-mscorefonts-eula select true | sudo debconf-set-selections
sudo apt-get install ttf-mscorefonts-installer

echo " >>> Installing the MonoGame SDK v$MONOGAME_VERSION Installer"
wget -c "$MONOGAME_DOWNLOAD_URL"
chmod +x monogame-sdk.run
sudo "./$INSTALLER_EXE" --noexec --keep --target "$MONOGAME_DIR"

echo " >>> Removing the user input prompt from the post-installation script"
cd "$MONOGAME_DIR"
sudo chmod 777 "$POSTINSTALL_SCRIPT"
sudo sed -i '62,66d' "$POSTINSTALL_SCRIPT"

echo " >>> Running the MonoGame post-installation script"
sudo chmod +x "$POSTINSTALL_SCRIPT"
sudo "./$POSTINSTALL_SCRIPT"
cd "$ORIGINAL_DIR"
