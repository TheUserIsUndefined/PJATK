import time

from entities.animals import *
from entities.foods import *
from game.game_state import GameState
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
            animal.play_end_time = time.time() + seconds

            return True

        return False

    def update_animals_stats(self):
        now = time.time()
        for animal in self.state.animals:

            if animal.playing and now >= animal.play_end_time:
                animal.playing = False

            animal.update_boredom()
            animal.update_hunger()

            if isinstance(animal, ProductionAnimal) and not animal.product_ready:
                change_by = (0.2 if animal.boredom >= animal.BOREDOM_THRESHOLD
                                    or animal.hunger >= animal.HUNGER_THRESHOLD else 1)
                animal.remaining = max(0, animal.remaining - change_by)
                if animal.remaining == 0: animal.product_ready = True

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