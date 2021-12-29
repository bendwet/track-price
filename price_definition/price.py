from datetime import datetime


# holds a price definition
class Price:
    def __init__(self, company_product_id: str, company_name: str, price_date: datetime.date,
                 original_price: float, sale_price: float, is_on_sale: bool):
        self.company_product_id = company_product_id
        self.company_name = company_name
        self.price_date = price_date
        self.original_price = original_price
        self.sale_price = sale_price
        self.is_on_sale = is_on_sale
