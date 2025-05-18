class GameState:
    def __init__(self):
        self.money = 100
        self.animals = []
        self.inventory = {}

    def add_animal(self, animal):
        new_animal = type(animal)()
        new_animal_name = new_animal.name
        existing_names = [a.name for a in self.animals]
        if new_animal_name in existing_names:
            idx = 1
            new_name = f"{new_animal_name} {idx}"
            while new_name in existing_names:
                idx += 1
                new_name = f"{new_animal_name} {idx}"
            new_animal.name = new_name
        self.animals.append(new_animal)

    def buy_animal(self, animal):
        price = animal.price
        if self.money >= price:
            new_animal = type(animal)()

            self.money -= price
            self.add_animal(new_animal)
            return True
        return False

    def buy_product(self, product):
        price = product.price

        if self.money >= price:
            product_type = type(product)

            self.money -= price
            self.add_product(product_type, 1)
            return True
        return False

    def sell_product(self, prod):
        product_type = type(prod)

        if self.inventory.get(product_type, 0) > 0:
            self.money += prod.price
            self.remove_product(product_type)
            return True
        return False

    def remove_product(self, product_type):
        self.inventory[product_type] -= 1

    def add_product(self, product_type, amount):
        self.inventory[product_type] = self.inventory.get(product_type, 0) + amount