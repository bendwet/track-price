from countdown_api.countdown_price_retriever import CountdownPriceRetriever
from products.populate_db import InsertPrice
from sqlalchemy import select
from products.product_db import db, StoreProducts


class InsertCountdownPrice:

    # store_id: index 0, product_id: index 1, store_product_code: index 2
    product_code_list = []

    def get_product_info(self):
        """
        Get all store product codes, store id's and product id's from the store_products table and add all info to list.
        """
        # select statement to get column where store name = countdown only
        product_info = select(StoreProducts.store_id, StoreProducts.product_id, StoreProducts.store_product_code)
        print(product_info)
        # execute the select
        result = db.session.execute(product_info)
        print(result)
        store_product_result = result.all()
        print(store_product_result)

        # Only append rows with non duplicate product codes to avoid retrieving duplicate prices for the same product
        for row in store_product_result:
            # store_product_code in index position 2
            if row[2] not in self.product_code_list:
                self.product_code_list.append(row[2])

        print(self.product_code_list)

    def create_countdown_price(self):
        """
        Call countdown price retriever with provided store product code, then insert relevant details into database, eg
        price, price date etc
        """
        # For each product code in the product code list, send relevant details to populate_db
        for product_code in self.product_code_list:
            print(product_code)
            price_retriever = CountdownPriceRetriever(product_code)
            countdown_price = price_retriever.get_product_price()
            send_price = InsertPrice(countdown_price.original_price, countdown_price.price_date,
                                     countdown_price.is_on_sale, countdown_price.sale_price, product_code)
            send_price.price_run_all()


# TODO: retrieve prices from paknsave
"""
def create_paknsave_price
    pass
"""

countdown_prices = InsertCountdownPrice()
countdown_prices.get_product_info()
countdown_prices.create_countdown_price()
