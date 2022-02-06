import requests
import re
from retrying import retry
from define_product.company_product import StoreProductModel

# temp variables
COMPANY_COUNTDOWN = 'countdown'


class CountdownProductRetriever:

    @staticmethod
    @retry(stop_max_attempt_number=20, wait_random_min=10000, wait_random_max=60000)
    def request_company_product(store_product_code: str):
        """
        Retrieve product from countdown api for provided product code and return json object of response. Retry
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
    def create_product(store_product_code: str, response_object) -> StoreProductModel:
        """
        Return an instance of StoreProductModel with appropriate details such as store product code, company name,
        product name, unit of measurement and unit of measurement size
        """
        # split object quantity into unit of measurement and size
        product_size = response_object["size"]["volumeSize"]

        # check if volume = "per kg" for fruits and vegetables and convert to 1kg
        if product_size == "per kg":
            product_size = "1kg"

        # split where there is a number 0-9 (and a '.' if there is one)
        split_size = re.split('([0-9.]+)', product_size)
        # set price with other details
        product = StoreProductModel(store_product_code, COMPANY_COUNTDOWN, response_object["name"], split_size[2],
                                    split_size[1])

        return product
