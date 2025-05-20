import math
import re
import tkinter as tk
from tkinter import ttk, messagebox
from PIL import Image, ImageTk

from models.animals.base_animal import BaseAnimal
from models.animals.production_animal import ProductionAnimal
from models.animals.companion_animal import CompanionAnimal
from gui.start_window import StartWindow
from gui.shop_window import ShopWindow
from utils.window_utils import center_window, GLOBAL_FONT

class MainWindow(tk.Tk):
    def __init__(self, game):
        super().__init__()
        self.title("Animal Simulator")

        self.game = game
        self.shop_window = None
        self.current_animal = None
        self.animal_images = {}

        top = tk.Frame(self)
        top.pack(fill='x', pady=10)
        self.shop_btn = tk.Button(top, text="Shop", fg='blue', disabledforeground='red',
                                  font=('Arial',GLOBAL_FONT+2), command=self.open_shop)
        self.shop_btn.pack(side='left', padx=5)
        self.animal_list = ttk.Combobox(top, state='readonly', font=('Arial',GLOBAL_FONT+2))
        self.animal_list.pack(side='left', padx=5)
        self.animal_list.bind('<<ComboboxSelected>>', lambda e: self.select_animal(True))
        self.status_label = tk.Label(top, font=('Arial', GLOBAL_FONT + 4))
        self.status_label.pack(side='left', padx=10)

        self.name_frame = tk.Frame(self)
        self.name_frame.pack(pady=5)
        self.name_label = tk.Label(self.name_frame, font=('Arial',GLOBAL_FONT+4))
        self.name_label.pack(side='left')
        (tk.Button(self.name_frame, text="Change", fg='green', font=('Arial',GLOBAL_FONT),
                   command=lambda: self.change_name())
         .pack(side='left', padx=5))

        mid = tk.Frame(self)
        mid.pack(fill='both', expand=True)

        self.img_label = tk.Label(mid)
        self.img_label.image_name = None
        self.img_label.bind('<Button-1>', lambda e: self.click_event())
        self.img_label.pack(side='right', expand=True, padx=10)

        stats = tk.Frame(mid)
        stats.pack(side='left', padx=10)
        self.hunger_label = tk.Label(stats, font=('Arial',GLOBAL_FONT+4))
        self.boredom_label = tk.Label(stats, font=('Arial',GLOBAL_FONT+4))
        self.hunger_label.pack()
        self.boredom_label.pack()

        self.prod_label = tk.Label(self, font=('Arial',GLOBAL_FONT+2))
        self.prod_label.pack(pady=10)

        bottom = tk.Frame(self)
        bottom.pack(fill='x', pady=10)

        feed_frame = tk.Frame(bottom)
        feed_frame.pack(side='top', anchor='w', pady=5)
        self.feed_btn = tk.Button(feed_frame, text="Feed", fg='green', disabledforeground='red',
                                  font=('Arial',GLOBAL_FONT), command=self.feed)
        self.food_list = ttk.Combobox(feed_frame, state='readonly', font=('Arial',GLOBAL_FONT))
        self.feed_cooldown_label = tk.Label(feed_frame, font=('Arial',GLOBAL_FONT))
        self.feed_btn.pack(side='left', padx=5)

        play_frame = tk.Frame(bottom)
        play_frame.pack(side='top', anchor='w')
        self.play_btn = tk.Button(play_frame, text="Play", fg='green', font=('Arial',GLOBAL_FONT),
                                  command=self.start_play)
        self.cancel_btn = tk.Button(play_frame,
                                    text=f"Cancel Play (Boredom +{BaseAnimal.BOREDOM_TO_ADD_ON_CANCELLATION})",
                                    fg='red', font=('Arial',GLOBAL_FONT), command=self.cancel_play)
        self.play_timer = tk.Label(play_frame, font=('Arial',GLOBAL_FONT+4))

        self.play_time_str = tk.StringVar(value='5')
        self.play_time_str.trace('w', lambda *args: self.update_play_frame())
        self.play_time = ttk.Spinbox(play_frame, from_=self.game.MINIMUM_PLAYTIME, to=100,
                                     textvariable=self.play_time_str, font=('Arial',GLOBAL_FONT))
        self.play_time.pack(side='right', padx=5)

        self.withdraw()
        StartWindow(self, self.game.animals, self.start_game)
        self.global_tick()

        self.protocol("WM_DELETE_WINDOW", self.on_close)

    def start_game(self, selected_animal):
        self.game.start_game(selected_animal)
        self.deiconify()

        self.update_animal_list()
        self.update_idletasks()
        center_window(self)

    def open_shop(self):
        self.shop_window = ShopWindow(self, self.game)
        self.shop_btn.config(state=tk.DISABLED)

    def close_shop(self):
        self.shop_window.destroy()
        self.shop_window = None
        self.shop_btn.config(state=tk.NORMAL)

    def update_animal_list(self):
        decorated_names = [self.get_animal_prefix(a) + a.name
                 for a in self.game.state.animals]

        decorated_names.sort(key=lambda d_name: 0 if d_name == "*" else 2 if d_name.startswith("(DEAD)") else 1)

        self.animal_list['values'] = decorated_names
        if decorated_names:
            if self.current_animal is not None:
                index = None
                for i, name in enumerate(decorated_names):
                    if name.replace("*", "") == self.current_animal.name:
                        index = i
                self.animal_list.current(0 if index is None else index)
            else:
                self.animal_list.current(0)
            self.select_animal()

    def select_animal(self, clicked=False):
        name = self.animal_list.get().replace('*','').replace("(DEAD) ", '')
        animal = next(an for an in self.game.state.animals if an.name == name)

        if not animal.isAlive:
            if clicked:
                self.game.state.animals.remove(animal)
                self.update_animal_list()
                return

            self.animal_list.current(0)
            self.select_animal()
            return

        self.current_animal = animal

        self.update_animal_image()

        self.update_animal_properties()

        self.update_animal_status()

        self.update_food_frame()

        self.update_play_frame()

    def update_animal_image(self):
        animal_type = type(self.current_animal).__name__
        if self.img_label.image_name != animal_type:
            self.img_label.image_name = animal_type

            if animal_type not in self.animal_images:
                image = Image.open(f"assets/{animal_type}.png").resize((700, 700), Image.Resampling.LANCZOS)
                self.animal_images[animal_type] = ImageTk.PhotoImage(image)

            self.img_label.configure(image=self.animal_images[animal_type])

    def update_animal_properties(self):
        animal = self.current_animal

        name = animal.name
        hunger = animal.hunger
        boredom = animal.boredom
        hunger_threshold_reached = hunger >= animal.HUNGER_THRESHOLD
        boredom_threshold_reached = boredom >= animal.BOREDOM_THRESHOLD

        self.name_label.config(text=name)
        self.hunger_label.config(text=f"Hunger: {int(hunger)}",
                                    fg='red' if hunger_threshold_reached else 'green')
        self.boredom_label.config(text=f"Boredom: {int(boredom)}",
                                     fg='red' if boredom_threshold_reached else 'green')

        if isinstance(animal, ProductionAnimal):
            if animal.product_ready:
                self.prod_label.config(text=f"{animal.name} is ready to give product(-s)!", fg='green')
            else:
                self.prod_label.config(text=f"Produces in: {math.ceil(animal.production_timer)}s", fg='red')
        else:
            self.prod_label.config(text="")

    def update_animal_status(self):
        animal = self.current_animal

        hunger_threshold_reached = animal.hunger >= animal.HUNGER_THRESHOLD
        boredom_threshold_reached = animal.boredom >= animal.BOREDOM_THRESHOLD

        status = ""
        if hunger_threshold_reached:
            status = "Hungry"
        if boredom_threshold_reached:
            if status:
                status += " and bored"
            else:
                status = "Bored"

        if status:
            if isinstance(animal, ProductionAnimal):
                status += " (production is slowed)!"
            elif isinstance(animal, CompanionAnimal):
                if hunger_threshold_reached:
                    status += " (life in danger)!"
                else:
                    status += " (animal gets hungry faster)!"
        else:
            status = "OK"

        self.status_label.configure(text=f"Status: {status}", fg='green' if status == "OK" else 'red')

    def update_food_frame(self):
        self.food_list.pack_forget()
        self.feed_cooldown_label.pack_forget()

        feed_cooldown = self.current_animal.feed_cooldown
        if feed_cooldown:
            self.feed_cooldown_label.config(text=f"Cooldown: {feed_cooldown}s")
            self.feed_btn.config(state=tk.DISABLED)
            self.feed_cooldown_label.pack(side='right', padx=5)
        else:
            self.feed_btn.config(state=tk.NORMAL)
            self.update_food_list()
            self.food_list.pack(side='right', padx=5)

    def update_food_list(self):
        food_list = []
        for prod_type, amount in self.game.state.inventory.items():
            product = next(p for p in self.game.products if type(p) == prod_type)

            if product.category in self.current_animal.allowed_food_categories and amount > 0:
                food_list.append(f"{product.name} ({amount})")
        food_list.sort()

        self.food_list['values'] = food_list
        if food_list and not self.food_list.get() in food_list:
            self.food_list.current(0)
            self.feed_btn.config(state=tk.NORMAL)
        if not food_list:
            self.food_list.set("")
            self.feed_btn.config(state=tk.DISABLED, disabledforeground='red')

    def change_name(self):
        for widget in self.name_frame.winfo_children(): widget.pack_forget()

        entry = tk.Entry(self.name_frame, textvariable=tk.StringVar(value=self.name_label.cget("text")),
                         font=('Arial',GLOBAL_FONT))
        cancel_btn = tk.Button(self.name_frame, text="Cancel", fg='red', font=('Arial', GLOBAL_FONT),
                               command=lambda: self.cancel_change_name(entry, cancel_btn, ok_btn))
        ok_btn = tk.Button(self.name_frame, text="OK", fg='green', font=('Arial',GLOBAL_FONT),
                           command=lambda: self.apply_name(entry, cancel_btn, ok_btn))

        entry.pack(side='left', padx=5)
        cancel_btn.pack(side='left')
        ok_btn.pack(padx=5)

    def apply_name(self, entry, cancel_btn, ok_btn):
        max_name_length = 20
        banned_words = ["(dead)"]

        new_name = entry.get()
        if any(animal.name == new_name for animal in self.game.state.animals):
            messagebox.showinfo("Animal", "Animal with this name already exists!")
            return

        if re.search(r'\W', new_name):
            messagebox.showerror("Error", "Name cannot contain special characters!")
            return

        if new_name == "":
            messagebox.showerror("Error", "Name cannot be an empty string!")
            return

        if len(new_name) > max_name_length:
            messagebox.showerror("Error",
                                 f"Name cannot be longer than {max_name_length} characters!")
            return

        for word in banned_words:
            if word.lower() in new_name.lower():
                messagebox.showerror("Error", f"Name cannot contain word {word}")
                return


        self.game.state.change_animal_name(self.current_animal, new_name)

        entry.destroy()
        cancel_btn.destroy()
        ok_btn.destroy()

        self.show_name_frame()
        self.update_animal_list()

    def cancel_change_name(self, entry, cancel_btn, ok_btn):
        entry.destroy()
        cancel_btn.destroy()
        ok_btn.destroy()

        self.show_name_frame()

    def show_name_frame(self):
        for widget in self.name_frame.winfo_children(): widget.pack(side='left', padx=5)

    def feed(self):
        selected_food = self.food_list.get()
        if self.current_animal and selected_food:
            food_name = selected_food.split()[0]
            self.game.feed_animal(self.current_animal, food_name)
            if self.shop_window is not None:
                self.shop_window.refresh_amount_labels()

            self.update_animal_properties()
            self.update_food_frame()

    def start_play(self):
        if self.game.start_animal_play(self.current_animal, int(self.play_time_str.get())):
            self.update_play_frame()

    def cancel_play(self):
        if self.current_animal.cancel_play():
            self.update_animal_properties()
            self.update_play_frame()

    def update_play_frame(self):
        self.play_btn.pack_forget()
        self.cancel_btn.pack_forget()
        self.play_time.pack_forget()
        self.play_timer.pack_forget()

        animal = self.current_animal
        playtime_left = animal.playtime_left
        if playtime_left:
            self.play_timer.config(text=f"Time left: {playtime_left}s")

            self.cancel_btn.pack(side='left', padx=5)
            self.play_timer.pack(side='right', padx=5)
        else:

            self.play_btn.pack(side='left', padx=5)
            if any(a.playtime_left for a in self.game.state.animals if a != animal):
                self.play_btn.config(text="Playing with another animal", disabledforeground='red', state=tk.DISABLED)
            else:
                self.play_time.pack(side='right', padx=5)
                self.play_btn.config(text="Play", fg='green')
                try:
                    value = int(self.play_time_str.get())
                    if (self.game.MINIMUM_PLAYTIME <= value <=
                            math.ceil(int(animal.boredom) / animal.BOREDOM_DECREASE_RATE)):
                        self.play_btn.config(state=tk.NORMAL)
                    else:
                        self.play_btn.config(state=tk.DISABLED)
                except ValueError:
                    self.play_btn.config(state=tk.DISABLED)

    def click_event(self):
        if isinstance(self.current_animal, ProductionAnimal) and self.current_animal.product_ready:
            self.game.collect_product(self.current_animal)

            if self.shop_window is not None:
                self.shop_window.refresh_amount_labels()
                self.shop_window.refresh_sell_buttons()
            self.update_animal_list()
        elif isinstance(self.current_animal, CompanionAnimal):
            self.current_animal.play_click()
            self.boredom_label.configure(text=f"Boredom: {int(self.current_animal.boredom)}")

        # x = self.winfo_pointerx() - self.winfo_rootx()
        # y = self.winfo_pointery() - self.winfo_rooty() - 2
        #
        # msg = tk.Label(self, text="test", font=('Arial', GLOBAL_FONT - 2), fg='green')
        # msg.place(x=x, y=y, anchor='s')
        #
        # msg.after(500, msg.destroy)

    def global_tick(self):
        self.game.update_animals_stats()
        if self.game.update_product_prices():
            if self.shop_window is not None:
                self.shop_window.refresh_sell_price_labels()

        if self.shop_window is not None:
            self.shop_window.refresh_update_timer_label()

        if self.game.state.animals:
            self.update_animal_list()
        self.after(1000, self.global_tick)

    def on_close(self):
        self.destroy()
        exit(0)

    @classmethod
    def get_animal_prefix(cls, animal):
        if not animal.isAlive:
            return "(DEAD) "
        elif ((isinstance(animal, ProductionAnimal) and animal.product_ready)
              or animal.hunger >= animal.HUNGER_THRESHOLD
              or animal.boredom >= animal.HUNGER_THRESHOLD):
            return "*"
        return ""