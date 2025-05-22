import bisect

from models.product.sellable_product import SellableProduct


class GameState:
    # Initializes the game state with starting money, animals, and inventory
    def __init__(self):
        self.money = 10055
        self.animals = []
        self.inventory = {}

    # Adds an animal to the game, ensuring unique names
    def add_animal(self, animal):
        new_animal = type(animal)()
        new_animal_name = new_animal.name
        existing_names = [a.name for a in self.animals]
        if new_animal_name in existing_names:
            idx = 1
            new_name = f"{new_animal_name}_{idx}"
            while new_name in existing_names:
                idx += 1
                new_name = f"{new_animal_name}_{idx}"
            new_animal.name = new_name
        bisect.insort(self.animals, new_animal)

    # Changes an animal's name and re-sorts the animal list
    def change_animal_name(self, animal, new_name):
        animal.name = new_name
        self.animals.sort(key=lambda a: a.name)

    # Purchases an animal if sufficient funds are available
    def buy_animal(self, animal):
        price = animal.price
        if self.money >= price:
            new_animal = type(animal)()

            self.money -= price
            self.add_animal(new_animal)
            return True
        return False

    # Purchases a product if sufficient funds are available
    def buy_product(self, product):
        price = product.total_price()

        if self.money >= price:
            product_type = type(product)

            self.money -= price
            self.add_product(product_type, 1)
            return True
        return False

    # Sells a product if available in inventory
    def sell_product(self, prod):
        product_type = type(prod)

        if isinstance(prod, SellableProduct) and self.inventory.get(product_type, 0) > 0:
            self.money += prod.total_price()
            self.remove_product(product_type)
            return True
        return False

    # Removes one unit of a product from inventory
    def remove_product(self, product_type):
        self.inventory[product_type] -= 1

    # Adds a specified amount of a product to inventory
    def add_product(self, product_type, amount):
        self.inventory[product_type] = self.inventory.get(product_type, 0) + amount