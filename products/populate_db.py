from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from countdown_api.countdown_price_retriever import product_price
from countdown_api.countdown_product_retriever import countdown_product
from sqlalchemy.ext.automap import automap_base
from product_db import db

Base = automap_base()
Base.prepare(db.engine, reflect=True)

Stores = Base.classes.stores

results = db.session.query(Stores).all()

for r in results:
    print(r.store_name)
