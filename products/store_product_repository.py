from products.product_db import db, StoreProduct


class StoreProductRepository:

    @staticmethod
    def create_store_product(product_id: int, store_id: int, store_product_code: str):
        """
        Insert a new store product into the store products table if the store product does not already exist
        """
        existing_store_product = db.session\
            .query(StoreProduct)\
            .filter(StoreProduct.store_id == store_id, StoreProduct.product_id == product_id)\
            .one_or_none()

        # if the store product does not exist, create one
        if existing_store_product is None:
            existing_store_product = StoreProduct()
            existing_store_product.store_id = store_id
            existing_store_product.product_id = product_id

        existing_store_product.store_product_code = store_product_code

        db.session.add(existing_store_product)
        db.session.commit()

    """def delete()"""
