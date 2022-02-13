import pytz
import requests
import re
import json
from datetime import datetime
from bs4 import BeautifulSoup
from price_definition.price import ProductPriceModel
from paknsave_api.paknsave_constants import cookies


class PaknsavePriceRetriever:

    @staticmethod
    def request_product_price(store_product_code: str):
        """
       Retrieve product info from paknsave website through an html parser, which extracts info eg price, name
       """

        url = f'https://www.paknsave.co.nz/shop/product/{store_product_code}_ea_000pns'

        response = requests.get(url, cookies=cookies)

        # if response fails, try again with link for per kg instead of each
        if not response:
            url = f'https://www.paknsave.co.nz/shop/product/{store_product_code}_kgm_000pns'
            response = requests.get(url, cookies=cookies)

        contents = response.content

        # convert html document to nested data structure
        page = BeautifulSoup(contents, 'html.parser')

        return page

    @staticmethod
    def create_price(page):
        """
        Return an instance of ProductPriceModel with appropriate details such as original price, sale price, date, if
        the product is on sale or not. Information is extracted from the parsed html page variable.
        """

        # extract useful portion of html into a json object
        response_object = json.loads(page.find('script', type='application/ld+json').string, strict=False)

        # split link of availability and only display InStock or OutOfStock
        current_availability = (response_object['offers']['availability']).split('/')[-1]
        if current_availability == 'InStock':
            is_available = True
        else:
            is_available = False

        if is_available:

            #  find span with sale info
            spans = page.find_all('span', {'aria-label': 'badge PNS/Everyday_Low.svg'}) or page.find_all \
                ('span', {'aria-label': 'badge PNS/6000-Extra_Low.svg'})
            # split the span into a list of strings which will either contain Everyday_Low or
            # Extra_Low indicating the product is onsale
            onsale_list = re.split('([-/.]+)', str(spans[0]))

            # TODO: infer original price by looking at the previous time that it wasn't on sale
            original_price = 0
            sale_price = response_object['offers']['price']

            # check if Everyday_Low or Extra_Low is in onsale_list and set is_onsale either to False or True accordingly
            if 'Extra_Low' in onsale_list:
                product_on_sale = True
            # if price not on sale, se original price = to sale price
            else:
                product_on_sale = False
                original_price = sale_price

        else:
            original_price = 0
            product_on_sale = 0
            sale_price = 0

        timezone = pytz.timezone('Pacific/Auckland')

        price = ProductPriceModel(datetime.now(timezone).date(), original_price, sale_price,
                                  product_on_sale, is_available)

        return price
