import tkinter as tk

from models.animals.production_animal import ProductionAnimal

class StartWindow(tk.Toplevel):
    def __init__(self, master, animals, callback):
        super().__init__(master)
        self.title("Select Starting Animal")
        self.callback = callback
        self.geometry("600x500")

        tk.Label(self, text="Choose your first animal:", font=('Arial',14)).pack(pady=5)
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
            row.pack(fill='x', pady=2)
            tk.Label(row, text=animal.name, width=12, font=('Arial',12)).pack(side='left')
            btn = tk.Button(row, text="Choose", font=('Arial',12), command=lambda a=animal: self.select(a))
            btn.pack(side='right', padx=100)

        self.protocol("WM_DELETE_WINDOW", self.on_close)

    def select(self, animal):
        self.callback(animal)
        self.destroy()

    def on_close(self):
        self.destroy()
        exit(0)