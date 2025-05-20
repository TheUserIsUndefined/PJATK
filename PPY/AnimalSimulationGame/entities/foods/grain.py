from models.product.product_category import ProductCategory
from models.product.buyable_product import BuyableProduct


class Grain(BuyableProduct):
    def __init__(self):
        super().__init__("Grain", 15, 8, ProductCategory.Grain)