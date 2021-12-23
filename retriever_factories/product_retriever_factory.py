from countdown_api.countdown_product_retriever import CountdownProductRetriever
from products.populate_db import InsertProduct


def create_countdown():
    product_retriever = CountdownProductRetriever(1,  '331899')
    countdown_product = product_retriever.get_product_details()
    send_product = InsertProduct(countdown_product.company_name, countdown_product.product_name,
                                 countdown_product.company_product_id,countdown_product.product_unit_of_measurement,
                                 countdown_product.product_quantity)
    send_product.product_run_all()


create_countdown()

# TODO: retrieve products from paknsave

