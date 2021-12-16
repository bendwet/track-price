import unittest
import datetime
from countdown_price_retriever import CountdownPriceRetriever, COMPANY_COUNTDOWN


class TestCountdownPriceRetriever(unittest.TestCase):

    @classmethod
    def setUpClass(cls):
        pass

    @classmethod
    def tearDownClass(cls):
        pass

    def setUp(self):
        self.c1 = CountdownPriceRetriever(1, '282819')
        self.c2 = CountdownPriceRetriever(2, '282819')

    def tearDown(self):
        pass

    def test_product_id(self):

        self.assertEqual(self.c1.product_id, 1)
        self.assertEqual(self.c2.product_id, 2)

        self.c1 = CountdownPriceRetriever(3, '282819')
        self.c2 = CountdownPriceRetriever(4, '282819')

        self.assertEqual(self.c1.product_id, 3)
        self.assertEqual(self.c2.product_id, 4)

    def test_company_product_id(self):
        self.assertEqual(self.c1.company_product_id, '282819')

    def test_company_name(self):
        result = self.c1.get_product_price()

        self.assertEqual(result.company_name, COMPANY_COUNTDOWN)

    def test_date(self):
        result = self.c1.get_product_price()

        self.assertEqual(result.price_date, datetime.date.today())

    def test_price(self):
        result = self.c1.get_product_price()

        self.assertEqual(result.price, 4.8)


if __name__ == '__main__':
    unittest.main()
