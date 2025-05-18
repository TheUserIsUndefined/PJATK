from entities.foods import Milk
from models.animals.production_animal import ProductionAnimal
from models.product.product_category import ProductCategory


class Cow(ProductionAnimal):
    def __init__(self):
        super().__init__("Cow", 15, [ProductCategory.Grain],
                         Milk, 1, 6, 30, 80)