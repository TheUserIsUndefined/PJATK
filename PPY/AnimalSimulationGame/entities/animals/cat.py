from models.animals.companion_animal import CompanionAnimal
from models.product.product_category import ProductCategory


class Cat(CompanionAnimal):
    def __init__(self):
        super().__init__("Cat", 1500, [ProductCategory.Meat, ProductCategory.Fish])