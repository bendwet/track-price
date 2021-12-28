from datetime import datetime


# holds a price definition
class Price:
    def __init__(self, company_product_id: str, company_name: str, price_date: datetime.date,
                 price: float):
        self.company_product_id = company_product_id
        self.company_name = company_name
        self.price_date = price_date
        self.price = price

