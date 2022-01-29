from define_product.company_product import StoreProductModel
from price_definition.price import ProductPriceModel
from products.product_db import Product
from products.store_repository import StoreRepository
from products.product_repository import ProductRepository
from products.store_product_repository import StoreProductRepository
from products.price_repository import PriceRepository


class DatabasePopulator:

    @staticmethod
    def save_product(store_product: StoreProductModel):

        store_repository = StoreRepository()
        product_repository = ProductRepository()
        store_product_repository = StoreProductRepository()

        store = store_repository.get_by_name(store_product.store_name)

        # Map the company product model to a product

        product = product_repository.get_by_name(store_product.product_name)
        # check if product does not exist already
        if product is None:
            product = Product()
            product.product_name = store_product.product_name

        # if product exists check if quantity size differs
        if product is not None and float(store_product.unit_of_measure_size) != product.unit_of_measure_size:
            product = Product()
            product.product_name = store_product.product_name

        product.unit_of_measure = store_product.unit_of_measure
        product.unit_of_measure_size = store_product.unit_of_measure_size

        product_repository.save(product)

        store_product_repository.create_store_product(product.product_id, store.store_id,
                                                      store_product.store_product_code)

    @staticmethod
    def save_price(product_price: ProductPriceModel, store_product_code: str):

        price_repository = PriceRepository()

        # get store_id and product_id
        store_product = price_repository.get_by_store_product_code(store_product_code)

        price_repository.create_price(store_product.product_id, store_product.store_id, product_price.price_date,
                                      product_price.price, product_price.is_onsale, product_price.price_sale,
                                      product_price.is_available)

