#!/bin/bash

# this bash script copies all files / file contents from one folder to another

# Check if both source and destination are provided
if [ "$#" -ne 2 ]; then
    echo "The command you tried to run the script with is wrong!"
    echo "Usage: $0 <source_folder> <destination_folder>"
    exit 1
fi

# Assign source and destination folders
source_folder=$1
destination_folder=$2

# Check if source folder exists
if [ ! -d "$source_folder" ]; then
    echo "source folder $source_folder was not found"
    exit 1
fi

# Check if destination folder exists
if [ ! -d "$destination_folder" ]; then
    echo "destination folder $destination_folder was not found"
    exit 1
fi

# For loop to copy files from source to destination
for file in "$source_folder"/*
do
    if [ -f "$file" ]; then
        cp "$file" "$destination_folder"/
    fi
done

echo "Files have been copied from $source_folder to $destination_folder."
