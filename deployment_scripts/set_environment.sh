#!/bin/bash
default_location=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.default_location')
sirmione_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.sirmione_alias')
limone_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.limone_alias')
scorpio_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.scorpio_alias')
virgo_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.virgo_alias')
libra_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.libra_alias')
db_admin_user=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.dbadmin_user')
db_admin_password=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.dbadmin_user_password_do_differently')

export DEFAULT_LOCATION=$default_location
export SIRMIONE_ALIAS=$sirmione_alias
export LIMONE_ALIAS=$limone_alias
export SCORPIO_ALIAS=$scorpio_alias
export VIRGO_ALIAS=$virgo_alias
export LIBRA_ALIAS=$libra_alias
export SIRMIONE_ALIAS=$sirmione_alias
export DB_ADMIN_USER=$db_admin_user
export DB_ADMIN_PASSWORD=$db_admin_password
