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

        if product is None:
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
                                      product_price.price, product_price.is_onsale, product_price.price_sale)

        pass


# class InsertPrice:
#
#     # id variables
#     price_store_id = 0
#     price_product_id = 0
#
#     def __init__(self, new_product_price, new_price_date, new_is_onsale, new_price_sale, store_item_code):
#         self.new_price_date = new_price_date
#         self.new_product_price = new_product_price
#         self.new_is_onsale = new_is_onsale
#         self.new_price_sale = new_price_sale
#         self.store_item_code = store_item_code
#
#     def get_id(self):
#         """
#         Get product_id and store_id from store_products table, raise error if provided product code does not exist
#         in the table.
#         """
#         get_ids = select(StoreProducts.store_id, StoreProducts.product_id).where(StoreProducts.store_product_code ==
#                                                                                  self.store_item_code)
#         result = db.session.execute(get_ids)
#         id_result = result.all()[0]
#         self.price_store_id = id_result[0]
#         self.price_product_id = id_result[1]
#
#     def new_price(self):
#         """
#         Insert new price of product into price table.
#         """
#         insert_new_price = Prices(product_id=self.price_product_id, store_id=self.price_store_id,
#                                   price_date=self.new_price_date, price=self.new_product_price,
#                                   is_onsale=self.new_is_onsale)
#
#         db.session.add(insert_new_price)
#         db.session.commit()
