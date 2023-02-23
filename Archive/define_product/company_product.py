# holds a product definition
class StoreProductModel:
    def __init__(self, store_product_code: str, store_name: str, product_name: str,
                 unit_of_measure: str, unit_of_measure_size: float):
        self.store_product_code = store_product_code
        self.store_name = store_name
        self.product_name = product_name
        self.unit_of_measure = unit_of_measure
        self.unit_of_measure_size = unit_of_measure_size

