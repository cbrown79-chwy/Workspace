#!/bin/bash

if [ "$#" -lt 1 ] || [ "$#" -gt 2 ]; then
    echo "Usage: ./delete-bucket-contents.sh BUCKET_NAME [PROFILE_NAME]"
    exit 1
fi

BUCKET_NAME="$1"
PROFILE_ARG=""

if [ ! -z "$2" ]; then
    PROFILE_ARG="--profile $2"
fi

echo "WARNING: This will delete ALL contents and versions from bucket: $BUCKET_NAME"
if [ ! -z "$2" ]; then
    echo "Using AWS profile: $2"
fi
echo "Are you sure you want to continue? (y/N)"
read -r confirm

if [ "$confirm" != "y" ] && [ "$confirm" != "Y" ]; then
    echo "Operation cancelled"
    exit 0
fi

echo "Deleting all objects and versions..."
aws s3api delete-objects \
    $PROFILE_ARG \
    --bucket "$BUCKET_NAME" \
    --delete "$(aws s3api list-object-versions \
        $PROFILE_ARG \
        --bucket "$BUCKET_NAME" \
        --output json \
        --query '{Objects: [].{Key:Key,VersionId:VersionId}}')"

echo "Removing delete markers..."
aws s3api delete-objects \
    $PROFILE_ARG \
    --bucket "$BUCKET_NAME" \
    --delete "$(aws s3api list-object-versions \
        $PROFILE_ARG \
        --bucket "$BUCKET_NAME" \
        --output json \
        --query '{Objects: DeleteMarkers[].{Key:Key,VersionId:VersionId}}')"

echo "Operation completed"