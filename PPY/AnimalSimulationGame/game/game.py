from entities.animals import *
from entities.foods import *
from .game_state import GameState
from models.animals.companion_animal import CompanionAnimal
from models.animals.production_animal import ProductionAnimal


class Game:
    BOREDOM_TO_ADD_ON_CANCELLATION = 3

    def __init__(self):
        self.state = GameState()
        self.products = [Beef(), Egg(), Grain(), Milk(), Salmon(), Wheat()]
        self.animals = [Chicken(), Cow(), Dog(), Cat()]

    def start_game(self, selected_animal):
        self.state.add_animal(selected_animal)

    def collect_product(self, animal):
        amount = animal.collect()
        produces = animal.produces

        self.state.add_product(produces, amount)

    def start_animal_play(self, animal, seconds):
        if any(a.playing for a in self.state.animals): return False
        if seconds > 0:
            animal.playing = True
            animal.playtime_left = seconds

            return True

        return False

    def update_animals_stats(self):
        for animal in self.state.animals:
            animal.update_boredom()
            animal.update_hunger()

            if animal.playing:
                animal.playtime_left -= 1

                if animal.playtime_left <= 0:
                    animal.playing = False

            if isinstance(animal, ProductionAnimal) and not animal.product_ready:
                animal.update_production_timer()
            elif isinstance(animal, CompanionAnimal):
                if animal.isAlive:
                    animal.update_death_timer()


    def feed_animal(self, animal, food_name):
        food = next(p for p in self.products if p.name == food_name)
        animal.feed(food.feed_value)
        self.state.remove_product(type(food))

    @classmethod
    def cancel_animal_play(cls, animal):
        if animal.playing:
            animal.add_boredom(cls.BOREDOM_TO_ADD_ON_CANCELLATION)
            animal.playing = False

            return True

        return False