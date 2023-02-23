from countdown_api.countdown_product_retriever import CountdownProductRetriever
from paknsave_api.paknsave_product_retriever import PaknsaveProductRetriever
from newworld_api.newwworld_product_retriever import NewWorldProductRetriever
from products.database_populator import DatabasePopulator


def save_countdown_product():
    """
    Call countdown product retriever with provided store product code, then send relevant details to database, eg
    product name, unit of measurement, company name etc.
    """

    product_retriever = CountdownProductRetriever()
    response_object = product_retriever.request_company_product('282768')
    countdown_product = product_retriever.create_product('282768', response_object)

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
    response_object = product_retriever.request_company_product('5045858')
    paknsave_product = product_retriever.create_product('5045858', response_object)

    # send product details to database
    database_populator = DatabasePopulator()
    database_populator.save_product(paknsave_product)


save_paknsave_product()


def save_newworld_product():
    """
    Call countdown product retriever with provided store product code, then send relevant details to database, eg
    product name, unit of measurement, company name etc.
    """

    # retrieve new world product
    product_retriever = NewWorldProductRetriever()
    try:
        response_object = product_retriever.request_company_product('5046547')
    except AttributeError as err:
        print(f'Error retrieving product with error: {err}')
        return

    newworld_product = product_retriever.create_product('5046547', response_object)

    # send product details to database
    database_populator = DatabasePopulator()
    database_populator.save_product(newworld_product)


# save_newworld_product()
