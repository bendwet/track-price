import re
from datetime import datetime
import pytz
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

        cookies = {"cw-lrkswrdjp": "dm-Pickup,f-9036,a-168,s-38"}

        headers = {
            # pretend to be chrome
            'user-agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) '
                          'Chrome/90.0.4430.93 Safari/537.36',
            'Accept-Encoding': 'gzip, deflate, br',
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Requested-With': 'OnlineShopping.WebApp'
        }

        response = requests.get(url, headers=headers, cookies=cookies, timeout=60)
        # if the response failed, raise an error
        if response.status_code != requests.codes.ok:
            response.raise_for_status()

        print(response.content)

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

        product_size = response_object['size']['volumeSize']
        package_type = response_object['size']['packageType']

        product_quantity = product_size

        if package_type == 'each' and product_size is None:
            product_quantity = 'ea'

        if package_type == 'bunch' and product_size is None:
            product_quantity = 'bunch'

        # check if volume = "per kg" for fruits and vegetables and convert to 1kg
        if product_size == 'per kg':
            product_quantity = '1kg'

        if product_size == '1kg pack':
            product_quantity = '1kg'

        if product_size == '4 serve':
            product_quantity = '4pk'

        # if toiler paper is in name, it will have no quantity, so search in product name for quantity
        if 'toilet paper' in response_object['name'] and 'pk' in response_object['name']:
            try:
                product_quantity = re.search('([0-9]+)(pk)', response_object['name']).group()
            except AttributeError:
                print('Could not retrieve quantity from name')

        # check if product is on sale
        if sale_price < original_price:
            product_on_sale = True
        else:
            product_on_sale = False

        if original_price == 0:
            is_available = False
        else:
            is_available = True

        timezone = pytz.timezone('Pacific/Auckland')

        # set price with other details
        price = ProductPriceModel(datetime.now(timezone).date(), original_price, sale_price,
                                  product_on_sale, is_available, product_quantity)
        return price


c = CountdownPriceRetriever()

c.request_product_price("282769")
