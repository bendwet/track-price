import json
from flask_cors import CORS 
from products.product_db import db, app, Store, Product, Price, StoreProduct
from sqlalchemy import desc

CORS(app)

@app.route('/retrieve_product', methods=['GET'])
def retrieve_product():

    """
    Retrieve product information and return as json object
    """

    store_product_result = db.session.query(StoreProduct).all()
    item_list = []
    
    # for each item in store products table, retrieve store name, price and product details from database
    for item in store_product_result:
        product_result = db.session.query(Product).filter(Product.product_id == item.product_id).first()
        store_result = db.session.query(Store).filter(Store.store_id == item.store_id).first()
        price_result = db.session.query(Price).filter(Price.product_id == item.product_id).order_by(desc(Price.price_id)).first()
        item_dict = {'product_name': product_result.product_name, 'unit_of_measure': product_result.unit_of_measure, 
        'unit_of_measure_size': product_result.unit_of_measure_size, 'current_price': price_result.price_sale, 'store_name': store_result.store_name}
        item_list.append(item_dict)

    # convert to json object
    json_object = json.dumps(item_list)

    return json_object