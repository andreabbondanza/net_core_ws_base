#!/bin/bash

arg1=$1

if [ -z "$arg1" ]; then
    echo "Usage: $0 <service_name>"
    exit 1
fi

service_name=$arg1

mkdir -p ../$service_name

cp -r ../ws_base/* ../$service_name

mv ../$service_name/src/it.naturasrl.ws_base ../$service_name/src/it.naturasrl.$service_name

# Replace "ws_base" with $service_name in ws_base.csproj
sed -i "s/ws_base/$service_name/g" ../$service_name/ws_base.csproj

# Rename ws_base.csproj to $service_name.csproj
mv ../$service_name/ws_base.csproj ../$service_name/$service_name.csproj

# Replace "ws_base" with $service_name in ws_base.csproj
sed -i "s/ws_base/$service_name/g" ../$service_name/ws_base.sln

# Rename ws_base.csproj to $service_name.csproj
mv ../$service_name/ws_base.sln ../$service_name/$service_name.sln

mv ../$service_name/ws_base.http ../$service_name/$service_name.http

# Replace "it.naturasrl.ws_base" with "it.naturasrl.$service_name" in .cs files
find ../$service_name/src -type f -name "*.cs" -exec sed -i "s/it.naturasrl.ws_base/it.naturasrl.$service_name/g" {} \;

rm -r ../$service_name/bin

rm -r ../$service_name/obj

