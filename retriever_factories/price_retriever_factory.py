from countdown_api.countdown_price_retriever import CountdownPriceRetriever
from products.populate_db import InsertPrice
from sqlalchemy import select
from products.product_db import db, StoreProducts


def create_countdown_price():
    """
    Call countdown product retriever with provided store product code, then send relevant details to database, eg
    product name, unit of measurement, company name etc.
    """

    product_code_list = []

    # select statement to get column where store name = countdown only
    product_info = select(StoreProducts.store_id, StoreProducts.product_id, StoreProducts.store_product_code)
    print(product_info)
    # execute the select
    result = db.session.execute(product_info)
    print(result)
    store_product_result = result.all()
    print(store_product_result)

    for row in store_product_result:
        if row[2] not in product_code_list:
            product_code_list.append(row[2])

    print(product_code_list)

    # price_retriever = CountdownPriceRetriever('331899')
    # countdown_product = price_retriever.get_product_price()
    # send_price = InsertPrice()
    # send_price.price_run_all()


create_countdown_price()

# TODO: retrieve prices from paknsave
"""
def create_paknsave_price
    pass
"""