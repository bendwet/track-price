from datetime import datetime


# holds a price definition
class ProductPriceModel:
    def __init__(self, price_date: datetime.date, price: float, price_sale: float, is_onsale: bool, is_available: bool):
        self.price_date = price_date
        self.price = price
        self.price_sale = price_sale
        self.is_onsale = is_onsale
        self.is_available = is_available
