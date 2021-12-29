from products.product_db import db, StoreProduct


class StoreProductRepository:

    @staticmethod
    def create_store_product(product_id: int, store_id: int, store_product_code: str):
        existing_store_product = db.session\
            .query(StoreProduct)\
            .filter(StoreProduct.store_id == store_id, StoreProduct.product_id == product_id)\
            .one_or_none()

        if existing_store_product is None:
            existing_store_product = StoreProduct()
            existing_store_product.store_id = store_id
            existing_store_product.product_id = product_id

        existing_store_product.store_product_code = store_product_code

        db.session.merge(existing_store_product)
        db.session.commit()

    """def delete()"""
