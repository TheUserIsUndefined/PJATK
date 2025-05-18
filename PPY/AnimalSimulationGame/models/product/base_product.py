from models.product.product_category import ProductCategory


class BaseProduct:
    def __init__(self, name, price, category: ProductCategory):
        self.name = name
        self.price = price
        self.category = category
