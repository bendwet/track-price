import requests
import json
from bs4 import BeautifulSoup


class PaknsaveProductRetriever:

    @staticmethod
    def get_company_product():
        """
        Retrieve product info from paknsave website through an html parser, which extracts info eg price, name
        """
        store_product_code = '5201479'

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
        product_info = json.loads(soup.find("script", type='application/ld+json').string)
        print(product_info)

        return product_info


p = PaknsaveProductRetriever

p.get_company_product()

