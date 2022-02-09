import pytz
import requests
import re
import json
from datetime import datetime
from bs4 import BeautifulSoup
from price_definition.price import ProductPriceModel


class PaknsavePriceRetriever:

    @staticmethod
    def request_product_price(store_product_code: str):
        """
       Retrieve product info from paknsave website through an html parser, which extracts info eg price, name
       """

        url = f'https://www.paknsave.co.nz/shop/product/{store_product_code}_ea_000pns'

        cookies = {
            'brands_store_id': '{815DCF68-9839-48AC-BF94-5F932A1254B5}',
            # Paknsave albany store ID
            'eCom_STORE_ID': '65defcf2-bc15-490e-a84f-1f13b769cd22'
        }

        response = requests.get(url, cookies=cookies)
        contents = response.content

        # convert html document to nested data structure
        page = BeautifulSoup(contents, 'html.parser')

        # extract useful portion of html into a json object
        response_object = json.loads(page.find('script', type='application/ld+json').string, strict=False)

        #  find span with sale info
        spans = page.find_all('span', {'aria-label': 'badge PNS/Everyday_Low.svg'}) or page.find_all\
            ('span', {'aria-label': 'badge PNS/6000-Extra_Low.svg'})
        # split the span into a list of strings which will either contain Everyday_Low or
        # Extra_Low indicating the product is onsale
        onsale_list = re.split('([-/.]+)', str(spans[0]))

        return response_object, onsale_list

    @staticmethod
    def create_price(response_object, onsale_list):
        """
        Return an instance of ProductPriceModel with appropriate details such as original price, sale price, date, if
        the product is on sale or not
        """

        # TODO: infer original price by looking at the previous time that it wasn't on sale
        original_price = 0
        sale_price = response_object['offers']['price']

        # split link of availability and only display InStock or OutOfStock
        current_availability = (response_object['offers']['availability']).split('/')[-1]
        if current_availability == 'InStock':
            is_available = True
        else:
            is_available = False

        # check if Everyday_Low or Extra_Low is in onsale_list and set is_onsale either to False or True accordingly
        if 'Extra_Low' in onsale_list:
            product_on_sale = True
        else:
            product_on_sale = False

        timezone = pytz.timezone('Pacific/Auckland')

        price = ProductPriceModel(datetime.now(timezone).date(), original_price, sale_price,
                                  product_on_sale, is_available)

        return price


p = PaknsavePriceRetriever

p.request_product_price('5201479')
