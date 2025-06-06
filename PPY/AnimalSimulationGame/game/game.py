from entities.animals import *
from entities.foods import *
from .game_state import GameState
from models.product.sellable_product import SellableProduct
from models.animals.companion_animal import CompanionAnimal
from models.animals.production_animal import ProductionAnimal


class Game:
    MINIMUM_PLAYTIME = 5
    UPDATE_PRICE_COOLDOWN = 30
    PRICE_DEVIATION_FACTOR = 0.15

    time_until_price_update = 0

    # Initializes the game with game state, products, and animals
    def __init__(self):
        self.state = GameState()
        self.products = [Beef(), Egg(), Grain(), Milk(), Salmon(), Wheat()]
        self.animals = [Chicken(), Cow(), Dog(), Cat()]

    # Adds an animal to the game state to begin the game
    def start_game(self, selected_animal):
        self.state.add_animal(selected_animal)

    # Updates product prices and returns whether an update occurred
    def update_product_prices(self):
        if self.time_until_price_update > 0:
            self.time_until_price_update -= 1

        if self.time_until_price_update <= 0:
            for product in self.products:
                if isinstance(product, SellableProduct):
                    product.calculate_price_to_add(self.PRICE_DEVIATION_FACTOR)

            self.time_until_price_update = self.UPDATE_PRICE_COOLDOWN
            return True

        return False

    # Collects products from a production animal and adds them to inventory
    def collect_product(self, animal):
        if isinstance(animal, ProductionAnimal):
            produces, amount = animal.collect()
            self.state.add_product(produces, amount)

    # Starts a play session for an animal if no other is playing
    def start_animal_play(self, animal, seconds):
        if any(a.playtime_left for a in self.state.animals): return False

        return animal.start_play(seconds)

    # Updates hunger, boredom, cooldowns, and production/death timers
    def update_animals_stats(self):
        for animal in self.state.animals:
            animal.update_boredom()
            animal.update_hunger()

            if animal.feed_cooldown:
                animal.feed_cooldown -= 1

            if animal.playtime_left:
                animal.playtime_left -= 1

            if isinstance(animal, ProductionAnimal) and animal.production_timer:
                animal.update_production_timer()
            elif isinstance(animal, CompanionAnimal):
                if animal.isAlive:
                    animal.update_death_timer()

    # Feeds the animal with selected food and removes it from inventory
    def feed_animal(self, animal, food_name):
        food = next(p for p in self.products if p.name == food_name)
        animal.feed(food.feed_value)
        animal.feed_cooldown = animal.FEED_COOLDOWN
        self.state.remove_product(type(food))