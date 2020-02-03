#!/bin/bash
sirmioneAlias=cat global/parameters.json | jq -r '.sirmione_alias'
echo "Application Name: $sirmioneAlias"
