#!/bin/bash

# Retrieve command-line parameters
param1="$1"

payload=$(echo "{
    \"catalogId\":1020,
    \"groupId\": $param1,
    \"from\": 0,
    \"fields\": [\"partNumber\", \"parentPartNumber\", \"id\", \"name\"],
    \"omitNullEntries\": true
}")

echo "Retrieving Chewy Auth Token..."

response=$(curl -X POST -H "Content-Type: application/x-www-form-urlencoded" -u seo-superlative-landing-page-service-s2s-client:AfFTPWVAJjxIaAYE0nuYVyOSwxYyMCcr -d 'grant_type=client_credentials' https://auth-stg.chewy.com/auth/realms/chewy-auth/protocol/openid-connect/token)

echo "Successfully retrieved the token"

token=$(echo "$response" | jq -r '.access_token')

echo "Sending request to search post endpoint..."

# Perform cURL request
searchresponse=$(curl -X POST -H "Authorization: Bearer $token" -H "Content-Type: application/json" -d "$payload" http://catalog-search-service-use1.demm.stg.chewy.com:8080/search)

catalogEntries=$(echo "$searchresponse" | jq -c '.catalogEntries[]')

index=0

while IFS= read -r object; do
    ((index++))
    id=$(echo "$object" | jq -r '.id')
    name=$(echo "$object" | jq -r '.name')
    echo "$index: product id - $id, name - $name"
done <<< "$catalogEntries"
