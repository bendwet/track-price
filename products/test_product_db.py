import unittest
from product_db import *

TABLE_LIST = [Stores, Products, StoreProducts, Prices]


class TestProductDb(unittest.TestCase):

    @classmethod
    def setUpClass(cls):
        for table in TABLE_LIST:
            table.query.delete()
        db.session.commit()

        cls.store1 = Stores(store_name='countdown')
        cls.store2 = Stores(store_name='paknsave')
        cls.milk = Products(product_name='milk', unit_of_measure='L', unit_of_measure_size=3.0)
        cls.cheese = Products(product_name='cheese', unit_of_measure='kg', unit_of_measure_size=1.0)

    def test_stores(self):
        db.session.add(self.store1)
        db.session.add(self.store2)
        db.session.commit()

    def test_products(self):
        db.session.add(self.milk)
        db.session.add(self.cheese)
        db.session.commit()

    def test_store_products(self):
        self.store_milk = StoreProducts(store_product_code='abc123', store=self.store1, product=self.milk)
        self.store_cheese = StoreProducts(store_product_code='def456', store=self.store2, product=self.cheese)
        db.session.add(self.store_milk)
        db.session.add(self.store_cheese)
        db.session.commit()

    def test_prices(self):
        self.price1 = Prices(price_date=date_now, price=5.50, is_onsale=False, store=self.store1, product=self.milk)
        self.price2 = Prices(price_date=date_now, price=7.50, is_onsale=False, store=self.store2, product=self.cheese)
        db.session.add(self.price1)
        db.session.add(self.price2)
        db.session.commit()


if __name__ == '__main__':
    unittest.main()

