from models.product.buyable_product import BuyableProduct
from models.product.product_category import ProductCategory


class Beef(BuyableProduct):
    def __init__(self):
        super().__init__("Beef", 30, 10, ProductCategory.Meat)