from countdown_api.countdown_product_retriever import CountdownProductRetriever
from products.populate_db import DatabasePopulator


def save_countdown_product():
    """
    Call countdown product retriever with provided store product code, then send relevant details to database, eg
    product name, unit of measurement, company name etc.
    """
    product_retriever = CountdownProductRetriever()
    countdown_product = product_retriever.get_product_details('769475')

    # Send product details to database
    database_populator = DatabasePopulator()
    database_populator.save_product(countdown_product)


save_countdown_product()

# TODO: retrieve products from paknsave
"""
def create_paknsave
    pass
"""
