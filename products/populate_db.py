from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from countdown_api.countdown_price_retriever import product_price
from countdown_api.countdown_product_retriever import countdown_product
from product_db import db, Stores, StoreProducts, Products, Prices

TABLE_NAMES = ['stores', 'products', 'store_products', 'prices']

# query all info from Stores table
get_store = db.session.query(Stores)
get_product = db.session.query(Products)

# default value of countdown position in table
countdown_position = 0
product_position = 0

# check which position countdown is in for retrieving correct store id.
for row in get_store.all():
    if row.store_name == 'countdown':
        countdown_position = get_store.all().index(row)

# check which position product name is in for retrieving product store id.
for row in get_product.all():
    if row.product_name == countdown_product.product_name:
        product_position = get_product.all().index(row)
        
# countdown_position = get_store.all().index('countdown')
# print(countdown_position)

countdown_id = get_store.all()[countdown_position].store_id
product_id = get_product.all()[product_position].product_id

print(countdown_product)

print(countdown_id)
print(product_id)
