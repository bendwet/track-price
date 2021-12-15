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

date_now = datetime.now().date()


# Create table in database
class Stores(db.Model):
    store_id: int = db.Column(db.Integer, primary_key=True)
    store_name: str = db.Column(db.String(40), unique=True, nullable=False)
    store = db.relationship('StoreProducts', backref='store', lazy=True)
    price = db.relationship('Prices', backref='store', lazy=True)


class Products(db.Model):
    product_id: int = db.Column(db.Integer, primary_key=True)
    product_name: str = db.Column(db.String(60), nullable=False)
    unit_of_measure: str = db.Column(db.String(15), nullable=False)
    unit_of_measure_size: float = db.Column(db.Float, nullable=False)
    p_store = db.relationship('StoreProducts', backref='product', lazy=True)
    p_price = db.relationship('Prices', backref='product', lazy=True)


class StoreProducts(db.Model):
    store_product_id: int = db.Column(db.Integer, primary_key=True)
    store_id: int = db.Column(db.Integer, db.ForeignKey('stores.store_id'), nullable=False)
    product_id: int = db.Column(db.Integer, db.ForeignKey('products.product_id'), nullable=False)
    store_product_code: str = db.Column(db.String(40), nullable=False)


class Prices(db.Model):
    price_id: int = db.Column(db.Integer, primary_key=True)
    product_id: int = db.Column(db.Integer, db.ForeignKey('products.product_id'), nullable=False)
    store_id: int = db.Column(db.Integer, db.ForeignKey('stores.store_id'), nullable=False)
    price_date = db.Column(db.Date, nullable=False, default=datetime.utcnow)
    price: float = db.Column(db.Float, nullable=False)
    is_onsale: bool = db.Column(db.Boolean, default=False)
    price_sale: float = db.Column(db.Float)


# db.create_all()
#
# countdown = Stores(store_name='countdown')
# db.session.add(countdown)
# milk = Products(product_name='milk', unit_of_measure='L', unit_of_measure_size=3.0)
# db.session.add(milk)
# c_milk = StoreProducts(store_product_code='abc123', store=countdown, product=milk)
# db.session.add(c_milk)
# price1 = Prices(price_date=date_now, price=5.50, is_onsale=False, store=countdown, product=milk)
# db.session.add(price1)
# db.session.commit()
