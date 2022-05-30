import requests
import sqlalchemy
from sqlalchemy import exc
from countdown_api.countdown_price_retriever import CountdownPriceRetriever
from paknsave_api.paknsave_price_retriever import PaknsavePriceRetriever
from products.database_populator import DatabasePopulator
from products.product_db import db, StoreProduct, Store
from newworld_api.newworld_price_retriever import NewWorldPriceRetriever


def save_price():
    """
    Get price of provided store product code and send relevant details to database.
    """

    # get store_id and store_name for each store in Store table
    get_stores = [store_row for store_row in db.session.query(Store.store_id, Store.store_name)]

    # store[0] = store_id, store[1] = store_name
    for store in get_stores:
        # get store product codes where the store id corresponds to store name
        store_product_codes = [store_product_code for store_product_code, in
                               db.session.query(StoreProduct.store_product_code).filter(
                                   StoreProduct.store_id == store[0])]

        # default price retriever is countdown
        price_retriever = CountdownPriceRetriever()
        if store[1] == 'paknsave':
            price_retriever = PaknsavePriceRetriever()
        elif store[1] == 'new world':
            price_retriever = NewWorldPriceRetriever()

        database_populator = DatabasePopulator()

        # call price retriever and send price to database for each store product code
        for store_product_code in store_product_codes:
            try:
                if store[1] == 'countdown':
                    print('skip')
                else:
                    print(store[1])
                    print(store_product_code)
                    response_object = price_retriever.request_product_price(store_product_code)
                    product_price = price_retriever.create_price(response_object)
                    database_populator.save_price(product_price, store_product_code, store[0])
            except requests.exceptions.HTTPError as err:
                print(f'Error number when retrieving price: {store_product_code} for {store[1]}: {err}')
            except sqlalchemy.exc.IntegrityError as err:
                print(f'Error number when retrieving price: {store_product_code} for {store[1]}: {err}')
                db.session.rollback()
