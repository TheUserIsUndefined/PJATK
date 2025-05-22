from models.animals.base_animal import BaseAnimal
import random

class ProductionAnimal(BaseAnimal):
    # Initializes a production animal with specific parameters, production-related attributes
    # and default boredom and hunger rates
    def __init__(self, name, price, allowed_food_categories, produces,
                 min_product_amount, max_product_amount,
                 min_production_time, max_production_time,
                 boredom_rate=0.3, hunger_rate=0.4
                 ):
        super().__init__(name, price, allowed_food_categories, boredom_rate, hunger_rate)
        self.produces = produces
        self.min_product_amount = min_product_amount
        self.max_product_amount = max_product_amount
        self.min_production_time = min_production_time
        self.max_production_time = max_production_time
        self.production_timer = 0
        self.schedule_next()

    # Updates the production timer based on animal's boredom and hunger
    def update_production_timer(self):
        change_by = (0.4 if self.boredom >= self.BOREDOM_THRESHOLD
                            or self.hunger >= self.HUNGER_THRESHOLD else 1)
        self.production_timer = max(0, self.production_timer - change_by)

    # Schedules the next production cycle with a random interval
    def schedule_next(self):
        interval = random.randint(self.min_production_time, self.max_production_time)
        self.production_timer = interval

    # Schedules the next cycle, returns the product and amount
    def collect(self):
        if self.production_timer:
            return 0
        self.schedule_next()
        return self.produces, random.randint(self.min_product_amount, self.max_product_amount)