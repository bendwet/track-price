from sqlalchemy import select

from products.product_db import db, Product


class ProductRepository:

    @staticmethod
    def get_by_name(name: str) -> Product:
        test_p = db.session.query(Product).filter(Product.product_name == name).one_or_none()
        print(test_p)
        return db.session.query(Product).filter(Product.product_name == name).one_or_none()

    @staticmethod
    def save(product: Product):
        """
        Insert a new product into the product table with provided information as columns.
        """

        # If product already exists then update product, else insert new product
        # a product is a new product if it does not have a product_id
        db.session.merge(product)
        db.session.commit()

    """def delete():"""

