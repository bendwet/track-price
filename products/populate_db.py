from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from countdown_api.countdown_product_retriever import countdown_product
from product_db import db, Stores, StoreProducts, Products, Prices
from sqlalchemy import select

TABLE_NAMES = ['stores', 'products', 'store_products', 'prices']


class InsertProduct:

    new_store_id = 0
    new_product_id = 0

    def __init__(self, name_of_store, new_product_name, new_product_code, new_unit_of_measure, new_size):
        self.name_of_store = name_of_store
        self.new_product_name = new_product_name
        self.new_product_code = new_product_code
        self.new_unit_of_measure = new_unit_of_measure
        self.new_size = new_size

    def get_store(self):
        """
        Get store id from database corresponding to provided store name. If the store does not exist, create a new
        store then get the new store id from the new store.
        """
        # select statement to get column where store name = countdown only
        store = select(Stores).where(Stores.store_name == self.name_of_store)
        # execute the select
        result = db.session.execute(store)
        # scalars() to return single element rather than row object, returns columns value instead of the column itself
        store_id_result = result.scalars().all()[0].store_id
        self.new_store_id = store_id_result
        print(self.new_store_id)

    def new_product(self):
        """
        Insert a new product into the product table with provided information as columns.
        """
        insert_new_product = Products(product_name=self.new_product_name, unit_of_measure=self.new_unit_of_measure,
                                      unit_of_measure_size=self.new_size)
        db.session.add(insert_new_product)
        db.commit()
        self.new_product_id = insert_new_product.product_id
        print(self.new_product_id)

    def new_store_product(self):
        """
        Insert a new store product
        """
        insert_new_store_product = StoreProducts(store_product_code=self.new_product_code, store_id=self.new_product_id,
                                                 product_id=self.new_product_id)
        db.session.add(insert_new_store_product)
        db.commit()

    def product_run_all(self):
        self.get_store()
        self.new_product()
        self.new_product()


class InsertPrice:

    new_store_id = 0
    new_product_id = 0

    def __init__(self, new_price_date, new_price, new_is_onsale, new_price_sale, store_item_code):
        self.new_price_date = new_price_date
        self.new_product_price = new_price
        self.new_is_onsale = new_is_onsale
        self.new_price_sale = new_price_sale
        self.store_item_code = store_item_code

    def get_id(self):
        """
        Get product_id and price_id from store_products table, raise error if provided product code does not exist
        in the table.
        """
        get_ids = select(StoreProducts.store_id, StoreProducts.product_id).where(StoreProducts.store_product_code ==
                                                                                 self.store_item_code)
        result = db.session.execute(get_ids)
        id_result = result.all()[0]
        self.new_store_id = id_result[0]
        self.new_product_id = id_result[1]

    def new_price(self):
        """
        Insert new price of product into price table.
        """
        insert_new_price = Prices(product_id=self.new_product_id, store_id=self.new_store_id,
                                  price_date=self.new_price_date, price=self.new_product_price,
                                  is_onsale=self.new_is_onsale)

        db.session.add(insert_new_price)
        db.session.commit()
        
    def price_run_all(self):
        self.get_id()
        self.new_price()
