from entities.foods import Egg
from models.animals.production_animal import ProductionAnimal
from models.product.product_category import ProductCategory


class Chicken(ProductionAnimal):
    def __init__(self):
        super().__init__("Chicken", 8, [ProductCategory.Grain],
                         Egg, 1, 2, 10, 40)