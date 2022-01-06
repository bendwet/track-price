from countdown_api.countdown_product_retriever import CountdownProductRetriever
from products.database_populator import DatabasePopulator


def save_countdown_product():
    """
    Call countdown product retriever with provided store product code, then send relevant details to database, eg
    product name, unit of measurement, company name etc.
    """
    product_retriever = CountdownProductRetriever()
    response_object = product_retriever.request_company_product('282819')
    countdown_product = product_retriever.create_product('282819', response_object)

    # Send product details to database
    database_populator = DatabasePopulator()
    database_populator.save_product(countdown_product)


save_countdown_product()

# TODO: retrieve products from paknsave
"""
def create_paknsave
    pass
"""
