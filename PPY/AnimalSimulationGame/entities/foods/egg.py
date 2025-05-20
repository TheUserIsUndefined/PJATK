from models.product.sellable_product import SellableProduct


class Egg(SellableProduct):
    def __init__(self):
        super().__init__("Egg", 10)