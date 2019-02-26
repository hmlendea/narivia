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
FONTS_TTF_DIR="/usr/share/fonts/truetype/MonoGameFonts"

echo " >>> Installing GTK#3"
sudo apt-get install gtk-sharp3

echo " >>> Installing FreeImage 3.17"
wget -c "$FREEIMAGE_DOWNLOAD_URL"
unzip "$FREEIMAGE_ZIP"
cd "FreeImage"
make
sudo make install
cd "$ORIGINAL_DIR"

echo " >>> Installing ttf-mscorefonts-installer"
echo ttf-mscorefonts-installer msttcorefonts/accepted-mscorefonts-eula select true | sudo debconf-set-selections
sudo apt-get install ttf-mscorefonts-installer

echo " >>> Installing the MonoGame SDK v$MONOGAME_VERSION Installer"
wget -c "$MONOGAME_DOWNLOAD_URL"
chmod +x monogame-sdk.run
sudo "./$INSTALLER_EXE" --noexec --keep --target "$MONOGAME_DIR"

echo " >>> Running the MonoGame post-installation script"
# Remove the user input prompt
cd "$MONOGAME_DIR"
sudo chmod 777 "$POSTINSTALL_SCRIPT"
sudo sed -i '62,66d' "$POSTINSTALL_SCRIPT"
# Run the script
sudo chmod +x "$POSTINSTALL_SCRIPT"
sudo "./$POSTINSTALL_SCRIPT"
cd "$ORIGINAL_DIR"

echo " >>> Installing the fonts distributed in the solution"
sudo mkdir -p "$FONTS_TTF_DIR"
# TODO: Handle spaces in the paths
FONTS=$(find . -type f \( -name "*.ttf" -or -name "*.TTF" \))
for FONT in $FONTS; do
    echo "Font found in solution: $FONT"
    sudo cp "$FONT" "$FONTS_TTF_DIR"
done
fc-cache -f -v
