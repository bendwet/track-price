import json
from ntpath import join
from flask_cors import CORS 
from products.product_db import db, app, Store, Product, Price, StoreProduct
from sqlalchemy import desc
from sqlalchemy import func
from datetime import datetime

CORS(app)

@app.route('/retrieve_product', methods=['GET'])
def retrieve_product():

    """
    Retrieve product information and return as json object
    """

    item_list = []
    # query and join price, store, and product tables
    join_query = (db.session.query(Price, Store, Product).join(Product).join(Store).filter(Price.price_date == db.session.query(func.max(Price.price_date))))
    product_info = join_query.all()

    # list of items
    item_list = []
    
    # for each queried item create a list of dictionaries from each table then merge into 1 dicitonary and append to item_list
    for result in product_info:
        item_dict_list = ([row.__dict__ for row in result])
        item_dict = {key: value for item_dict in item_dict_list for key, value in item_dict.items()}
        item_dict.pop('_sa_instance_state')
        item_list.append(item_dict)

    json_object = json.dumps(item_list, default=str)

    return json_object
