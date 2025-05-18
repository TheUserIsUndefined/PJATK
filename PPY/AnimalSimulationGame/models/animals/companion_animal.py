from models.animals.base_animal import BaseAnimal

class CompanionAnimal(BaseAnimal):
    BOREDOM_DECREASE_PER_CLICK = 0.15

    def __init__(self, name, price, allowed_food_categories, boredom_rate=0.6, hunger_rate=0.3):
        super().__init__(name, price, allowed_food_categories, boredom_rate, hunger_rate)

    def play_click(self):
        self.boredom = max(0, self.boredom - self.BOREDOM_DECREASE_PER_CLICK)