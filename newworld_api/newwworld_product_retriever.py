import requests
import json
import re
import httpx
from bs4 import BeautifulSoup
from define_product.company_product import StoreProductModel
from newworld_api.newworld_constants import cookies


class NewWorldProductRetriever:

    @staticmethod
    def request_company_product(store_product_code: str):
        """
        Retrieve product info from new world website through an html parser, which extracts info eg name
        """

        # link used for 'each' products
        url = f'https://www.newworld.co.nz/shop/product/{store_product_code}_ea_000nw'

        headers = {
            'User-agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:98.0) Gecko/20100101 Firefox/98.0',
            'Accept-Encoding': 'gzip, deflate, br',
            'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8',
            'Content-Type': 'text/html; charset=utf-8',
        }

        # configure use of http2
        client = httpx.Client(http2=True)
        # perform ge request
        response = client.get(url, headers=headers, cookies=cookies)

        # if response fails, try again with link for per kg instead of each
        if response.status_code != 200:
            url = f'https://www.paknsave.co.nz/shop/product/{store_product_code}_kgm_000pns'
            response = client.get(url, headers=headers, cookies=cookies)

        contents = response.content

        # convert html document to nested data structure
        page = BeautifulSoup(contents, 'html.parser')

        # extract useful portion of html into a json object, strict allows control character such as \n
        response_object = json.loads(page.find("script", type='application/ld+json').string, strict=False)

        return response_object

    @staticmethod
    def create_product(store_product_code: str, product_info):
        # split name into product name and product quantity
        split_name = product_info['name'].split()
        product_name = ' '.join(split_name[:-1])
        quantity = split_name[-1]

        # check if quantity is just 'kg' without any numeric value
        if quantity == 'kg':
            unit_of_measure = 'kg'
            unit_of_measure_size = float(1)
        # check if quantity is 'ea' for each with no numeric value
        elif quantity == 'ea':
            unit_of_measure = 'ea'
            unit_of_measure_size = float(1)
        else:
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

        product = StoreProductModel(store_product_code, 'new world', product_name, unit_of_measure,
                                    unit_of_measure_size)

        return product
