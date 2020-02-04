#!/bin/bash
sirmione_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.sirmione_alias')
export SIRMIONE_ALIAS=$sirmione_alias
