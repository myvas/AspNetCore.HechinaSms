##!/bin/sh

## If $1 is not passed, set to the current working dir using $PWD
_dir="${1:-${PWD}}"

## Die if $dir does not exists
[ ! -d "$_dir" ] && { echo "Error: Directory $_dir not found."; exit 2; }

## git
git stash save
git pull qcloud master

## dotnet restore
dotnet restore

## systemctl stop ...
systemctl stop ruhu.service

## dotnet build
cd src/Ruhu.Web
dotnet build

## dotnet ef database update
cd ../Ruhu.Core
dotnet ef --startup-project ../Ruhu.Web database update

## dotnet publish ...
cd ../Ruhu.Web
dotnet publish --configuration release

## systemctl start
systemctl daemon-reload
systemctl start ruhu.service
systemctl status ruhu.service
journalctl -fu ruhu.service