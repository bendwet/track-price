from countdown_api.countdown_product_retriever import CountdownProductRetriever
from paknsave_api.paknsave_product_retriever import PaknsaveProductRetriever
from products.database_populator import DatabasePopulator


def save_countdown_product():
    """
    Call countdown product retriever with provided store product code, then send relevant details to database, eg
    product name, unit of measurement, company name etc.
    """

    product_retriever = CountdownProductRetriever()
    response_object = product_retriever.request_company_product('479502')
    countdown_product = product_retriever.create_product('479502', response_object)

    # Send product details to database
    database_populator = DatabasePopulator()
    database_populator.save_product(countdown_product)


# save_countdown_product()


def save_paknsave_product():
    """
    Call countdown product retriever with provided store product code, then send relevant details to database, eg
    product name, unit of measurement, company name etc.
    """

    # retrieve paknsave product
    product_retriever = PaknsaveProductRetriever()
    response_object = product_retriever.request_company_product('5046510')
    paknsave_product = product_retriever.create_product('5046510', response_object)

    # send product details to database
    database_populator = DatabasePopulator()
    database_populator.save_product(paknsave_product)


save_paknsave_product()
