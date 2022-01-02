from sqlalchemy import select

from products.product_db import db, Product


class ProductRepository:

    @staticmethod
    def get_by_name(name: str) -> Product:
        return db.session.query(Product).filter(Product.product_name == name).one_or_none()

    @staticmethod
    def save(product: Product):
        """
        Insert a new product into the product table with provided information as columns.
        """

        # If product already exists then update product, else insert new product
        # a product is a new product if it does not have a product_id/product_id = None
        if product.product_id is None:
            # add is called in order to have access to product.product_id
            db.session.add(product)
        else:
            db.session.merge(product)
        db.session.commit()

    """def delete():"""

