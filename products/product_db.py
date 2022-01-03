from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from datetime import datetime, date
import os

# Temporary code
COMPANY_COUNTDOWN = 'countdown'
COMPANY_PACKNSAVE = 'packnsave'
# end temporary code

USER = os.environ['MYSQLUSER']
PASSWORD = os.environ['MYSQLPASSWORD']

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = f'mysql://{USER}:{PASSWORD}@localhost/pricedb'

# SQLAlchemy database instance
db = SQLAlchemy(app)


# Create tables in database

# Stores table
class Store(db.Model):
    __tablename__: str = 'stores'
    store_id: int = db.Column(db.Integer, primary_key=True)
    store_name: str = db.Column(db.String(40), unique=True, nullable=False)
    store_products = db.relationship('StoreProduct', cascade='all, delete-orphan', backref='store', lazy=True)
    prices = db.relationship('Price', cascade='all, delete-orphan', backref='store', lazy=True)


# Products table
class Product(db.Model):
    __tablename__: str = 'products'
    product_id: int = db.Column(db.Integer, primary_key=True)
    product_name: str = db.Column(db.String(60), nullable=False)
    unit_of_measure: str = db.Column(db.String(15), nullable=False)
    unit_of_measure_size: float = db.Column(db.Float, nullable=False)
    store_products = db.relationship('StoreProduct', cascade='all, delete-orphan', backref='product', lazy=True)
    prices = db.relationship('Price', cascade='all, delete-orphan', backref='product', lazy=True)


# Store product table
class StoreProduct(db.Model):
    __tablename__: str = 'store_products'
    store_product_id: int = db.Column(db.Integer, primary_key=True)
    store_id: int = db.Column(db.Integer, db.ForeignKey('stores.store_id', ondelete='CASCADE'), nullable=False)
    product_id: int = db.Column(db.Integer, db.ForeignKey('products.product_id', ondelete='CASCADE'), nullable=False)
    store_product_code: str = db.Column(db.String(40), nullable=False)


# Price table
class Price(db.Model):
    __tablename__: str = 'prices'
    price_id: int = db.Column(db.Integer, primary_key=True)
    product_id: int = db.Column(db.Integer, db.ForeignKey('products.product_id', ondelete='CASCADE'), nullable=False)
    store_id: int = db.Column(db.Integer, db.ForeignKey('stores.store_id', ondelete='CASCADE'), nullable=False)
    price_date = db.Column(db.Date, nullable=False, default=datetime.utcnow)
    price: float = db.Column(db.Float, nullable=False)
    is_onsale: bool = db.Column(db.Boolean, default=False)
    price_sale: float = db.Column(db.Float)
    __table_args__ = (db.UniqueConstraint('product_id', 'store_id', 'price_date'),)


# db.create_all()
#
# countdown = Store(store_name='countdown')
# db.session.add(countdown)
# db.session.commit()
