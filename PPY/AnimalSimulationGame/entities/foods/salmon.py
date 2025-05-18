from models.product.product_category import ProductCategory
from models.product.buyable_product import BuyableProduct


class Salmon(BuyableProduct):
    def __init__(self):
        super().__init__("Salmon", 50, 15, ProductCategory.Fish)