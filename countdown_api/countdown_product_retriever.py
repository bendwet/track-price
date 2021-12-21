import requests
import re
from define_product.product import Product

# temp variables
COMPANY_COUNTDOWN = 'countdown'
COMPANY_PACKNSAVE = 'packnsave'


class CountdownProductRetriever:

    def __init__(self, product_id: int, company_product_id: str):
        self.company_product_id = company_product_id
        self.product_id = product_id

    def get_product_details(self):
        """
        Retrieve product details from countdown api for provided product code and return details about the product,
        name, id, store product code, company name, object quantity.
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
        # print(response_object)
        # print(response_object["name"])

        # split object quantity into unit of measurement and size
        product_size = response_object["size"]["volumeSize"]
        split_size = re.split('([0-9.]+)', product_size)
        # print(split_size)

        # set price with other details
        product_details = Product(self.product_id, self.company_product_id, COMPANY_COUNTDOWN, response_object["name"],
                                  split_size[2], split_size[1])

        return product_details


c = CountdownProductRetriever(1,  '148425')
countdown_product = c.get_product_details()

# print(countdown_product.product_name)
