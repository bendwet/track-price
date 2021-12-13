from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from datetime import datetime
import os

COMPANY_COUNTDOWN = 'countdown'
COMPANY_PACKNSAVE = 'packnsave'

USER = os.environ['MYSQLUSER']
PASSWORD = os.environ['MYSQLPASSWORD']

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = f'mysql://{USER}:{PASSWORD}@localhost/pricedb'

# SQLAlchemy database instance
db = SQLAlchemy(app)


# Create table in database
class Stores(db.Model):
    store_id: int = db.Column(db.Integer, primary_key=True)
    store_name: str = db.Column(db.String(40), unique=True, nullable=False)
    store_id_relationships = db.relationship('StoreProducts', secondary='Prices', lazy=True)


class Products(db.Model):
    product_id = db.Column(db.Integer, primary_key=True)
    product_name = db.Column(db.String(60), nullable=False)
    unit_of_measure = db.Column(db.String(15), nullable=False)
    unit_of_measure_size = db.Column(db.Float, nullable=False)
    p_id_relationships = db.relationship('Prices', secondary='StoreProducts', lazy=True)


class StoreProducts(db.Model):
    store_product_id = db.Column(db.Integer, primary_key=True)
    store_id = db.Column(db.Integer, db.ForeignKey('stores.store_id'), nullable=False)
    product_id = db.Column(db.Integer, db.ForeignKey('products.product_id'), nullable=False)
    store_product_code = db.Column(db.String(40), nullable=False)


class Prices(db.Model):
    price_id = db.Column(db.Integer, primary_key=True)
    product_id = db.Column(db.Integer, db.ForeignKey('products.product_id'), nullable=False)
    store_id = db.Column(db.Integer, db.ForeignKey('stores.store_id'), nullable=False)
    price_date = db.Column(db.DateTime, nullable=False, default=datetime.utcnow)
    price = db.Column(db.Float, nullable=False)
    is_onsale = db.Column(db.Boolean, default=False)
    price_sale = db.Column(db.Float)


# db.create_all()


