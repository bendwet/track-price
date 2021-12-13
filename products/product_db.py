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

