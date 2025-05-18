from models.product.base_product import BaseProduct
from models.product.product_category import ProductCategory


class BuyableProduct(BaseProduct):
    def __init__(self, name, price, feed_value, category: ProductCategory = None):
        super().__init__(name, price, category)
        self.feed_value = feed_value