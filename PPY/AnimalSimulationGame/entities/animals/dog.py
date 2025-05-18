from models.animals.companion_animal import CompanionAnimal
from models.product.product_category import ProductCategory


class Dog(CompanionAnimal):
    def __init__(self):
        super().__init__("Dog", 800, [ProductCategory.Fish, ProductCategory.Meat])