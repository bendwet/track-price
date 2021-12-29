import datetime
import requests
from price_definition.price import Price

# temp variables
COMPANY_COUNTDOWN = 'countdown'


class CountdownPriceRetriever:

    def __init__(self, company_product_id: str):
        self.company_product_id = company_product_id

    def get_product_price(self):
        """
        Retrieve product price from countdown api for provided product code and return price of product along
        with the provided product code, date retrieved, company name and sale price.
        """
        url = f'https://shop.countdown.co.nz/api/v1/products/{self.company_product_id}'
        headers = {
            # pretend to be chrome
            'user-agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) '
                          'Chrome/90.0.4430.93 Safari/537.36',
            'Accept-Encoding': 'gzip, deflate, br',
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Requested-With': 'OnlineShopping.WebApp'
        }
        response = requests.get(url, headers=headers)

        # if the response failed, raise an error
        if response.status_code != requests.codes.ok:
            response.raise_for_status()

        response_object = response.json()

        # get original price and sale price
        original_price = response_object["price"]["originalPrice"]
        sale_price = response_object["price"]["salePrice"]

        # check if product is on sale
        if sale_price < original_price:
            product_on_sale = True
        else:
            product_on_sale = False

        # set price with other details
        price = Price(self.company_product_id, COMPANY_COUNTDOWN, datetime.date.today(),
                      original_price, sale_price, product_on_sale)

        return price

