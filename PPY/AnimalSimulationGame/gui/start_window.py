import tkinter as tk

from models.animals.production_animal import ProductionAnimal
from utils.window_utils import GLOBAL_FONT, center_window, resize_window

class StartWindow(tk.Toplevel):
    # Initializes the start window UI
    def __init__(self, master, animals, callback):
        super().__init__(master)
        self.title("Select Starting Animal")
        self.callback = callback

        tk.Label(self, text="Choose your first animal:", font=('Arial',GLOBAL_FONT+2)).pack(pady=5)
        container = tk.Frame(self)
        container.pack(fill='both', expand=True)
        canvas = tk.Canvas(container)
        scrollbar = tk.Scrollbar(container, orient='vertical', command=canvas.yview)
        scroll_frame = tk.Frame(canvas)
        scroll_frame.bind(
            '<Configure>',
            lambda e: canvas.configure(scrollregion=canvas.bbox('all'))
        )
        canvas.create_window((0,0), window=scroll_frame, anchor='nw')
        canvas.configure(yscrollcommand=scrollbar.set)
        canvas.pack(side='left', fill='both', expand=True)
        scrollbar.pack(side='right', fill='y')

        for animal in animals:
            if not isinstance(animal, ProductionAnimal):
                continue
            row = tk.Frame(scroll_frame)
            row.pack(padx=5, pady=2)
            tk.Label(row, text=animal.name, width=12, font=('Arial',GLOBAL_FONT)).pack(side='left')
            btn = tk.Button(row, text="Choose", fg='green', font=('Arial',GLOBAL_FONT), command=lambda a=animal: self.select(a))
            btn.pack(padx=100)

        self.update_idletasks()
        resize_window(self, new_height=self.winfo_height()+100)
        center_window(self)

        self.protocol("WM_DELETE_WINDOW", self.on_close)

    # Handles animal selection and closes the window
    def select(self, animal):
        self.callback(animal)
        self.destroy()

    # Closes the window and application
    def on_close(self):
        self.destroy()
        exit(0)