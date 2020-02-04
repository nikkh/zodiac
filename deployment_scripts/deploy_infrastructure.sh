#!/bin/bash
source ./set_environment.sh
./deploy_sirmione_web.sh
./deploy_scorpio_api.sh
./deploy_limone_api.sh
./deploy_virgo.sh
./deploy_libra.sh
