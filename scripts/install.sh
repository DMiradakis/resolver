#!/bin/bash
set -e

BINARY_NAME="resolver"
REPO="DMiradakis/resolver"

# Check for --system flag
SYSTEM_INSTALL=false
for arg in "$@"; do
    case $arg in
        --system)
            SYSTEM_INSTALL=true
            shift
            ;;
    esac
done

if [ "$SYSTEM_INSTALL" = true ]; then
    INSTALL_DIR="/usr/local/bin"
    NEEDS_SUDO=true
else
    INSTALL_DIR="$HOME/.local/bin"
    NEEDS_SUDO=false
fi

# Detect OS
OS=$(uname -s | tr '[:upper:]' '[:lower:]')
case "$OS" in
    linux*) OS="linux" ;;
    darwin*) OS="macos" ;;
    *) echo "Unsupported OS: $OS" && exit 1 ;;
esac

# Detect architecture
ARCH=$(uname -m)
case "$ARCH" in
    x86_64) ARCH="x64" ;;
    aarch64|arm64) ARCH="arm64" ;;
    *) echo "Unsupported architecture: $ARCH" && exit 1 ;;
esac

# Get latest release version
VERSION=${BINARY_VERSION:-latest}
if [ "$VERSION" = "latest" ]; then
    VERSION=$(curl -fsSL "https://api.github.com/repos/$REPO/releases/latest" | grep '"tag_name":' | sed -E 's/.*"([^"]+)".*/\1/')
fi

DOWNLOAD_URL="https://github.com/$REPO/releases/download/$VERSION/$BINARY_NAME-$OS-$ARCH"

echo "Installing $BINARY_NAME $VERSION for $OS-$ARCH to $INSTALL_DIR..."

# Create install directory
if [ "$NEEDS_SUDO" = true ]; then
    sudo mkdir -p "$INSTALL_DIR"
    
    # Download to temp location
    TMP_FILE=$(mktemp)
    curl -fSL "$DOWNLOAD_URL" -o "$TMP_FILE"
    chmod +x "$TMP_FILE"
    
    # Move with sudo
    sudo mv "$TMP_FILE" "$INSTALL_DIR/$BINARY_NAME"
else
    mkdir -p "$INSTALL_DIR"
    curl -fSL "$DOWNLOAD_URL" -o "$INSTALL_DIR/$BINARY_NAME"
    chmod +x "$INSTALL_DIR/$BINARY_NAME"
fi

echo "✓ Installed $BINARY_NAME to $INSTALL_DIR/$BINARY_NAME"

# PATH check (only for user installs)
if [ "$NEEDS_SUDO" = false ] && [[ ":$PATH:" != *":$INSTALL_DIR:"* ]]; then
    echo ""
    echo "⚠ $INSTALL_DIR is not in your PATH."
    echo "Add it by running:"
    echo ""
    echo "  echo 'export PATH=\"\$HOME/.local/bin:\$PATH\"' >> ~/.bashrc"
    echo "  source ~/.bashrc"
    echo ""
    echo "Or for zsh:"
    echo "  echo 'export PATH=\"\$HOME/.local/bin:\$PATH\"' >> ~/.zshrc"
    echo "  source ~/.zshrc"
else
    echo "✓ Ready to use! Run: $BINARY_NAME"
fi