import datetime
import requests
from price_definition.price import ProductPriceModel
from retrying import retry


class CountdownPriceRetriever:

    @staticmethod
    @retry(stop_max_attempt_number=20, wait_random_min=10000, wait_random_max=60000)
    def request_product_price(store_product_code: str):
        """
        Retrieve product price from countdown api for provided product code and return json object of response. Retry
        request after a period of time if request fails.
        """
        url = f'https://shop.countdown.co.nz/api/v1/products/{store_product_code}'
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

        return response_object

    @staticmethod
    def create_price(response_object):
        """
        Return an instance of ProductPriceModel with appropriate details such as original price, sale price, date, if
        the product is on sale or not
        """
        # get original price and sale price
        original_price = response_object["price"]["originalPrice"]
        sale_price = response_object["price"]["salePrice"]

        # check if product is on sale
        if sale_price < original_price:
            product_on_sale = True
        else:
            product_on_sale = False

        if original_price == 0:
            is_available = False
        else:
            is_available = True

        # set price with other details
        price = ProductPriceModel(datetime.date.today(), original_price, sale_price,
                                  product_on_sale, is_available)

        return price

