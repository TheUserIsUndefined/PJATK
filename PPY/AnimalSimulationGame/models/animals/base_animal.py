class BaseAnimal:
    MAX_BOREDOM = 100
    MAX_HUNGER = 100

    def __init__(self, name, price, allowed_food_categories, boredom_rate, hunger_rate, hunger=50, boredom=50):
        self.type = self.__class__.__name__
        self.name = name
        self.price = price
        self.allowed_food_categories = allowed_food_categories
        self.hunger = hunger
        self.boredom = boredom
        self.boredom_rate = boredom_rate
        self.hunger_rate = hunger_rate
        self.playing = False
        self.play_end_time = None

    def feed(self, amount):
        self.hunger = max(0, self.hunger - amount)

    def update_boredom(self):
        if self.playing:
            self.boredom = max(0, self.boredom - 2)
        else:
            self.add_boredom(self.boredom_rate)

    def update_hunger(self):
        if self.playing:
            self.add_hunger(self.hunger_rate * 1.2)
        else:
            self.add_hunger(self.hunger_rate)

    def add_boredom(self, amount):
        self.boredom = min(self.MAX_BOREDOM, self.boredom + amount)

    def add_hunger(self, amount):
        self.hunger = min(self.MAX_HUNGER, self.hunger + amount)