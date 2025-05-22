from models.product.product_category import ProductCategory


class BaseProduct:
    # Initializes a product base with name, price, and category
    def __init__(self, name, price, category: ProductCategory):
        self.name = name
        self.price = price
        self.category = category

    # Returns the total price of the product
    def total_price(self):
        return self.price