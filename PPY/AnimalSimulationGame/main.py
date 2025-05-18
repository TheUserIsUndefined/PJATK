from game.game import Game
from gui.main_window import MainWindow

if __name__ == "__main__":
    game = Game()
    app = MainWindow(game)
    app.mainloop()