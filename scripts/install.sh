#!/usr/bin/env bash

git --version

if [[ $? -ne 0 ]]; then
    os=$(grep ^ID= /etc/os-release | cut -d= -f2 | tr -d '"')

    case "$os" in
        fedora)
        sudo dnf install git
        ;;
        ubuntu)
        sudo apt install git
        ;;
    esac
fi

git clone https://github.com/jsusmachaca/tf-idapi.git

read -p "Ingresa el usuario que se usará para Mongodb: " mongo_user
read -p "Ingresa la contraseña que se usará para Mongodb: " mongo_password

echo "# client
NODE_PORT=3000

# api
ALLOWED_IPS=
KAFKA_SERVER=kafka
KAFKA_TOPIC=test

# gateway
API_URL=http://api:8080

# mongo
MONGO_URI=mongodb://$mongo_user:$mongo_password@db:27017/api?authSource=admin
MONGO_USER=$mongo_user
MONGO_PASSWORD=$mongo_password
" > tf-idapi/.env


ssh-keygen -t rsa -b 2048 -m PEM -f jwt.key -P ""
openssl rsa -inform PEM -in jwt.key -pubout > jwt.key.pub

mv jwt.key tf-idapi/Client
mv jwt.key.pub tf-idapi/Gateway

docker compose -f tf-idapi/docker-compose.yml up -d

