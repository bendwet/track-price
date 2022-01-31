import json
from products.product_db import db, app, Store, Product, StoreProduct


@app.route('/retrieve_product', methods=['GET'])
def retrieve_product():
    """
    Retrieve product information and return as json object
    """
    result = db.session.query(Product).all()
    item_list = []
    # create dictionary for each product
    for item in result:
        item_dict = {'product_id': item.product_id, 'product_name': item.product_name,
                     'unit_of_measure': item.unit_of_measure, 'unit_of_measure_size': item.unit_of_measure_size}
        item_list.append(item_dict)

    # convert to json object
    json_object = json.dumps(item_list)
    
    return json_object


app.run()
