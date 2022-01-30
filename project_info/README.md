# track-price

## Requirements
Ensure you are running python 3.9
 
## create virtual environment
First install virtualenv

Then in the root code folder, create the virtual env

virtualenv --python=python3.9 .venv

vs code should allow you to select the correct virtual env

## Oracle config

Download wallet from oracle cloud
Edit the sqlnet.ora file and replace ?/network/admin with $TNS_ADMIN
Ensure tns_admin and oracle_home environment variables are set
    tns_admin must point to the location of the downloaded extracted wallet files
    oracle_home must point to the instance client folder

## Oracle folder path variables

windows:
    SET TNS_ADMIN=E:\OracleCloudWallet\Wallet_pricedb
    SET ORACLE_HOME=E:\OracleInstantClient\instantclient\instantclient_21_3

linux:
    export ORACLE_HOME=/opt/oracle/instantclient_21_4
    export TNS_ADMIN=/mnt/e/OracleCloudWallet/Wallet_pricedb
    
or set lib_dir = oracle_lib/instantclient_21_4

config_dir = oracle_config/wallet_pricedb

Edit sqlnet.ora with path to instant client