import tkinter as tk
from tkinter import messagebox

from models.animals.companion_animal import CompanionAnimal
from models.product.buyable_product import BuyableProduct
from models.product.sellable_product import SellableProduct
from utils.window_utils import GLOBAL_FONT, center_window, resize_window


class ShopWindow(tk.Toplevel):
    # Initializes and displays the shop window UI
    def __init__(self, master, game):
        super().__init__(master)
        self.game = game
        self.title("Shop")

        self.buy_buttons = []
        self.sell_buttons = []
        self.amount_labels = []
        self.price_labels = []

        top = tk.Frame(self)
        top.pack(fill='x', pady=5)
        self.money_label = tk.Label(top, font=('Arial', GLOBAL_FONT+2))
        self.money_label.pack(anchor='w', padx=10)
        self.update_timer_label = tk.Label(top, fg='green', font=('Arial', GLOBAL_FONT))
        self.update_timer_label.pack(anchor='w', padx=10)

        self.main_frame = tk.Frame(self)
        self.main_frame.pack(fill='both', expand=True)

        self.refresh_money_label()
        self.refresh_update_timer_label()
        self.initialize_sell_section()
        self.initialize_buy_section()

        self.update_idletasks()
        resize_window(self, new_height=self.winfo_height()+100)
        center_window(self)

        self.protocol("WM_DELETE_WINDOW", self.master.close_shop)

    # Builds the UI section for selling
    def initialize_sell_section(self):
        sell_frame = tk.LabelFrame(self.main_frame, text='Sell')
        sell_frame.pack(side='left', fill='y', padx=5, pady=5)

        self.initialize_sell_food_category(sell_frame)

    # Builds the UI section for buying
    def initialize_buy_section(self):
        buy_frame = tk.LabelFrame(self.main_frame, text='Buy')
        buy_frame.pack(side='left', fill='both', expand=True, padx=5, pady=5)

        self.initialize_buy_animal_category(buy_frame)
        self.initialize_buy_food_category(buy_frame)

    # Adds food to sell frame
    def initialize_sell_food_category(self, sell_frame):
        tk.Label(sell_frame, text="Product", width=10, font=('Arial', GLOBAL_FONT), borderwidth=1,
                 relief='raised').grid(row=0, column=0)
        tk.Label(sell_frame, text="Price", width=6, font=('Arial', GLOBAL_FONT), borderwidth=1,
                 relief='raised').grid(row=0, column=1)
        tk.Label(sell_frame, text="Inventory", width=9, font=('Arial', GLOBAL_FONT), borderwidth=1,
                 relief='raised').grid(row=0, column=2)
        for i, prod in enumerate(self.game.products, start=1):
            if not isinstance(prod, SellableProduct): continue

            price = prod.total_price()
            amount = self.game.state.inventory.get(type(prod), 0)

            tk.Label(sell_frame, text=prod.name, width=10, font=('Arial', GLOBAL_FONT - 2)).grid(row=i, column=0)

            price_label = tk.Label(sell_frame, text=self.format_money(price), width=6, font=('Arial', GLOBAL_FONT - 2))
            price_label.grid(row=i, column=1)

            amount_label = tk.Label(sell_frame, text=amount, width=9, font=('Arial', GLOBAL_FONT - 2))
            amount_label.grid(row=i, column=2)

            sell_btn = tk.Button(
                sell_frame, text="Sell", font=('Arial', GLOBAL_FONT - 2),
                state=(tk.NORMAL if amount > 0 else tk.DISABLED),
                command=lambda p=prod: self.sell(p)
            )
            sell_btn.grid(row=i, column=3)

            self.sell_buttons.append((sell_btn, prod))
            self.amount_labels.append((amount_label, prod))
            self.price_labels.append((price_label, prod))

    # Adds animal section to buy frame
    def initialize_buy_animal_category(self, buy_frame):
        tk.Label(buy_frame, text="Animals", font=('Arial', GLOBAL_FONT+2)).pack(anchor='w')
        animal_grid = tk.Frame(buy_frame)
        animal_grid.pack(fill='x')

        tk.Label(animal_grid, text="Name", width=12, font=('Arial', GLOBAL_FONT), borderwidth=1,
                 relief='raised').grid(row=0, column=0)
        tk.Label(animal_grid, text="Type", width=12, font=('Arial', GLOBAL_FONT), borderwidth=1,
                 relief='raised').grid(row=0, column=1)
        tk.Label(animal_grid, text="Produces", width=10, font=('Arial', GLOBAL_FONT), borderwidth=1,
                 relief='raised').grid(row=0, column=2)
        tk.Label(animal_grid, text="Produced Amount", width=16, font=('Arial', GLOBAL_FONT), borderwidth=1,
                 relief='raised').grid(row=0, column=3)
        (tk.Label(animal_grid, text="Price", width=6, font=('Arial', GLOBAL_FONT), borderwidth=1,
                  relief='raised').grid(row=0, column=4))

        for i, animal in enumerate(self.game.animals, start=1):
            animal_type = type(animal).__bases__[0].__name__.replace("Animal", "")
            produces = '-' if isinstance(animal, CompanionAnimal) else animal.produces.__name__
            produced_amount = '-' if isinstance(animal, CompanionAnimal) else (
                str(animal.min_product_amount) + '-' + str(animal.max_product_amount))
            price = animal.price

            tk.Label(animal_grid, text=animal.name, width=12, font=('Arial', GLOBAL_FONT-2)).grid(row=i, column=0)
            tk.Label(animal_grid, text=animal_type, width=12, font=('Arial', GLOBAL_FONT-2)).grid(row=i, column=1)
            tk.Label(animal_grid, text=produces, width=10, font=('Arial', GLOBAL_FONT-2)).grid(row=i, column=2)
            tk.Label(animal_grid, text=produced_amount, width=16, font=('Arial', GLOBAL_FONT-2)).grid(row=i, column=3)
            (tk.Label(animal_grid, text=self.format_money(price), width=6, font=('Arial', GLOBAL_FONT-2))
             .grid(row=i, column=4))

            buy_btn = tk.Button(
                animal_grid, text="Buy", font=('Arial', GLOBAL_FONT-2),
                state=(tk.NORMAL if self.game.state.money >= price else tk.DISABLED),
                command=lambda a=animal: self.buy_animal(a)
            )
            buy_btn.grid(row=i, column=5)

            self.buy_buttons.append((buy_btn, price))

    # Adds food section to buy frame
    def initialize_buy_food_category(self, buy_frame):
        tk.Label(buy_frame, text="Food", font=('Arial', GLOBAL_FONT+2)).pack(anchor='w', pady=(10, 0))
        food_grid = tk.Frame(buy_frame)
        food_grid.pack(fill='x')

        tk.Label(food_grid, text="Name", width=12, font=('Arial', GLOBAL_FONT), borderwidth=1, relief='raised').grid(row=0,
                                                                                                            column=0)
        tk.Label(food_grid, text="Type", width=10, font=('Arial', GLOBAL_FONT), borderwidth=1, relief='raised').grid(row=0,
                                                                                                            column=1)
        tk.Label(food_grid, text="Feeds By", width=8, font=('Arial', GLOBAL_FONT), borderwidth=1, relief='raised').grid(
            row=0,
            column=2)
        tk.Label(food_grid, text="Price", width=6, font=('Arial', GLOBAL_FONT), borderwidth=1, relief='raised').grid(
            row=0,
            column=3)
        tk.Label(food_grid, text="Inventory", width=9, font=('Arial', GLOBAL_FONT), borderwidth=1, relief='raised').grid(
            row=0,
            column=4
        )

        for i, prod in enumerate(self.game.products, start=1):
            if not isinstance(prod, BuyableProduct): continue

            price = prod.total_price()
            amount = self.game.state.inventory.get(type(prod), 0)

            tk.Label(food_grid, text=prod.name, width=12, font=('Arial', GLOBAL_FONT-2)).grid(row=i, column=0)
            tk.Label(food_grid, text=prod.category.name, width=10, font=('Arial', GLOBAL_FONT-2)).grid(row=i, column=1)
            tk.Label(food_grid, text=prod.feed_value, width=8, font=('Arial', GLOBAL_FONT-2)).grid(row=i, column=2)
            tk.Label(food_grid, text=self.format_money(price), width=6, font=('Arial', GLOBAL_FONT-2)).grid(row=i, column=3)

            amount_label = tk.Label(food_grid, text=amount, width=6, font=('Arial', GLOBAL_FONT-2))
            amount_label.grid(row=i, column=4)

            food_btn = tk.Button(
                food_grid, text="Buy", font=('Arial', GLOBAL_FONT-2),
                state=(tk.NORMAL if self.game.state.money >= price else tk.DISABLED),
                command=lambda p=prod: self.buy_product(p)
            )
            food_btn.grid(row=i, column=5)

            self.buy_buttons.append((food_btn, price))
            self.amount_labels.append((amount_label, prod))

    # Updates buy buttons depending on current money
    def refresh_buy_buttons(self):
        for btn, cost in self.buy_buttons:
            if self.game.state.money >= cost:
                btn.config(state='normal')
            else:
                btn.config(state='disabled')

    # Updates sell buttons depending on current inventory
    def refresh_sell_buttons(self):
        for btn, prod in self.sell_buttons:
            if self.game.state.inventory.get(type(prod), 0) <= 0:
                btn.config(state='disabled')
            else:
                btn.config(state='normal')

    # Updates product amount labels based on current inventory
    def refresh_amount_labels(self):
        for label, prod in self.amount_labels:
            label.config(text=self.game.state.inventory.get(type(prod), 0))

    # Updates sell price labels with current product prices
    def refresh_sell_price_labels(self):
        for label, prod in self.price_labels:
            label.config(text=self.format_money(prod.total_price()))

    # Updates the price update timer label
    def refresh_update_timer_label(self):
        self.update_timer_label.config(text=f"Prices refresh in: {self.game.time_until_price_update}s")

    # Updates the money label
    def refresh_money_label(self):
        self.money_label.config(text=f"Money: " + self.format_money(self.game.state.money))

    # Attempts to sell a product and updates UI or shows error
    def sell(self, prod):
        if self.game.state.sell_product(prod):
            self.refresh_money_label()
            self.refresh_sell_buttons()
            self.refresh_buy_buttons()
            self.refresh_amount_labels()
        else:
            messagebox.showinfo("Sell", "No product to sell.")

    # Attempts to buy an animal and updates UI or shows error
    def buy_animal(self, animal):
        if self.game.state.buy_animal(animal):
            self.refresh_money_label()
            self.refresh_buy_buttons()
        else:
            messagebox.showinfo("Buy", "Not enough money.")

    # Attempts to buy a product and updates UI or shows error
    def buy_product(self, product):
        if self.game.state.buy_product(product):
            self.refresh_money_label()
            self.refresh_buy_buttons()
            self.refresh_amount_labels()
            self.master.update_food_list()
        else:
            messagebox.showinfo("Buy", "Not enough money.")

    # Formats a price as currency
    @classmethod
    def format_money(cls, price):
        return f"${price:.1f}"