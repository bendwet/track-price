import json
from flask_cors import CORS 
from products.product_db import db, app, Store, Product, Price
from sqlalchemy import func

CORS(app)

@app.route('/product', methods=['GET'])
def retrieve_product():

    """
    Retrieve product information and return as json object
    """

    item_list = []
    # query and join price, store, and product tables
    join_query = (db.session.query(Price.price_date, func.min(Price.price), Price.is_onsale, func.min(Price.price_sale), Price.is_available, Store.store_id, Store.store_name, 
    Product.product_id, Product.product_name, Product.unit_of_measure, Product.unit_of_measure_size).join(Product).join(Store)
    .filter(Price.price_date == db.session.query(func.max(Price.price_date)), Price.is_available == 1).group_by(Price.product_id).order_by(Price.product_id))

    product_info = join_query.all()

    # list of items
    item_list = []
    
    # for each queried item create a list of dictionaries from each table then merge into 1 dicitonary and append to item_list
    for result in product_info:

        item_dict = {
            'price_date': result[0],
            'price': result[1],
            'is_onsale': result[2],
            'price_sale': result[3],
            'is_available': result[4],
            'store_name': result[6],
            'product_id': result[7],
            'product_name': result[8],
            'unit_of_measure': result[9],
            'unit_of_measure_size': result[10],
        }

        item_list.append(item_dict)

    json_object = json.dumps(item_list, default=str)

    return json_object

@app.route('/product/<productId>', methods=['GET', 'POST'])
def retrieve_product_by_id(productId):
    """
    Retrive products by product id
    """

    item_list = []

    # query and join price, store, and product tables
    join_query = (db.session.query(Price.price_date, Price.price, Price.is_onsale, Price.price_sale, Price.is_available, Store.store_id, Store.store_name, 
    Product.product_id, Product.product_name, Product.unit_of_measure, Product.unit_of_measure_size).join(Product).join(Store)
    .filter(Price.price_date == db.session.query(func.max(Price.price_date)), Product.product_id == productId))

    product_info = join_query.all()

    # list of items
    item_list = []
    
    # for each queried item create a list of dictionaries from each table then merge into 1 dicitonary and append to item_list
    for result in product_info:

        item_dict = {
            'price_date': result[0],
            'price': result[1],
            'is_onsale': result[2],
            'price_sale': result[3],
            'is_available': result[4],
            'store_name': result[6],
            'product_id': result[7],
            'product_name': result[8],
            'unit_of_measure': result[9],
            'unit_of_measure_size': result[10],
        }

        item_list.append(item_dict)

    json_object = json.dumps(item_list, default=str)

    return json_object

@app.route('/price/<productId>', methods=['GET', 'POST'])
def retrieve_lowest_price_by_product_id(productId):
    """
    Retrieve lowest price and date by product id and return array of objects
    """

    price_list = []

    query = (db.session.query(func.min(Price.price_sale), Price.price_date, Price.is_available).filter(Price.product_id == productId, Price.is_available == True)
    .group_by(Price.price_date).order_by(Price.price_date))

    price_info = query.all()

    for result in price_info:

        item_dict = {
            'price_sale': result[0],
            'price_date': result[1],
        }

        price_list.append(item_dict)


    json_object = json.dumps(price_list, default=str)

    return json_object
