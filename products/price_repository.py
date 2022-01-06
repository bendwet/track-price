import datetime

from products.product_db import db, Price, StoreProduct


class PriceRepository:

    @staticmethod
    def get_by_store_product_code(store_product_code):
        return db.session.query(StoreProduct).filter(StoreProduct.store_product_code == store_product_code)\
            .one_or_none()

    @staticmethod
    def create_price(product_id: int, store_id: int, price_date: datetime.date, price: float,
                     is_onsale: bool, price_sale: float):

        price_exists = db.session.query(Price).filter(Price.product_id == product_id, Price.store_id == store_id,
                                                      Price.price_date == price_date).one_or_none()

        if price_exists is None:
            product_price = Price()
            product_price.product_id = product_id
            product_price.store_id = store_id
            product_price.price_date = price_date
            product_price.price = price
            product_price.is_onsale = is_onsale
            product_price.price_sale = price_sale
            db.session.add(product_price)
        else:
            price_exists.price = price
            price_exists.is_onsale = is_onsale
            price_exists.price_sale = price_sale

        db.session.commit()

