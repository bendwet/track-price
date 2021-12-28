# holds a product definition
class Product:
    def __init__(self, company_product_id: str, company_name: str, product_name: str,
                 product_unit_of_measurement: str, product_quantity: float):
        self.company_product_id = company_product_id
        self.company_name = company_name
        self.product_name = product_name
        self.product_unit_of_measurement = product_unit_of_measurement
        self.product_quantity = product_quantity

