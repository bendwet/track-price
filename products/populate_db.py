from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from countdown_api.countdown_product_retriever import countdown_product
from product_db import db, Stores, StoreProducts, Products
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


class InsertPrice:
    pass


