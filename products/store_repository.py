from products.product_db import Store, db
from sqlalchemy import select


class StoreRepository:

    @staticmethod
    def get_by_name(name: str):
        """
        Get store id from database corresponding to provided store name. If the store does not exist, create a new
        store then get the new store id from the new store.
        """
        # select statement to get column where store name = countdown only
        # store = select(Store).where(Store.store_name == name).one_or_none()
        store = db.session.query(Store).filter(Store.store_name == name).one_or_none()
        return store

    """
    def save()
    """

    """
    def delete()
    """