#!/bin/bash
default_location=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.default_location')
sirmione_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.sirmione_alias')
limone_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.limone_alias')
scorpio_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.scorpio_alias')
virgo_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.virgo_alias')
libra_alias=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.libra_alias')
dbadmin_user=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.dbadmin_user')
dbadmin_user_password_do_differently=$(curl -s https://raw.githubusercontent.com/nikkh/zodiac/master/global/parameters.json | jq -r '.dbadmin_user_password_do_differently')

export DEFAULT_LOCATION=$default_location
export SIRMIONE_ALIAS=$sirmione_alias
export LIMONE_ALIAS=$limone_alias
export SCORPIO_ALIAS=$scorpio_alias
export VIRGO_ALIAS=$virgo_alias
export LIBRA_ALIAS=$libra_alias
export SIRMIONE_ALIAS=$sirmione_alias
export DBADMIN_USER=$dbadmin_user
export DBADMIN_USER_PASSWORD_DO_DIFFERENTLY=$dbadmin_user_password_do_differently

./deploy_sirmione_web.sh
./deploy_scorpio_api.sh
./deploy_limone_api.sh
./deploy_virgo.sh
./deploy_libra.sh
