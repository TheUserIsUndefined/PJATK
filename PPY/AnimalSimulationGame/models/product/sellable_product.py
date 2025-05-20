import random

from models.product.base_product import BaseProduct
from models.product.product_category import ProductCategory


class SellableProduct(BaseProduct):

    def __init__(self, name, price, category: ProductCategory = None):
        super().__init__(name, price, category)
        self.price_to_add = 0

    def total_price(self):
        return self.price + self.price_to_add

    def calculate_price_to_add(self, deviation):
        self.price_to_add = round(self.price * (random.random() * 2 - 1) * deviation, 1)