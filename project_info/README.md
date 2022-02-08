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

## local mysql remote from alpine

GRANT ALL PRIVILEGES ON *.* TO 'root' IDENTIFIED BY 'password' WITH GRANT OPTION;
for root user with connection access from non local connections.

sudo nano/etc/my.cnf to access mariadb config file

then add skip-network=0 to not skip network configuration and set port = 3036 
(default) otherwise it will be 0

skip-bind-address - to not bind address to 127.0.0.1 by default

## Frontend

useEffect() https://www.digitalocean.com/community/tutorials/how-to-handle-async-data-loading-lazy-loading-and-code-splitting-with-react
