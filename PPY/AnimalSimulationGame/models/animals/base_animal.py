class BaseAnimal:
    MAX_BOREDOM = 100
    MAX_HUNGER = 100
    BOREDOM_THRESHOLD = 90
    HUNGER_THRESHOLD = 90
    BOREDOM_DECREASE_RATE = 2
    BOREDOM_TO_ADD_ON_CANCELLATION = 3
    FEED_COOLDOWN = 3

    # Initializes an animal base with basic attributes
    def __init__(self, name, price, allowed_food_categories, boredom_rate, hunger_rate, hunger=50, boredom=50):
        self.type = self.__class__.__name__
        self.name = name
        self.price = price
        self.allowed_food_categories = allowed_food_categories
        self.hunger = hunger
        self.boredom = boredom
        self.boredom_rate = boredom_rate
        self.hunger_rate = hunger_rate
        self.isAlive = True
        self.playtime_left = None
        self.feed_cooldown = None

    # Reduces hunger by a specified amount
    def feed(self, amount):
        self.hunger = max(0, self.hunger - amount)

    # Updates boredom based on playtime or boredom rate
    def update_boredom(self):
        if self.playtime_left:
            self.boredom = max(0, self.boredom - self.BOREDOM_DECREASE_RATE)
        else:
            self.add_boredom(self.boredom_rate)

    # Updates hunger, increasing it with a multiplier
    def update_hunger(self, multiplier = 1):
        if self.playtime_left and multiplier == 1:
            multiplier = 1.2

        self.add_hunger(self.hunger_rate * multiplier)

    # Starts playtime for a specified duration
    def start_play(self, seconds):
        if seconds > 0:
            self.playtime_left = seconds

            return True

        return False

    # Cancels play and adds boredom penalty
    def cancel_play(self):
        if self.playtime_left:
            self.add_boredom(self.BOREDOM_TO_ADD_ON_CANCELLATION)
            self.playtime_left = 0

            return True

        return False

    # Increases boredom by a specified amount
    def add_boredom(self, amount):
        self.boredom = min(self.MAX_BOREDOM, self.boredom + amount)

    # Increases hunger by a specified amount
    def add_hunger(self, amount):
        self.hunger = min(self.MAX_HUNGER, self.hunger + amount)

    # Compares animals by name for sorting
    def __lt__(self, other):
        return self.name < other.name