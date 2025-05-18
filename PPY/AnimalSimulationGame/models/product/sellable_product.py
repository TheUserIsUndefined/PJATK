from models.product.base_product import BaseProduct
from models.product.product_category import ProductCategory


class SellableProduct(BaseProduct):
    def __init__(self, name, price, category: ProductCategory = None):
        super().__init__(name, price, category)