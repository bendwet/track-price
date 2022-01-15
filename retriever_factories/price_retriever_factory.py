from countdown_api.countdown_price_retriever import CountdownPriceRetriever
from products.database_populator import DatabasePopulator
from products.product_db import db, StoreProduct, Store
import requests
import os

# print(os.environ)


def save_price():
    """
    Get price of provided store product code and send relevant details to database.
    """

    # tns_admin_folder = os.environ["TNS_ADMIN"]
    # print(f"test tns_admin folder = {tns_admin_folder}")

    # get store_id and store_name for each store in Store table
    get_stores = [store_row for store_row in db.session.query(Store.store_id, Store.store_name)]

    print(get_stores)

    for store in get_stores:

        # get store product codes where the store id corresponds to countdown
        store_product_codes = [store_product_code for store_product_code, in
                               db.session.query(StoreProduct.store_product_code).filter(
                                   StoreProduct.store_id == store[0])]

        price_retriever = CountdownPriceRetriever()

        if store[1] == "paknsave":
            # price_retriever = PaknsavePriceRetriever()
            pass

        database_populator = DatabasePopulator()

        # call price retriever and send price to database for each store product code
        for store_product_code in store_product_codes:
            try:
                response_object = price_retriever.request_product_price(store_product_code)
                product_price = price_retriever.create_price(response_object)
                database_populator.save_price(product_price, store_product_code)
            except requests.exceptions.HTTPError as err:
                print(f'Error number when retrieving price: {store_product_code} for {store[1]}: {err}')
