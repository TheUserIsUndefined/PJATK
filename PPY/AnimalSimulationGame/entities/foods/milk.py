from models.product.product_category import ProductCategory
from models.product.sellable_product import SellableProduct


class Milk(SellableProduct):
    def __init__(self):
        super().__init__("Milk", 5, ProductCategory.Diary)