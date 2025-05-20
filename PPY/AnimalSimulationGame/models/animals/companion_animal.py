import random

from models.animals.base_animal import BaseAnimal

class CompanionAnimal(BaseAnimal):
    BOREDOM_DECREASE_PER_CLICK = 0.15
    HUNGRY_MULTIPLIER = 1.5

    def __init__(self, name, price, allowed_food_categories, boredom_rate=0.4, hunger_rate=0.3):
        super().__init__(name, price, allowed_food_categories, boredom_rate, hunger_rate)
        self.until_death = 0

    def play_click(self):
        self.boredom = max(0, self.boredom - self.BOREDOM_DECREASE_PER_CLICK)

    def update_hunger(self, multiplier = 1):
        is_bored = self.boredom >= self.BOREDOM_THRESHOLD
        if is_bored:
            multiplier = self.HUNGRY_MULTIPLIER
        super().update_hunger(multiplier)

    def update_death_timer(self):
        is_hungry = self.hunger >= self.HUNGER_THRESHOLD

        if is_hungry:
            if self.until_death == 0:
                self.until_death = random.randint(120,240)
            else:
                self.until_death -= 1
                if self.until_death == 0:
                    self.isAlive = False
        elif self.until_death > 0:
            self.until_death = 0