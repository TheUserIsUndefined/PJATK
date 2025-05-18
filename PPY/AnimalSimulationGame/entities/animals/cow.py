from entities.foods import Milk
from models.animals.production_animal import ProductionAnimal
from models.product.product_category import ProductCategory


class Cow(ProductionAnimal):
    def __init__(self):
        super().__init__("Cow", 175, [ProductCategory.Grain],
                         Milk, 1, 5, 30, 80)