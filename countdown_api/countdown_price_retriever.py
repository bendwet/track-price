import datetime
import requests
from price_definition.price import Price

# temp variables
COMPANY_COUNTDOWN = 'countdown'
COMPANY_PACKNSAVE = 'packnsave'


class CountdownPriceRetriever:

    def __init__(self, product_id: str, company_product_id: str):
        self.company_product_id = company_product_id
        self.product_id = product_id

    def get_product_price(self):
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
        print(response_object["name"])

        # set price with other details
        price = Price(self.product_id, self.company_product_id, COMPANY_COUNTDOWN,
                      datetime.date.today(), response_object["price"]["salePrice"])

        return price


print(datetime.date.today())
c = CountdownPriceRetriever('1',  '282819')
c.get_product_price()
