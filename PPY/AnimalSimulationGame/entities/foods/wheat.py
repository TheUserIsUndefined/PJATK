from models.product.product_category import ProductCategory
from models.product.buyable_product import BuyableProduct


class Wheat(BuyableProduct):
    def __init__(self):
        super().__init__("Wheat", 10, 5, ProductCategory.Grain)