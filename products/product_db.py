from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from datetime import datetime, date
import os

COMPANY_COUNTDOWN = 'countdown'
COMPANY_PACKNSAVE = 'packnsave'

USER = os.environ['MYSQLUSER']
PASSWORD = os.environ['MYSQLPASSWORD']

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = f'mysql://{USER}:{PASSWORD}@localhost/pricedb'

# SQLAlchemy database instance
db = SQLAlchemy(app)


# Create tables in database

# Stores table
class Stores(db.Model):
    store_id: int = db.Column(db.Integer, primary_key=True)
    store_name: str = db.Column(db.String(40), unique=True, nullable=False)
    store = db.relationship('StoreProducts', cascade='all, delete-orphan', backref='store', lazy=True)
    price = db.relationship('Prices', cascade='all, delete-orphan', backref='store', lazy=True)


# Products table
class Products(db.Model):
    product_id: int = db.Column(db.Integer, primary_key=True)
    product_name: str = db.Column(db.String(60), nullable=False)
    unit_of_measure: str = db.Column(db.String(15), nullable=False)
    unit_of_measure_size: float = db.Column(db.Float, nullable=False)
    p_store = db.relationship('StoreProducts', cascade='all, delete-orphan', backref='product', lazy=True)
    p_price = db.relationship('Prices', cascade='all, delete-orphan', backref='product', lazy=True)


# Store product table
class StoreProducts(db.Model):
    store_product_id: int = db.Column(db.Integer, primary_key=True)
    store_id: int = db.Column(db.Integer, db.ForeignKey('stores.store_id', ondelete='CASCADE'), nullable=False)
    product_id: int = db.Column(db.Integer, db.ForeignKey('products.product_id', ondelete='CASCADE'), nullable=False)
    store_product_code: str = db.Column(db.String(40), nullable=False)


# Price table
class Prices(db.Model):
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
# countdown = Stores(store_name='countdown')
# db.session.add(countdown)
# db.session.commit()
# countdown_product = Products(product_name="Test Milk",
#                              unit_of_measure="L",
#                              unit_of_measure_size=3.0)
# db.session.add(countdown_product)
# countdown_store_product = StoreProducts(store_product_code=product_price.company_product_id, store=countdown,
#                                         product=countdown_product)
# db.session.add(countdown_store_product)
# countdown_price = Prices(price_date=product_price.price_date, price=product_price.price, is_onsale=False,
#                          store=countdown, product=countdown_product)
# db.session.add(countdown_price)
# db.session.commit()
