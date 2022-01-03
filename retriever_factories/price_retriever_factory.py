from countdown_api.countdown_price_retriever import CountdownPriceRetriever
from products.database_populator import DatabasePopulator
from sqlalchemy import select
from products.product_db import db, Price, StoreProduct, Store


def save_countdown_price():
    """
    Get price of provided store product code and send relevant details to database.
    """

    # get store_id of countdown
    countdown_store_id = db.session \
        .query(Store.store_id) \
        .filter(Store.store_name == 'countdown') \
        .one_or_none()[0]

    # select store_product_code from store_products table if the store is countdown
    select_store_product_codes = select(StoreProduct.store_product_code) \
        .where(StoreProduct.store_id == countdown_store_id)
    # execute the select
    result = db.session.execute(select_store_product_codes)
    # scalars to return single element rather than column row
    store_product_codes = result.scalars().all()

    countdown_price_retriever = CountdownPriceRetriever()
    database_populator = DatabasePopulator()

    for store_product_code in store_product_codes:
        countdown_product_price = countdown_price_retriever.get_product_price(store_product_code)
        database_populator.save_price(countdown_product_price, store_product_code)


save_countdown_price()

