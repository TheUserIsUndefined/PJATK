from models.product.base_product import BaseProduct
from models.product.product_category import ProductCategory


class BuyableProduct(BaseProduct):
    # Initializes a buyable product with specific parameters and feed value
    def __init__(self, name, price, feed_value, category: ProductCategory = None):
        super().__init__(name, price, category)
        self.feed_value = feed_value