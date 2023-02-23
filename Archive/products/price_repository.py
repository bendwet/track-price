import datetime

from products.product_db import db, Price, StoreProduct


class PriceRepository:

    @staticmethod
    def get_by_store_product_code(store_product_code, store_id):
        """
        Get store product by store product code and store id
        """

        return db.session.query(StoreProduct).filter(StoreProduct.store_product_code == store_product_code,
                                                     StoreProduct.store_id == store_id).first()

    @staticmethod
    def create_price(product_id: int, store_id: int, price_date: datetime.date, price: float,
                     is_onsale: bool, price_sale: float, is_available: bool, price_quantity: str):

        price_exists = db.session.query(Price).filter(Price.product_id == product_id, Price.store_id == store_id,
                                                      Price.price_date == price_date).one_or_none()

        # Check if price exists and update or add new price if does not exist
        if price_exists is None:
            product_price = Price()
            product_price.product_id = product_id
            product_price.store_id = store_id
            product_price.price_date = price_date
            product_price.price = price
            product_price.is_onsale = is_onsale
            product_price.price_sale = price_sale
            product_price.is_available = is_available
            product_price.price_quantity = price_quantity
            db.session.add(product_price)
        else:
            price_exists.price = price
            price_exists.is_onsale = is_onsale
            price_exists.price_sale = price_sale
            price_exists.is_available = is_available

        db.session.commit()
