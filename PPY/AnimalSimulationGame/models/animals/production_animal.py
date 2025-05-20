from models.animals.base_animal import BaseAnimal
import random

class ProductionAnimal(BaseAnimal):
    def __init__(self, name, price, allowed_food_categories, produces,
                 min_product_amount, max_product_amount,
                 min_production_time, max_production_time,
                 boredom_rate=0.3, hunger_rate=0.4):
        super().__init__(name, price, allowed_food_categories, boredom_rate, hunger_rate)
        self.produces = produces
        self.min_product_amount = min_product_amount
        self.max_product_amount = max_product_amount
        self.min_production_time = min_production_time
        self.max_production_time = max_production_time
        self.production_timer = 0
        self.product_ready = False
        self.schedule_next()

    def update_production_timer(self):
        change_by = (0.4 if self.boredom >= self.BOREDOM_THRESHOLD
                            or self.hunger >= self.HUNGER_THRESHOLD else 1)
        self.production_timer = max(0, self.production_timer - change_by)

        if self.production_timer == 0: self.product_ready = True

    def schedule_next(self):
        interval = random.randint(self.min_production_time, self.max_production_time)
        self.production_timer = interval
        self.product_ready = False

    def collect(self):
        if not self.product_ready:
            return 0
        self.product_ready = False
        self.schedule_next()
        return self.produces, random.randint(self.min_product_amount, self.max_product_amount)