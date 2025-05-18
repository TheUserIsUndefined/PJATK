import tkinter as tk
from tkinter import ttk, messagebox
from models.animals.production_animal import ProductionAnimal
from models.animals.companion_animal import CompanionAnimal
from gui.start_window import StartWindow
from gui.shop_window import ShopWindow

class MainWindow(tk.Tk):
    def __init__(self, game):
        super().__init__()
        self.game = game
        self.shop_window = None
        self.current_animal = None
        self.title("Animal Simulator")
        self.geometry("1920x1080")

        top = tk.Frame(self)
        top.pack(fill='x', pady=10)
        tk.Button(top, text="Shop", font=('Arial',12), command=self.open_shop).pack(side='left', padx=5)
        self.animal_list = ttk.Combobox(top, state='readonly', font=('Arial',12))
        self.animal_list.pack(side='left', padx=5)
        self.animal_list.bind('<<ComboboxSelected>>', lambda e: self.select_animal())

        self.name_frame = tk.Frame(self)
        self.name_frame.pack(pady=5)
        self.name_str = tk.StringVar()
        (tk.Label(self.name_frame, textvariable=self.name_str, font=('Arial',16))
         .pack(side='left'))
        (tk.Button(self.name_frame, text="Change", font=('Arial',12), command=lambda: self.change_name())
         .pack(side='left', padx=5))

        mid = tk.Frame(self)
        mid.pack(fill='both', expand=True)
        self.img_label = tk.Label(mid, text="[Image Here]", bg='gray', width=40, height=10)
        self.img_label.pack(side='right', expand=True, padx=10)
        stats = tk.Frame(mid)
        stats.pack(side='left', padx=10)
        self.hunger_str = tk.StringVar(); tk.Label(stats, textvariable=self.hunger_str, font=('Arial',12)).pack()
        self.boredom_str = tk.StringVar(); tk.Label(stats, textvariable=self.boredom_str, font=('Arial',12)).pack()

        self.prod_str = tk.StringVar()
        self.prod_label = tk.Label(self, textvariable=self.prod_str, font=('Arial',14))
        self.prod_label.pack(pady=10)

        ctrl = tk.Frame(self)
        ctrl.pack(fill='x', pady=10)

        feed_frame = tk.Frame(ctrl)
        feed_frame.pack(side='top', anchor='w')
        tk.Button(feed_frame, text="Feed", font=('Arial',12), command=self.feed).pack(side='left', padx=5)
        self.food_list = ttk.Combobox(feed_frame, state='readonly', font=('Arial',12))
        self.food_list.pack(side='left')

        play_frame = tk.Frame(ctrl)
        play_frame.pack(side='top', anchor='w')
        self.play_time_str = tk.StringVar(value='1')
        self.play_btn = tk.Button(play_frame, text="Play", fg='green', font=('Arial',12), command=self.start_play)
        self.cancel_btn = tk.Button(play_frame,
                                    text=f"Cancel Play (Boredom +{self.game.BOREDOM_TO_ADD_ON_CANCELLATION})",
                                    fg='red', font=('Arial',12), command=self.cancel_play)
        self.play_time = ttk.Spinbox(play_frame, from_=1, to=100, textvariable=self.play_time_str, font=('Arial',12))
        self.play_time.pack(side='right', padx=5)
        self.play_time_str.trace('w', lambda *args: self.update_play_buttons())

        self.img_label.bind('<Button-1>', lambda e: self.collect())

        self.withdraw()
        StartWindow(self, self.game.animals, self.start_game)
        self.global_tick()

        self.protocol("WM_DELETE_WINDOW", self.on_close)

    def start_game(self, selected_animal):
        self.game.start_game(selected_animal)
        self.deiconify()

        self.update_animal_list()

    def open_shop(self):
        self.shop_window = ShopWindow(self, self.game)

    def update_animal_list(self, selected_index=None):
        names = [a.name + (' *' if (isinstance(a, ProductionAnimal) and a.product_ready)
                                   or a.hunger >= 90 or a.boredom >= 90 else '')
                 for a in self.game.state.animals]

        self.animal_list['values'] = names
        if names:
            if selected_index is not None and 0 <= selected_index < len(names):
                self.animal_list.current(selected_index)
            else:
                self.animal_list.current(0)
            self.select_animal()

    def select_animal(self):
        name = self.animal_list.get().replace(' *','')
        animal = next(an for an in self.game.state.animals if an.name == name)
        self.current_animal = animal

        self.name_str.set(animal.name)
        self.hunger_str.set(f"Hunger: {int(animal.hunger)}")
        self.boredom_str.set(f"Boredom: {int(animal.boredom)}")

        if isinstance(animal, ProductionAnimal):
            if animal.product_ready:
                self.prod_label.config(fg='green')
                self.prod_str.set(f"{animal.name} is ready to give product(-s)!")
            else:
                self.prod_label.config(fg='red')
                self.prod_str.set(f"Produces in: {animal.remaining}s")
        else:
            self.prod_str.set("")


        food_list = []
        for prod_type, amount in self.game.state.inventory.items():
            product = next(p for p in self.game.products if type(p) == prod_type)

            if product.category in animal.allowed_food_categories and amount > 0:
                food_list.append(f"{product.name} ({amount})")

        self.food_list['values'] = food_list
        if food_list and not self.food_list.get() in food_list:
            self.food_list.current(0)
        if not food_list:
            self.food_list.set("")

        self.update_play_buttons()

    def change_name(self):
        for widget in self.name_frame.winfo_children(): widget.pack_forget()

        entry = tk.Entry(self.name_frame, textvariable=tk.StringVar(value=self.name_str.get()), font=('Arial',12))
        cancel_btn = tk.Button(self.name_frame, text="Cancel", font=('Arial', 12),
                               command=lambda: self.cancel_change_name(entry, cancel_btn, ok_btn))
        ok_btn = tk.Button(self.name_frame, text="OK", font=('Arial',12),
                           command=lambda: self.apply_name(entry, cancel_btn, ok_btn))

        entry.pack(side='left', padx=5)
        cancel_btn.pack(side='left')
        ok_btn.pack(padx=5)

    def apply_name(self, entry, cancel_btn, ok_btn):
        new_name = entry.get()
        if any(animal.name == new_name for animal in self.game.state.animals):
            messagebox.showinfo("Animal", "Animal with this name already exists!")
            return

        self.current_animal.name = new_name

        entry.destroy()
        cancel_btn.destroy()
        ok_btn.destroy()

        self.show_name_frame()
        index = self.game.state.animals.index(self.current_animal)
        self.update_animal_list(index)

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
            self.select_animal()

    def start_play(self):
        if self.game.start_animal_play(self.current_animal, int(self.play_time_str.get())):
            self.update_play_buttons()

    def cancel_play(self):
        if self.game.cancel_animal_play(self.current_animal):
            self.boredom_str.set(f"Boredom: {int(self.current_animal.boredom)}")
            self.update_play_buttons()

    def update_play_buttons(self):
        self.play_btn.pack_forget()
        self.cancel_btn.pack_forget()
        if self.current_animal.playing:
            self.cancel_btn.pack(side='left', padx=5)
        else:
            self.play_btn.pack(side='left', padx=5)
            if any(a.playing for a in self.game.state.animals if a != self.current_animal):
                self.play_btn.config(text="Playing with another animal", fg='red', state='disabled')
            else:
                self.play_btn.config(text="Play", fg='green')
                try:
                    value = int(self.play_time_str.get())
                    if value > 0:
                        self.play_btn.config(state='normal')
                    else:
                        self.play_btn.config(state='disabled')
                except ValueError:
                    self.play_btn.config(state='disabled')

    def collect(self):
        if isinstance(self.current_animal, ProductionAnimal) and self.current_animal.product_ready:
            self.game.collect_product(self.current_animal)

            current_index = self.game.state.animals.index(self.current_animal)
            if self.shop_window is not None:
                print(self.shop_window)
                self.shop_window.refresh_amount_labels()
                self.shop_window.refresh_sell_buttons()
            self.update_animal_list(current_index)
        elif isinstance(self.current_animal, CompanionAnimal):
            self.current_animal.play_click()
            self.boredom_str.set(f"Boredom: {int(self.current_animal.boredom)}")

    def global_tick(self):
        self.game.update_animals_stats()

        if self.game.state.animals:
            if self.current_animal is not None:
                current_index = self.game.state.animals.index(self.current_animal)
                self.update_animal_list(current_index)
            else:
                self.update_animal_list()
        self.after(1000, self.global_tick)

    def on_close(self):
        self.destroy()
        exit(0)