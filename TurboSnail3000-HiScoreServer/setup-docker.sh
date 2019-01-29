#!/usr/bin/env bash

set -e

SCRIPT_DIR="$(dirname "$BASH_SOURCE")"

docker build -t snail .
docker run --detach --name snail --restart=always --publish 5000:5000 snail

echo "a@_ server up and running on port 5000"
