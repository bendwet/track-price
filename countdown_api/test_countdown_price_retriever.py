import unittest
import datetime
from countdown_price_retriever import CountdownPriceRetriever, COMPANY_COUNTDOWN


class TestCountdownPriceRetriever(unittest.TestCase):

    def test_product_id(self):
        p_id1 = CountdownPriceRetriever(1, '282819')
        p_id2 = CountdownPriceRetriever(2, '282819')

        self.assertEqual(p_id1.product_id, 1)
        self.assertEqual(p_id2.product_id, 2)

        p_id1 = CountdownPriceRetriever(3, '282819')
        p_id2 = CountdownPriceRetriever(4, '282819')

        self.assertEqual(p_id1.product_id, 3)
        self.assertEqual(p_id2.product_id, 4)

    def test_company_product_id(self):
        cp_id1 = CountdownPriceRetriever(1, '282819')

        self.assertEqual(cp_id1.company_product_id, '282819')

    def test_company_name(self):
        c_name1 = CountdownPriceRetriever(1, '282819')
        result = c_name1.get_product_price()

        self.assertEqual(result.company_name, COMPANY_COUNTDOWN)

    def test_date(self):
        c = CountdownPriceRetriever(1, '282819')
        result = c.get_product_price()

        self.assertEqual(result.price_date, datetime.date.today())

    def test_price(self):
        c = CountdownPriceRetriever(1, '282819')
        result = c.get_product_price()

        self.assertEqual(result.price, 4.8)


if __name__ == '__main__':
    unittest.main()
