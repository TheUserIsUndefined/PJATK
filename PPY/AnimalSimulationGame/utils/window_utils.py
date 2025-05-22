GLOBAL_FONT=16

# Centers the window on the screen
def center_window(window):
    x = (window.winfo_screenwidth() - window.winfo_width()) // 2
    y = (window.winfo_screenheight() - window.winfo_height()) // 2
    window.geometry(f"+{x}+{y}")

# Resizes the window to specified or current dimensions
def resize_window(window, new_width = None, new_height = None):
    if new_width is None:
        new_width = window.winfo_width()
    if new_height is None:
        new_height = window.winfo_height()
    window.geometry(f"{new_width}x{new_height}")