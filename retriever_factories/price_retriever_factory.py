from countdown_api.countdown_price_retriever import CountdownPriceRetriever
from products.database_populator import DatabasePopulator
from sqlalchemy import select
from products.product_db import db, StoreProduct, Store


def save_countdown_price():
    """
    Get price of provided store product code and send relevant details to database.
    """

    # get store_id of countdown
    countdown_store_id = db.session \
        .query(Store.store_id) \
        .filter(Store.store_name == 'countdown') \
        .one_or_none()[0]

    # get store product codes where the store id corresponds to countdown
    store_product_codes = [store_product_code for store_product_code, in
                           db.session.query(StoreProduct.store_product_code).filter(
                               StoreProduct.store_id == countdown_store_id)]

    countdown_price_retriever = CountdownPriceRetriever()
    database_populator = DatabasePopulator()

    for store_product_code in store_product_codes:
        countdown_product_price = countdown_price_retriever.get_product_price(store_product_code)
        database_populator.save_price(countdown_product_price, store_product_code)


save_countdown_price()

