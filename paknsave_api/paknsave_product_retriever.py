import requests
import json
import re
from bs4 import BeautifulSoup
from define_product.company_product import StoreProductModel


class PaknsaveProductRetriever:

    @staticmethod
    def request_company_product(store_product_code: str):
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
        soup = BeautifulSoup(contents)
        # extract useful portion of html into a json object
        response_object = json.loads(soup.find("script", type='application/ld+json').string)

        return response_object

    @staticmethod
    def create_product(store_product_code: str, product_info):
        # split name into product name and product quantity
        split_name = product_info['name'].split()
        product_name = ' '.join(split_name[:-1])
        quantity = split_name[-1]
        # split quantity into unit of measure and size eg 1kg -> '1' 'kg'
        # split where there is a number 0-9 (and a '.' if there is one)
        split_size = re.split('([0-9.]+)', quantity)
        unit_of_measure = split_size[2]
        unit_of_measure_size = split_size[1]

        # convert 'l' to 'L' and 'ml' to 'mL' for consistency of units
        if unit_of_measure == 'l':
            unit_of_measure = 'L'
        elif unit_of_measure == 'ml':
            unit_of_measure = 'mL'

        product = StoreProductModel(store_product_code, 'paknsave', product_name, unit_of_measure,
                                    unit_of_measure_size)

        return product
