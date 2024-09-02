#!/usr/bin/env bash

git -v

if [[ $? -ne 0 ]]; then
    os=$(grep ^ID= /etc/os-release | cut -d= -f2 | tr -d '"')

    case "$os" in
        fedora)
        sudo dnf install git
        ;;
        ubuntu)
        sudo apt install git
        *)
    esac

git clone https://github.com/jsusmachaca/tf-idapi.git

docker compose -f tf-idapi/docker-compose.yml up -d

